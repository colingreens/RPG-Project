using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField]
        private DamageText damageTextPrefab = null;

        private void Start()
        {
            Spawn(10);
        }

        public void Spawn(float damageAmount)
        {
            var instance = Instantiate<DamageText>(damageTextPrefab, transform);
            instance.SetValue(damageAmount);
        }
    }
}
