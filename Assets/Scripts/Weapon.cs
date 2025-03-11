using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected Transform firePoint; // Punto de disparo
    public int ammo = 15; // Munición actual
    public int maxAmmo = 15; // Munición máxima
    public float cooldown = 0.5f; // Tiempo de espera entre disparos
    public GameObject projectilePrefab; // Prefab del proyectil
    private float _lastShotTime; // Última vez que se disparó

    protected virtual void Start()
    {
        if (firePoint == null)
        {
            Debug.LogError("FirePoint no está asignado en " + GetType().Name);
        }

        if (projectilePrefab == null)
        {
            Debug.LogError("ProjectilePrefab no está asignado en " + GetType().Name);
        }
    }

    public bool CanShoot()
    {
        return ammo > 0 && Time.time > _lastShotTime + cooldown;
    }

    public void Reload()
    {
        ammo = maxAmmo;
        Debug.Log("Munición recargada: " + ammo);
    }

    // 🔹 Hacemos `Shoot(Vector3)` obligatorio para todas las armas
    public abstract void Shoot(Vector3 direction);
}
