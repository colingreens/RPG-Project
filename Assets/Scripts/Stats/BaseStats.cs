using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;

        private void Update()
        {
            if (gameObject.tag == "Player")
                print(GetLevel());
        }

        public float GetStat(StatClass stat)
        {
            return progression.GetStat(characterClass, stat, GetLevel());
        }

        public int GetLevel()
        {
            var experiencePoints = GetComponent<Experience>();
            if (experiencePoints == null) 
                return startingLevel;

            var currentXP = experiencePoints.ExperiencePoints;
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
