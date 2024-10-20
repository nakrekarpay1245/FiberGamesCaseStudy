using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using _Game._helpers;
using CS3D.Data;

namespace CS3D.ProgressSystem
{
    /// <summary>
    /// Manages the player's progress in completing levels.
    /// Tracks the score required to complete a level and updates a progress bar UI.
    /// </summary>
    public class ProgressManager : MonoBehaviour
    {
        [Header("Game Data")]
        [Tooltip("Reference to the current game data.")]
        [SerializeField] private GameData _gameData;

        [Header("Progress Settings")]
        [Tooltip("The current progress score of the player.")]
        [SerializeField] private int _currentScore = 0;

        [Header("UI Elements")]
        [Tooltip("The UI Image representing the progress bar.")]
        [SerializeField] private Image _progressBarFill;

        private void Start()
        {
            UpdateProgressBar();
        }

        /// <summary>
        /// Adds score to the current progress. If the score reaches or exceeds the required score,
        /// the level is considered complete, and the progress bar is filled accordingly.
        /// </summary>
        /// <param name="scoreToAdd">The amount of score to add.</param>
        public void AddScore(int scoreToAdd)
        {
            _currentScore += scoreToAdd;
            UpdateProgressBar();

            if (_currentScore >= _gameData.CurrentLevel.RequiredScore)
            {
                LevelComplete();
            }
        }

        /// <summary>
        /// Updates the fill amount of the progress bar based on the current score.
        /// Uses DOTween to animate the transition smoothly.
        /// </summary>
        private void UpdateProgressBar()
        {
            float fillAmount = (float)_currentScore / _gameData.CurrentLevel.RequiredScore;
            _progressBarFill.DOFillAmount(fillAmount, GlobalBinder.singleton.TimeManager.ProgressBarFillTime).SetEase(Ease.OutSine);
        }

        /// <summary>
        /// Called when the level is completed. This can trigger any win logic.
        /// </summary>
        private void LevelComplete()
        {
            Debug.Log("Level Complete! You've scored enough points.");
            GlobalBinder.singleton.LevelManager.CompleteLevel();
            // Trigger win logic or transition to the next level here.
            // Example: Load next level, show victory screen, etc.
        }
    }
}