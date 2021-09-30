using System;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpEffect = null;

        private int currentLevel = 0;
        private Experience experience = null;

        public event Action onLevelUp;

        private void Start() 
        {
            currentLevel = GetLevel();
            experience = GetComponent<Experience>();
            if (experience != null) 
                experience.onExperienceGained += UpdateLevel;
        }

        private void UpdateLevel()
        {
            var newLevel = CalculateLevel();
            if (newLevel > currentLevel)
            {
                currentLevel = newLevel;
                LevelUpEffect(); 
                onLevelUp();           
            }
        }

        public float GetStat(StatClass stat) => progression.GetStat(characterClass, stat, GetLevel()) + GetAdditiveModifier(stat);

        public int GetLevel()
        {
            if (currentLevel < 1)
                currentLevel = CalculateLevel();
                
            return currentLevel;
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
            throw new NotImplementedException();
        }        
    }
}
