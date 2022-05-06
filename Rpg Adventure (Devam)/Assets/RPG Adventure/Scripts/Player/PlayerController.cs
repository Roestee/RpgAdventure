using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure
{
    public class PlayerController : MonoBehaviour, IAttackAnimListener
    {
        public InventoryObject inventory;

        public static PlayerController instance
        {
            get
            {
                return s_Instance;
            }
        }

        [SerializeField]
        private float maxForwardSpeed = 8.0f;

        [SerializeField]
        private float gravity = -9.81f;

        [SerializeField]
        private MeleeWeapon meleeWeapon;

        private CameraController cameraController;
        private Animator anim;
        private PlayerInput playerInput;
        private CharacterController characterController;

        private Quaternion m_TargetRotation;

        private readonly int hashForwardSpeed = Animator.StringToHash("ForwardSpeed");
        private readonly int hashMeleeAttack = Animator.StringToHash("MeleeAttack");

        private readonly int maxRotationSpeed = 1200;
        private readonly int minRotationSpeed = 800;

        const float k_Acceleration = 35;
        const float k_Deceleration = 500;

        private static PlayerController s_Instance;

        private float desiredForwardSpeed;
        private float forwardSpeed;
        private float verticalSpeed;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            anim = GetComponent<Animator>();
            cameraController = Camera.main.GetComponent<CameraController>();
            characterController = GetComponent<CharacterController>();
            s_Instance = this;
            meleeWeapon.SetOwner(gameObject);
        }

        private void FixedUpdate()
        {
            ComputeForwardMovement();
            ComputeVerticalMovement();
            ComputeRotation();

            if (playerInput.IsMoveInput)
            {
                float rotationSpeed = Mathf.Lerp(maxRotationSpeed, minRotationSpeed, forwardSpeed / desiredForwardSpeed);
                m_TargetRotation = Quaternion.RotateTowards(transform.rotation, m_TargetRotation, 400 * Time.fixedDeltaTime);
                transform.rotation = m_TargetRotation;  
            }

            anim.ResetTrigger(hashMeleeAttack);

            if (playerInput.isAttacking)
            {
                anim.SetTrigger(hashMeleeAttack);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var item = other.GetComponent<Item>();
            if (item)
            {
                inventory.AddItem(item.item, 1);
                Destroy(other.gameObject);
            }
        }
        private void OnApplicationQuit()
        {
            inventory.Container.Clear();
        }

        public void MeleeAttackStart()
        {
            meleeWeapon.BeginAttack();
        }

        public void MeleeAttackEnd()
        {
            meleeWeapon.EndAttack();
        }

        private void OnAnimatorMove()
        {
            Vector3 movement = anim.deltaPosition;
            movement += verticalSpeed * Vector3.up * Time.fixedDeltaTime;
            characterController.Move(movement);
        }

        private void ComputeForwardMovement()
        {
            Vector3 moveInput = playerInput.MoveInput.normalized;

            desiredForwardSpeed = moveInput.magnitude * maxForwardSpeed;

            float acceleration = playerInput.IsMoveInput ? k_Acceleration : k_Deceleration;

            forwardSpeed = Mathf.MoveTowards(forwardSpeed, desiredForwardSpeed, acceleration * Time.fixedDeltaTime);

            anim.SetFloat(hashForwardSpeed, forwardSpeed);       
        }

        private void ComputeVerticalMovement()
        {
            verticalSpeed = gravity;
        }

        private void ComputeRotation()
        {
            Vector3 moveInput = playerInput.MoveInput.normalized;
            Vector3 camDirection = Quaternion.Euler(0, cameraController.PlayerCam.m_XAxis.Value, 0) * Vector3.forward;
            Quaternion targetRotation;

            if (Mathf.Approximately(Vector3.Dot(moveInput,Vector3.forward),-1.0f))
            {
                targetRotation = Quaternion.LookRotation(-camDirection);
            }
            else
            {
                Quaternion movementRotation = Quaternion.FromToRotation(Vector3.forward, moveInput);
                targetRotation = Quaternion.LookRotation(movementRotation * camDirection);
            }

            m_TargetRotation = targetRotation;
        }

    }

}
