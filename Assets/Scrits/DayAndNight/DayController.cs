using UnityEngine;
using System.Collections;

public class DayController : MonoBehaviour
{
    [Range(0.0f, 24f)] public float _hoursInGame = 12;
    [SerializeField] private Transform _sun;
    
    public float realSecondsPerHour = 1f; // Cuántos segundos reales equivalen a 1 hora en el juego

    private void Start()
    {
        if (_sun != null)
        {
            _sun.localEulerAngles = new Vector3(-90, 0, 0);
        }

        StartCoroutine(UpdateTime());
    }

    private IEnumerator UpdateTime()
    {
        while (true) // Bucle infinito controlado
        {
            yield return new WaitForSeconds(realSecondsPerHour);

            _hoursInGame += 0.06f;

            if (_hoursInGame >= 24)
            {
                _hoursInGame = 0; // Reiniciar el día
            }

            RotateSun();
        }
    }

    private void RotateSun()
    {
        if (_sun != null)
        {
            float rotationX = (_hoursInGame / 24f) * 360f;
            _sun.localEulerAngles = new Vector3(rotationX - 90, 0, 0);
        }
    }
}
