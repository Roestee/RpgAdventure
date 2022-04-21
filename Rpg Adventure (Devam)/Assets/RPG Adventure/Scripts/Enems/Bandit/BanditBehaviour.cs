using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace RpgAdventure
{
    public class BanditBehaviour : MonoBehaviour
    {
        #region Serializable Variables
        [SerializeField] private float timeToStopPursuit = 2.0f;
        [SerializeField] private float timeToWaitOnPursuit = 2.0f;
        [SerializeField] private float attackDistance = 3f;
        [SerializeField] private float smoothRotation = 3f;

        [SerializeField] private Color m_Color = new Color(0, 0, 0.7f, 0.4f);
        [SerializeField] private PlayerScanner playerScanner;
        #endregion

        #region public Variables
        public bool hasFollowingTarget
        {
            get
            {
                return FollowTarget != null;
            }
        }
        #endregion

        #region private Variables
        private PlayerController FollowTarget;
        private EnemyController enemyController;
        private Animator anim;

        private Vector3 originalPos;
        private Quaternion originalRot;

        private readonly int hashInPursuit = Animator.StringToHash("InPursuit");
        private readonly int hashNearBase= Animator.StringToHash("NearBase");
        private readonly int hashAttack = Animator.StringToHash("Attack");

        private float timeSinceLostTarget = 0;
        #endregion

        #region Unity
        private void Awake()
        {
            enemyController = GetComponent<EnemyController>();
            anim = GetComponent<Animator>();

            originalPos = transform.position;
            originalRot = transform.rotation;
        }

        private void Update()
        {
            GuardPosition();
        }
        #endregion

        #region Functions
        private void GuardPosition()
        {
            var detectedTarget = playerScanner.Detect(this.transform);

            bool hasDetectedTarget = detectedTarget != null;

            if (hasDetectedTarget) { FollowTarget = detectedTarget; }

            if (hasFollowingTarget)
            {
                AttackOrFollowTarget();

                if (hasDetectedTarget)
                {
                    timeSinceLostTarget = 0;
                }
                else
                {
                    StopPursuit();
                }
            }

            CheckIfNearBase();
        }

        private void AttackOrFollowTarget()
        {
            Vector3 toTarget = FollowTarget.transform.position - transform.position;

            if (toTarget.magnitude <= attackDistance)
            {
                AttackTarget(toTarget);
            }
            else
            {
                FollowTheTarget();
            }
        }
        private void AttackTarget(Vector3 toTarget)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(toTarget), smoothRotation * Time.deltaTime);
            enemyController.StopFollowTarget();
            anim.SetTrigger(hashAttack);
        }
        private void FollowTheTarget()
        {
            anim.SetBool(hashInPursuit, true);
            enemyController.FollowTarget(FollowTarget.transform.position);
        }

        private void StopPursuit() 
        {
            timeSinceLostTarget += Time.deltaTime;

            if (timeSinceLostTarget >= timeToStopPursuit)
            {
                FollowTarget = null;
                anim.SetBool(hashInPursuit, false);
                StartCoroutine(WaitBeforeReturn());
            }
        }

        private IEnumerator WaitBeforeReturn()
        {
            yield return new WaitForSeconds(timeToWaitOnPursuit);
            enemyController.FollowTarget(originalPos);
        }

        private void CheckIfNearBase()
        {
            Vector3 toBase = originalPos - transform.position;
            toBase.y = 0;

            bool nearBase = toBase.magnitude < 0.01f;

            anim.SetBool(hashNearBase, nearBase);

            if (nearBase)
            {
                Quaternion targetRotation = Quaternion.RotateTowards(transform.rotation, originalRot, 360 * Time.deltaTime);
                transform.rotation = targetRotation;
            }
        }
        #endregion

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = m_Color;
            Vector3 rotatedForward = Quaternion.Euler(0, -playerScanner.detectionAngle * 0.5f, 0) * transform.forward;
            UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, rotatedForward, playerScanner.detectionAngle, playerScanner.detectionRadius);

            UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, rotatedForward, 360, playerScanner.meleeDetectionRadius);
        }
#endif
    }
}

