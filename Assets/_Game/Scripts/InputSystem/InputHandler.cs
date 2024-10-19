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

        private void Update()
        {
            HandleMousePositionInput();
            HandleMouseButtonInput();
        }

        /// <summary>
        /// Retrieves and forwards mouse position input to the PlayerInputSO.
        /// Passes the raw mouse screen position (not world space) to the input system.
        /// </summary>
        private void HandleMousePositionInput()
        {
            Vector2 mousePosition = GetMousePosition();
            _playerInput?.OnMousePositionInput.Invoke(mousePosition); // Safely invoke the event if it exists
        }

        /// <summary>
        /// Retrieves the raw mouse position from the screen coordinates.
        /// This is a 2D position (in pixels) representing where the mouse is on the screen.
        /// </summary>
        /// <returns>Vector2 representing the mouse position on the screen.</returns>
        private Vector2 GetMousePosition()
        {
            Vector3 mousePosition = Input.mousePosition; // Retrieves the raw screen-space position of the mouse
            return mousePosition; // Converts to Vector2 as required for the event
        }

        /// <summary>
        /// Handles mouse button inputs and invokes the corresponding events in the PlayerInputSO.
        /// This processes both mouse button down and mouse button up events for the left mouse button.
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