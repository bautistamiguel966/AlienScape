using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class SoldierController : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 20f;
    public float shootingRange = 10f;
    public float stoppingDistance = 5f;
    public SoldierGun soldierGun;
    public float fireRate = 1f;
    public float reloadDelay = 2f;

    [Header("Patrolling Settings")]
    public Transform[] waypoints; 
    private int currentWaypointIndex = 0;
    private bool isChasingPlayer = false;
    private bool isReloading = false;

    private NavMeshAgent navMeshAgent;
    private float nextFireTime;
    private Collider playerCollider;
    // Animador para controlar las animaciones
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>(); // Obtén el Animator del soldado
        anim.SetFloat("MovementS", 0.3f); //Animacion estar
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = stoppingDistance;
        navMeshAgent.autoBraking = false; // 🔹 Evita que el agente frene en cada waypoint

        if (player != null)
        {
            playerCollider = player.GetComponent<Collider>();
        }

        if (waypoints.Length > 0)
        {
            currentWaypointIndex = Random.Range(0, waypoints.Length); // 🔹 Empezar en un waypoint aleatorio
            GoToNextWaypoint();
        }
    }

    private void Update()
    {
        if (player == null || navMeshAgent == null || !navMeshAgent.isActiveAndEnabled)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        if (distanceToPlayer <= detectionRange)
        {
            if (!isChasingPlayer)
            {
                anim.SetFloat("MovementS", 0.5f); //Animacion estar
                isChasingPlayer = true;
                Debug.Log($"{gameObject.name}: Persiguiendo al jugador.");
            }

            navMeshAgent.SetDestination(player.position);

            if (distanceToPlayer <= shootingRange)
            {
                ShootAtPlayer();
            }
        }
        else
        {
            if (isChasingPlayer)
            {
                isChasingPlayer = false;
                Debug.Log($"{gameObject.name}: Perdió al jugador. Volviendo a patrullar.");
                GoToNextWaypoint();
            }

            Patrol();
        }
    }

    private void Patrol()
    {
        if (waypoints.Length == 0 || isChasingPlayer)
        {
            return;
        }

        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            GoToNextWaypoint();
        }
    }

    private void GoToNextWaypoint()
    {
        if (waypoints.Length == 0 || isChasingPlayer)
        {
            return;
        }

        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // 🔹 Ciclo infinito de waypoints
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
        Debug.Log($"{gameObject.name}: Moviéndose al waypoint {waypoints[currentWaypointIndex].name}");
    }

    private void ShootAtPlayer()
    {
        Vector3 targetPoint = playerCollider.bounds.center;
        Vector3 shootDirection = (targetPoint - soldierGun.firePoint.position).normalized;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(shootDirection), Time.deltaTime * 5f);

        if (soldierGun != null && Time.time >= nextFireTime && !isReloading)
        {
            if (soldierGun.ammo > 0)
            {
                soldierGun.Shoot(shootDirection);
                nextFireTime = Time.time + 1f / fireRate;
            }
            else
            {
                StartCoroutine(ReloadAfterDelay());
            }
        }
    }

    private IEnumerator ReloadAfterDelay()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadDelay);
        soldierGun.Reload();
        isReloading = false;
    }
}
