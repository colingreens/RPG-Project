using RPG.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {        
        [SerializeField] private float projectileSpeed = 1f;
        [SerializeField] private bool isHomingProjectile = false;
        [SerializeField] private GameObject hitEffect = null;
        [SerializeField] private float maxLifeTime = 10f;
        [SerializeField] private GameObject[] destroyOnHit = null;
        [SerializeField] private float lifeAfterImpact = 2f;
        [SerializeField] private UnityEvent onHit;

        GameObject instigator;
        Health target = null;
        float damage = 0;

        private void Start()
        {
            transform.LookAt(GetAimLocation());

        }

        void Update()
        {
            if (target == null)
                return;

            if (isHomingProjectile && !target.IsDead())
                transform.LookAt(GetAimLocation());

            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;

            Destroy(gameObject, maxLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            var targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
                return target.transform.position;

            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            var hitTarget = other.GetComponent<Health>();

            if (hitTarget == null)
                return;

            if (hitTarget != target)
                return;

            if (hitTarget.IsDead())
                return;

            onHit.Invoke();

            hitTarget.TakeDamage(instigator, damage);
            projectileSpeed = 0;

            if (hitEffect != null)
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);

            foreach (var item in destroyOnHit)
            {
                Destroy(item);
            }

            Destroy(gameObject, lifeAfterImpact);
        }
    }
}
