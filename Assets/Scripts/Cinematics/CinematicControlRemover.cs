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
        private void Start()
        {
            GetComponent<PlayableDirector>().stopped += EnableControl;
            GetComponent<PlayableDirector>().played += DisableControl;

            player = GameObject.FindGameObjectWithTag("Player");
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
            playerController.enabled = true;
        }
    }
}
