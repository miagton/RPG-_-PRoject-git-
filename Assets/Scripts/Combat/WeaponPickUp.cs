using RPG.Attributes;
using RPG.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickUp : MonoBehaviour,IRaycastable
    {
        [SerializeField] WeaponConfig Weapon;
        [SerializeField] float respawnTIme = 10f;
        [SerializeField] float healthToRestore = 0;

        Transform player;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.name == "Player")
            {
                PickUp(other.gameObject);
            }
        }

        private void PickUp(GameObject player)
        {
            if (Weapon != null)
            {
                player.GetComponent<Fighter>().EquipWeapon(Weapon);
            }
            if(healthToRestore > 0)
            {
                player.GetComponent<Health>().Heal(healthToRestore);
            }

            // Destroy(this.gameObject);
            StartCoroutine(HideForSeconds(respawnTIme));
        }

        private IEnumerator HideForSeconds(float time)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(time);
            ShowPickup(true);
        }

        private void ShowPickup(bool ShouldShow)
        {
            GetComponent<Collider>().enabled = ShouldShow;
                       
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(ShouldShow);
            }
        }

        public bool HandleRaycast(PlayerController callingController)
        {

            
             if (Input.GetMouseButtonDown(0))//&& Vector3.Distance(transform.position,player.position) <=5)
                {
                PickUp(callingController.gameObject);
                }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.PickUp;
        }
    }
}
