using UnityEngine;
using UnityEngine.UI; // Para manejar la UI
using UnityEngine.SceneManagement; // Para reiniciar o cambiar escena
using TMPro; // Para mostrar texto en pantalla

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Slider healthBar; // Referencia a la barra de vida
    public TextMeshProUGUI healthText; // Referencia al texto de la vida


    public GameObject deathScreenUI; // UI de mensaje de muerte
    public Transform playerModel; // Referencia al modelo del Player
    public static bool isPlayerDead = false;

    private CharacterController characterController;
    private PlayerController playerController;

    private void Start()
    {
        isPlayerDead = false; // 🔹 Resetear cuando comienza la escena
        currentHealth = maxHealth;
        characterController = GetComponent<CharacterController>();
        playerController = GetComponent<PlayerController>();

        if (deathScreenUI != null)
        {
            deathScreenUI.SetActive(false); // Asegurar que el mensaje de muerte está oculto al inicio
        }

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth; // Configurar el máximo de la barra
            healthBar.value = currentHealth; // Inicializar con la salud actual
        }

        if (healthText != null)
        {
            healthText.text = currentHealth + " / " + maxHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Jugador recibió " + damage + " de daño. Salud actual: " + currentHealth);

        if (healthBar != null)
        {
            healthBar.value = currentHealth; // Actualizar la barra de vida
        }

        if (healthText != null)
        {
            healthText.text = currentHealth + " / " + maxHealth;
        }

        if (currentHealth <= 0)
        {

            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Jugador ha muerto");
        isPlayerDead = true; // 🔹 Marca que el jugador está muerto

        // 🔹 Deshabilitar controles
        if (playerController != null)
        {
            playerController.enabled = false;
            Debug.Log("Deshabilitado PlayerController");
        }
        if (characterController != null)
        {
            characterController.enabled = false;
            Debug.Log("Deshabilitado CharacterController");
        }

        // 🔹 Mostrar mensaje de muerte
        if (deathScreenUI != null)
        {
            deathScreenUI.SetActive(true);
            Debug.Log("Mensaje de muerte activado.");
        }
        else
        {
            Debug.LogWarning("⚠ deathScreenUI no está asignado.");
        }

        // 🔹 Reiniciar escena después de 3 segundos
        Invoke("RestartScene", 3f);
    }



    private void RestartScene()
    {
        // 🔹 TRUCO para limpiar la consola al reiniciar
        Application.logMessageReceived += SuppressLogs;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Invoke(nameof(EnableLogs), 0.1f); // Reactivar logs después de reiniciar
    }

    private void SuppressLogs(string condition, string stackTrace, LogType type) { }

    private void EnableLogs()
    {
        Application.logMessageReceived -= SuppressLogs;
    }
}
