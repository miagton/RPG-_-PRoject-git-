
using UnityEngine;
using System;
using UnityEngine.UI;
using RPG.Attributes;

namespace RPG.Combat
{
    public class EnemyHealthDIsplay : MonoBehaviour
    {
        Health health;
        Fighter fighter;
        private void Awake()
        {
            fighter = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();
        }
        private void Update()
        {
            if(fighter.GetTarget()== null)
            {
                GetComponent<Text>().text = "N/A";
                return;
            }
            health = fighter.GetTarget();
            GetComponent<Text>().text = String.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints()); // formating the health display to not show decimals
        }
    }
}


