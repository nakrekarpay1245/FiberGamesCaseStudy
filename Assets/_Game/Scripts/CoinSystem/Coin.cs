using UnityEngine;
using TMPro;
using DG.Tweening;
using CS3D._Enums;
using System.Collections.Generic;
using _Game._helpers;

namespace CS3D.CoinSystem
{
    /// <summary>
    /// Represents a coin in the game, which can be collected or moved around the grid.
    /// The coin has different levels that determine its value or appearance.
    /// The movement of the coin is animated using DOTween for smooth transitions.
    /// </summary>
    public class Coin : MonoBehaviour
    {
        [Header("Coin Settings")]
        [Tooltip("Level of the coin which determines its value or appearance.")]
        [SerializeField] private CoinLevel _coinLevel = CoinLevel.Level1;

        [Header("Coin Display Settings")]
        [Tooltip("Text component used to display the coin's level.")]
        [SerializeField] private TextMeshPro _coinLevelText;

        [Tooltip("Renderer component of the coin for changing color.")]
        [SerializeField] private MeshRenderer _coinMeshRenderer;

        [Tooltip("Renderer component of the coin for changing color.")]
        [SerializeField] private TrailRenderer _coinTrailRenderer;

        /// <summary>
        /// Gets the level of the coin.
        /// </summary>
        public CoinLevel Level
        {
            get => _coinLevel;
            private set
            {
                _coinLevel = value;
            }
        }

        private void Awake()
        {
            _coinLevelText = GetComponentInChildren<TextMeshPro>();
            _coinMeshRenderer = GetComponentInChildren<MeshRenderer>();
            _coinTrailRenderer = GetComponentInChildren<TrailRenderer>();
        }

        public void Initialize(CoinLevel coinLevel, int score, Color coinColor)
        {
            _coinLevel = coinLevel;
            string scoreText = score.ToString();
            UpdateCoinVisuals(scoreText, coinColor);
        }

        /// <summary>
        /// Moves the coin to the specified position with a smooth animation.
        /// The coin rotates based on the movement direction and performs a flip using DOTween.
        /// </summary>
        /// <param name="targetPosition">The position to move the coin to.</param>
        public void MoveTo(Vector3 targetPosition)
        {
            // Calculate the direction of movement
            Vector3 direction = targetPosition - transform.localPosition;
            float rotationAngleX = 0f;
            float rotationAngleZ = 0f;

            // Determine the rotation angle based on the movement direction
            if (Mathf.Abs(direction.z) > Mathf.Abs(direction.x))
            {
                // Vertical movement
                rotationAngleX = direction.z > 0 ? 360f : -360f;
            }
            else
            {
                // Horizontal movement
                rotationAngleZ = direction.x > 0 ? -360f : 360f;
            }

            float coinFlipDuraitonHalf = GlobalBinder.singleton.TimeManager.CoinFlipMovementDuration / 2;
            // Animate the coin's movement with a flip effect using DOTween
            transform.DOLocalMove(targetPosition + Vector3.up, coinFlipDuraitonHalf).OnComplete(() =>
            {
                transform.DOLocalMove(targetPosition, coinFlipDuraitonHalf);
            }).SetEase(Ease.InOutQuad); // Smooth easing for the movement

            // Animate the rotation to create a flipping effect while moving
            transform.DORotate(new Vector3(rotationAngleX, 0f, rotationAngleZ),
                GlobalBinder.singleton.TimeManager.CoinFlipMovementDuration, RotateMode.LocalAxisAdd)
                .SetEase(Ease.InOutQuad); // Smooth easing for the rotation
        }

        /// <summary>
        /// Updates the coin's visuals based on its level, including text display and color.
        /// </summary>
        private void UpdateCoinVisuals(string scoreText, Color coinColor)
        {
            // Update the text to display the coin's level number
            _coinLevelText.text = scoreText;

            if (_coinMeshRenderer != null && _coinTrailRenderer != null)
            {
                _coinMeshRenderer.material.color = coinColor;
                _coinTrailRenderer.startColor = coinColor;
                _coinTrailRenderer.endColor = coinColor;
            }
        }
    }
}