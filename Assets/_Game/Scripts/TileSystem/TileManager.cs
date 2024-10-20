using UnityEngine;
using CS3D.Data;
using CS3D.TileSystem;
using System;
using _Game._helpers;

namespace CS3D.TileSystem
{
    /// <summary>
    /// Manages the creation and organization of the tile grid for the match-3 puzzle game.
    /// This class handles tile generation, non-placeable tile setup, and ensures efficient grid management.
    /// </summary>
    public class TileManager : MonoBehaviour
    {
        [Header("Game Data")]
        [Tooltip("The current game data containing level and tile configurations.")]
        [SerializeField] private GameData _gameData;

        [Header("Tile Prefab")]
        [Tooltip("Resource path for the Tile prefab to be instantiated.")]
        [SerializeField] private string _tilePrefabResourceKey = "Tile/Tile";

        private Tile[,] _tileGrid; // Internal representation of the tile grid

        /// <summary>
        /// Initializes the tile grid when the game starts.
        /// </summary>
        private void Start()
        {
            GenerateTileGrid();
        }

        /// <summary>
        /// Generates the tile grid based on the defined width and height from the level data.
        /// Instantiates tiles and handles the creation of non-placeable tiles (obstacles).
        /// </summary>
        private void GenerateTileGrid()
        {
            int gridWidth = _gameData.CurrentLevel.LevelTileMapData.GridWidth;
            int gridHeight = _gameData.CurrentLevel.LevelTileMapData.GridHeight;
            _tileGrid = new Tile[gridWidth, gridHeight];

            Vector3 gridOrigin = GlobalBinder.singleton.TileGrid.transform.position;
            Vector2 gridOffset = new Vector2((gridWidth - 1) * 0.5f, (gridHeight - 1) * 0.5f);

            Tile tilePrefab = LoadTilePrefab();
            if (tilePrefab == null)
            {
                Debug.LogError($"Failed to load tile prefab from Resources path: {_tilePrefabResourceKey}");
                return;
            }

            CreateTileGrid(gridWidth, gridHeight, gridOrigin, gridOffset, tilePrefab);
        }

        /// <summary>
        /// Loads the tile prefab from the Resources folder using the resource key.
        /// </summary>
        /// <returns>The loaded tile prefab or null if it fails to load.</returns>
        private Tile LoadTilePrefab()
        {
            Tile tilePrefab = Resources.Load<Tile>(_tilePrefabResourceKey);
            if (tilePrefab == null)
            {
                Debug.LogError($"Tile prefab not found at the specified path: {_tilePrefabResourceKey}");
            }
            return tilePrefab;
        }

        /// <summary>
        /// Creates the tile grid by instantiating each tile at the correct position in the grid.
        /// Sets non-placeable tiles according to the level's configuration.
        /// </summary>
        private void CreateTileGrid(int width, int height, Vector3 origin, Vector2 offset, Tile tilePrefab)
        {
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    Vector3 tilePosition = new Vector3(x - offset.x, 0, z - offset.y) + origin;
                    Tile generatedTile = InstantiateTile(tilePrefab, tilePosition);

                    bool isNonPlaceable = _gameData.CurrentLevel.LevelTileMapData
                        .NonPlaceableTilePositions.Contains(new Vector2(x, z));

                    InitializeTile(generatedTile, x, z, isNonPlaceable);
                }
            }
        }

        /// <summary>
        /// Instantiates a tile prefab at the given position with no rotation.
        /// </summary>
        /// <param name="tilePrefab">The tile prefab to instantiate.</param>
        /// <param name="position">The position to place the instantiated tile.</param>
        /// <returns>The instantiated Tile object.</returns>
        private Tile InstantiateTile(Tile tilePrefab, Vector3 position)
        {
            return Instantiate(tilePrefab, position, Quaternion.identity, GlobalBinder.singleton.TileGrid.transform);
        }

        /// <summary>
        /// Initializes a tile's grid position and non-placeable state.
        /// Adds the tile to the TileGrid.
        /// </summary>
        private void InitializeTile(Tile tile, int x, int z, bool isNonPlaceable)
        {
            if (tile == null)
            {
                Debug.LogWarning($"Attempted to initialize a null tile at position ({x}, {z}).");
                return;
            }

            tile.Initialize(x, z, isNonPlaceable);
            GlobalBinder.singleton.TileGrid.AddTile(x, z, tile);
            _tileGrid[x, z] = tile; // Store the tile in the internal grid for easy access
        }
    }
}