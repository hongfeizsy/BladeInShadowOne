using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using UnityStandardAssets.CrossPlatformInput;

namespace RPG.Combat
{
    public class JoystickJump : MonoBehaviour, IAction
    {
        CharacterController characterController;
        float ifJump;
        bool isGrounded;
        bool isJumping = false;

        // Start is called before the first frame update
        void Start()
        {
            characterController = GetComponent<CharacterController>();
            isGrounded = characterController.isGrounded;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool IsJumping()
        {
            ifJump = CrossPlatformInputManager.VirtualAxisReference("Jump").GetValue;
            if (ifJump > Mathf.Epsilon)
            {
                isJumping = true;
                return true;
            }

            else { return false; }
        }

        public void StartJumpAction()
        {
            
        }

        public void Cancel()
        {

        }
    }
}
