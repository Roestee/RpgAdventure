using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure
{
    public class UniqueID : MonoBehaviour
    {
        [SerializeField]
        private string m_uid = Guid.NewGuid().ToString();

        public string Uid { get { return m_uid; } }
    }
}

