using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        private Fighter player;
        private Text text;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            text = GetComponent<Text>();
        }

        private void Update()
        {
            if (player.GetTarget() == null)
                text.text = "N/A";
            else
                text.text = $"{player.GetTarget().GetHealthPoints()} / {player.GetTarget().GetMaxHealthPoints()}";

        }
    }
}
