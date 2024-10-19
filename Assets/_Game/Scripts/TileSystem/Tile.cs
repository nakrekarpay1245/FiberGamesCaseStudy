using CS3D.CoinSystem;
using UnityEngine;

namespace CS3D.TileSystem
{
    /// <summary>
    /// Represents a tile in the match-3 puzzle game. The tile can have other objects placed on it
    /// and can be marked as occupied or not. This class follows SOLID principles, ensuring
    /// encapsulation and data integrity.
    /// </summary>
    public class Tile : MonoBehaviour
    {
        [Header("Tile State Settings")]
        [Tooltip("Indicates whether an object can be placed on this tile.")]
        [SerializeField] private bool _isPlaceable = true;

        [Tooltip("Indicates whether this tile is currently occupied by another object.")]
        [SerializeField] private bool _isOccupied = false;

        [Tooltip("Indicates whether this tile is currently reserved by another object.")]
        [SerializeField] private bool _isReserved = false;

        [Header("Path Visualizer")]
        [Tooltip("Reference to the path visualizer object for this tile.")]
        [SerializeField] private GameObject _pathVisualizer;

        private Vector2Int _tileGridPosition;

        [SerializeField] private CoinStack _coinStack;
        public CoinStack CoinStack
        {
            get => _coinStack;
            set => _coinStack = value;
        }

        /// <summary>
        /// Gets the grid position of this tile as a Vector2Int.
        /// </summary>
        public Vector2Int TileGridPosition
        {
            get => _tileGridPosition;
            private set => _tileGridPosition = value;
        }

        /// <summary>
        /// Gets a value indicating whether an object can be placed on this tile.
        /// This property is read-only.
        /// </summary>
        public bool IsPlaceable
        {
            get => _isPlaceable;
            private set => _isPlaceable = value;
        }

        /// <summary>
        /// Gets a value indicating whether this tile is currently occupied by another object.
        /// This property is read-only.
        /// </summary>
        public bool IsOccupied
        {
            get => _isOccupied;
            private set => _isOccupied = value;
        }

        /// <summary>
        /// Gets a value indicating whether this tile is currently reserved by another object.
        /// This property is read-only.
        /// </summary>
        public bool IsReserved
        {
            get => _isReserved;
            private set => _isReserved = value;
        }

        /// <summary>
        /// Initializes the tile with its grid position and its placeable status.
        /// This method sets up the tile's position in the grid and defines whether it is placeable or not.
        /// </summary>
        /// <param name="x">The x-coordinate of the tile in the grid.</param>
        /// <param name="z">The z-coordinate of the tile in the grid.</param>
        /// <param name="isNonPlaceable">Indicates whether the tile is non-placeable.</param>
        public void Initialize(int x, int z, bool isNonPlaceable)
        {
            _tileGridPosition = new Vector2Int(x, z);
            SetPlaceable(!isNonPlaceable);
        }

        /// <summary>
        /// Sets the occupied state of the tile.
        /// This method should be used to mark the tile as occupied or not based on gameplay logic.
        /// </summary>
        /// <param name="occupied">The new occupied state of the tile.</param>
        public void SetOccupied(bool occupied)
        {
            _isOccupied = occupied;
        }

        /// <summary>
        /// Sets whether the tile can have objects placed on it.
        /// This method should be used when updating tile availability in gameplay logic.
        /// </summary>
        /// <param name="placeable">The new placeable state of the tile.</param>
        public void SetPlaceable(bool placeable)
        {
            _isPlaceable = placeable;
        }

        /// <summary>
        /// Sets the reserved state of the tile.
        /// This method should be used to mark the tile as reserved or not for specific operations.
        /// </summary>
        /// <param name="reserved">The new reserved state of the tile.</param>
        public void SetReserved(bool reserved)
        {
            _isReserved = reserved;
        }

        /// <summary>
        /// Makes the path visualizer visible on this tile.
        /// </summary>
        public void PathVisible()
        {
            if (_pathVisualizer != null)
            {
                _pathVisualizer.SetActive(true);
            }
        }

        /// <summary>
        /// Hides the path visualizer on this tile.
        /// </summary>
        public void PathHidden()
        {
            if (_pathVisualizer != null)
            {
                _pathVisualizer.SetActive(false);
            }
        }

        public void ControlCoinStack()
        {
            if (_coinStack.CurrentCoinsInStack.Count <= 0)
            {
                _coinStack = null;
                _isReserved = false;
                _isOccupied = false;
            }
        }
    }
}