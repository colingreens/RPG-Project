using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression: ScriptableObject
    {
        [SerializeField] List<ProgressionCharacterClass> characterClasses;

        public float GetStat(CharacterClass characterClass, StatClass statClass, int level)
        {
            return characterClasses.Where(x => x.Class == characterClass).First().Stats.Where(y => y.statClass == statClass).First().valuesPerLevel[level];
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
