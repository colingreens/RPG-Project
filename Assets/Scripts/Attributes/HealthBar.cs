using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField]
        private Health healthComponent = null;
        [SerializeField]
        private RectTransform foreground = null;
        [SerializeField]
        private Canvas rootCanvas;

        void Update()
        {
            var healthFraction = healthComponent.GetFraction();

            if (Mathf.Approximately(healthFraction,0) 
                || Mathf.Approximately(healthFraction, 1))
            {
                rootCanvas.enabled = false;
                return;
            }

            rootCanvas.enabled = true;
            foreground.localScale = new Vector3(healthFraction, 1,1);
        }
    }
}
