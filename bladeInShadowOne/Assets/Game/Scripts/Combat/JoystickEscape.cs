using RPG.Core;
using RPG.Movement;
using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.CrossPlatformInput;

namespace RPG.Combat
{
    public class JoystickEscape : MonoBehaviour, IAction
    {
        float ifEscape;
        bool isEscaping = false;
        float timeSinceEscape = 0f;
        float timeToFinishEscapeAnimation = 1f; // To be adjusted due to GetComponent<Animator>().SetFloat("RunMultiplier", 1f);

        private void Update()
        {
            // This is to prevent from escaping twice due to Blend Tree.
            if (isEscaping)
            {
                timeSinceEscape += Time.deltaTime;
                if (timeSinceEscape >= timeToFinishEscapeAnimation)
                {
                    isEscaping = false;
                    timeSinceEscape = 0;
                    Cancel();
                }
            }
        }

        public bool IsEscaping()
        {
            ifEscape = CrossPlatformInputManager.VirtualAxisReference("Escape").GetValue;
            if (ifEscape > Mathf.Epsilon)
            {
                isEscaping = true;
                return true;
            }
            else { return false; }
        }

        public void StartEscapeAction()
        {
            GetComponent<ActionScheduler>().StartAction(this);
            GetComponent<Animator>().ResetTrigger("StopEscape");
            GetComponent<Animator>().SetTrigger("Escape");
            GetComponent<Animator>().SetFloat("RunMultiplier", 1.2f);
            //GetComponent<NavMeshAgent>().height = 0.0f;
            //GetComponent<NavMeshAgent>().enabled = false;
        }

        public void Cancel()
        {
            GetComponent<Animator>().ResetTrigger("Escape");
            GetComponent<Animator>().SetTrigger("StopEscape");
            //GetComponent<NavMeshAgent>().height = 2f;
            //GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}
