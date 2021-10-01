using RPG.Control;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D, E
        }

        [SerializeField] int sceneToLoad = -1;
        [SerializeField] float fadeOutTime = 2f;
        [SerializeField] float fadeInTime = 1f;
        [SerializeField] float fadeWaitTime = 1f;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;

        private PlayerController player;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
                StartCoroutine(Transition());
        }

        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError("Scene to load not set");
                yield break;
            }
            
            DontDestroyOnLoad(gameObject);
            var fader = FindObjectOfType<Fader>();
            var saver = FindObjectOfType<SavingWrapper>();
            player.enabled = false;

            yield return fader.FadeOut(fadeOutTime);
            saver.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            var newPlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            newPlayerController.enabled = false;
            saver.Load();

            var otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            yield return new WaitForSeconds(fadeWaitTime);
            fader.FadeIn(fadeInTime);
            newPlayerController.enabled = enabled;

            saver.Save();

            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            var player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
        }

        private Portal GetOtherPortal()
        {
            foreach (var portal in FindObjectsOfType<Portal>())
            {
                if (portal == this)
                    continue;
                if (portal.destination == this.destination)
                    return portal;
            }
            return null;
        }
    }
}
