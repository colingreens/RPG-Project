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
            return progression.GetStat(characterClass, stat, startingLevel);
        }

        public int GetLevel()
        {
            var currentXP = GetComponent<Experience>().ExperiencePoints;
            var penultimateLevel = progression.GetLevels(StatClass.XpToLevelUp, characterClass);

            for (int level = 1; level <= penultimateLevel; level++)
            {
                var xpToLevel = progression.GetStat(characterClass, StatClass.XpToLevelUp, level);
                if (currentXP < xpToLevel)
                    return level;                
            }

            return penultimateLevel + 1; ;
        }
    }
}
