using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {        
        [SerializeField] private float timeBetweenAttacks = 1f;               
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        [SerializeField] private Weapon defaultWeapon = null;

        private Health target;
        private float timeSinceLastAttack = Mathf.Infinity;
        private Weapon currentWeapon = null;
        private BaseStats baseStats = null;

        private void Start()
        {
            if (currentWeapon == null)
                EquipWeapon(defaultWeapon);

            baseStats = GetComponent<BaseStats>();

        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null)
                return;
            if (target.IsDead())
                return;

            if (target != null && !GetIsInRange())
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null)
                return false;
            var targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }
        public void Cancel()
        {
            StopAttack();
            target = null;
        }

        public Health GetTarget()
        {
            return target;
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            var animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            EquipWeapon(Resources.Load<Weapon>((string)state));
        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                //this will trigger the hit event
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }        

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.Range ;
        }  

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        //Animation Event
        void Hit()
        {
            if (target == null) 
                return;

            float damage = baseStats.GetStat(StatClass.Damage);

            if (currentWeapon.HasProjectile())
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject,damage);
            else
                target.TakeDamage(gameObject, damage);
        }

        void Shoot()
        {
            Hit();
        }        
    }
}
