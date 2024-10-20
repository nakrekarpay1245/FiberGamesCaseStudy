using _Game._helpers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CS3D.TileSystem
{
    public class TileGrid : MonoBehaviour
    {
        [Header("Tile Grid Settings")]
        [Tooltip("Width of the tile grid.")]
        [SerializeField, Range(2, 20)] private int _gridWidth = 8;
        public int GridWidth { get => _gridWidth; set => _gridWidth = value; }

        [Tooltip("Height of the tile grid.")]
        [SerializeField, Range(2, 20)] private int _gridHeight = 8;
        public int GridHeight { get => _gridHeight; set => _gridHeight = value; }

        private Tile[,] _grid;
        public Tile[,] Grid { get => _grid; private set => _grid = value; }

        [Header("Non-Placeable Tile Settings")]
        [Tooltip("List of grid positions where tiles cannot be placed.")]
        [SerializeField] private List<Vector2> _nonPlaceableTilePositions = new List<Vector2>();
        public List<Vector2> NonPlaceableTilePositions { get => _nonPlaceableTilePositions; set => _nonPlaceableTilePositions = value; }

        private void Awake()
        {
            _grid = new Tile[_gridWidth, _gridHeight];
        }

        public void AddTile(int x, int z, Tile generatedTile)
        {
            _grid[x, z] = generatedTile;
        }

        /// <summary>
        /// Finds and returns the closest tile at the specified grid level (y index 0).
        /// This method calculates the distance from the given position to each tile at y index 0
        /// and returns the tile with the shortest distance. If no such tile exists, it returns null.
        /// </summary>
        /// <param name="position">The world position to find the closest tile to.</param>
        /// <returns>The Tile object that is closest to the specified position at y index 0, or null if none exist.</returns>
        public Tile GetStartTile(Vector3 position)
        {
            // Use LINQ to find the tile with the minimum distance to the specified position at y index 0
            Tile closestTile = _grid.Cast<Tile>()
                .Where(tile => tile != null && tile.TileGridPosition.y == 0) // Filter for tiles at y index 0
                .OrderBy(tile => Vector3.Distance(tile.transform.position, position)) // Order by distance
                .FirstOrDefault(); // Get the closest tile or null if none found

            return closestTile;
        }

        /// <summary>
        /// Gets the tile at the specified grid coordinates.
        /// </summary>
        /// <param name="x">The x-coordinate of the tile.</param>
        /// <param name="z">The z-coordinate of the tile.</param>
        /// <returns>The Tile object at the specified coordinates, or null if out of bounds.</returns>
        public Tile GetTileAt(int x, int z)
        {
            if (x >= 0 && x < _gridWidth && z >= 0 && z < _gridHeight)
            {
                return _grid[x, z];
            }

            return null;
        }

        /// <summary>
        /// Gets a list of neighboring tiles for a given tile.
        /// Uses predefined directions to check neighboring positions and adds them to the list.
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
                int newZ = tile.TileGridPosition.y + direction.y;

                if (newX >= 0 && newX < _gridWidth && newZ >= 0 && newZ < _gridHeight)
                {
                    Tile neighborTile = _grid[newX, newZ];
                    if (neighborTile != null)
                    {
                        neighbors.Add(neighborTile);
                    }
                }
            }

            return neighbors;
        }


        /// <summary>
        /// Draws Gizmos in the Scene view to visualize the tile grid.
        /// Visualizes placeable tiles in green and non-placeable tiles in red.
        /// </summary>
        private void OnDrawGizmos()
        {
            if (_gridWidth <= 0 || _gridHeight <= 0 || transform == null) return;

            Vector3 gridOffset = new Vector3((_gridWidth - 1) * 0.5f, 0, (_gridHeight - 1) * 0.5f);

            for (int x = 0; x < _gridWidth; x++)
            {
                for (int z = 0; z < _gridHeight; z++)
                {
                    Vector3 tilePosition = new Vector3(x - gridOffset.x, 0, z - gridOffset.z) + transform.position;
                    Gizmos.color = _nonPlaceableTilePositions.Contains(new Vector2(x, z)) ? Color.red : Color.green;
                    float cubeHeight = _nonPlaceableTilePositions.Contains(new Vector2(x, z)) ? 0.25f : 0.1f;
                    Gizmos.DrawCube(tilePosition + Vector3.up * (cubeHeight / 2), new Vector3(0.9f, cubeHeight, 0.9f));
                }
            }
        }
    }
}