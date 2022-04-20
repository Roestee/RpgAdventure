using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure
{
    public class PlayerInput : MonoBehaviour
    {
        private Vector3 movement;
        private bool isAttack;

        public Vector3 MoveInput
        {
            get
            {
                return movement;
            }
        }

        public bool IsMoveInput
        {
            get
            {
                return !Mathf.Approximately(MoveInput.magnitude, 0);
            }
        }

        public bool isAttacking
        {
            get
            {
                return isAttack;
            }
        }

        private void Update()
        {
            movement.Set(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            if (Input.GetButtonDown("Fire1") && !isAttack)
            {
                StartCoroutine(AttackAndWait());
            }
        }

        private IEnumerator AttackAndWait()
        {
            isAttack = true;
            yield return new WaitForSeconds(0.03f);
            isAttack = false;
        }
    }
}

