using UnityEngine;

namespace _Game.InputHandling
{
    /// <summary>
    /// Handles player input, including mouse position and button events.
    /// Uses a PlayerInputSO ScriptableObject to trigger corresponding input events.
    /// This class is designed following SOLID principles for modular and testable code.
    /// </summary>
    public class InputHandler : MonoBehaviour
    {
        [Header("Input Settings")]
        [Tooltip("ScriptableObject for handling player input.")]
        [SerializeField] private PlayerInputSO _playerInput; // PlayerInput ScriptableObject to manage input events

        [Header("Mouse Raycast Settings")]
        [Tooltip("Layer mask used for raycasting to determine mouse position on the ground.")]
        [SerializeField] private LayerMask _groundLayerMask; // Layer to target for mouse raycast (e.g., ground)

        private Camera _mainCamera;

        private void Awake()
        {
            // Cache the main camera for performance optimization
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            HandleMousePositionInput();
            HandleMouseButtonInput();
        }

        /// <summary>
        /// Retrieves and forwards mouse position input to the PlayerInputSO.
        /// </summary>
        private void HandleMousePositionInput()
        {
            Vector2 mousePosition = GetMousePosition();
            _playerInput?.OnMousePositionInput.Invoke(mousePosition); // Safely invoke the event if it exists
        }

        /// <summary>
        /// Retrieves the mouse position in world space using a raycast.
        /// </summary>
        /// <returns>Vector2 representing the mouse position on the ground in world coordinates.</returns>
        private Vector2 GetMousePosition()
        {
            // Create a ray from the camera through the mouse position
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _groundLayerMask))
            {
                // Return the mouse position on the ground in 2D coordinates (x, z)
                return new Vector2(hit.point.x, hit.point.z);
            }
            return Vector2.zero; // Return a default value if no ground is hit
        }

        /// <summary>
        /// Handles mouse button inputs and invokes the corresponding events in the PlayerInputSO.
        /// </summary>
        private void HandleMouseButtonInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _playerInput?.OnLeftMouseButtonDown.Invoke(); // Trigger event for left mouse button down
            }
            if (Input.GetMouseButtonUp(0))
            {
                _playerInput?.OnLeftMouseButtonUp.Invoke(); // Trigger event for left mouse button up
            }
        }
    }
}