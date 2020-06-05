using GameDevTV.Inventories;
using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
   [RequireComponent(typeof(Pickup))]
    public class ClickablePickup : MonoBehaviour,IRaycastable
    {

        Pickup pickup;
       
        void Awake()
        {
            pickup = GetComponent<Pickup>();
        }

       

        public CursorType GetCursorType()
        {
            if (pickup.CanBePickedUp())
            {
                return CursorType.PickUp;
            }
            else
            {
                return CursorType.None;
            }
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                pickup.PickupItem();
            }
            return true;
        }
    }
}
