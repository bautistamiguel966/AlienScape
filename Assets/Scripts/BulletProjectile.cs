using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    public int damage = 10; // Daño del proyectil
    public float speed = 20f; // Velocidad del proyectil
    public float lifetime = 3f; // Tiempo de vida del proyectil
    public string shooterTag; // Quién disparó ("Player" o "Soldier")
    
    public Material playerMaterial;  // 🔹 Material del Player
    public Material soldierMaterial; // 🔹 Material del Soldier

    private Rigidbody rb;
    private Renderer projectileRenderer;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        projectileRenderer = GetComponent<Renderer>();

        if (rb != null)
        {
            rb.linearVelocity = transform.forward * speed; // Disparo en línea recta
        }
        else
        {
            Debug.LogError($"{gameObject.name}: No tiene un Rigidbody.");
        }

        // 🔹 Cambiar color según quién disparó
        if (projectileRenderer != null)
        {
            if (shooterTag == "Player" && playerMaterial != null)
            {
                projectileRenderer.material = playerMaterial;
            }
            else if (shooterTag == "Soldier" && soldierMaterial != null)
            {
                projectileRenderer.material = soldierMaterial;
            }
        }

        Destroy(gameObject, lifetime); // Se destruye después de un tiempo
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(shooterTag)) return; // No dañar al que disparó

        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log($"💥 {shooterTag} impactó al Player por {damage} de daño.");
            }
        }
        else if (other.CompareTag("Soldier"))
        {
            SoldierHealth soldierHealth = other.GetComponent<SoldierHealth>();
            if (soldierHealth != null)
            {
                soldierHealth.TakeDamage(damage);
                Debug.Log($"🔥 {shooterTag} impactó a un Soldier por {damage} de daño.");
            }
        }
        else
        {
            Debug.Log($"💥 {gameObject.name} impactó contra {other.gameObject.name}");
        }

        Destroy(gameObject); // Se destruye al impactar
    }
}
