using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using UnityStandardAssets.CrossPlatformInput;
using RPG.Resources;

namespace RPG.Combat
{
    public class JoystickJump : MonoBehaviour, IAction
    {
        [SerializeField] float jumpHeight = 3f;
        CharacterController characterController;
        float ifJump;
        bool isGrounded;
        bool isJumping = false;
        float gravityValue = -9.81f;
        Vector3 playerVelocity;
        float timeSinceJump = 0f;
        float timeToFinishJumpAnimation = 1f; // To be adjusted due to GetComponent<Animator>().SetFloat("RunMultiplier", 1f);

        // Start is called before the first frame update
        void Start()
        {
            characterController = GetComponent<CharacterController>();
            characterController.enabled = !GetComponent<Health>().IsDead();
            characterController.minMoveDistance = 0.01f;
            isGrounded = characterController.isGrounded;
        }

        // Update is called once per frame
        void Update()
        {
            if (isJumping)
            {
                isGrounded = characterController.isGrounded;
                if (isGrounded && playerVelocity.y < 0)
                {
                    playerVelocity.y = 0f;
                }

                playerVelocity.y += gravityValue * Time.deltaTime;
                characterController.Move(playerVelocity * Time.deltaTime);

                timeSinceJump += Time.deltaTime;
                if (timeSinceJump >= timeToFinishJumpAnimation)
                {
                    isJumping = false;
                    timeSinceJump = 0;
                    Cancel();
                }
            }
        }

        public bool IsJumping()
        {
            ifJump = CrossPlatformInputManager.VirtualAxisReference("Jump").GetValue;
            if (ifJump > Mathf.Epsilon)
            {
                isGrounded = characterController.isGrounded;
                if (isGrounded)
                {
                    isJumping = true;
                    return true;
                }
                return false;
            }

            else { return false; }
        }

        public void StartJumpAction()
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
            characterController.Move(playerVelocity * Time.deltaTime);
        }

        public void Cancel()
        {

        }
    }
}
