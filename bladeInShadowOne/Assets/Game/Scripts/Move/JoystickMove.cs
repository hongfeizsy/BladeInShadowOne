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
        }

        void LateUpdate()
        {
            //agent.enabled = !GetComponent<Health>().IsDead();
            //if (inputVec.magnitude > 0.1f) { print(inputVec.magnitude); }
            UpdateAnimator();
            lastPos = currentPos;
        }

        private void UpdateAnimator()
        {
            //Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
            //float speed = MeasureSpeed();
            inputVec = joystick.GetJoystickInputVector();
            float speed = inputVec.magnitude;
            //print(speed);
            //Vector3 velocity = characterController.velocity;
            //Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            //float speed = localVelocity.z;
            if (speed < 10f)
            {
                GetComponent<Animator>().SetFloat("ForwardSpeed", speed);
            }
        }

        private float MeasureSpeed()   // It seems that Character.Velocity does not work.
        {
            currentPos = transform.position;
            Vector3 velocity = (currentPos - lastPos) / Time.deltaTime;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            lastPos = currentPos;
            return Mathf.Abs(localVelocity.z);
        }

        public void StartMoveAction(float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            //if (agent.enabled) { MoveTo(speedFraction); }
            if (characterController.enabled) { MoveTo(speedFraction); }
        }

        private void MoveTo(float speedFraction)
        {
            //Vector3 movement = joystick.GetJoystickInputVector();
            //Vector3 movement = inputVec;
            characterController.Move(inputVec * maxSpeed * speedFraction * Time.deltaTime);
            //print("Character controll velocity: " + characterController.velocity);

            //agent.Move(movement * maxSpeed * speedFraction * Time.deltaTime);
            //agent.updateRotation = true;
            //agent.velocity = movement * maxSpeed * speedFraction;
            //agent.acceleration = 100;
            //agent.isStopped = false;
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

