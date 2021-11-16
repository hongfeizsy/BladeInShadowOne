using RPG.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "JoystickWeapon", menuName = "JoystickWeapons/Make New JoystickWeapon", order = 0)]
    public class JoystickWeapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController weaponOverride;
        [SerializeField] WeaponComponent weaponPrefab = null;
        [SerializeField] float weaponRange;
        [SerializeField] float weaponDamage;
        [SerializeField] float percentageBonus = 0;
        [SerializeField] bool isRighthand = true;
        [SerializeField] JoystickProjectile joystickProjectile = null;
        [SerializeField] AttackWave attackWave = null;

        //[SerializeField] public AudioClip audioClip;
        const string weaponName = "Weapon";

        public void Spawn(Transform rightHandTransform, Transform leftHandTransform, Animator animator)
        {
            DestroyCurrentWeapon(rightHandTransform, leftHandTransform);
            if (weaponPrefab != null)
            {
                Transform handTransform = GetHandTransition(rightHandTransform, leftHandTransform);
                WeaponComponent clonedWeapon = Instantiate(weaponPrefab, handTransform);
                clonedWeapon.gameObject.name = weaponName;
            }
            if (weaponOverride != null)
            {
                animator.runtimeAnimatorController = weaponOverride;
            }
        }

        private Transform GetHandTransition(Transform rightHandTransform, Transform leftHandTransform)
        {
            Transform handTransform;
            if (isRighthand) { handTransform = rightHandTransform; }
            else { handTransform = leftHandTransform; }

            return handTransform;
        }

        //public float GetWeaponRange() { return weaponRange; }

        public float GetWeaponDamage() { return weaponDamage; }

        public float GetPercentageBonus() { return percentageBonus; }

        public bool HasProjectile() { return joystickProjectile != null; }

        public void LaunchProjectile(Transform rightHandTransform, Transform leftHandTransform, float damage)
        {
            JoystickProjectile projectileInstance = Instantiate(joystickProjectile,
                GetHandTransition(rightHandTransform, leftHandTransform).position, Quaternion.identity);
            projectileInstance.SetDamage(damage);
        }

        private void DestroyCurrentWeapon(Transform rightHandTransform, Transform leftHandTransform)
        {
            Transform currentWeapon = rightHandTransform.Find(weaponName);
            if (currentWeapon == null) { currentWeapon = leftHandTransform.Find(weaponName); }
            if (currentWeapon == null) { return; }
            Destroy(currentWeapon.gameObject);
        }

        public WeaponComponent GetWeaponcomponent() { return weaponPrefab; }

        public string GetWeaponName() { return weaponName; }

        public AttackWave GetAttackWave() { return attackWave; }
    }
}
