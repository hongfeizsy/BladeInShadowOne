using RPG.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class WeaponComponent : MonoBehaviour
    {
        [SerializeField] bool isDisposable = false;

        public bool GetIsDisposable()
        {
            return isDisposable;
        }
    }
}
