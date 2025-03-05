using UnityEngine;

public class OrganThrow : Weapon
{
    public Transform firePointOrgan; // Punto de disparo del lanzamiento de órgano
    public float throwForce = 10f;
    public float arcHeight = 2f;

    protected override void Start()
    {
        // Asignar el firePoint específico para OrganThrow
        firePoint = firePointOrgan;

        // Llamar al Start de la clase base para verificar asignaciones
        base.Start();
    }

    protected override void OnShoot()
    {
        // Lógica específica de disparo para OrganThrow
        GameObject organ = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = organ.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direction = firePoint.forward + Vector3.up * arcHeight;
            rb.AddForce(direction * throwForce, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("Organ does not have a Rigidbody component");
        }
    }
}