using UnityEngine;
using UnityEngine.Serialization;

public class SpawnManager : MonoBehaviour
{
    public Transform[] spawnPointsPlayer;
    public Transform[] spawnPointsEnemy;

    public static SpawnManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else 
            Destroy(this.gameObject);
    }

    public Transform GetSpawnPointPlayer()
    {
        int ran = Random.Range(0, spawnPointsPlayer.Length);
        return spawnPointsPlayer[ran];
    }
    
    public Transform GetSpawnPointEnemy()
    {
        int ran = Random.Range(0, spawnPointsPlayer.Length);
        return spawnPointsEnemy[ran];
    }
}
