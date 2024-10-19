using System.Collections;
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

        [Header("Grid Parent Settings")]
        [Tooltip("Transform to act as the parent for all created tiles.")]
        [SerializeField] private Transform _tileGrid;

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
    }
}