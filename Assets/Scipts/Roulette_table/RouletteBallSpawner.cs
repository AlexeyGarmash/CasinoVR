using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteBallSpawner : MonoBehaviourPun
{
    [SerializeField] private Transform[] BallSpawnPoints;
    [SerializeField] private GameObject BallPrefab;

    private GameObject _createdBallPrefab;

    public GameObject CreatedBallPrefab { get => _createdBallPrefab; }

    public void SpawnBall(int winNumber)
    {
        //photonView.RequestOwnership();
        if(_createdBallPrefab != null)
        {
            //
            //photonView.RPC("DestroyBall_RPC", RpcTarget.All);
            DestroyBall();
        }

        if (BallSpawnPoints != null && BallSpawnPoints.Length > 0)
        {
            //
            int randomSpawnPoint = Random.Range(0, BallSpawnPoints.Length);
            //photonView.RPC("SpawnBall_RPC", RpcTarget.All, winNumber, randomSpawnPoint);
            SpawnBall(winNumber, randomSpawnPoint);
        }
        else
        {
            print("No spawn points!");
        }
    }

    private void DestroyBall()
    {
        Destroy(_createdBallPrefab);
        _createdBallPrefab = null;
    }

    private void SpawnBall(int winNumber, int randSpPoint)
    {
        Vector3 spawnPos = BallSpawnPoints[randSpPoint].position;
        _createdBallPrefab = Instantiate(BallPrefab, spawnPos, Quaternion.identity);
        _createdBallPrefab.GetComponent<BallTrigger>().winingNumber = winNumber;
    }

    [PunRPC]
    public void DestroyBall_RPC()
    {
        Destroy(_createdBallPrefab);
        _createdBallPrefab = null;
    }

    [PunRPC]
    public void SpawnBall_RPC(int winNumber, int randSpPoint)
    {
        Vector3 spawnPos = BallSpawnPoints[randSpPoint].position;
        _createdBallPrefab = Instantiate(BallPrefab, spawnPos, Quaternion.identity);
        _createdBallPrefab.GetComponent<BallTrigger>().winingNumber = winNumber;
    }
}
