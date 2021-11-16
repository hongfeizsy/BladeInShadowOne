using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        [SerializeField] IAction currentAction;    // I do not know why [SerialField? doesn't work.

        public void StartAction(IAction action)
        {
            if (currentAction == action) return;
            //currentAction = action;
            if (currentAction != null)
            {
                currentAction.Cancel();
            }
            currentAction = action;
            //print(currentAction);
        }

        public IAction GetCurrentAction()
        {
            return currentAction;
        }

        public void CancelCurrentAction()
        {
            StartAction(null);
        }
    }
}

