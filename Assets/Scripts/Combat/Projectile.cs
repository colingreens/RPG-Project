using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Transform target = null;
        [SerializeField] private float projectileSpeed = 1f;        

        void Update()
        {
            if (target == null)
                return;

            transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
        }

        private Vector3 GetAimLocation()
        {
            var targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
                return target.position;

            return target.position + Vector3.up * targetCapsule.height / 2;
        }
    }
}
