using CS3D._Enums;
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

        [Header("Obstacle Settings")]
        [Tooltip("Prefab of the Obstacle to be instantiated at non-placeable positions.")]
        [SerializeField] private string _tileObstaclePrefabResourceKey = "Tile/TileObstacle";

        private Vector2Int _tileGridPosition;

        [SerializeField] private CoinStack _coinStack;
        public CoinStack CoinStack
        {
            get => _coinStack;
            set => _coinStack = value;
        }

        /// <summary>
        /// Gets the CoinLevel of the coin at the top of the stack.
        /// Returns null if the stack is empty.
        /// </summary>
        public CoinLevel? Level
        {
            get => CoinStack.Level;
        }

        /// <summary>
        /// Gets the count of coins in the stack that have the same level as the top coin.
        /// If the stack is empty, returns 0.
        /// </summary>
        public int Weight
        {
            get
            {
                return CoinStack ? CoinStack.Weight : -1;
            }
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
            get => CoinStack;
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
            if (isNonPlaceable)
            {
                InstantiateObstacle();
            }
        }

        /// <summary>
        /// Instantiates an obstacle at the specified position.
        /// </summary>
        private void InstantiateObstacle()
        {
            GameObject obstaclePrefab = Resources.Load<GameObject>(_tileObstaclePrefabResourceKey);
            GameObject obstacle = Instantiate(obstaclePrefab, transform.position, Quaternion.identity, transform);
            obstacle.name = $"Obstacle_{_tileGridPosition.x}_{_tileGridPosition.y}";
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

        public void ControlCoinStack()
        {
            if (Weight <= 0)
            {
                _coinStack = null;
                _isReserved = false;
                _isOccupied = false;
            }
        }

        /// <summary>
        /// Checks if the coin stack's weight exceeds the specified limit.
        /// If the weight is greater than or equal to the defined threshold, 
        /// it removes and destroys coins from the top of the stack until the weight falls below the limit.
        /// </summary>
        public void EnsureCoinWeightLimit()
        {
            _coinStack?.CheckAndDestroyTopCoinsIfWeightExceedsLimit();
        }
    }
}