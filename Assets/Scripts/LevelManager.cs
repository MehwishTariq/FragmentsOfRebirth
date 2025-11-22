using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Utility.ObjectSpawner[] CharacterSpawner;

    public void SetUpLevel(GameObject player, CameraFollow cam)
    {
        foreach (Utility.ObjectSpawner spawner in CharacterSpawner)
        {
            if (spawner.ObjectToSpawn == Utility.Character.Player)
            {
                GameObject playerSpawned = Instantiate(player, spawner.PositionToSpawnAt.position, spawner.PositionToSpawnAt.rotation);
                playerSpawned.transform.SetParent(transform);
                cam.target = playerSpawned.transform;
                
            }
            else
            {
                //EnemyPoolerManager.PoolEnemies()
            }

        }
    }
    
}
