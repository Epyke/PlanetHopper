using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TempleRun
{
    public class TileSpawner : MonoBehaviour
    {
        [SerializeField]
        private int tileStartCount = 3;
        [SerializeField]
        private int minimumStraightTiles = 3;
        [SerializeField]
        private int maximumStraightTiles = 15;
        [SerializeField]
        private GameObject startingTile;
        [SerializeField]
        private List<GameObject> turnTiles;
        [SerializeField]
        private List<GameObject> obstacles;

        [SerializeField] private float middleObstacleSafetyDistance = 15f;
        private float laneDistance = 2.5f;
        private float lastMiddleObstacleDistance = -999f;
        private Vector3 currentTileLocation = Vector3.zero;
        private Vector3 currentTileDirection = Vector3.forward;
        private GameObject prevTile;

        private List<GameObject> currentTiles;
        private List<GameObject> currentObstacles;

        private void Start()
        {
            currentTiles = new List<GameObject>();
            currentObstacles = new List<GameObject>();

            Random.InitState(System.DateTime.Now.Millisecond);

            for (int i = 0; i < tileStartCount; i++)
            {
                if (i < 3)
                {
                    SpawnTile(startingTile.GetComponent<Tile>(), false);
                }
                else
                {
                    SpawnTile(startingTile.GetComponent<Tile>(), true);
                }
            }

            SpawnTile(SelectRandomGameObjectFromList(turnTiles).GetComponent<Tile>(), true);
        }

        private void SpawnTile(Tile tile, bool spawnObstacle)
        {

            Quaternion newTileRotation = tile.gameObject.transform.rotation * Quaternion.LookRotation(currentTileDirection, Vector3.up);

            Vector3 entryOffset = newTileRotation * tile.entryPoint.localPosition;
            Vector3 spawnPos = currentTileLocation - entryOffset;

            prevTile = GameObject.Instantiate(tile.gameObject, spawnPos, newTileRotation);
            currentTiles.Add(prevTile);

            if (spawnObstacle) SpawnObstacle();


            if (tile.exitPoint)
            {
                currentTileLocation = prevTile.transform.position + newTileRotation * tile.exitPoint.localPosition;
            }
        }

        private void DeletePreviousTiles()
        {
            while (currentTiles.Count != 1)
            {
                GameObject tile = currentTiles[0];
                currentTiles.RemoveAt(0);
                Destroy(tile);
            }

            while (currentObstacles.Count != 0)
            {
                GameObject obstacle = currentObstacles[0];
                currentObstacles.RemoveAt(0);
                Destroy(obstacle);
            }
        }

        public void AddNewDirection(Vector3 direction)
        {
            currentTileDirection = direction;
            DeletePreviousTiles();

            Tile prevTileComponent = prevTile.GetComponent<Tile>();
            if (prevTileComponent.type == TileType.SIDEWAYS)
            {
                float cross = Vector3.Cross(Vector3.forward, direction).y;

                Transform chosenExit = cross > 0 ?
                    prevTileComponent.exitPointLeft :
                    prevTileComponent.exitPointRight;

                currentTileLocation = prevTile.transform.position
                    + prevTile.transform.rotation * chosenExit.localPosition;
            }

            int currenPathLength = Random.Range(minimumStraightTiles, maximumStraightTiles);
            for (int i = 0; i < currenPathLength; ++i)
            {
                SpawnTile(startingTile.GetComponent<Tile>(),
                    (i == 0 || i == currenPathLength - 1) ? false : true);
            }

            SpawnTile(SelectRandomGameObjectFromList(turnTiles).GetComponent<Tile>(), false);
        }

        private void SpawnObstacle()
        {
            if (Random.value > 0.8f) return;

            GameObject obstaclePrefab = SelectRandomGameObjectFromList(obstacles);
            Obstacle obstacleData = obstaclePrefab.GetComponent<Obstacle>();

            Quaternion newObjectRotation = obstaclePrefab.transform.rotation * Quaternion.LookRotation(currentTileDirection, Vector3.up);
            Vector3 rightDirection = Vector3.Cross(Vector3.up, currentTileDirection).normalized;

            if (obstacleData != null && obstacleData.spawnType == ObstacleSpawnType.MiddleOnly)
            {
                float distanceSinceLastMiddle = Vector3.Distance(currentTileLocation, new Vector3(lastMiddleObstacleDistance, currentTileLocation.y, currentTileLocation.z));

                if (distanceSinceLastMiddle < middleObstacleSafetyDistance)
                {
                    return;
                }

                GameObject obstacle = Instantiate(obstaclePrefab, currentTileLocation, newObjectRotation);
                currentObstacles.Add(obstacle);
                lastMiddleObstacleDistance = currentTileLocation.x;
            }
            else if (obstacleData != null && obstacleData.spawnType == ObstacleSpawnType.SidesOnly)
            {
                for (int lane = 0; lane <= 2; lane += 2)
                {
                    float laneOffset = (lane - 1) * laneDistance;
                    Vector3 spawnPosition = currentTileLocation + (rightDirection * laneOffset);
                    GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, newObjectRotation);
                    currentObstacles.Add(obstacle);
                }
            }
            else
            {
                int randomLane = Random.Range(0, 3);
                float laneOffset = (randomLane - 1) * laneDistance;
                Vector3 spawnPosition = currentTileLocation + (rightDirection * laneOffset);
                GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, newObjectRotation);
                currentObstacles.Add(obstacle);
            }
        }

        private GameObject SelectRandomGameObjectFromList(List<GameObject> list)
        {
            if (list.Count == 0) return null;
            return list[Random.Range(0, list.Count)];
        }
    }
}

