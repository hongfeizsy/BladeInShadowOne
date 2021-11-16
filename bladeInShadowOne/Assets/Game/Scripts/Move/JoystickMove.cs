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
        NavMeshAgent agent;
        float maxSpeed;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            maxSpeed = 4.5f;
        }

        void LateUpdate()
        {
            agent.enabled = !GetComponent<Health>().IsDead();
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("ForwardSpeed", speed);
        }

        public void StartMoveAction(float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            if (agent.enabled) { MoveTo(speedFraction); }
        }

        private void MoveTo(float speedFraction)
        {
            Vector3 movement = joystick.GetJoystickInputVector();
            agent.Move(movement * maxSpeed * speedFraction * Time.deltaTime);
            agent.updateRotation = true;
            agent.velocity = movement * maxSpeed * speedFraction;
            //agent.destination = agent.pathEndPosition;
            agent.acceleration = 100;
            agent.isStopped = false;
        }

        public void Cancel()
        {
            agent.isStopped = true;
        }

        public VirtualJoystick GetJoystick()
        {
            return joystick;
        }
    }
}

