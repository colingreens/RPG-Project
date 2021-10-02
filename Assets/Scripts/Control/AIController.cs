using GameDevTV.Utils;
using RPG.Attributes;
using RPG.Combat;
using RPG.Control;
using RPG.Movement;
using UnityEngine;

namespace RPG.Core
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float aggrevatedTime = 4f;
        [SerializeField] float chasedistance = 5f;
        [SerializeField] float suspicionTime = 5f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointWaitTime = 4f;
        [SerializeField] float wayPointTolerance = 1f;
        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;

        private Fighter fighter;
        private Health health;
        private Mover mover;
        private GameObject player;

        private LazyValue<Vector3> guardPosition;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private int currentWayPointIndex = 0;
        private float timeSinceArrivedInWayPoint = Mathf.Infinity;
        private float timeSinceAggrevated = Mathf.Infinity;

        private void Awake() {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");
            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
        }

        private void Start()
        {
            guardPosition.ForceInit();
        }

        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }

        private void Update()
        {
            if (health.IsDead())
                return;

            if (IsAggrevated() && fighter.CanAttack(player))
                AttackBehavior();

            else if (timeSinceLastSawPlayer < suspicionTime)
                SuspicionBehavior();

            else
                PatrolBehavior();

            UpdateTimers();
        }

        public void Aggrevate()
        {
            timeSinceAggrevated = 0f;
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedInWayPoint += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;
        }

        private void PatrolBehavior()
        {
            var nextPosition = guardPosition.value;
            if (patrolPath != null)
            {
                if (AtWayPoint())
                {
                    timeSinceArrivedInWayPoint = 0;
                    CycleWayPoint();
                }
                nextPosition = GetCurrentWayPoint();
            }

            if (timeSinceArrivedInWayPoint > waypointWaitTime)
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);

        }

        private bool AtWayPoint()
        {
            var distance = Vector3.Distance(transform.position, GetCurrentWayPoint());
            return distance < wayPointTolerance;
        }

        private void CycleWayPoint()
        {
            currentWayPointIndex = patrolPath.GetNextIndex(currentWayPointIndex);
        }

        private Vector3 GetCurrentWayPoint()
        {
            return patrolPath.GetWayPoint(currentWayPointIndex);
        }

        private void SuspicionBehavior()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehavior()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
        }

        private bool IsAggrevated()
        {
            return Vector3.Distance(transform.position, player.transform.position) < chasedistance ||
                timeSinceAggrevated < aggrevatedTime;
        }

        //Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chasedistance);
        }
    }
}
