using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Resources;
using RPG.Control;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour
        //, IRaycastable
    {
        //public CursorType GetCursorType()
        //{
        //    return CursorType.Fight;
        //}

        //public bool HandleRaycast(PlayerControler playerController)
        //{
        //    RaycastHit[] hits = Physics.RaycastAll(PlayerControler.GetMouseRay());
        //    foreach (RaycastHit hit in hits)
        //    {
        //        if (!playerController.GetComponent<Fighter>().CanAttack(this)) { continue; }
        //        if (Input.GetMouseButtonDown(0))
        //        {
        //            playerController.GetComponent<Fighter>().Attack(this);
        //        }
        //        return true;
        //    }
        //    return false;
        //}
    }

}
