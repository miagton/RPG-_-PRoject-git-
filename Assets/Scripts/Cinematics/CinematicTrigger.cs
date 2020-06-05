using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        bool istriggered;
        private void OnTriggerEnter(Collider other)
        {
           if(other.gameObject.tag=="Player" && !istriggered)
            {
                GetComponent<PlayableDirector>().Play();
                istriggered = true;
            }
            
        }
    }
}
