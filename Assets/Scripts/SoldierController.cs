using UnityEngine;
using UnityEngine.AI;

public class SoldierController : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public float detectionRange = 20f; // Rango de detección del jugador
    public float shootingRange = 10f; // Rango de disparo
    public float stoppingDistance = 5f; // Margen de alejamiento
    public SoldierGun soldierGun; // Referencia al arma del Soldier
    public float fireRate = 1f; // Tasa de disparo (disparos por segundo)

    private NavMeshAgent navMeshAgent;
    private float nextFireTime;

    private void Start()
    {
        // Obtener el componente NavMeshAgent
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Configurar la distancia de detención
        navMeshAgent.stoppingDistance = stoppingDistance;

        // Verificar que el jugador esté asignado
        if (player == null)
        {
            Debug.LogError("Player no está asignado en SoldierController");
        }

        // Verificar que el NavMeshAgent esté activo y asignado
        if (navMeshAgent == null || !navMeshAgent.isActiveAndEnabled)
        {
            Debug.LogError("NavMeshAgent no está activo o no está asignado en SoldierController");
        }
    }

    private void Update()
    {
        // Verificar que el jugador esté asignado y el NavMeshAgent esté activo
        if (player == null || navMeshAgent == null || !navMeshAgent.isActiveAndEnabled)
        {
            return; // Salir si no hay jugador o el NavMeshAgent no está listo
        }

        // Calcular la distancia al jugador
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Verificar si la posición del jugador es válida en el NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(player.position, out hit, 1.0f, NavMesh.AllAreas))
            {
                // Moverse hacia el jugador, respetando el margen de alejamiento
                navMeshAgent.SetDestination(hit.position);
            }
            else
            {
                Debug.LogWarning("La posición del jugador no es válida en el NavMesh");
                return; // Salir si la posición no es válida
            }

            if (distanceToPlayer <= shootingRange)
            {
                // Rotar hacia el jugador
                Vector3 direction = (player.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

                // Disparar al jugador con cooldown
                if (soldierGun != null && Time.time >= nextFireTime)
                {
                    soldierGun.Shoot();
                    nextFireTime = Time.time + 1f / fireRate; // Establecer el próximo tiempo de disparo
                }
            }
        }
        else if (soldierGun.ammo <= 0) // Si no hay munición
        {
            // Detener el movimiento y la rotación
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = Vector3.zero;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Dibujar el rango de detección y disparo en el editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, shootingRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance); // Dibujar el margen de alejamiento
    }
}