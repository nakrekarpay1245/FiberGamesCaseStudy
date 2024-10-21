using UnityEngine;
using UnityEngine.Events;
using CS3D.Data;
using _Game._helpers;
using DG.Tweening;
using MaskTransitions;

namespace CS3D.LevelSystem
{
    /// <summary>
    /// Manages the level states, specifically handling level completion and failure.
    /// Uses Unity Events to trigger external reactions in a modular way.
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        [Header("Game Data")]
        [Tooltip("Reference to the current game data.")]
        [SerializeField] private GameData _gameData;

        [Header("Level Events")]
        [Tooltip("Event triggered when the level is started.")]
        public UnityEvent OnLevelStart;

        [Tooltip("Event triggered when the level is successfully completed.")]
        public UnityEvent<int> OnLevelComplete;

        [Tooltip("Event triggered when the level fails.")]
        public UnityEvent<int> OnLevelFail;

        [Header("Audio Settings")]
        [Tooltip("Sound key for level completion.")]
        [SerializeField] private string _levelCompleteSoundKey = "level_complete";

        [Tooltip("Sound key for level failure.")]
        [SerializeField] private string _levelFailSoundKey = "level_fail";

        private bool _isLevelEnded; // Flag to track if the level has ended
        private bool _isLevelStarted; // Flag to track if the level has ended

        private void Start()
        {
            Invoke(nameof(StartLevel), 0.01f);
        }

        public void StartLevel()
        {
            Sequence sequence = DOTween.Sequence();

            // Play the start transition
            sequence.AppendCallback(() => TransitionManager.Instance.PlayStartHalfTransition(GlobalBinder.singleton.TimeManager.SceneTransitionTime / 3, 0f));

            sequence.AppendInterval(GlobalBinder.singleton.TimeManager.SceneTransitionTime / 3);

            sequence.AppendCallback(() =>
            {
                if (!_isLevelStarted)
                {
                    _isLevelStarted = true;
                    _isLevelEnded = false;
                    OnLevelStart?.Invoke();
                    //temp
                    //Debug.Log("StartLevel!");
                }
            });

            // Wait for 1 second before loading the scene
            sequence.AppendInterval(GlobalBinder.singleton.TimeManager.SceneTransitionTime * 2 / 3);

            // Play the end transition
            sequence.AppendCallback(() => TransitionManager.Instance.PlayEndHalfTransition(GlobalBinder.singleton.TimeManager.SceneTransitionTime * 2 / 3, 0f));
        }

        /// <summary>
        /// Completes the current level, triggers the appropriate sound, and invokes the level complete event.
        /// </summary>
        public void CompleteLevel()
        {
            if (_isLevelEnded)
            {
                Debug.LogWarning("Level has already ended. Cannot trigger completion again.");
                return;
            }

            ResetLevelState();
            //temp
            //Debug.Log("Level Completed");

            // Play completion sound using the audio manager service
            GlobalBinder.singleton.AudioManager.PlaySound(_levelCompleteSoundKey);
            OnLevelComplete?.Invoke(_gameData.CurrentLevelIndex);
            _gameData.CurrentLevelIndex++;
        }

        /// <summary>
        /// Fails the current level, triggers the appropriate sound, and invokes the level fail event.
        /// </summary>
        public void FailLevel()
        {
            if (_isLevelEnded)
            {
                Debug.LogWarning("Level has already ended. Cannot trigger failure again.");
                return;
            }

            ResetLevelState();
            //temp
            //Debug.Log("Level Failed");

            // Play failure sound using the audio manager service
            GlobalBinder.singleton.AudioManager.PlaySound(_levelFailSoundKey);
            OnLevelFail?.Invoke(_gameData.CurrentLevelIndex);
        }

        /// <summary>
        /// Resets the level state to allow re-triggering of level events.
        /// Should be called when restarting or moving to the next level.
        /// </summary>
        public void ResetLevelState()
        {
            _isLevelEnded = true;
            _isLevelStarted = false;
            //temp
            //Debug.Log("Level state has been reset.");
        }
    }
}