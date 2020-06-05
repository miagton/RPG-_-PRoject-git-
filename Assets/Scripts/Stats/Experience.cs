using GameDevTV.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour,ISaveable
    {
        [SerializeField] float experiencePoints = 0;

        
        public event Action onExperienceGained;
        
        public void GainExperience(float xpGained)
        {
            experiencePoints += xpGained;
            onExperienceGained();
        }
        public object CaptureState()
        {
            return experiencePoints;
        }
        public float GetXPValue()
        {
            return experiencePoints;
        }
       

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
    }
}
