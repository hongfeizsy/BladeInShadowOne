using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Resources;


namespace RPG.Movement
{
    public class JoystickMove : MonoBehaviour, IAction
    {
        [SerializeField] VirtualJoystick joystick;
        //NavMeshAgent agent;
        CharacterController characterController;
        float maxSpeed;
        Vector3 lastPos;
        Vector3 currentPos;
        Vector3 inputVec;
        Vector3 velocity;
        float gravity;

        void Start()
        {
            //agent = GetComponent<NavMeshAgent>();
            characterController = GetComponent<CharacterController>();
            lastPos = transform.position;
            //agent.enabled = !GetComponent<Health>().IsDead();
            characterController.enabled = !GetComponent<Health>().IsDead();
            characterController.minMoveDistance = 0.01f;
            maxSpeed = 6f;
            inputVec = joystick.GetJoystickInputVector();
            gravity = 9.8f;
        }

        void LateUpdate()
        {
            //agent.enabled = !GetComponent<Health>().IsDead();
            //if (inputVec.magnitude > 0.1f) { print(inputVec.magnitude); }
            UpdateAnimator();
            lastPos = currentPos;
            //if (characterController.isGrounded && velocity.y < 0) { velocity.y = -2f; }
            //velocity.y += gravity * Time.deltaTime;

            //characterController.Move(velocity * Time.deltaTime);
            //Physics.gravity = new Vector3(0f, 0f, 0f);
        }

        private void UpdateAnimator()
        {
            inputVec = joystick.GetJoystickInputVector();
            float speed = inputVec.magnitude;
            if (speed < 10f)
            {
                GetComponent<Animator>().SetFloat("ForwardSpeed", speed);
            }
        }

        public void StartMoveAction(float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            if (characterController.enabled) { MoveTo(speedFraction); }
        }

        private void MoveTo(float speedFraction)
        {
            characterController.Move(inputVec * maxSpeed * speedFraction * Time.deltaTime);
        }

        public void Cancel()
        {
            //agent.isStopped = true;
        }

        public VirtualJoystick GetJoystick()
        {
            return joystick;
        }
    }
}

