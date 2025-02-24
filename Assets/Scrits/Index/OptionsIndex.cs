using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsIndex : MonoBehaviour
{
    // Método para cargar una escena
    public void Init(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Método para salir de la aplicación
    public void Exit()
    {
        Debug.Log("Se ejecutó salir"); // Mensaje en la consola
        Application.Quit(); // Cierra la aplicación
    }
}