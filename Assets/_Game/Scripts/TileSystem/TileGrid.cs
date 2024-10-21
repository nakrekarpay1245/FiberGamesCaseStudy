using _Game._helpers;
using CS3D._Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CS3D.TileSystem
{
    /// <summary>
    /// Manages the grid of tiles in the game, including initialization, retrieval, and neighbor calculation.
    /// Handles tile operations like fetching the closest tile, tile initialization, and updating grid structure.
    /// </summary>
    public class TileGrid : MonoBehaviour
    {
        [Header("Tile Grid Settings")]
        [Tooltip("Width of the tile grid.")]
        [SerializeField, Range(2, 20)]
        private int _gridWidth = 8;
        public int GridWidth
        {
            get => _gridWidth;
            set => _gridWidth = value;
        }

        [Tooltip("Height of the tile grid.")]
        [SerializeField, Range(2, 20)]
        private int _gridHeight = 8;
        public int GridHeight
        {
            get => _gridHeight;
            set => _gridHeight = value;
        }

        private Tile[,] _grid;
        public Tile[,] Grid
        {
            get => _grid;
            private set => _grid = value;
        }

        [Header("Non-Placeable Tile Settings")]
        [Tooltip("List of grid positions where tiles cannot be placed.")]
        [SerializeField]
        private List<Vector2> _nonPlaceableTilePositions = new List<Vector2>();
        public List<Vector2> NonPlaceableTilePositions
        {
            get => _nonPlaceableTilePositions;
            set => _nonPlaceableTilePositions = value;
        }

        private void Start()
        {
            GlobalBinder.singleton.LevelManager.OnLevelStart.AddListener(Initialize);
        }

        /// <summary>
        /// Initializes the tile grid based on the specified configurations.
        /// Ensures that any previously existing tiles are cleared before setting up a new grid.
        /// </summary>
        public void Initialize()
        {
            ClearExistingTiles();

            _grid = new Tile[_gridWidth, _gridHeight];
            InitializeNonPlaceableTiles();
        }

        /// <summary>
        /// Clears any existing tiles from the grid to prepare for a fresh initialization.
        /// </summary>
        private void ClearExistingTiles()
        {
            if (_grid == null) return;

            foreach (Tile tile in _grid)
            {
                if (tile != null)
                {
                    Destroy(tile.gameObject);
                }
            }
        }

        /// <summary>
        /// Marks specific tiles as non-placeable based on the configured positions.
        /// </summary>
        private void InitializeNonPlaceableTiles()
        {
            foreach (Vector2 position in _nonPlaceableTilePositions)
            {
                int x = (int)position.x;
                int y = (int)position.y;

                if (IsWithinGridBounds(x, y))
                {
                    Tile nonPlaceableTile = _grid[x, y];
                    if (nonPlaceableTile != null)
                    {
                        nonPlaceableTile.SetPlaceable(false);
                    }
                }
            }
        }

        /// <summary>
        /// Finds and returns the closest tile at the grid's y index 0 based on the specified position.
        /// </summary>
        /// <param name="position">The world position to find the closest tile to.</param>
        /// <returns>The closest placeable and unoccupied tile at y index 0, or null if none exist.</returns>
        public Tile GetStartTile(Vector3 position)
        {
            Tile closestTile = _grid.Cast<Tile>()
                .Where(tile => tile != null &&
                               tile.TileGridPosition.y == 0 &&
                               tile.IsPlaceable &&
                               !tile.IsOccupied &&
                               !tile.IsReserved)
                .OrderBy(tile => Vector3.Distance(tile.transform.position, position))
                .FirstOrDefault();

            if (closestTile == null)
            {
                Debug.LogWarning("No suitable tile found at y index 0. Check if tiles are either occupied, reserved, or not placeable.");
            }

            return closestTile;
        }

        /// <summary>
        /// Retrieves the tile at the specified grid coordinates.
        /// </summary>
        /// <param name="x">The x-coordinate of the tile.</param>
        /// <param name="z">The z-coordinate of the tile.</param>
        /// <returns>The Tile object at the specified coordinates, or null if out of bounds.</returns>
        public Tile GetTileAt(int x, int z)
        {
            return IsWithinGridBounds(x, z) ? _grid[x, z] : null;
        }

        /// <summary>
        /// Gets a list of neighboring tiles for a given tile using predefined directions.
        /// </summary>
        /// <param name="tile">The reference tile for which to find neighbors.</param>
        /// <returns>A list of neighboring Tile objects.</returns>
        public List<Tile> GetNeighbors(Tile tile)
        {
            List<Tile> neighbors = new List<Tile>();
            Vector2Int[] directions = { new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0), new Vector2Int(0, 1) };

            foreach (Vector2Int direction in directions)
            {
                int newX = tile.TileGridPosition.x + direction.x;
                int newY = tile.TileGridPosition.y + direction.y;

                if (IsWithinGridBounds(newX, newY))
                {
                    Tile neighborTile = _grid[newX, newY];
                    if (neighborTile != null)
                    {
                        neighbors.Add(neighborTile);
                    }
                }
            }

            return neighbors;
        }

        /// <summary>
        /// Adds a tile to the grid at the specified coordinates.
        /// </summary>
        /// <param name="x">The x-coordinate for the tile.</param>
        /// <param name="z">The z-coordinate for the tile.</param>
        /// <param name="tile">The tile to add to the grid.</param>
        public void AddTile(int x, int z, Tile tile)
        {
            if (IsWithinGridBounds(x, z))
            {
                _grid[x, z] = tile;
            }
        }

        /// <summary>
        /// Checks if the specified coordinates are within the bounds of the grid.
        /// </summary>
        /// <param name="x">The x-coordinate to check.</param>
        /// <param name="y">The y-coordinate to check.</param>
        /// <returns>True if within bounds, otherwise false.</returns>
        private bool IsWithinGridBounds(int x, int y)
        {
            return x >= 0 && x < _gridWidth && y >= 0 && y < _gridHeight;
        }

        /// <summary>
        /// Retrieves a tile from the grid that matches the specified level.
        /// </summary>
        /// <param name="level">The level to check for in the tiles.</param>
        /// <returns>The Tile object matching the specified level, or null if none exist.</returns>
        public Tile GetTileWithLevel(int level)
        {
            return _grid.Cast<Tile>().FirstOrDefault(tile => tile.Level == (CoinLevel)level);
        }
    }
}