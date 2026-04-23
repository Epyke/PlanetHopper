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

        [SerializeField]
        private GameObject coinPrefab;
        private List<GameObject> currentCoins = new List<GameObject>();

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

            prevTile = GameObject.Instantiate(tile.gameObject, currentTileLocation, newTileRotation);
            currentTiles.Add(prevTile);

            if (spawnObstacle) SpawnObstacle();

            if (tile.type == TileType.STRAIGHT && !spawnObstacle)
            {
                SpawnCoin(); 
            }
            
            // (3,4,5) * (0,0,1) => (0,0,5)
            if (tile.type == TileType.STRAIGHT)
            {
                currentTileLocation += Vector3.Scale(prevTile.GetComponent<Renderer>().bounds.size, currentTileDirection);
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

            while (currentCoins.Count != 0)
            {
                GameObject coin = currentCoins[0];
                currentCoins.RemoveAt(0);
                if(coin != null) Destroy(coin); // Destrói apenas se não tiver sido apanhada
            }
        }

        public void AddNewDirection(Vector3 direction)
        {
            currentTileDirection = direction;
            DeletePreviousTiles();

            Vector3 tilePlacementScale;
            if (prevTile.GetComponent<Tile>().type == TileType.SIDEWAYS)
            {
                tilePlacementScale = Vector3.Scale(prevTile.GetComponent<Renderer>().bounds.size / 2 + (Vector3.one * startingTile.GetComponent<BoxCollider>().size.z / 2), currentTileDirection);
            }
            else
            {
                tilePlacementScale = Vector3.Scale(prevTile.GetComponent<Renderer>().bounds.size - (Vector3.one * 2) + (Vector3.one * startingTile.GetComponent<BoxCollider>().size.z / 2), currentTileDirection);
            }

            currentTileLocation += tilePlacementScale;

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

            GameObject obstacle = Instantiate(obstaclePrefab, currentTileLocation, newObjectRotation);
            currentObstacles.Add(obstacle);
        }

      private void SpawnCoin()
        {
            // A moeda tem 70% de probabilidade de aparecer neste pedaço de chão
            if (Random.value > 0.7f) return; 

            // Dá uma pequena variação na altura para não ficarem coladas ao chão
            Vector3 coinPosition = currentTileLocation + new Vector3(0, 0.5f, 0);

            GameObject coin = Instantiate(coinPrefab, coinPosition, Quaternion.identity);
            currentCoins.Add(coin);
        }

        private GameObject SelectRandomGameObjectFromList(List<GameObject> list)
        {
            if (list.Count == 0) return null;
            return list[Random.Range(0, list.Count)];
        }
    }
}

