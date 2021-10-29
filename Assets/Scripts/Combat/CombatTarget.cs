using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRayCastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            //if (!callingController.GetComponent<Fighter>().CanAttack(gameObject))
            //    return false;

            if (Input.GetMouseButtonDown(0))
                callingController.GetGameObject().GetComponent<Fighter>().Attack(gameObject);

            return true;
        }
    }
}
