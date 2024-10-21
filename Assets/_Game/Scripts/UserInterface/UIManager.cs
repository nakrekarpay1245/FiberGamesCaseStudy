using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using _Game._helpers;
using CS3D.LevelSystem;

namespace CS3D.UI
{
    /// <summary>
    /// Manages the UI elements related to level progress, win/lose states, and user interactions such as restarting 
    /// and moving to the next level.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        [Header("Level Management Buttons")]
        [Tooltip("Button used to restart the current level.")]
        [SerializeField] private CustomButton _restartButton;

        [Tooltip("Button used to progress to the next level.")]
        [SerializeField] private CustomButton _nextLevelButton;

        [Header("Level Result Menus")]
        [Tooltip("UI panel displayed when the player fails the level.")]
        [SerializeField] private GameObject _levelFailTitle;

        [Tooltip("UI panel displayed when the player wins the level.")]
        [SerializeField] private GameObject _levelCompleteTitle;

        [Header("Level Information Display")]
        [Tooltip("Text element displaying the number of the completed level.")]
        [SerializeField] private TextMeshProUGUI _currentLevelText;

        [Header("Level Result Emojis")]
        [Tooltip("Image displayed when the player wins the level.")]
        [SerializeField] private GameObject _completeEmojiImage;

        [Tooltip("Image displayed when the player fails the level.")]
        [SerializeField] private GameObject _failEmojiImage;

        [Tooltip("UI panel that displays the result.")]
        [SerializeField] private GameObject _resultMenu;

        [Header("Animation Settings")]
        [Tooltip("")]
        [SerializeField, Range(0.1f, 2f)] private Ease _showEase = Ease.OutFlash;

        [Tooltip("Duration of the scale-up animation when activating UI elements.")]
        [SerializeField, Range(0.1f, 2f)] private float _scaleUpDuration = 0.5f;

        [Tooltip("Target scale for the scale-up animation.")]
        [SerializeField, Range(0.5f, 3f)] private float _showScale = 1f;

        [Tooltip("Starting scale for the scale-up animation.")]
        [SerializeField, Range(0f, 1f)] private float _hideScale = 0f;

        [Tooltip("Delay between each UI element's pop-up animation.")]
        [SerializeField, Range(0f, 1f)] private float _animationDelay = 0.2f;

        private void Start()
        {
            GlobalBinder.singleton.LevelManager.OnLevelStart.AddListener(Initialize);
        }

        public void Initialize()
        {
            // Initialize button listeners
            _restartButton.onButtonDown.AddListener(RestartLevel);
            _nextLevelButton.onButtonDown.AddListener(StartNextLevel);

            // Hide all UI elements initially
            HideAllUIElements();

            GlobalBinder.singleton.LevelManager.OnLevelComplete.AddListener(ShowLevelCompleteUI);
            GlobalBinder.singleton.LevelManager.OnLevelFail.AddListener(ShowLevelFailUI);
        }

        /// <summary>
        /// Hides all relevant UI elements to ensure they are not visible at the start.
        /// </summary>
        private void HideAllUIElements()
        {
            HideUIElements(_levelFailTitle, _levelCompleteTitle, _currentLevelText.gameObject, _completeEmojiImage,
                _failEmojiImage, _resultMenu, _restartButton.gameObject, _nextLevelButton.gameObject);
        }

        /// <summary>
        /// Hides the specified UI elements.
        /// </summary>
        /// <param name="elements">UI elements to hide.</param>
        private void HideUIElements(params GameObject[] elements)
        {
            foreach (var element in elements)
            {
                if (element != null)
                {
                    element.transform.localScale = Vector3.one * _hideScale;
                    element.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Displays the specified UI elements sequentially with a scale-up animation using DOTween.
        /// </summary>
        /// <param name="elements">UI elements to activate and animate sequentially.</param>
        private void ShowUIElementsWithSequentialAnimation(params GameObject[] elements)
        {
            float delay = 0f;
            foreach (var element in elements)
            {
                if (element != null)
                {
                    element.SetActive(true);
                    element.transform
                        .DOScale(Vector3.one * _showScale, _scaleUpDuration)
                        .SetEase(_showEase)
                        .SetDelay(delay);
                    delay += _animationDelay; // Increase delay for the next element
                }
            }
        }

        /// <summary>
        /// Displays the UI for when a level is completed successfully.
        /// </summary>
        /// <param name="levelNumber">The number of the level that was completed.</param>
        public void ShowLevelCompleteUI(int levelNumber)
        {
            //temp
            //Debug.Log("ShowLevelCompleteUI!");
            HideAllUIElements();

            // Show relevant UI elements for level completion with staggered animation
            ShowUIElementsWithSequentialAnimation(_resultMenu, _levelCompleteTitle, _currentLevelText.gameObject,
                _completeEmojiImage, _nextLevelButton.gameObject);

            UpdateLevelNumberText(levelNumber + 1);
        }

        /// <summary>
        /// Displays the UI for when a level fails.
        /// </summary>
        /// <param name="levelNumber">The number of the level that was failed.</param>
        public void ShowLevelFailUI(int levelNumber)
        {
            HideAllUIElements();

            // Show relevant UI elements for level failure with staggered animation
            ShowUIElementsWithSequentialAnimation(_resultMenu, _levelFailTitle, _currentLevelText.gameObject,
                _failEmojiImage, _restartButton.gameObject);

            UpdateLevelNumberText(levelNumber + 1);
        }

        /// <summary>
        /// Updates the level number text on the UI.
        /// </summary>
        /// <param name="levelNumber">The number of the level to display.</param>
        private void UpdateLevelNumberText(int levelNumber)
        {
            if (_currentLevelText != null)
            {
                _currentLevelText.text = $"Level {levelNumber}";
                _currentLevelText.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Restarts the current level by reloading the level's scene or resetting the game state.
        /// </summary>
        private void RestartLevel()
        {
            //temp
            //Debug.Log("Restarting Level...");
            GlobalBinder.singleton.LevelManager.StartLevel();
        }

        /// <summary>
        /// Starts the next level by progressing the game's state to the next level.
        /// </summary>
        private void StartNextLevel()
        {
            //temp
            //Debug.Log("Starting Next Level...");
            GlobalBinder.singleton.LevelManager.StartLevel();
        }
    }
}