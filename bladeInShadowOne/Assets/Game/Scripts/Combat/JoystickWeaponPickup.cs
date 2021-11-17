using System.Collections;
using UnityEngine;
using RPG.Control;


namespace RPG.Combat
{
    public class JoystickWeaponPickup : MonoBehaviour
    {
        [SerializeField] JoystickWeapon joystickWeapon = null;
        //[SerializeField] float respawnTime = 5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player" && joystickWeapon != null)
            {
                //Pickup(other.GetComponent<Fighter>());
                PickUp(other.GetComponent<JoystickFight>());
            }
        }

        private void PickUp(JoystickFight joystickFight)
        {
            joystickFight.EquipWeapon(joystickWeapon);
            HidePickup();
        }

        //private void Pickup(JoystickFight joystickFight)
        //{
        //    joystickFight.EquipWeapon(joystickWeapon);
        //    StartCoroutine(HideForSeconds(respawnTime));
        //}

        private IEnumerator HideForSeconds(float respawnTime)
        {
            HidePickup();
            yield return new WaitForSeconds(respawnTime);
            ShowPickup();
        }

        private void HidePickup()
        {
            GetComponent<Collider>().enabled = false;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        private void ShowPickup()
        {
            GetComponent<Collider>().enabled = true;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }

    }
}

