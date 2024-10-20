using _Game._helpers;
using UnityEngine;

namespace CS3D.TileSystem
{
    /// <summary>
    /// Manages the creation and organization of the tile grid for the match-3 puzzle game.
    /// This class follows SOLID principles and ensures efficient tile grid management.
    /// It handles tile generation, non-placeable tile handling, and neighbor detection.
    /// </summary>
    public class TileManager : MonoBehaviour
    {
        [Tooltip("Prefab of the Tile to be instantiated.")]
        [SerializeField] private string _tilePrefabResourceKey = "Tile/Tile";

        /// <summary>
        /// Initializes the tile grid when the game starts.
        /// </summary>
        private void Start()
        {
            GenerateTileGrid();
        }

        /// <summary>
        /// Generates the tile grid based on the defined width and height.
        /// Instantiates tiles and handles the creation of obstacles for non-placeable tiles.
        /// </summary>
        private void GenerateTileGrid()
        {
            int gridWidth = GlobalBinder.singleton.TileGrid.GridWidth;
            int gridHeight = GlobalBinder.singleton.TileGrid.GridHeight;
            Vector3 tileGridPosition = GlobalBinder.singleton.TileGrid.transform.position;

            Vector2 gridOffset = new Vector2((gridWidth - 1) * 0.5f, (gridHeight - 1) * 0.5f);

            for (int x = 0; x < gridWidth; x++)
            {
                for (int z = 0; z < gridHeight; z++)
                {
                    Vector3 tilePosition = new Vector3(x - gridOffset.x, 0, z - gridOffset.y) +
                        tileGridPosition;

                    // Load the tile prefab from the Resources folder using the resource key
                    Tile tilePrefab = Resources.Load<Tile>(_tilePrefabResourceKey);

                    // Check if the tile prefab is successfully loaded
                    if (tilePrefab == null)
                    {
                        Debug.LogError($"Tile prefab not found at path: {_tilePrefabResourceKey}");
                        return;
                    }

                    // Instantiate the tile prefab at the given position with no rotation
                    Tile generatedTile = Instantiate(tilePrefab, tilePosition,
                        Quaternion.identity, GlobalBinder.singleton.TileGrid.transform);

                    // Additional initialization of the tile if necessary
                    generatedTile?.Initialize(x, z, 
                        GlobalBinder.singleton.TileGrid.NonPlaceableTilePositions.Contains(new Vector2(x, z)));

                    GlobalBinder.singleton.TileGrid.AddTile(x, z, generatedTile);
                }
            }
        }
    }
}