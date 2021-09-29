using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;

        private int currentLevel = 0;
        private Experience experience = null;

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
                print("Leveled Up!");            
            }
        }

        public float GetStat(StatClass stat)
        {
            return progression.GetStat(characterClass, stat, GetLevel());
        }

        public int GetLevel()
        {
            if (currentLevel < 1)
                currentLevel = CalculateLevel();
                
            return currentLevel;
        }

        public int CalculateLevel()
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
    }
}
