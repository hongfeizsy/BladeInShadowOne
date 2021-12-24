using UnityEngine;
using RPG.Resources;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName ="Weapon", menuName = "Weapons/Make New Weapon", order =0)]
    public class Weapon: ScriptableObject
    {
        [SerializeField] AnimatorOverrideController weaponOverride;
        [SerializeField] WeaponComponent weaponPrefab = null;
        [SerializeField] float weaponRange;
        [SerializeField] float weaponDamage;
        [SerializeField] float percentageBonus = 0;
        [SerializeField] bool isRighthand = true;
        [SerializeField] Projectile projectile = null;
        [SerializeField] float timeToFinishAttackAnimation;
        [SerializeField] float animationRunMultiplier;

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

        public float GetWeaponRange() { return weaponRange; }

        public float GetWeaponDamage() { return weaponDamage; }

        public float GetPercentageBonus() { return percentageBonus; }

        public bool HasProjectile() { return projectile != null; }

        public void LaunchProjectile(Transform rightHandTransform, Transform leftHandTransform, Health target, GameObject instigator, float damage)
        {
            Projectile projectileInstance = Instantiate(projectile, 
                GetHandTransition(rightHandTransform, leftHandTransform).position, Quaternion.identity);
            projectileInstance.SetTarget(target, damage, instigator);
        }

        private void DestroyCurrentWeapon(Transform rightHandTransform, Transform leftHandTransform)
        {
            Transform currentWeapon = rightHandTransform.Find(weaponName);
            if (currentWeapon == null) { currentWeapon = leftHandTransform.Find(weaponName); }
            if (currentWeapon == null) { return; }
            Destroy(currentWeapon.gameObject);
        }

        public WeaponComponent GetWeaponcomponent() { return weaponPrefab; }

        public float GetTimeToFinishAttackAnimation() { return timeToFinishAttackAnimation; }

        public float GetAnimationRunMultiplier() { return animationRunMultiplier; }
    }
}