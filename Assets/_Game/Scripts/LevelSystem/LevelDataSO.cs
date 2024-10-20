using UnityEngine;
using CS3D.CoinSystem;
using CS3D.TileSystem;
using System.Collections.Generic;

namespace CS3D.LevelSystem
{
    /// <summary>
    /// ScriptableObject that holds the data for a specific level, including configuration settings and tile map data.
    /// </summary>
    [CreateAssetMenu(fileName = "LevelData", menuName = "Data/LevelData")]
    public class LevelDataSO : ScriptableObject
    {
        [Header("Level Configuration")]
        [Tooltip("Contains settings related to level configuration such as coin stacks.")]
        [SerializeField] private LevelConfigurationData _levelConfigurationData;

        [Header("Tile Map Data")]
        [Tooltip("Contains settings related to the tile grid and non-placeable tiles.")]
        [SerializeField] private LevelTileMapData _levelTileMapData;

        [Header("Level Completion Settings")]
        [Tooltip("The score required to complete the level.")]
        [SerializeField] private int _requiredScore = 100;

        /// <summary>
        /// Gets the level configuration data for this level.
        /// </summary>
        public LevelConfigurationData LevelConfigurationData
        {
            get => _levelConfigurationData;
            private set => _levelConfigurationData = value;
        }

        /// <summary>
        /// Gets the tile map data for this level.
        /// </summary>
        public LevelTileMapData LevelTileMapData
        {
            get => _levelTileMapData;
            private set => _levelTileMapData = value;
        }

        public int RequiredScore
        {
            get => _requiredScore;
            private set => _requiredScore = value;
        }
    }

    /// <summary>
    /// Holds configuration settings for a level, such as coin stack properties.
    /// </summary>
    [System.Serializable]
    public class LevelConfigurationData
    {
        [Header("Coin Stack Configuration")]
        [Tooltip("List of configurations for different coin stacks in the level.")]
        [SerializeField] private List<CoinStackConfig> _coinStackList = new List<CoinStackConfig>();

        /// <summary>
        /// Gets or sets the list of coin stack configurations for this level.
        /// </summary>
        public List<CoinStackConfig> CoinStackList
        {
            get => _coinStackList;
            private set => _coinStackList = value;
        }
    }

    /// <summary>
    /// Holds the data and configuration for the tile grid in a level.
    /// </summary>
    [System.Serializable]
    public class LevelTileMapData
    {
        [Header("Tile Grid Settings")]
        [Tooltip("Width of the tile grid.")]
        [SerializeField, Range(2, 20)] private int _gridWidth = 6;

        [Tooltip("Height of the tile grid.")]
        [SerializeField, Range(2, 20)] private int _gridHeight = 7;

        private Tile[,] _grid; // The tile grid structure

        [Header("Non-Placeable Tile Settings")]
        [Tooltip("List of grid positions where tiles cannot be placed.")]
        [SerializeField] private List<Vector2> _nonPlaceableTilePositions = new List<Vector2>();

        /// <summary>
        /// Gets or sets the width of the tile grid.
        /// </summary>
        public int GridWidth
        {
            get => _gridWidth;
            private set => _gridWidth = Mathf.Clamp(value, 2, 20); // Ensures the grid width stays within the specified range
        }

        /// <summary>
        /// Gets or sets the height of the tile grid.
        /// </summary>
        public int GridHeight
        {
            get => _gridHeight;
            private set => _gridHeight = Mathf.Clamp(value, 2, 20); // Ensures the grid height stays within the specified range
        }

        /// <summary>
        /// Gets or privately sets the grid of tiles.
        /// </summary>
        public Tile[,] Grid
        {
            get => _grid;
            private set => _grid = value;
        }

        /// <summary>
        /// Gets or sets the list of non-placeable tile positions in the grid.
        /// </summary>
        public List<Vector2> NonPlaceableTilePositions
        {
            get => _nonPlaceableTilePositions;
            private set => _nonPlaceableTilePositions = value;
        }
    }
}