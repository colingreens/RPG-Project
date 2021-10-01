using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {    
        [SerializeField] private float levelUpHealthPercentage = 70f;
        [SerializeField] private TakeDamageEvent takeDamage;
        [SerializeField] private UnityEvent onDie;

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
            damage = Mathf.Round(damage);
            print(gameObject.name + " took damage: " + damage);
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
            takeDamage.Invoke(damage);
            this.instigator = instigator;
            CheckForDeath();
        }

        public float GetHealthPoints() => healthPoints.value;

        public float GetMaxHealthPoints() => baseStats.GetStat(StatClass.Health);

        public float GetPercentage()
        {
            return Mathf.Round(GetFraction() * 100f);
        }

        public float GetFraction()
        {
            return healthPoints.value / baseStats.GetStat(StatClass.Health);
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
                onDie.Invoke();
                AwardExperience(instigator);
                Die();
            }
            //else
            //{
            //    takeDamage.Invoke(); commenting out because I dont want to pass damage down here or move this back into the TakeDamage()
            //}
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

        [Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {

        }
    }
}

