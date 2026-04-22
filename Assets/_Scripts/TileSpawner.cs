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
        private float laneDistance = 2.5f;
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

            currentTileLocation = prevTile.transform.position + newTileRotation * tile.exitPoint.localPosition;
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
                Transform chosenExit = cross < 0 ?
                prevTileComponent.exitPointLeft :
                prevTileComponent.exitPointRight;

                currentTileLocation = prevTile.transform.position + prevTile.transform.rotation * chosenExit.localPosition;
            }

            int currenPathLength = Random.Range(minimumStraightTiles, maximumStraightTiles);
            for (int i = 0; i < currenPathLength; ++i)
            {

                SpawnTile(startingTile.GetComponent<Tile>(), (i == 0 | i == currenPathLength - 1) ? false : true);
            }

            SpawnTile(SelectRandomGameObjectFromList(turnTiles).GetComponent<Tile>(), false);
        }

        private void SpawnObstacle()
        {
            if (Random.value > 0.5f) return;

            GameObject obstaclePrefab = SelectRandomGameObjectFromList(obstacles);

            Quaternion newObjectRotation = obstaclePrefab.transform.rotation * Quaternion.LookRotation(currentTileDirection, Vector3.up);

            int randomLane = Random.Range(0, 3);

            Vector3 rightDirection = Vector3.Cross(Vector3.up, currentTileDirection).normalized;

            float laneOffset = (randomLane - 1) * laneDistance;

            Vector3 spawnPosition = currentTileLocation + (rightDirection * laneOffset);

            GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, newObjectRotation);

            currentObstacles.Add(obstacle);
        }

        private GameObject SelectRandomGameObjectFromList(List<GameObject> list)
        {
            if (list.Count == 0) return null;
            return list[Random.Range(0, list.Count)];
        }
    }
}

