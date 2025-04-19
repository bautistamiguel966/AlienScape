using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AI;

public class SoldierSpawner : MonoBehaviour
{
    public GameObject soldierPrefab;
    public int numberOfSoldiers = 5;
    private Transform player;
    public Transform[] waypoints;
    public float minSpawnDistance = 3f; // Distancia mínima entre soldados

    private List<Vector3> usedPositions = new List<Vector3>();

    private void Start()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogError("⚠ No hay waypoints asignados en SoldierSpawner.");
            return;
        }

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        SpawnSoldiers();
    }

    private void SpawnSoldiers()
    {
        int spawned = 0;
        int attempts = 0;

        while (spawned < numberOfSoldiers && attempts < numberOfSoldiers * 5)
        {
            Transform spawnPoint = waypoints[Random.Range(0, waypoints.Length)];
            Vector3 spawnPosition = spawnPoint.position;

            if (!IsPositionOccupied(spawnPosition))
            {
                if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit hit, 5f, NavMesh.AllAreas))
                {
                    spawnPosition = hit.position;
                }

                GameObject soldier = Instantiate(soldierPrefab, spawnPosition, Quaternion.identity);
                SoldierController soldierController = soldier.GetComponent<SoldierController>();

                if (soldierController != null)
                {
                    soldierController.player = player;
                    soldierController.waypoints = waypoints.OrderBy(x => Random.value).ToArray();
                }

                usedPositions.Add(spawnPosition);
                spawned++;
            }

            attempts++;
        }
    }

    private bool IsPositionOccupied(Vector3 position)
    {
        foreach (Vector3 usedPos in usedPositions)
        {
            if (Vector3.Distance(usedPos, position) < minSpawnDistance)
            {
                return true;
            }
        }
        return false;
    }
}
