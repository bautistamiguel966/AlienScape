using UnityEngine;

public class Spaceship : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        // Verificar si el proyectil colisiona con el jugador
        // PlayerHealth playerHealth = collider.gameObject.GetComponent<PlayerHealth>();
        // if (playerHealth != null)
        // {
        //     playerHealth.TakeDamage(damage);
        // }

        // Verificar si el proyectil colisiona con un Soldier
        PlayerController playerController = collider.gameObject.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.Win();
        }

    }
}
