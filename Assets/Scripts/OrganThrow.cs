using UnityEngine;

public class OrganThrow : Weapon
{
    public Transform firePointOrgan; // Punto de lanzamiento
    public float throwForce = 5f; // Fuerza del lanzamiento
    public float throwAngle = 45f; // Ángulo de disparo en grados

    protected override void Start()
    {
        firePoint = firePointOrgan;
        base.Start();
    }

    public override void Shoot(Vector3 direction)
    {
        if (!CanShoot() || firePoint == null || projectilePrefab == null) return;

        GameObject organ = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(direction));
        Rigidbody rb = organ.GetComponent<Rigidbody>();

        if (rb != null)
        {
            float angleInRadians = throwAngle * Mathf.Deg2Rad;

            // Aplicar un ángulo al disparo (como una parábola)
            Vector3 throwDirection = new Vector3(
                direction.x,
                Mathf.Sin(angleInRadians),
                direction.z
            ).normalized;

            rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("OrganProjectile no tiene un Rigidbody.");
        }

        ammo--; // Restar munición
    }
}
