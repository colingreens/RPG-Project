using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter health;
        Text text;

        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            text = GetComponent<Text>();
        }

        private void Update()
        {
            if (health.GetTarget() == null)
                text.text = "N/A";
            else
                text.text = $"{health.GetTarget().GetPercentage()}%";

        }
    }
}
