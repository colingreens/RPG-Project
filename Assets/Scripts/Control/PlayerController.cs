using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                MoveToCursor();
            }
        }

        private void MoveToCursor()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hasHit = Physics.Raycast(ray, out RaycastHit hit);

            if (hasHit)
                GetComponent<Mover>().MoveTo(hit.point);

        }
    }
}
