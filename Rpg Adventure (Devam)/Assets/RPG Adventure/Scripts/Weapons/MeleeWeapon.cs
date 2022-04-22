using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure
{
    public class MeleeWeapon : MonoBehaviour
    {
        [System.Serializable]
        public class AttackPoint
        {
            public float radius;
            public Vector3 offset;
            public Transform rootTransform;
            public Color color;
        }

        [SerializeField]
        private int damage = 10;

        [SerializeField]
        private LayerMask targetLayers;

        public AttackPoint[] attackPoints = new AttackPoint[0];
        

        private bool isAttack = false;
        private Vector3[] originAttackPositions;
        private RaycastHit[] rayCastHitCache = new RaycastHit[32];
        private GameObject owner;

        private void FixedUpdate()
        {
            if (isAttack)
            {
                for (int i = 0; i < attackPoints.Length; i++)
                {
                    AttackPoint ap = attackPoints[i];
                    Vector3 worldPosition = ap.rootTransform.position + ap.rootTransform.TransformDirection(ap.offset);
                    Vector3 attackVector = (worldPosition - originAttackPositions[i]).normalized;

                    Ray ray = new Ray(worldPosition, attackVector);
                    Debug.DrawRay(worldPosition, attackVector, Color.red, 4.0f);

                    int contacts = Physics.SphereCastNonAlloc(ray, ap.radius, rayCastHitCache, attackVector.magnitude, targetLayers, QueryTriggerInteraction.Ignore);
                   
                    for(int k =0; k < contacts; k++)
                    {
                        Collider collider = rayCastHitCache[k].collider;

                        if(collider != null)
                        {
                            CheckDamage(collider, ap);
                        }
                    }

                    originAttackPositions[0] = worldPosition;
                } 
            }
        }

        private void CheckDamage(Collider collider, AttackPoint ap)
        {
            if ((targetLayers.value & (1 << collider.gameObject.layer)) == 0)
            {
                return;
            }

            Damageable damageable = collider.GetComponent<Damageable>();

            if(damageable != null)
            {
                Damageable.DamageMessage data;
                data.amount = damage;
                data.damager = this;
                data.damageSource = owner.transform.position;
                damageable.ApplyDamage(data);
            }
        }

        public void SetOwner(GameObject owner)
        {
            this.owner = owner;
        }

        public void BeginAttack()
        {
            isAttack = true;
            originAttackPositions = new Vector3[attackPoints.Length];

            for (int i = 0; i < attackPoints.Length; i++)
            {
                AttackPoint ap = attackPoints[i];
                originAttackPositions[i] = ap.rootTransform.position + ap.rootTransform.TransformDirection(ap.offset);          
            }
        }

        public void EndAttack()
        {
            isAttack = false;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            foreach (AttackPoint attackPoint in attackPoints)
            {
                if(attackPoint.rootTransform != null)
                {
                    Vector3 worldPosition = attackPoint.rootTransform.TransformVector(attackPoint.offset);
                    Gizmos.color = attackPoint.color;
                    Gizmos.DrawSphere(attackPoint.rootTransform.position + worldPosition, attackPoint.radius);
                }
            }
        }
#endif
    }
}