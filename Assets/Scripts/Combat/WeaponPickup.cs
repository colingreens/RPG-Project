using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon;
        private void OnTriggerEnter(Collider other)
        {
            print("Player has entered pickup");
            if (other.tag == "Player")
            {                
                other.GetComponent<Fighter>().EquipWeapon(weapon);
                Destroy(gameObject);
            }
        }
    }
}
