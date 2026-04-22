using UnityEngine;

namespace TempleRun
{
    public enum ObstacleSpawnType
    {
        Any,
        MiddleOnly,
        SidesOnly
    }

    public class Obstacle : MonoBehaviour
    {
        public ObstacleSpawnType spawnType = ObstacleSpawnType.Any;
    }
}