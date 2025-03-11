using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class SoldierController : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public float detectionRange = 20f; // Rango de detección del jugador
    public float shootingRange = 10f; // Rango de disparo
    public float stoppingDistance = 5f; // Margen de alejamiento
    public SoldierGun soldierGun; // Referencia al arma del Soldier
    public float fireRate = 1f; // Tasa de disparo (disparos por segundo)
    public float reloadDelay = 2f; // Tiempo de espera antes de recargar

    private NavMeshAgent navMeshAgent;
    private float nextFireTime;
    private Collider playerCollider; // Colisionador del Player
    private bool isReloading = false; // Indica si el Soldier está recargando

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = stoppingDistance;

        if (player != null)
        {
            playerCollider = player.GetComponent<Collider>();

            if (playerCollider == null)
            {
                Debug.LogError("El Player no tiene un componente Collider");
            }
        }
        else
        {
            Debug.LogError("Player no está asignado en SoldierController");
        }

        if (navMeshAgent == null || !navMeshAgent.isActiveAndEnabled)
        {
            Debug.LogError("NavMeshAgent no está activo o no está asignado en SoldierController");
        }
    }

    private void Update()
    {
        if (player == null || navMeshAgent == null || !navMeshAgent.isActiveAndEnabled || playerCollider == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            navMeshAgent.SetDestination(player.position);

            if (distanceToPlayer <= shootingRange)
            {
                // 🔹 Asegurar que el Soldier apunta exactamente al Player
                Vector3 targetPoint = playerCollider.bounds.center; // Centro del collider del Player
                Vector3 shootDirection = (targetPoint - soldierGun.firePointSoldier.position).normalized;

                // 🔹 Soldier mira al Player
                Quaternion lookRotation = Quaternion.LookRotation(shootDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

                Debug.DrawRay(soldierGun.firePointSoldier.position, shootDirection * 10f, Color.green); // Línea de depuración

                if (soldierGun != null && Time.time >= nextFireTime && !isReloading)
                {
                    if (soldierGun.ammo > 0)
                    {
                        soldierGun.Shoot(shootDirection); // 🔹 Ahora dispara correctamente
                        nextFireTime = Time.time + 1f / fireRate;
                    }
                    else
                    {
                        StartCoroutine(ReloadAfterDelay());
                    }
                }
            }
        }
    }

    private IEnumerator ReloadAfterDelay()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadDelay);
        soldierGun.Reload();
        isReloading = false;
        navMeshAgent.isStopped = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, shootingRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);
    }
}
