using RPG.Combat;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Fighter fighter;
        private void Start()
        {
            fighter = GetComponent<Fighter>();
        }

        void Update()
        {
            if (InteractWithCombat())
                return;
            if (InteractWithMovement())
                return;
        }

        private bool InteractWithCombat()
        {
            var hitArray = Physics.RaycastAll(GetMouseRay());
            foreach (var hit in hitArray)
            {
                var target = hit.transform.GetComponent<CombatTarget>();

                if (!fighter.CanAttack(target))
                    continue;

                if (Input.GetMouseButtonDown(0))
                    fighter.Attack(target);

                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            Ray ray = GetMouseRay();
            var hasHit = Physics.Raycast(ray, out RaycastHit hit);

            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point);
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
