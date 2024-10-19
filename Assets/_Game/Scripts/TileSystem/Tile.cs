using System.Collections;
using System.Collections.Generic;
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
    }
}