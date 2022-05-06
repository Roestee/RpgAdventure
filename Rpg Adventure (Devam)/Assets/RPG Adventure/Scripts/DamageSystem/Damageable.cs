using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure
{
    public partial class Damageable : MonoBehaviour
    {
        [Range(0.0f, 360.0f)]
        public float hitAngle = 360.0f;
        public float invulnerabilityTime = 0.3f;
        public int maxHitPoints;

        public int currentHitPoints { get; private set; }

        public List<MonoBehaviour> onDamageMessageReceivers;

        private bool isInvulnerable = false;
        private float timeSinceLastHit = 0.0f;

        private void Awake()
        {
            currentHitPoints = maxHitPoints;
        }

        private void Update()
        {
            if (isInvulnerable)
            {
                timeSinceLastHit += Time.deltaTime;
                if(timeSinceLastHit >= invulnerabilityTime)
                {
                    isInvulnerable = false;
                    timeSinceLastHit = 0.0f;
                }
            }
        }

        public void ApplyDamage(DamageMessage data)
        {
            if (currentHitPoints <= 0 || isInvulnerable)
                return;

            Vector3 positionToDamager = data.damageSource - transform.position;
            positionToDamager.y = 0;

            if (Vector3.Angle(transform.forward, positionToDamager) > hitAngle * 0.5f){
                return;
            }

            isInvulnerable = true;
            currentHitPoints -= data.amount;

            var messageType = currentHitPoints <= 0 ? MessageType.DEAD : MessageType.DAMAGED;

            for (int i = 0; i < onDamageMessageReceivers.Count; i++)
            {
                var receiver = onDamageMessageReceivers[i] as IMessageReceiver;
                receiver.OnReceiveMessage(messageType);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = new Color(0.0f, 0.0f, 1.0f, 0.5f);
            Vector3 rotatedForward = Quaternion.AngleAxis(-hitAngle * 0.5f, transform.up) * transform.forward;
            UnityEditor.Handles.DrawSolidArc(transform.position, transform.up, rotatedForward, hitAngle, 1.0f);
        }
#endif
    }
}
