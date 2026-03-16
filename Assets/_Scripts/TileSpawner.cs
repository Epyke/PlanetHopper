using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TempleRun
{
    public class TileSpawner : MonoBehaviour
    {
        [SerializeField]
        private int tileStartCount = 10;
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
                SpawnTile(startingTile.GetComponent<Tile>(), false);
            }

            SpawnTile(turnTiles[0].GetComponent<Tile>());
            AddNewDirection(Vector3.left);
            //SpawnTile(SelectRandomGameObjectFromList(turnTiles).GetComponent<Tile>());
        }

        private void SpawnTile(Tile tile, bool spawnObstacle = false)
        {
            Quaternion newTileRotation = tile.gameObject.transform.rotation * Quaternion.LookRotation(currentTileDirection, Vector3.up);

            prevTile = GameObject.Instantiate(tile.gameObject, currentTileLocation, newTileRotation);
            currentTiles.Add(prevTile);
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
                SpawnTile(startingTile.GetComponent<Tile>(), (i == 0) ? false : true);
            }

            SpawnTile(SelectRandomGameObjectFromList(turnTiles).GetComponent<Tile>(), false);
        }

        private GameObject SelectRandomGameObjectFromList(List<GameObject> list)
        {
            if (list.Count == 0) return null;
            return list[Random.Range(0, list.Count)];
        }
    }
}

