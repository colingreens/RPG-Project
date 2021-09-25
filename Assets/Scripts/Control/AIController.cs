using RPG.Combat;
using RPG.Control;
using RPG.Movement;
using System;
using UnityEngine;

namespace RPG.Core
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chasedistance = 5f;
        [SerializeField] float suspicionTime = 5f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointWaitTime = 4f;
        [SerializeField] float wayPointTolerance = 1f;
        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;

        Fighter fighter;
        Health health;
        Mover mover;
        GameObject player;

        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        int currentWayPointIndex = 0;
        float timeSinceArrivedInWayPoint = Mathf.Infinity;

        private void Start()
        {
            fighter = GetComponent<Fighter>();
            player = GameObject.FindGameObjectWithTag("Player");
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();

            guardPosition = transform.position;
        }

        private void Update()
        {
            if (health.IsDead())
                return;

            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
                AttackBehavior();

            else if (timeSinceLastSawPlayer < suspicionTime)
                SuspicionBehavior();

            else
                PatrolBehavior();

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedInWayPoint += Time.deltaTime;
        }

        private void PatrolBehavior()
        {
            var nextPosition = guardPosition;
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

        private bool InAttackRangeOfPlayer()
        {
            return Vector3.Distance(transform.position, player.transform.position) < chasedistance;
        }

        //Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chasedistance);
        }
    }
}
