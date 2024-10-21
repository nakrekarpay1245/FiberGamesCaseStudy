using _Game._helpers;
using CS3D.LevelSystem;
using UnityEngine;

namespace _Game.InputHandling
{
    /// <summary>
    /// Handles player input, including mouse position and button events.
    /// Utilizes a PlayerInputSO ScriptableObject to trigger corresponding input events.
    /// This class is designed following SOLID principles for modular and testable code.
    /// It provides functionality to temporarily lock and unlock input handling.
    /// </summary>
    public class InputHandler : MonoBehaviour
    {
        [Header("Input Settings")]
        [Tooltip("ScriptableObject for handling player input events.")]
        [SerializeField] private PlayerInputSO _playerInput; // PlayerInput ScriptableObject to manage input events

        private bool _inputLocked; // Flag to indicate whether input is currently locked

        private void Start()
        {
            GlobalBinder.singleton.LevelManager.OnLevelStart.AddListener(Initialize);
        }

        public void Initialize()
        {
            UnlockInput();
        }

        private void Update()
        {
            if (!_inputLocked) // Check if input is not locked before processing
            {
                HandleMousePositionInput();
                HandleMouseButtonInput();
            }
        }

        /// <summary>
        /// Temporarily locks the input handling to prevent player input processing.
        /// Call this method to disable input events until UnlockInput is called.
        /// </summary>
        public void LockInput()
        {
            Debug.Log("LockInput!");
            _inputLocked = true; // Set the input locked flag to true
        }

        /// <summary>
        /// Unlocks the input immediately, allowing player input processing to resume.
        /// Call this method to re-enable input events.
        /// </summary>
        public void UnlockInput()
        {
            Debug.Log("UnlockInput!");
            _inputLocked = false; // Set the input locked flag to false
        }

        /// <summary>
        /// Retrieves and forwards the mouse position input to the PlayerInputSO.
        /// Passes the raw mouse screen position (in pixels) to the input system.
        /// </summary>
        private void HandleMousePositionInput()
        {
            Vector2 mousePosition = GetMousePosition();
            _playerInput?.OnMousePositionInput.Invoke(mousePosition); // Safely invoke the event if it exists
        }

        /// <summary>
        /// Retrieves the current mouse position from the screen coordinates.
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