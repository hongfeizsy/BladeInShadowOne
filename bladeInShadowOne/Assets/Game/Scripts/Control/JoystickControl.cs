using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using RPG.Movement;
using UnityEngine.AI;
using RPG.Resources;
using System;
using RPG.Combat;

namespace RPG.Control
{
    public class JoystickControl : MonoBehaviour
    {
        [SerializeField] GameObject jumpButton;
        [SerializeField] GameObject escapeButton;
        CapsuleCollider capCollider;
        bool willJump = false;

        private void Start()
        {
            capCollider = GetComponent<CapsuleCollider>();
            Physics.gravity = new Vector3(0f, -9.81f, 0f); // This can also be implemented with the method described by Brackeys (First Person Movement in Unity).

            escapeButton.SetActive(true);
            jumpButton.SetActive(false);
        }

        private void Update()
        {
            if (GetComponent<Health>().IsDead()) { return; }

            if (!willJump) 
            {
                escapeButton.SetActive(true);
                jumpButton.SetActive(false);
                if (InteractWithEscape()) { return; } 
            }

            if (willJump)
            {
                escapeButton.SetActive(false);
                jumpButton.SetActive(true);
                if (InteractWithJump())
                {
                    ResetJumpStatus();
                    return;
                }
            }

            if (InteractWithEnemy()) { return; }

            if (InteractWithMovement()) { return; }
        }

        private bool InteractWithEscape()
        {
            JoystickEscape joystickEscape = GetComponent<JoystickEscape>();
            if (joystickEscape.IsEscaping())
            {
                joystickEscape.StartEscapeAction();
                return true;
            }
            return false;
        }

        private bool InteractWithJump()
        {
            JoystickJump joystickJump = GetComponent<JoystickJump>();
            if (joystickJump.IsJumping())
            {
                joystickJump.StartJumpAction();
                return true;
            }
            return false;
        }

        private bool InteractWithEnemy()
        {
            JoystickFight joystickFight = GetComponent<JoystickFight>();
            if (joystickFight.IsAttacking()) 
            {
                joystickFight.StartAttackAction();
                return true; 
            }

            else 
            { 
                return false; 
            }
        }

        private bool InteractWithMovement()
        {
            Vector3 inputVec = GetComponent<JoystickMove>().GetJoystick().GetJoystickInputVector();
            if (inputVec.magnitude > Mathf.Epsilon)
            {
                if (inputVec.magnitude < 0.3f)
                {
                    RotateBody(inputVec);
                    //float targetAngle = GetRotationAngle(new Vector2(0, 1), new Vector2(inputVec.x, inputVec.z));
                    //float currentAngle = transform.eulerAngles.y;
                    //float direction = GetRotationDirection(currentAngle, targetAngle);
                    //if (Mathf.Abs(currentAngle - targetAngle) > 5) { transform.Rotate(new Vector3(0f, direction * 500 * Time.deltaTime, 0)); }
                }
                else
                {
                    RotateBody(inputVec);
                    GetComponent<JoystickMove>().StartMoveAction(1f);
                    //print(inputVec.magnitude);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private void RotateBody(Vector3 inputVec)
        {
            float targetAngle = GetRotationAngle(new Vector2(0, 1), new Vector2(inputVec.x, inputVec.z));
            float currentAngle = transform.eulerAngles.y;
            float direction = GetRotationDirection(currentAngle, targetAngle);
            if (Mathf.Abs(currentAngle - targetAngle) > 5) { transform.Rotate(new Vector3(0f, direction * 500 * Time.deltaTime, 0)); }
        }

        private float GetRotationAngle(Vector2 from, Vector2 to)
        {
            float angle = Vector2.Angle(from, to);
            if (to.x < 0) { angle = 360 - angle; }
            return angle;
        }

        // Positive: clockwise rotation; otherwise: anti-clockwise rotation.
        private float GetRotationDirection(float currentAngle, float targetAngle)
        {
            float diffAngle = currentAngle - targetAngle;
            if (diffAngle >= 180 || (diffAngle > -180 && diffAngle < 0)) { return 1; }
            return -1;
        }

        public Vector3 GetDirectionFacing()
        {
            return transform.position + Vector3.up * capCollider.height / 3 + transform.TransformDirection(Vector3.forward * 10);
        }

        public void SetJumpStatus() { willJump = true; }
        public void ResetJumpStatus() { willJump = false; }
    }
}
