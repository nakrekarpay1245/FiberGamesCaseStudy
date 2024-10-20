using CS3D.TileSystem;
using System.Collections.Generic;
using UnityEngine;

namespace CS3D.TileSystem
{
    [System.Serializable]
    public class TileMapData
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

        public TileMapData(Vector2Int gridSize)
        {
            _gridWidth = gridSize.x;
            _gridHeight = gridSize.y;

            Debug.Log("Tile generate: " + gridSize);

            _grid = new Tile[_gridWidth, _gridHeight];
        }
    }
}