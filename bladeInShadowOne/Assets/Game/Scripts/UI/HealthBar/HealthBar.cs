using RPG.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Image healthBarForeground;
        [SerializeField] GameObject canvas;

        void Update()
        {
            Health health = GetComponentInParent<Health>();
            bool isDead = health.IsDead();
            if (!isDead)
            {
                canvas.SetActive(true);
                healthBarForeground.transform.localScale 
                    = new Vector3(health.GetHealthPercentage() / 100, 1, 1);
            }
            else
            {
                canvas.SetActive(false);
                //gameObject.SetActive(false);
            }
        }
    }
}
