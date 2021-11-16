using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Resources;
using UnityEngine.Events;


namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 5;
        [SerializeField] bool isHoming = false;
        [SerializeField] float maxLifeTime = 3f;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float lifeAterImpact = 2f;
        [SerializeField] UnityEvent onHit;

        Health target;
        float damage;
        CapsuleCollider capCollider;
        GameObject instigator = null;

        private void Start()
        {
            capCollider = target.GetComponent<CapsuleCollider>();
            transform.LookAt(GetTargetLocation());
        }

        void Update()
        {
            if (target == null) { return; }
            if (isHoming && !target.IsDead())
            {
                transform.LookAt(GetTargetLocation());
            }
            transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * speed);
        }

        private Vector3 GetTargetLocation()
        {
            return target.transform.position + Vector3.up * (capCollider.height / 2);
        }

        public void SetTarget(Health target, float damage, GameObject instigator)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;
            Destroy(gameObject, maxLifeTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Projectile>()) { return; }
            Health targetHealth = other.GetComponent<Health>();
            if (!targetHealth) return;
            if (targetHealth.IsDead()) return;
            GameObject FXEffect = Instantiate(hitEffect, target.transform.position + Vector3.up * (capCollider.height / 2),
                Quaternion.identity);
            targetHealth.TakeDamage(instigator, damage);
            onHit.Invoke();
            speed = 0;
            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }
            Destroy(gameObject, lifeAterImpact);
        }
    }
}
