using RPG.Core;
using RPG.Movement;
using RPG.Resources;
using RPG.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityStandardAssets.CrossPlatformInput;

namespace RPG.Combat
{
    public class JoystickFight : MonoBehaviour, IAction, IModifierProvider
    {
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] JoystickWeapon defaultJoystickWeapon = null;
        [SerializeField] UnityEvent attackEvent;

        JoystickWeapon currentJoystickWeapon;
        AttackWave currentAttackWave;
        float attackEnemy;
        float timeToFinishAttackAnimation; // To be adjusted due to GetComponent<Animator>().SetFloat("RunMultiplier", 0.8f);
        float timeSinceAttack = 0f;
        float animationRunMultiplier;
        bool isAttacking = false;
        float totalDamage;

        private void Start()
        {
            if (currentJoystickWeapon == null)
            {
                EquipWeapon(defaultJoystickWeapon);
            }
        }

        private void Update()
        {
            // This is to prevent from attacking twice due to Blend Tree.
            if (isAttacking)
            {
                timeSinceAttack += Time.deltaTime;
                if (timeSinceAttack >= timeToFinishAttackAnimation)
                {
                    isAttacking = false;
                    timeSinceAttack = 0;
                    Cancel();
                }
            }
        }

        public void EquipWeapon(JoystickWeapon joystickWeapon)
        {
            currentJoystickWeapon = joystickWeapon;
            joystickWeapon.Spawn(rightHandTransform, leftHandTransform, GetComponent<Animator>());
            timeToFinishAttackAnimation = joystickWeapon.GetTimeToFinishAttackAnimation();
            animationRunMultiplier = joystickWeapon.GetAnimationRunMultiplier();
            if (currentJoystickWeapon.GetWeaponcomponent().transform.Find("HitSound"))
            {
                transform.Find("SoundObjects").Find("MeleeSound").GetComponent<AudioSource>().clip =
                    currentJoystickWeapon.GetWeaponcomponent().transform.Find("HitSound").GetComponent<AudioSource>().clip;
            }
        }

        public void StartAttackAction()
        {
            GetComponent<ActionScheduler>().StartAction(this);
            GetComponent<Animator>().ResetTrigger("StopAttack");
            GetComponent<Animator>().SetTrigger("Attack");
            //GetComponent<Animator>().SetFloat("RunMultiplier", 1.2f);
            GetComponent<Animator>().SetFloat("RunMultiplier", animationRunMultiplier);
        }

        public bool IsAttacking()
        {
            attackEnemy = CrossPlatformInputManager.VirtualAxisReference("Attack").GetValue;
            if (attackEnemy > Mathf.Epsilon) 
            {
                isAttacking = true;
                return true; 
            }
            else { return false; }
        }

        public void Cancel()
        {
            GetComponent<Animator>().ResetTrigger("Attack");
            GetComponent<Animator>().SetTrigger("StopAttack");
            WeaponComponent weapon = FindWeaponComponent();
            if (!weapon.GetIsDisposable()) 
            {
                weapon.gameObject.SetActive(true); 
            }
            GetComponent<JoystickMove>().Cancel();
        }

        // Animation Event.
        void Hit()
        {
            totalDamage = GetComponent<BaseStats>().GetDamage();
            if (currentJoystickWeapon.HasProjectile())
            {
                WeaponComponent weapon = FindWeaponComponent();
                weapon.gameObject.SetActive(false);
                Vector3 instanPosition = transform.position + 
                    Vector3.up * GetComponent<CapsuleCollider>().height / 3 + transform.TransformDirection(Vector3.forward * 1);
                currentJoystickWeapon.LaunchProjectile(instanPosition, totalDamage);
            }
            else
            {
                attackEvent.Invoke();
                currentAttackWave = currentJoystickWeapon.GetAttackWave();
                AttackWave attackWaveClone = Instantiate(currentAttackWave, transform.position, transform.rotation);
                attackWaveClone.transform.parent = transform;
                attackWaveClone.SetDamage(totalDamage);
            }
        }

        // Animation Event.
        void Shoot()
        {
            Hit();
        }

        void ChangeWeapon()
        {
            // Be careful where the animation event is inserted. Due to Blend tree, the event may not be triggered.
            WeaponComponent weapon = FindWeaponComponent();
            if (!weapon.GetIsDisposable()) { return; }
            EquipWeapon(defaultJoystickWeapon);
        }

        private void ActiveWeaponHolding(WeaponComponent weaponComponent)
        {
            if (weaponComponent.GetComponent<Collider>() != null)
            {
                weaponComponent.GetComponent<Collider>().enabled = true;
            }
        }

        private IEnumerator WaitAndDeactiveCollider(WeaponComponent weaponComponent)
        {
            yield return new WaitForSeconds(0.3f);
            weaponComponent.GetComponent<Collider>().enabled = false;
        }

        private WeaponComponent FindWeaponComponent()
        {
            WeaponComponent weaponComponent =
                rightHandTransform.Find(currentJoystickWeapon.GetWeaponName()).GetComponent<WeaponComponent>();
            if (weaponComponent != null)
            {
                return weaponComponent;
            }
            else
            {
                weaponComponent = leftHandTransform.Find(currentJoystickWeapon.GetWeaponName()).GetComponent<WeaponComponent>();
            }
            return weaponComponent;
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentJoystickWeapon.GetWeaponDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentJoystickWeapon.GetPercentageBonus();
            }
        }
    }
}
