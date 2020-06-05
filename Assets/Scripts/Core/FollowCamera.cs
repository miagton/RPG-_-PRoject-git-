using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField]
        GameObject target;
        private void Awake()
        {
            target = GameObject.Find("Player");
        }

        // Update is called once per frame
        void LateUpdate()
        {
            //Camera.main.transform.position = target.transform.position;
            transform.position = target.transform.position;
        }
    }
}
