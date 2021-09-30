using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    class CinematicControlRemover: MonoBehaviour
    {
        PlayerController playerController;
        GameObject player;
        private void Awake()       
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void OnEnable() {
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        private void OnDisable() {
            GetComponent<PlayableDirector>().played += DisableControl;
        }

        private void DisableControl(PlayableDirector pd)
        {
            print("Disabled Control");

            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            playerController = player.GetComponent<PlayerController>();
            playerController.enabled = false;
        }

        private void EnableControl(PlayableDirector pd)
        {
            print("Enabled Control");

            if (playerController == null)
                return;

            playerController.enabled = true;
        }
    }
}
