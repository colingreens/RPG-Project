using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using System;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float healthPoints = 100f;

        private GameObject instigator;
        private bool isDead;
        private float startingHealth;
        private float experience;

        private void Start()
        {
            healthPoints = GetComponent<BaseStats>().GetStat(StatClass.Health);
            experience = GetComponent<BaseStats>().GetStat(StatClass.Xp);
            startingHealth = healthPoints;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            this.instigator = instigator;
            CheckForDeath();
        }

        public float GetPercentage()
        {
            return (float)Math.Round(healthPoints / startingHealth * 100f,0);
        }

        private void CheckForDeath()
        {
            if (healthPoints == 0 && !isDead)
            {
                AwardExperience(instigator);
                Die();
            }
                
        }

        private void AwardExperience(GameObject instigator)
        {
            if (instigator == null)
                return;

            var instigatorExperience = instigator.GetComponent<Experience>();

            if (instigatorExperience == null)
                return;

            instigatorExperience.GainExperience(experience);
        }

        private void Die()
        {
            if (isDead)
                return;

            isDead = true;

            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
            
        }

        public bool IsDead()
        {
            return isDead;
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;
            CheckForDeath();
        }
    }
}

