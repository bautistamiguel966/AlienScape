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
    public float accuracyError = 0.01f; // Margen de error en la precisión del disparo (ajustado para ~90% de aciertos)
    public float reloadDelay = 2f; // Tiempo de espera antes de recargar

    private NavMeshAgent navMeshAgent;
    private float nextFireTime;
    private Collider playerCollider; // Colisionador del Player
    private bool isReloading = false; // Indica si el Soldier está recargando
    private Rigidbody playerRigidbody; // Rigidbody del Player para predecir su movimiento

    private void Start()
    {
        // Obtener el componente NavMeshAgent
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Configurar la distancia de detención
        navMeshAgent.stoppingDistance = stoppingDistance;

        // Obtener el colisionador y Rigidbody del Player
        if (player != null)
        {
            playerCollider = player.GetComponent<Collider>();
            playerRigidbody = player.GetComponent<Rigidbody>();

            if (playerCollider == null)
            {
                Debug.LogError("El Player no tiene un componente Collider");
            }
            if (playerRigidbody == null)
            {
                Debug.LogError("El Player no tiene un componente Rigidbody");
            }
        }
        else
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
        if (player == null || navMeshAgent == null || !navMeshAgent.isActiveAndEnabled || playerCollider == null)
        {
            return; // Salir si no hay jugador, NavMeshAgent o colisionador
        }

        // Calcular la distancia al jugador
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Moverse hacia el jugador
            navMeshAgent.SetDestination(player.position);

            if (distanceToPlayer <= shootingRange)
            {
                // Obtener la posición del centro del colisionador del Player
                Vector3 playerCenter = playerCollider.bounds.center;

                // Predecir la posición futura del Player si se mueve
                Vector3 predictedPosition = playerCenter;
                if (playerRigidbody != null)
                {
                    predictedPosition = PredictPlayerPosition(playerCenter, playerRigidbody.linearVelocity, soldierGun.projectileSpeed);
                }

                // Rotar hacia la posición predicha del Player
                Vector3 direction = (predictedPosition - transform.position).normalized;
                Debug.DrawLine(transform.position, predictedPosition, Color.red); // Dibujar una línea de depuración
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

                // Disparar al jugador con cooldown
                if (soldierGun != null && Time.time >= nextFireTime && !isReloading)
                {
                    if (soldierGun.ammo > 0)
                    {
                        // Aplicar un margen de error a la dirección del disparo
                        Vector3 shootDirection = ApplyAccuracyError(direction);
                        soldierGun.ShootTowards(shootDirection);
                        nextFireTime = Time.time + 1f / fireRate; // Establecer el próximo tiempo de disparo
                    }
                    else
                    {
                        // Iniciar la recarga si no hay munición
                        StartCoroutine(ReloadAfterDelay());
                    }
                }
            }
        }
        else if (soldierGun.ammo <= 0 && !isReloading) // Si no hay munición y no está recargando
        {
            // Detener el movimiento y la rotación
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = Vector3.zero;

            // Iniciar la recarga
            StartCoroutine(ReloadAfterDelay());
        }
    }

    private Vector3 PredictPlayerPosition(Vector3 playerPosition, Vector3 playerVelocity, float projectileSpeed)
    {
        // Calcular el tiempo que tardará el proyectil en llegar al Player
        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition);
        float timeToHit = distanceToPlayer / projectileSpeed;

        // Predecir la posición futura del Player
        return playerPosition + playerVelocity * timeToHit;
    }

    private Vector3 ApplyAccuracyError(Vector3 direction)
    {
        // Aplicar un margen de error aleatorio solo en los ejes X y Z (horizontal)
        Vector3 error = new Vector3(
            Random.Range(-accuracyError, accuracyError),
            0, // No aplicar error en el eje Y (altura)
            Random.Range(-accuracyError, accuracyError)
        );

        // Devolver la dirección con el error aplicado
        return (direction + error).normalized;
    }

    private IEnumerator ReloadAfterDelay()
    {
        // Indicar que el Soldier está recargando
        isReloading = true;

        // Esperar el tiempo de recarga
        yield return new WaitForSeconds(reloadDelay);

        // Recargar el arma
        soldierGun.Reload();

        // Indicar que el Soldier ha terminado de recargar
        isReloading = false;

        // Reanudar el movimiento
        navMeshAgent.isStopped = false;
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