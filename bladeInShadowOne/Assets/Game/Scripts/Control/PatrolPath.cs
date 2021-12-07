using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath: MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(transform.GetChild(i).transform.position, .5f);
                if (i > 0)
                {
                    Gizmos.DrawLine(transform.GetChild(i - 1).transform.position, transform.GetChild(i).transform.position);
                }
                else
                {
                    Gizmos.DrawLine(transform.GetChild(transform.childCount - 1).transform.position, transform.GetChild(i).transform.position);
                }
            }
        }

        public Vector3 GetCurrentWaypointPos(int index)
        {
            return transform.GetChild(index).transform.position;
        }

        public int GetNextWaypointIndex(int index)
        {
            if (index == transform.childCount - 1) 
            { 
                return 0; 
            }
            else 
            { 
                return index + 1; 
            }
        }
    }
}
