using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression: ScriptableObject
    {
        [SerializeField] List<ProgressionCharacterClass> characterClasses;

        private Dictionary<CharacterClass, Dictionary<StatClass, List<float>>> statBook;

        public float GetStat(CharacterClass characterClass, StatClass stat, int level)
        {
            if (ValidateLevel(stat, characterClass, level) == 0)
                return 0;

            return statBook[characterClass][stat][level - 1];
        }

        public int GetLevels(StatClass stat, CharacterClass characterClass)
        {
            return ValidateLevel(stat, characterClass);
        }

        private int ValidateLevel(StatClass stat, CharacterClass characterClass, int level = 1)
        {
            BuildLookupDictionary();

            var levels = statBook[characterClass][stat];

            if (levels.Count < level)
                return 0;

            return levels.Count;
        }

        private void BuildLookupDictionary()
        {
            if (statBook != null)
                return;

            statBook = new Dictionary<CharacterClass, Dictionary<StatClass, List<float>>>();

            foreach (var characterClass in characterClasses)
            {
                var statChapter = new Dictionary<StatClass, List<float>>();

                foreach (var stat in characterClass.Stats)
                {
                    statChapter.Add(stat.statClass, stat.valuesPerLevel);
                }

                statBook.Add(characterClass.Class, statChapter);
            }
        }

        [System.Serializable]
        private class ProgressionCharacterClass
        {
            public CharacterClass Class;
            public List<StatProgression> Stats;
            
            [System.Serializable]
            public class StatProgression
            {
                public StatClass statClass;
                public List<float> valuesPerLevel = null;
            }
        }
    }
}
