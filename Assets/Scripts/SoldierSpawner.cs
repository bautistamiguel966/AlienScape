using UnityEngine;

public class SoldierSpawner : MonoBehaviour
{
    public GameObject soldierPrefab;
    public int numberOfSoldiers = 5; // Cantidad de Soldiers a generar
    public Vector3 spawnAreaCenter; // Centro del área de spawn
    public Vector3 spawnAreaSize; // Tamaño del área donde pueden aparecer
    private Transform player; // Referencia al Player

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        SpawnSoldiers();
    }

    private void SpawnSoldiers()
    {
        for (int i = 0; i < numberOfSoldiers; i++)
        {
            Vector3 randomPosition = spawnAreaCenter + new Vector3(
                Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
                0, // Mantener en el suelo
                Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
            );

            GameObject soldier = Instantiate(soldierPrefab, randomPosition, Quaternion.identity);

            // 🔹 Asignar el `Player` al `Soldier`
            SoldierController soldierController = soldier.GetComponent<SoldierController>();
            if (soldierController != null && player != null)
            {
                soldierController.player = player;
            }
        }
    }
}
