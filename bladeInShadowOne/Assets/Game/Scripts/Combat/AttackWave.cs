using RPG.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class AttackWave : MonoBehaviour
    {
        [SerializeField] Vector3 colliderCenter;
        [SerializeField] Vector3 colliderSize;
        [SerializeField] float maxLifeTime = 0.1f;
        [SerializeField] float speed = 5f;
        [SerializeField] GameObject hitEffect = null;
        float damage;
        GameObject playerObject;

        private void Start()
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
            SetBoxCollider();
        }

        private void Update()
        {
            transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * speed);
        }

        public void SetBoxCollider()
        {
            BoxCollider boxCol = GetComponent<BoxCollider>();
            boxCol.center = colliderCenter;
            boxCol.size = colliderSize;
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
            targetHealth.TakeDamage(playerObject, this.damage);
            if (hitEffect) 
            {
                CapsuleCollider capCollider;
                capCollider = playerObject.GetComponent<CapsuleCollider>();
                GameObject FXEffect = Instantiate(hitEffect, other.transform.position + Vector3.up * (capCollider.height / 2),
                    Quaternion.identity);
            }
            
        }
    }
}
