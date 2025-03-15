using UnityEngine;

public class OrganThrow : Weapon
{
    public float throwForce = 10f; // Fuerza del lanzamiento
    public float throwAngle = 45f; // Ángulo de disparo en grados

    public override void Shoot(Vector3 direction)
    {
        if (!CanShoot() || firePoint == null || projectilePrefab == null) return;

        ammo--; // Restar munición
        lastShotTime = Time.time; // Aplicar cooldown

        GameObject organ = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = organ.GetComponent<Rigidbody>();

        if (rb != null)
        {
            float angleInRadians = throwAngle * Mathf.Deg2Rad;

            // 🔹 Aplicar un ángulo al lanzamiento
            Vector3 throwDirection = direction.normalized;
            throwDirection.y = Mathf.Tan(angleInRadians); // Elevar el proyectil

            rb.AddForce(throwDirection.normalized * throwForce, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("OrganProjectile no tiene un Rigidbody.");
        }
    }
}
