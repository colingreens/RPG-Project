using RPG.Attributes;
using RPG.Combat;
using RPG.Movement;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public partial class PlayerController : MonoBehaviour
    {
        [SerializeField] CursorMapping[] cursorMappings = null;

        private Fighter fighter;
        private Health health;

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotSpot;
        }

        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            if (InteractWithUI())
                return;

            if (health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }

            if (InteractWithComponent())
                return;

            if (InteractWithMovement())
                return;

            SetCursor(CursorType.None);
        }

        public void SetCursor(CursorType type)
        {
            var mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotSpot, CursorMode.Auto);
        }

        private bool InteractWithComponent()
        {
            var hits = RaycastAllSorted();
            foreach (var hit in hits)
            {
                var raycastables = hit.transform.GetComponents<IRayCastable>();
                foreach (var raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        private RaycastHit[] RaycastAllSorted()
        {
            var hits = Physics.RaycastAll(GetMouseRay());

            var distances = new float[hits.Length];

            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }

            Array.Sort(distances, hits);

            return hits;
        }

        private bool InteractWithUI()
        {
            if(EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            var ray = GetMouseRay();
            var hasHit = Physics.Raycast(ray, out RaycastHit hit);

            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point, 1f);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }


        private CursorMapping GetCursorMapping(CursorType type)
        {
            return cursorMappings.Where(x => x.type == type).FirstOrDefault();
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
