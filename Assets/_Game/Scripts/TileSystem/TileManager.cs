using System.Collections.Generic;
using System.Linq;
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
        [Header("Tile Grid Settings")]
        [Tooltip("Width of the tile grid.")]
        [SerializeField, Range(2, 20)] private int _gridWidth = 8;

        [Tooltip("Height of the tile grid.")]
        [SerializeField, Range(2, 20)] private int _gridHeight = 8;

        [Tooltip("Prefab of the Tile to be instantiated.")]
        [SerializeField] private Tile _tilePrefab;

        [Header("Obstacle Settings")]
        [Tooltip("Prefab of the Obstacle to be instantiated at non-placeable positions.")]
        [SerializeField] private GameObject _obstaclePrefab;

        [Header("Grid Parent Settings")]
        [Tooltip("Transform to act as the parent for all created tiles.")]
        [SerializeField] private Transform _tileGridTransform;

        [Header("Non-Placeable Tile Settings")]
        [Tooltip("List of grid positions where tiles cannot be placed.")]
        [SerializeField] private List<Vector2> _nonPlaceableTilePositions = new List<Vector2>();

        private Tile[,] _tileGrid;

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
            _tileGrid = new Tile[_gridWidth, _gridHeight];
            Vector2 gridOffset = new Vector2((_gridWidth - 1) * 0.5f, (_gridHeight - 1) * 0.5f);

            for (int x = 0; x < _gridWidth; x++)
            {
                for (int z = 0; z < _gridHeight; z++)
                {
                    Vector3 tilePosition = new Vector3(x - gridOffset.x, 0, z - gridOffset.y) + _tileGridTransform.position;
                    Tile generatedTile = Instantiate(_tilePrefab, tilePosition, Quaternion.identity, _tileGridTransform);

                    generatedTile.Initialize(x, z, _nonPlaceableTilePositions.Contains(new Vector2(x, z)));

                    if (generatedTile.IsPlaceable == false)
                    {
                        InstantiateObstacle(tilePosition, generatedTile.transform, x, z);
                    }

                    _tileGrid[x, z] = generatedTile;
                }
            }
        }

        /// <summary>
        /// Instantiates an obstacle at the specified position.
        /// </summary>
        /// <param name="position">The position where the obstacle should be created.</param>
        /// <param name="parentTransform">The parent transform for the obstacle.</param>
        /// <param name="x">The x-coordinate of the obstacle's grid position.</param>
        /// <param name="z">The z-coordinate of the obstacle's grid position.</param>
        private void InstantiateObstacle(Vector3 position, Transform parentTransform, int x, int z)
        {
            GameObject obstacle = Instantiate(_obstaclePrefab, position, Quaternion.identity, parentTransform);
            obstacle.name = $"Obstacle_{x}_{z}";
        }

        /// <summary>
        /// Finds and returns the closest tile to the specified world position.
        /// This method calculates the distance from the given position to each tile
        /// and returns the tile with the shortest distance.
        /// </summary>
        /// <param name="position">The world position to find the closest tile to.</param>
        /// <returns>The Tile object that is closest to the specified position.</returns>
        public Tile GetClosestTile(Vector3 position)
        {
            // Use LINQ to find the tile with the minimum distance to the specified position
            Tile closestTile = _tileGrid.Cast<Tile>()
                                        .Where(tile => tile != null)
                                        .OrderBy(tile => Vector3.Distance(tile.transform.position, position))
                                        .FirstOrDefault();

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
                return _tileGrid[x, z];
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
                    Tile neighborTile = _tileGrid[newX, newZ];
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
            if (_gridWidth <= 0 || _gridHeight <= 0 || _tileGridTransform == null) return;

            Vector3 gridOffset = new Vector3((_gridWidth - 1) * 0.5f, 0, (_gridHeight - 1) * 0.5f);

            for (int x = 0; x < _gridWidth; x++)
            {
                for (int z = 0; z < _gridHeight; z++)
                {
                    Vector3 tilePosition = new Vector3(x - gridOffset.x, 0, z - gridOffset.z) + _tileGridTransform.position;
                    Gizmos.color = _nonPlaceableTilePositions.Contains(new Vector2(x, z)) ? Color.red : Color.green;
                    float cubeHeight = _nonPlaceableTilePositions.Contains(new Vector2(x, z)) ? 0.25f : 0.1f;
                    Gizmos.DrawCube(tilePosition + Vector3.up * (cubeHeight / 2), new Vector3(0.9f, cubeHeight, 0.9f));
                }
            }
        }
    }
}