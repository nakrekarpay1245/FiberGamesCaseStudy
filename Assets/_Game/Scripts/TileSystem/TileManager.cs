using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace CS3D.TileSystem
{
    /// <summary>
    /// Manages the creation and organization of the tile grid for the match-3 puzzle game.
    /// This class follows SOLID principles and ensures efficient tile grid management.
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
        [SerializeField] private Transform _tileGrid;

        [Header("Non-Placeable Tile Settings")]
        [Tooltip("List of grid positions where tiles cannot be placed.")]
        [SerializeField] private List<Vector2> _nonPlaceableTilePositions = new List<Vector2>();

        private Tile[,] _tiles;

        /// <summary>
        /// Initializes the tile grid based on the specified width and height.
        /// </summary>
        private void Start()
        {
            GenerateTileGrid();
        }

        /// <summary>
        /// Generates a grid of tiles based on the specified width and height, and positions
        /// them within the _tileGrid transform, centering the grid.
        /// Sets the IsPlaceable property to false for non-placeable tiles and generates obstacles.
        /// </summary>
        private void GenerateTileGrid()
        {
            _tiles = new Tile[_gridWidth, _gridHeight];

            Vector2 gridOffset = new Vector2((_gridWidth - 1) * 0.5f, (_gridHeight - 1) * 0.5f);

            for (int x = 0; x < _gridWidth; x++)
            {
                for (int z = 0; z < _gridHeight; z++)
                {
                    Vector3 tilePosition = new Vector3(x - gridOffset.x, 0, z - gridOffset.y) + _tileGrid.position;
                    Tile generatedTile = Instantiate(_tilePrefab, tilePosition, Quaternion.identity, _tileGrid);

                    generatedTile.name = $"Tile_{x}_{z}";

                    // Check if the current position is non-placeable and update the IsPlaceable property
                    if (_nonPlaceableTilePositions.Contains(new Vector2(x, z)))
                    {
                        generatedTile.SetPlaceable(false);

                        // Instantiate the obstacle prefab at the non-placeable position
                        GameObject obstacle = Instantiate(_obstaclePrefab, tilePosition, Quaternion.identity, generatedTile.transform);
                        obstacle.name = $"Obstacle_{x}_{z}"; // Naming the obstacle for easier identification
                    }

                    if (generatedTile != null)
                    {
                        _tiles[x, z] = generatedTile;
                    }
                }
            }
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
                return _tiles[x, z];
            }

            return null;
        }

        /// <summary>
        /// Draws Gizmos in the Scene view to visualize the tile grid.
        /// Green cubes represent placeable tiles, while red cubes indicate non-placeable tiles.
        /// The size and color of the cubes provide a clear distinction between the two types of tiles:
        /// - Non-placeable tiles are drawn as red cubes with dimensions (0.9f, 0.25f, 0.9f).
        /// - Placeable tiles are drawn as green cubes with dimensions (0.9f, 0.1f, 0.9f).
        /// The cubes are slightly raised above the ground for better visibility.
        /// This visualization assists in debugging and designing the tile grid layout.
        /// </summary>
        private void OnDrawGizmos()
        {
            // Check if Gizmos should be drawn based on the grid size
            if (_gridWidth <= 0 || _gridHeight <= 0) return;

            Vector3 gridOffset = new Vector3((_gridWidth - 1) * 0.5f, 0, (_gridHeight - 1) * 0.5f);

            for (int x = 0; x < _gridWidth; x++)
            {
                for (int z = 0; z < _gridHeight; z++)
                {
                    Vector3 tilePosition = new Vector3(x - gridOffset.x, 0, z - gridOffset.z) + _tileGrid.position;

                    // Check if the current position is non-placeable
                    if (_nonPlaceableTilePositions.Contains(new Vector2(x, z)))
                    {
                        // Draw a red cube for non-placeable tiles
                        Gizmos.color = Color.red;
                        Gizmos.DrawCube(tilePosition + Vector3.up * 0.125f, new Vector3(0.9f, 0.25f, 0.9f));
                    }
                    else
                    {
                        // Draw a green cube for placeable tiles
                        Gizmos.color = Color.green;
                        Gizmos.DrawCube(tilePosition + Vector3.up * 0.05f, new Vector3(0.9f, 0.1f, 0.9f));
                    }
                }
            }
        }
    }
}