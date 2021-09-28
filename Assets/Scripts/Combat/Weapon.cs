using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private AnimatorOverrideController animatorOverride = null;
        [SerializeField] private GameObject equippedPrefab = null;
        [SerializeField] private float range = 2f;
        [SerializeField] private float damage = 5f;
        [SerializeField] private bool isRightHanded = true;

        public float Damage => damage;

        public float Range => range;

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (equippedPrefab != null)
            {
                Transform handTransform;
                if (isRightHanded)
                    handTransform = rightHand;
                else
                    handTransform = leftHand;

                Instantiate(equippedPrefab, handTransform);
            }
                

            if (animatorOverride != null)
                animator.runtimeAnimatorController = animatorOverride;


        }
    }
}