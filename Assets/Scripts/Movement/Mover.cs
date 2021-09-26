using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using System.Collections.Generic;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float maxSpeed = 6f;
        [SerializeField] Transform target;
        NavMeshAgent navMeshAgent;
        Health health;

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            navMeshAgent.enabled = !health.IsDead();
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localeVelocity = transform.InverseTransformDirection(velocity);
            float speed = localeVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        [System.Serializable]
        private struct MoverSaveData
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        }

        public object CaptureState()
        {
            return new MoverSaveData
            {
                position = new SerializableVector3(transform.position),
                rotation = new SerializableVector3(transform.eulerAngles)
            };
        }

        public void RestoreState(object state)
        {
            var data = (MoverSaveData) state;
            _ = GetComponent<NavMeshAgent>().Warp(data.position.ToVector());
            transform.eulerAngles = (data.rotation).ToVector();
        }
    }
}
