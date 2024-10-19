using UnityEngine;
using UnityEngine.Events;

namespace _Game.InputHandling
{
    /// <summary>
    /// ScriptableObject that manages player input events for mouse interactions.
    /// Allows different parts of the game to subscribe to mouse position and button events.
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerInput", menuName = "Data/PlayerInput")]
    public class PlayerInputSO : ScriptableObject
    {
        [Header("Mouse Position Event")]
        [Tooltip("Event triggered when mouse position input is received.")]
        public UnityEvent<Vector2> OnMousePositionInput = new UnityEvent<Vector2>();

        [Header("Mouse Button Events")]
        [Tooltip("Event triggered when the left mouse button is pressed down.")]
        public UnityEvent OnLeftMouseButtonDown = new UnityEvent();

        [Tooltip("Event triggered when the left mouse button is released.")]
        public UnityEvent OnLeftMouseButtonUp = new UnityEvent();

        private void OnEnable()
        {
            // Initialize events to ensure they are never null
            OnMousePositionInput ??= new UnityEvent<Vector2>();
            OnLeftMouseButtonDown ??= new UnityEvent();
            OnLeftMouseButtonUp ??= new UnityEvent();
        }

        /// <summary>
        /// Clears all registered listeners for the input events. Useful for resetting event states.
        /// </summary>
        public void ClearAllListeners()
        {
            OnMousePositionInput.RemoveAllListeners();
            OnLeftMouseButtonDown.RemoveAllListeners();
            OnLeftMouseButtonUp.RemoveAllListeners();
        }
    }
}