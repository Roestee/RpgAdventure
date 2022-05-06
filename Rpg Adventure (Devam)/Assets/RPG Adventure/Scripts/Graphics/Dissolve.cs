using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure
{
    public class Dissolve : MonoBehaviour
    {
        public float dissolveTime = 6;

        private void Awake()
        {
            dissolveTime += Time.time;
        }
        private void Update()
        {
            if(Time.time >= dissolveTime)
            {
                Destroy(gameObject);
            }
        }
    }
}

