using UnityEngine;

public class OrganThrow : Weapon
{
    public Transform firePointOrgan; // Punto de disparo del lanzamiento de órgano
    public float throwForce = 5f; // Fuerza del lanzamiento
    public float throwAngle = 45f; // Ángulo de disparo en grados

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
            // Convertir el ángulo de grados a radianes
            float angleInRadians = throwAngle * Mathf.Deg2Rad;

            // Calcular la dirección en el plano vertical (hacia adelante y hacia arriba)
            Vector3 direction = new Vector3(
                0, // No hay componente lateral
                Mathf.Sin(angleInRadians), // Componente vertical (hacia arriba)
                Mathf.Cos(angleInRadians) // Componente horizontal (hacia adelante)
            );

            // Aplicar la rotación del firePoint a la dirección
            direction = firePoint.rotation * direction;

            // Aplicar la fuerza al proyectil
            rb.AddForce(direction * throwForce, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("Organ does not have a Rigidbody component");
        }
    }
}