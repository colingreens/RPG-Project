using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using System;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {    
        [SerializeField] private float levelUpHealthPercentage = 70f;

        private GameObject instigator;
        private BaseStats baseStats;
        private bool isDead;
        private float experience;
        private LazyValue<float> healthPoints;

        private void Awake() {
            baseStats = GetComponent<BaseStats>(); 
            healthPoints = new LazyValue<float>(GetInitialHealth);          
        }

        private void Start()
        {    
            healthPoints.ForceInit();

            experience = baseStats.GetStat(StatClass.Xp); 
        }
        private void OnEnable() {
            baseStats.onLevelUp += AddLevelUpHealth;
        }

        private void OnDisable() {
            baseStats.onLevelUp -= AddLevelUpHealth;

        }
        private float GetInitialHealth(){
            return baseStats.GetStat(StatClass.Health);
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            print(gameObject.name + " took damage: " + damage);
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
            this.instigator = instigator;
            CheckForDeath();
        }

        public float GetHealthPoints() => healthPoints.value;

        public float GetMaxHealthPoints() => baseStats.GetStat(StatClass.Health);

        public float GetPercentage()
        {
            return (float)Math.Round(healthPoints.value / baseStats.GetStat(StatClass.Health) * 100f,0);
        }
          public bool IsDead()
        {
            return isDead;
        }

        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;
            CheckForDeath();
        }

        private void CheckForDeath()
        {
            if (healthPoints.value == 0 && !isDead)
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

        private void AddLevelUpHealth()
        {
            var regenHealthPoints = baseStats.GetStat(StatClass.Health) * (levelUpHealthPercentage/100f);
            healthPoints.value = Mathf.Max(healthPoints.value,regenHealthPoints);
        }
    }
}

