using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] UnityEvent onHit;
        [SerializeField] UnityEvent onAttack;
        [SerializeField] Transform Trailposition= null;
        [SerializeField] GameObject weaponTrail = null;
        public void OnHit() 
        {
           // print("weapon hit!" + gameObject.name);
            

            onHit.Invoke();

        }

        public void OnAttack()
        {
            if (weaponTrail == null) return;
            Instantiate(weaponTrail, Trailposition);
            onAttack.Invoke();
        }
    }
}
