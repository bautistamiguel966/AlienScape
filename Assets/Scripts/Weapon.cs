using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public Transform firePoint; // Punto de disparo
    public int ammo = 15;
    public int maxAmmo = 15;
    public float cooldown = 0.5f;
    public GameObject projectilePrefab;

    protected float lastShotTime;

    public abstract void Shoot(Vector3 direction); // 🔹 Método abstracto obligatorio

    public virtual bool CanShoot()
    {
        return ammo > 0 && Time.time > lastShotTime + cooldown;
    }

    public virtual void Reload()
    {
        ammo = maxAmmo;
        Debug.Log($"{gameObject.name}: Munición recargada ({ammo}/{maxAmmo})");
    }
}
