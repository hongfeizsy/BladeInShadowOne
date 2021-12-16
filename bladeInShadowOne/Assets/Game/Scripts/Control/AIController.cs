using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using RPG.Resources;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float shoutDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] PatrolPath patrolPath = null;
        [SerializeField] float waypointDwellTime = 2f;
        [SerializeField] float patrolSpeedFraction = 0.5f;
        int currentWaypointIndex = 0;
        bool isAlarmed = false;
        bool isAggregated = false;
        float timeSinceLastSawPlayer = 0f;
        float timeSinceArrivedAtWaypoint = 0f;
        float timeSinceAggrevated = 0f;
        float aggreCoolDownTime = 3f;

        private void Update()
        {
            if (GetComponent<Health>().IsDead()) { return; }

            if (!isAlarmed & patrolPath != null)
            {
                Vector3 currentWaypoint = patrolPath.GetCurrentWaypointPos(currentWaypointIndex);
                GetComponent<Move>().StartMoveAction(currentWaypoint, patrolSpeedFraction);
                float distanceToWaypoint = Vector3.Distance(transform.position, currentWaypoint);
                if (distanceToWaypoint < .15f)
                {
                    if (timeSinceArrivedAtWaypoint > waypointDwellTime)
                    {
                        currentWaypointIndex = patrolPath.GetNextWaypointIndex(currentWaypointIndex);
                        timeSinceArrivedAtWaypoint = 0f;
                    }
                    else
                    {
                        GetComponent<ActionScheduler>().GetCurrentAction().Cancel();
                        timeSinceArrivedAtWaypoint += Time.deltaTime;
                    }
                }
            }

            GameObject player = GameObject.FindWithTag("Player");
            float distanceToPlayer = Vector3.Distance(gameObject.transform.position, player.transform.position);
            if (player.GetComponent<Health>().IsDead()) return;
            if (distanceToPlayer < chaseDistance && !GetComponent<Health>().IsDead())
            {
                isAlarmed = true;
                timeSinceLastSawPlayer = 0f;
                if (InteractWithCombat(distanceToPlayer, player)) { return; }
                InteractWithMove(player.transform.position, 1f);
            }

            if (isAlarmed)
            {
                if (distanceToPlayer >= chaseDistance)
                {
                    if (timeSinceLastSawPlayer > suspicionTime)
                    {
                        isAlarmed = false;
                    }
                    else
                    {
                        IAction currentAction = GetComponent<ActionScheduler>().GetCurrentAction();
                        if (currentAction != null) { currentAction.Cancel(); }
                    }
                    timeSinceLastSawPlayer += Time.deltaTime;
                }
            }

            if (isAggregated)
            {
                InteractWithMove(player.transform.position, 1f);
                if (timeSinceAggrevated > aggreCoolDownTime) 
                {
                    isAggregated = false;
                    IAction currentAction = GetComponent<ActionScheduler>().GetCurrentAction();
                    if (currentAction != null) { currentAction.Cancel(); }
                }
                else
                {
                    timeSinceAggrevated += Time.deltaTime;
                }
            }
        }

        private bool InteractWithCombat(float currentDistance, GameObject player)
        {
            if (GetComponent<Fight>().GetWeaponRange() > currentDistance)
            {
                if (GetComponent<Fight>().CanAttack(player.GetComponent<CombatTarget>()))
                {
                    GetComponent<Fight>().Attack(player.GetComponent<CombatTarget>());
                    return true;
                }
            }
            return false;
        }

        private void InteractWithMove(Vector3 playerPosition, float speedFraction)
        {
            GetComponent<Move>().StartMoveAction(playerPosition, speedFraction);
        }

        // Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }

        public void Aggrevate()
        {
            isAggregated = true;
            timeSinceAggrevated = 0f;
            AggravateNearbyEnemies();
        }

        private void AggravateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up);
            foreach (RaycastHit hit in hits)
            {
                AIController ai = hit.collider.GetComponent<AIController>();
                if (ai == null) continue;
                ai.LetOtherAggrevate();
            }
        }

        private void LetOtherAggrevate()
        {
            isAggregated = true;
            timeSinceAggrevated = 0f;
        }
    }
}
