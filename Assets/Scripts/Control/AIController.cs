using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using RPG.Attributes;
using GameDevTV.Utils;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 6f;
        [SerializeField] float suspicisTIme = 3f;
        [SerializeField] float agroTime = 3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float WayPointTolerance = 1f;
        [SerializeField] float WayPointDwellTime = 3f;
       //[SerializeField] float patrolSpeed = 1.5f;
        //[SerializeField] float chaseSpeed = 3.5f;
        [Range(0,1)][SerializeField] float patrolSpeedFraction = 0.2f;
        [SerializeField] float shoutDistance = 10f;




        GameObject target;
        Health health;
        Fighter fighter;
        Mover mover;
       
        int currentWaypointIndex = 0;
        
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceAriveAtWaypoint = Mathf.Infinity;
        float timeSinceAggrovated = Mathf.Infinity;

       LazyValue <Vector3> guardPosition;

        private void Awake()
        {
            target = GameObject.FindGameObjectWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            
            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
        }
        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }
        private void Start()
        {
            guardPosition.ForceInit();
        }
        private void Update()
        {
            if (health.IsDead()) { return; }
          
            if (IsAgrovated() && fighter.CanAttack(target))
            {
                //GetComponent<NavMeshAgent>().speed = chaseSpeed;
                AttackBehaviour();
            }
            // else if(!InAttackRangeOfPlayer()&&  timeSinceLastSawPlayer <3) 
            else if (timeSinceLastSawPlayer < suspicisTIme)
            {
                SuspiciousBehaviour();
            }
            else
            {
                // fighter.Cancel();
                PatrolBehaviour();
            }
            UpdateTImers();
        }

        public void Aggrovate()
        {
            timeSinceAggrovated = 0;
        }


        private void UpdateTImers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceAriveAtWaypoint += Time.deltaTime;
            timeSinceAggrovated += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            //GetComponent<NavMeshAgent>().speed = patrolSpeed;
            Vector3 nextPosition = guardPosition.value;
            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceAriveAtWaypoint = 0;
                    CycleWaypoint();
                }
               
                nextPosition = GetCurrentWaypoint();
            }

            if (timeSinceAriveAtWaypoint > WayPointDwellTime)
            {
                mover.StartMoveAction(nextPosition,patrolSpeedFraction);   // starting 1 action automatically stopes otehr action in ActionSchedular
            }
                
                
            
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < WayPointTolerance;
        }
        private Vector3 GetCurrentWaypoint()
        {
            
           
                return patrolPath.GetWaypoint(currentWaypointIndex);
           
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
            
        }

       

        private void SuspiciousBehaviour()
        {
            GetComponent<ActionSchedular>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(target);
            AggroNearEnemies();
        }

        private bool IsAgrovated()
        {
            
            float distanceToPlayer = Vector3.Distance(transform.position, target.transform.position);

            return distanceToPlayer < chaseDistance || timeSinceAggrovated< agroTime;
        }

        private void AggroNearEnemies()
        {
          RaycastHit[] hits=  Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach(var enemy in hits)
            {
                AIController ai = enemy.collider.GetComponent<AIController>();
                if (ai == null) continue;
                ai.Aggrovate();
            }
        }

        //called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
