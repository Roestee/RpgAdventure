using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace RpgAdventure
{
    public class CameraController : MonoBehaviour
    {      
        [SerializeField]
        private CinemachineFreeLook freeLookCamera;

        public CinemachineFreeLook PlayerCam
        {
            get
            {
                return freeLookCamera;
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                freeLookCamera.m_XAxis.m_MaxSpeed = 2000;
                freeLookCamera.m_YAxis.m_MaxSpeed = 20;
            }
            if (Input.GetMouseButtonUp(1))
            {
                freeLookCamera.m_XAxis.m_MaxSpeed = 0;
                freeLookCamera.m_YAxis.m_MaxSpeed = 0;
            }
        }
    }
}

