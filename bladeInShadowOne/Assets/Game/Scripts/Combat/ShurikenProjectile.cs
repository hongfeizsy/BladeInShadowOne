using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class ShurikenProjectile : MonoBehaviour
    {
        Transform childObjectTransform;
        float rotationSpeed;
        float sizeIncreaseSpeed;

        void Start()
        {
            rotationSpeed = 50f;
            sizeIncreaseSpeed = 0.02f;
            childObjectTransform = transform.GetChild(0).GetComponent<Transform>();
        }

        void Update()
        {
            childObjectTransform.Rotate(new Vector3(rotationSpeed, 0, 0));
            transform.localScale += new Vector3(1, 1, 1) * sizeIncreaseSpeed;
        }
    }
}
