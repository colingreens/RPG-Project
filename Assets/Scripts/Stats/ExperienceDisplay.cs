using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience xp;
        Text text;

        private void Awake()
        {
            xp = GameObject.FindWithTag("Player").GetComponent<Experience>();
            text = GetComponent<Text>();
        }

        private void Update()
        {
            text.text = xp.ExperiencePoints.ToString();
        }
    }
}
