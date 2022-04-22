using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RpgAdventure
{
    public class EnemyController : MonoBehaviour, IAttackAnimListener
    {
        private NavMeshAgent navMeshAgent;
        private Animator anim;

        private float speedModifier = 0.7f;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void OnAnimatorMove()
        {
            if (navMeshAgent.enabled)
            {
                navMeshAgent.speed = (anim.deltaPosition / Time.fixedDeltaTime).magnitude * speedModifier;
            }
        }

        public bool FollowTarget(Vector3 position)
        {
            if (!navMeshAgent.enabled)
                navMeshAgent.enabled = true;

            return navMeshAgent.SetDestination(position);
        }

        public void StopFollowTarget()
        {
            navMeshAgent.enabled = false;
        }

        public void MeleeAttackStart(){}

        public void MeleeAttackEnd(){}
    }
}

