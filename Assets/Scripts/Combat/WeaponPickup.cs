using RPG.Control;
using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRayCastable
    {
        [SerializeField] WeaponConfig weapon;
        [SerializeField] float respawnTime = 5f;

        private void OnTriggerEnter(Collider other)
        {
            print("Player has entered pickup");
            if (other.tag == "Player")
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                    Pickup(other.GetComponent<InventoryHandler>());
            }
        }

        private void Pickup(InventoryHandler inventory)
        {
            //fighter.EquipWeapon(weapon);
            inventory.AddToInventory(weapon);
            StartCoroutine(HideForSeconds(respawnTime));
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;
            foreach (Transform item in transform)
            {
                item.gameObject.SetActive(shouldShow);
            }
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
                Pickup(callingController.GetComponent<InventoryHandler>());
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.PickupWeapon;
        }
    }
}
