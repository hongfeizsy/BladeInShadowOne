using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Stats;
using RPG.Resources;
using RPG.Combat;


namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText damageTextPrefab = null;
        
        public void Spawn()
        {
            DamageText textObject = Instantiate(damageTextPrefab, transform.position, Quaternion.identity);
            textObject.transform.parent = transform;
            float damageAmount = GetComponentInParent<Health>().GetDamageAmount();
            //print("Text Spawner: " + damageAmount);
            textObject.transform.GetChild(0).GetComponentInChildren<Text>().text = string.Format("{0:0}", damageAmount);
        }
    }
}
