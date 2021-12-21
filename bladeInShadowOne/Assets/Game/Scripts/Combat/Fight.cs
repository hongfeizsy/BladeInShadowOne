using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Resources;
using RPG.Stats;
using System.Collections.Generic;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Fight : MonoBehaviour, IAction, ISavable, IModifierProvider
    {
        
        [SerializeField] Health target;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;
        //[SerializeField] string defaultWeaponName = "Unarmed";
        [SerializeField] UnityEvent attackEvent;

        float timeBetweenAttacks = 1.5f;
        float timeSinceLastAttack = 0f;
        float weaponRange;
        //float weaponDamage;
        float totalDamage;
        Weapon currentWeapon;

        private void Start()
        {
            if (currentWeapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target == null) return;
            if (target.IsDead()) 
            {
                target = null; 
                return; 
            }
            if (GetComponent<Health>().IsDead()) { return; }
            bool isInRange = Vector3.Distance(transform.position, target.transform.position) <= weaponRange;
            
            if (!isInRange)
            {
                GetComponent<Move>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                GetComponent<Move>().Cancel();
                AttackBehavior();
            }
        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform, Vector3.up);
            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                // This will trigger the Hit() event
                GetComponent<Animator>().ResetTrigger("StopAttack");
                GetComponent<Animator>().SetTrigger("Attack");
                timeSinceLastAttack = 0f;
            }
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);  // ActionScheduler.currentAction is RPG.Combat.Fighter, even Player is still moving towards to the target.
            target = combatTarget.GetComponent<Health>();
            GetComponent<Animator>().SetFloat("RunMultiplier", 0.7f);
        }

        public void Cancel()
        {
            GetComponent<Animator>().ResetTrigger("Attack");
            GetComponent<Animator>().SetTrigger("StopAttack");
            GetComponent<Move>().Cancel();
            target = null;
        }

        // Animation Event.
        void Hit()
        {
            if (target == null) return;
            totalDamage = GetComponent<BaseStats>().GetDamage();
            if (currentWeapon.HasProjectile()) 
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, totalDamage); 
            }
            else
            {
                //currentWeapon.GetWeaponcomponent().OnHit();
                attackEvent.Invoke();
                target.TakeDamage(gameObject, totalDamage);
            }
        }

        // Animation Event
        void Shoot()
        {
            Hit();
        }

        public bool CanAttack(CombatTarget combatTarget)
        {
            return !combatTarget.GetComponent<Health>().IsDead();
        }

        public float GetWeaponRange() { return weaponRange; }

        public void EquipWeapon(Weapon weapon) 
        {
            currentWeapon = weapon;
            weaponRange = weapon.GetWeaponRange();
            weapon.Spawn(rightHandTransform, leftHandTransform, GetComponent<Animator>());

            if (currentWeapon.GetWeaponcomponent().transform.Find("HitSound"))
            {
                transform.Find("SoundObjects").Find("MeleeSound").GetComponent<AudioSource>().clip =
                    currentWeapon.GetWeaponcomponent().transform.Find("HitSound").GetComponent<AudioSource>().clip;
            }
        }

        public Health GetTarget()
        {
            return target;
        }

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = UnityEngine.Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }

        //public IEnumerable<float> GetAdditiveModifier(Stat stat)
        //{
        //    if (stat == Stat.Damage)
        //    {
        //        yield return currentWeapon.GetWeaponDamage();
        //    }
        //}

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.GetWeaponDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.GetPercentageBonus();
            }
        }
    }
}
