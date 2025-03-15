using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Panels")]
    public GameObject deathScreenUI;
    public GameObject winScreenUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("🔄 Escena cargada, reasignando UI y ocultando paneles.");

        // 🔹 Buscar los paneles nuevamente en la nueva escena
        deathScreenUI = GameObject.Find("DeathPanel");
        winScreenUI = GameObject.Find("WinPanel");

        if (deathScreenUI == null) Debug.LogWarning("⚠ No se encontró DeathPanel en la escena.");
        if (winScreenUI == null) Debug.LogWarning("⚠ No se encontró WinPanel en la escena.");

        HideAllPanels(); // 🔹 Asegurar que los paneles se ocultan al inicio
    }


    public void ShowDeathScreen()
    {
        HideAllPanels(); // 🔹 Ocultar cualquier otro panel antes de mostrar este
        if (deathScreenUI != null)
        {
            deathScreenUI.SetActive(true);
            Debug.Log("🔴 Pantalla de muerte activada.");
        }
        else
        {
            Debug.LogError("⚠ deathScreenUI no está asignado en GameManager.");
        }
        Invoke("RestartScene", 3f);
    }

    public void ShowWinScreen()
    {
        HideAllPanels(); // 🔹 Ocultar cualquier otro panel antes de mostrar este
        if (winScreenUI != null)
        {
            winScreenUI.SetActive(true);
            Debug.Log("🟢 Pantalla de victoria activada.");
        }
        else
        {
            Debug.LogError("⚠ winScreenUI no está asignado en GameManager.");
        }
        Invoke("RestartScene", 3f);
    }

    private void HideAllPanels()
    {
        if (deathScreenUI != null) deathScreenUI.SetActive(false);
        if (winScreenUI != null) winScreenUI.SetActive(false);
    }

    public void RestartScene()
    {
        Debug.Log("🔄 Reiniciando escena...");
        HideAllPanels(); // 🔹 Asegurar que los paneles están ocultos antes de recargar
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
