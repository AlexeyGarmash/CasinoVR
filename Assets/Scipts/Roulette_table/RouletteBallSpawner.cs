using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteBallSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] BallSpawnPoints;
    [SerializeField] private GameObject BallPrefab;

    private GameObject _createdBallPrefab;

    public GameObject CreatedBallPrefab { get => _createdBallPrefab; }

    public void SpawnBall(int winNumber)
    {
        if(_createdBallPrefab != null)
        {
            Destroy(_createdBallPrefab);
            _createdBallPrefab = null;
        }

        if (BallSpawnPoints != null && BallSpawnPoints.Length > 0)
        {
            Vector3 spawnPos = BallSpawnPoints[Random.Range(0, BallSpawnPoints.Length)].position;
            _createdBallPrefab = Instantiate(BallPrefab, spawnPos, Quaternion.identity);
            _createdBallPrefab.GetComponent<BallTrigger>().winingNumber = winNumber;
        }
        else
        {
            print("No spawn points!");
        }
    }
}
