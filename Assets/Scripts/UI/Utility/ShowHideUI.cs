using UnityEngine;

namespace RPG.UI.Utility
{
    public class ShowHideUI : MonoBehaviour
    {
        [SerializeField] KeyCode toggleKey = KeyCode.I;
        [SerializeField] GameObject uiContainer = null;

        // Start is called before the first frame update
        void Start()
        {
            uiContainer.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                uiContainer.SetActive(!uiContainer.activeSelf);
            }
        }
    }
}