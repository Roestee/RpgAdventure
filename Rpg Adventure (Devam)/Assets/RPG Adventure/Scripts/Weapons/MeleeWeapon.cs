using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure
{
    public class MeleeWeapon : MonoBehaviour
    {
        public class AttackPoint
        {
            public float radius;
            public Vector3 offset;
            public Transform rootTransform;
        }

        [SerializeField] private int damage = 10;

        public void BeginAttack()
        {

        }
    }
}