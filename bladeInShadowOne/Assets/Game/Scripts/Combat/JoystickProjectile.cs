using RPG.Combat;
using RPG.Control;
using RPG.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class JoystickProjectile : MonoBehaviour
    {
        [SerializeField] float speed = 5;
        //[SerializeField] bool isHoming = false;
        [SerializeField] float maxLifeTime = 3f;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float lifeAterImpact;
        [SerializeField] UnityEvent onHit;

        GameObject playerObject;
        //GameObject instigator = null;
        CapsuleCollider capCollider;
        float damage;

        private void Start()
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
            transform.LookAt(playerObject.GetComponent<JoystickControl>().GetDirectionFacing());
            capCollider = playerObject.GetComponent<CapsuleCollider>();
        }

        private void Update()
        {
            transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * speed);
        }

        public void SetDamage(float damage)
        {
            this.damage = damage;
            Destroy(gameObject, maxLifeTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Projectile>()) { return; }
            Health targetHealth = other.GetComponent<Health>();
            if (!targetHealth) return;
            if (targetHealth.IsDead()) return;
            GameObject FXEffect = Instantiate(hitEffect, other.transform.position + Vector3.up * (capCollider.height / 2),
                    Quaternion.identity);
            targetHealth.TakeDamage(playerObject, this.damage);
            onHit.Invoke();
            speed = 0f;
            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }
            Destroy(gameObject, lifeAterImpact);
        }
    }
}
