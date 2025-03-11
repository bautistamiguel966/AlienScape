using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected Transform firePoint; // Punto de disparo (oculto en el inspector)
    public int ammo = 15; // Munición actual
    public int maxAmmo = 15; // Munición máxima
    public float cooldown = 0.5f; // Tiempo de enfriamiento entre disparos
    public GameObject projectilePrefab; // Prefab del proyectil

    protected virtual void Start()
    {
        // Verificar que el firePoint esté asignado
        if (firePoint == null)
        {
            Debug.LogError("FirePoint is not assigned in " + GetType().Name);
        }

        // Verificar que el projectilePrefab esté asignado
        if (projectilePrefab == null)
        {
            Debug.LogError("ProjectilePrefab is not assigned in " + GetType().Name);
        }
    }

    public void Shoot()
    {
        if (CanShoot())
        {
            if (firePoint == null || projectilePrefab == null)
            {
                Debug.LogError("FirePoint or ProjectilePrefab is not assigned in " + GetType().Name);
                return;
            }

            ammo--;
            OnShoot();
        }
    }

    protected virtual void OnShoot()
    {
        // Lógica común de disparo (si la hay)
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }

    public bool CanShoot()
    {
        // Verificar si hay munición y si el cooldown ha terminado
        return (ammo > 0) && (Time.time > _lastShotTime + cooldown);
    }

    public void Reload()
    {
        // Recargar la munición al máximo
        ammo = maxAmmo;
        Debug.Log("Munición recargada: " + ammo);
    }

    private float _lastShotTime;
}