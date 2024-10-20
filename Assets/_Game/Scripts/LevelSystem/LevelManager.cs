using CS3D.Data;
using UnityEngine;
using UnityEngine.Events;

namespace CS3D.LevelSystem
{
    /// <summary>
    /// Manages the level states, specifically handling level completion and failure.
    /// It uses Unity Events to allow external components to respond to these events in a modular and decoupled manner.
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        [Header("Level Event Settings")]
        [Tooltip("Unity Event invoked when the level is completed.")]
        public UnityEvent<int> OnLevelComplete;

        [Tooltip("Unity Event invoked when the level fails.")]
        public UnityEvent<int> OnLevelFail;

        [Header("Level Settings")]
        [Tooltip("The current level index.")]
        [SerializeField] private GameData _gameData;

        private bool _isLevelEnded = false; // Flag to track if the level has already ended

        /// <summary>
        /// Called to trigger the level completion sequence.
        /// Executes the completion logic and invokes the Level Complete Unity Event.
        /// </summary>
        public void CompleteLevel()
        {
            if (_isLevelEnded)
            {
                Debug.LogWarning("Level has already ended. Completion cannot be triggered again.");
                return;
            }

            Debug.Log("Level Completed");
            _isLevelEnded = true; // Mark the level as ended
            OnLevelComplete?.Invoke(_gameData.CurrentLevelIndex);
        }

        /// <summary>
        /// Called to trigger the level failure sequence.
        /// Executes the failure logic and invokes the Level Fail Unity Event.
        /// </summary>
        public void FailLevel()
        {
            if (_isLevelEnded)
            {
                Debug.LogWarning("Level has already ended. Failure cannot be triggered again.");
                return;
            }

            Debug.Log("Level Failed");
            _isLevelEnded = true; // Mark the level as ended
            OnLevelFail?.Invoke(_gameData.CurrentLevelIndex);
        }

        /// <summary>
        /// Resets the level state to allow re-triggering of level events if necessary.
        /// This method can be called when restarting or moving to the next level.
        /// </summary>
        public void ResetLevelState()
        {
            _isLevelEnded = false; // Reset the level ended flag
            Debug.Log("Level state has been reset.");
        }
    }
}