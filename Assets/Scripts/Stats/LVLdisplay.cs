using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class LVLdisplay : MonoBehaviour
    {
        BaseStats stats;
        private void Awake()
        {
            stats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
           
        }
        void Update()
        {
            GetComponent<Text>().text = String.Format("{0:0}", stats.GetLevel());
        }
    }
}
