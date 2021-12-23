using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.Resources
{
    public class HealthDisplay : MonoBehaviour
    {
        GameObject playerObject;

        private void Awake()
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update()
        {
            GetComponent<Text>().text = string.Format("{0:N0}", playerObject.GetComponent<Health>().GetHealthPercentage()) + "%";
        }
    }
}

