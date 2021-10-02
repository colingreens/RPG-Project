using System;
using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {        
        [SerializeField] private float timeBetweenAttacks = 1f;               
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        [SerializeField] private WeaponConfig defaultWeaponConfig = null;

        private Health target;
        private float timeSinceLastAttack = Mathf.Infinity;
        private WeaponConfig currentWeaponConfig;
        private BaseStats baseStats = null;
        private LazyValue<Weapon> currentWeapon;

        private void Awake() {            

            baseStats = GetComponent<BaseStats>();
            currentWeaponConfig = defaultWeaponConfig;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        private void Start()
        {
            currentWeapon.ForceInit();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null)
                return;
            if (target.IsDead())
                return;

            if (target != null && !GetIsInRange(target.transform))
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }
        }
        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeaponConfig);
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null)
                return false;

            if (!GetComponent<Mover>().CanMoveTo(combatTarget.transform.position) && !GetIsInRange(combatTarget.transform))
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

        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            var animator = GetComponent<Animator>();
            return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public object CaptureState()
        {
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            EquipWeapon(Resources.Load<WeaponConfig>((string)state));
        }        

        public IEnumerable<float> GetAdditiveModifiers(StatClass stat)
        {
            if (stat == StatClass.Damage)
                yield return currentWeaponConfig.Damage;
        }

        public IEnumerable<float> GetPercentageModifiers(StatClass stat)
        {
            if (stat == StatClass.Damage)
                yield return currentWeaponConfig.PercentageBonus;
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

        private bool GetIsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) < currentWeaponConfig.Range;
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

            if (currentWeapon.value != null)
            {
                currentWeapon.value.OnHit();
            }

            if (currentWeaponConfig.HasProjectile())
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject,damage);
            else
                target.TakeDamage(gameObject, damage);
        }

        void Shoot()
        {
            Hit();
        }        
    }
}
