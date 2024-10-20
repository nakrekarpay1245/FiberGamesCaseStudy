using CS3D.TileSystem;
using CS3D.Pathfinding;
using UnityEngine;
using _Game._helpers;
using DG.Tweening;
using TMPro.EditorUtilities;
using CS3D.CoinSystem;

namespace _Game.InputHandling
{
    /// <summary>
    /// Handles tile selection based on player input.
    /// Listens for input events from PlayerInputSO and performs a raycast to determine the clicked tile.
    /// </summary>
    public class TileSelector : MonoBehaviour
    {
        [Header("Input Settings")]
        [Tooltip("ScriptableObject for handling player input.")]
        [SerializeField] private PlayerInputSO _playerInput; // PlayerInput ScriptableObject to manage input events

        [Header("Raycast Settings")]
        [Tooltip("Layer mask used for raycasting to detect tiles.")]
        [SerializeField] private LayerMask _tileLayerMask; // Layer mask to target tiles

        private Vector2 _currentMousePosition; // Stores the latest mouse position from the PlayerInputSO

        private Pathfinder _currentPathfinder;

        private void OnEnable()
        {
            // Subscribe to PlayerInput events when this object is enabled
            if (_playerInput != null)
            {
                _playerInput.OnLeftMouseButtonDown.AddListener(HandleTileSelection);
                _playerInput.OnMousePositionInput.AddListener(UpdateMousePosition);
            }
        }

        private void OnDisable()
        {
            // Unsubscribe from PlayerInput events when this object is disabled
            if (_playerInput != null)
            {
                _playerInput.OnLeftMouseButtonDown.RemoveListener(HandleTileSelection);
                _playerInput.OnMousePositionInput.RemoveListener(UpdateMousePosition);
            }
        }

        /// <summary>
        /// Updates the current mouse position from the PlayerInputSO event.
        /// </summary>
        /// <param name="mousePosition">The mouse position in world coordinates.</param>
        private void UpdateMousePosition(Vector2 mousePosition)
        {
            _currentMousePosition = mousePosition;
        }

        /// <summary>
        /// Handles the tile selection when the player clicks on a point in the game world.
        /// Uses a raycast to detect the Tile at the clicked position.
        /// </summary>
        private void HandleTileSelection()
        {
            UpdatePathfinder();

            Ray ray = Camera.main.ScreenPointToRay(_currentMousePosition);
            RaycastHit hit;

            // Perform a raycast to detect objects on the tile layer
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _tileLayerMask))
            {
                // Check if the hit object has a Tile component
                Tile selectedTile = hit.collider.GetComponent<Tile>();

                if (selectedTile != null)
                {
                    if (_currentPathfinder.TryMoveToTile(selectedTile))
                    {
                        CoinStack coinStack = _currentPathfinder as CoinStack;
                        GlobalBinder.singleton.CoinStackManager.RemoveCoinStack(_currentPathfinder as CoinStack);

                        selectedTile.CoinStack = coinStack;
                        coinStack.Tile = selectedTile;

                        GlobalBinder.singleton.MatchChecker.CheckForMatches();
                    }

                    // Match control will be provided after the stack system is written.
                    //Debug.Log($"Tile selected: {selectedTile.name}");
                }
                else
                {
                    Debug.Log("No tile found at the clicked position.");
                }
            }
        }

        private void UpdatePathfinder()
        {
            _currentPathfinder = GlobalBinder.singleton.CoinStackManager.GetCoinStack();
        }
    }
}