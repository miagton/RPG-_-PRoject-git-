using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace RPG.Stats
{
    public class XPdisplay : MonoBehaviour
    {
        Experience experience;
        private void Awake()
        {
            experience = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
        }
        private void Update()
        {
            GetComponent<Text>().text = String.Format("{0:0}", experience.GetXPValue());// formating the health display to not show decimals
        }
    }
}
