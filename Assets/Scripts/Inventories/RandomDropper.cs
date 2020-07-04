using GameDevTV.Inventories;
using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Inventories
{
    public class RandomDropper : ItemDropper
    {
        //config data
        [Tooltip("How far pickups drop from Dropper")]
        [SerializeField] float scatterDistance = 1f;
        [SerializeField] DropLibrary DropItems;
        


        //constants
        const int Attempts = 30;
        
       
        
        public void RandomDrop()
        {
            var basestats = GetComponent<BaseStats>();
           
            var drops = DropItems.GetRandomDrops(basestats.GetLevel());
             foreach(var drop in drops)
             {
                DropItem(drop.item, drop.number);                

             }
            

           
        }
        
        protected override Vector3 GetDropLocation()
        {
            
            for(int i = 0; i < Attempts; i++)
            {
                          
              Vector3 randomPoint = transform.position + Random.insideUnitSphere * scatterDistance;
              NavMeshHit hit;
              if(NavMesh.SamplePosition(randomPoint,out hit, 0.1f, NavMesh.AllAreas))
              {
                return hit.position;

              }

            }
            return transform.position;
        }
    }

}