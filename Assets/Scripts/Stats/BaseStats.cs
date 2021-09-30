using System;
using GameDevTV.Utils;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpEffect = null;
        [SerializeField] private bool shouldUseModifiers = false;

        private LazyValue<int> currentLevel;
        private Experience experience = null;

        public event Action onLevelUp;

        private void Awake() {
            experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel); 
        }
        private void Start()
        {
               currentLevel.ForceInit();      
        }

        private void OnEnable() {
            if (experience != null)
                experience.onExperienceGained += UpdateLevel;
        }

        private void OnDisable() {
            if (experience != null)
                experience.onExperienceGained -= UpdateLevel;
        }

        private void UpdateLevel()
        {
            var newLevel = CalculateLevel();
            if (newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                LevelUpEffect();
                onLevelUp();
            }
        }

        public float GetStat(StatClass stat) => GetBaseStat(stat) + GetAdditiveModifier(stat) * (1f + GetPercentageModifier(stat))/100f;

        public int GetLevel()
        {
            if (currentLevel.value < 1)
                currentLevel.value = CalculateLevel();

            return currentLevel.value;
        }

        private void LevelUpEffect() => Instantiate(levelUpEffect, transform);

        private int CalculateLevel()
        {
            if (experience == null)
                return startingLevel;

            var currentXP = experience.ExperiencePoints;
            var penultimateLevel = progression.GetLevels(StatClass.XpToLevelUp, characterClass);

            for (int level = 1; level <= penultimateLevel; level++)
            {
                var xpToLevel = progression.GetStat(characterClass, StatClass.XpToLevelUp, level);
                if (currentXP < xpToLevel)
                    return level;
            }
            return penultimateLevel + 1;
        }

        private float GetAdditiveModifier(StatClass stat)
        {
            if (!shouldUseModifiers) return 0;

            float sum = 0;
            foreach (var provider in GetComponents<IModifierProvider>())
            {
                foreach (var modifier in provider.GetAdditiveModifiers(stat))
                {
                    sum += modifier;
                }
            }
            return sum;
        }

        private float GetPercentageModifier(StatClass stat)
        {
            if (!shouldUseModifiers) return 0;
            
            float sum = 0;
            foreach (var provider in GetComponents<IModifierProvider>())
            {
                foreach (var modifier in provider.GetPercentageModifiers(stat))
                {
                    sum += modifier;
                }
            }
            return sum;
        }

        private float GetBaseStat(StatClass stat)
        {
            return progression.GetStat(characterClass, stat, GetLevel());
        }
    }
}
