using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure
{
    public class PlayerInput : MonoBehaviour
    {
        public float distanceToInteractWithNpc = 3.0f;

        private Vector3 movement;
        private bool isAttack;
        private bool isTalk;

        public Vector3 MoveInput {get{return movement;}}
        public bool IsMoveInput {get{return !Mathf.Approximately(MoveInput.magnitude, 0);}}
        public bool isAttacking {get{return isAttack;}}
        public bool IsTalk {get{return isTalk;}}

        private void Update()
        {
            movement.Set(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            bool isLeftMouseClick = Input.GetMouseButtonDown(0);
            bool isRightMouseClick = Input.GetMouseButtonDown(1);

            if (isLeftMouseClick)
            {
                HandleLeftMouseBtnDown();

            }

            if (isRightMouseClick)
            {
                HandleRightMouseBtnDown();
            }
        }

        private void HandleLeftMouseBtnDown()
        {
            if (!isAttack)
            {
                StartCoroutine(AttackAndWait());
            }
        }

        private void HandleRightMouseBtnDown()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            bool hasHit = Physics.Raycast(ray, out RaycastHit hit);

            if (hasHit && hit.transform.CompareTag("QuestGiver"))
            {
                var distanceToTarget = (transform.position - hit.transform.position).magnitude;

                if (distanceToTarget <= distanceToInteractWithNpc)
                {
                    isTalk = true;
                }
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

