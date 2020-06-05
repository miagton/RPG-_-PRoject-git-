
using RPG.Core;
using GameDevTV.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Attributes;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour,IAction,ISaveable
    {

        [SerializeField] private Transform _target;
        [SerializeField] float MaxSpeed = 6f;
        [SerializeField] float maxNavPathLenght = 40f;

        NavMeshAgent agent;
        Health health;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }
        

        void Update()
        {
            agent.enabled = !health.IsDead();// Enableing NavMeshAgent if we are not dead 
            
           UpdateAnimator();
        }

       public void StartMoveAction(Vector3 destination,float speedFraction)
        {
            GetComponent<ActionSchedular>().StartAction(this);
            
            MoveTo(destination,speedFraction);
            
        }
       
        public bool CanMoveTo(Vector3 destination)
        {
            // Upgrading our Navmesh Path finding
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLenght(path) > maxNavPathLenght) return false;

            return true;
        }
        public void MoveTo(Vector3 destination,float speedFraction)
        {

            agent.speed = MaxSpeed * Mathf.Clamp01(speedFraction);
            agent.destination = destination;
            agent.isStopped = false;
        }
        public void Cancel()
        {
            agent.isStopped = true;
        }
        

       
        private void UpdateAnimator()
        {
            Vector3 velocity = agent.velocity;//getting GLOBAL velocity
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);//converting to Local velocity
            float speed = localVelocity.z;// geting velocity by Z = forward velocity
            GetComponent<Animator>().SetFloat("ForwardSpeed", speed);
        }

        public object CaptureState()
        {
            return new SerializableVector3( transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }
        private float GetPathLenght(NavMeshPath path)// calculating NavMeshPAth lenght
        {
            float total = 0;
            if (path.corners.Length < 2) return total;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return total;
        }
    }
}
