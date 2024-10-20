using UnityEngine;

namespace _Game._helpers.TimeManagement
{
    /// <summary>
    /// Manages the game timer, including starting, stopping, adding extra time, and freezing the timer.
    /// </summary>
    public class TimeManager : MonoBehaviour
    {
        [Header("TimeManager Parameters")]
        [Tooltip("Duration for the scale change animation when removing coins.")]
        [SerializeField, Range(0.1f, 1f)]
        private float _coinScaleChangeDuration = 0.25f;
        public float CoinScaleChangeDuration { get => _coinScaleChangeDuration; private set => _coinScaleChangeDuration = value; }

        [Header("Coin Settings")]
        [SerializeField, Tooltip("The interval (in seconds) between consecutive coin removal actions.")]
        [Range(0.001f, 0.1f)]
        private float _coinRemovalInterval = 0.025f;
        public float CoinRemovalInterval { get => _coinRemovalInterval; private set => _coinRemovalInterval = value; }

        [Tooltip("The speed at which the coin moves to its target position.")]
        [SerializeField, Range(0.1f, 0.75f)] private float _coinFlipMovementDuration = 1f;
        public float CoinFlipMovementDuration { get => _coinFlipMovementDuration; set => _coinFlipMovementDuration = value; }

        [Header("Pathfinder Settings")]
        [Tooltip("The movement speed of the enemy (time to move between tiles in seconds).")]
        [Range(0.001f, 0.25f)]
        [SerializeField]
        private float _tileMovementTime = 1f;
        public float TileMovementTime { get => _tileMovementTime; private set => _tileMovementTime = value; }

        [Header("Match Checker Settings")]
        [Tooltip("The delay between each coin transfer animation during a match.")]
        [SerializeField, Range(0.01f, 1f)]
        private float _coinTransferInterval = 0.05f;
        public float CoinTransferInterval { get => _coinTransferInterval; private set => _coinTransferInterval = value; }

        [Tooltip("The delay before initiating the match processing animation.")]
        [SerializeField, Range(0.1f, 1f)]
        private float _matchProcessingDelay = 0.35f;
        public float MatchProcessingDelay { get => _matchProcessingDelay; private set => _matchProcessingDelay = value; }

        [Tooltip("The delay between consecutive match checks to ensure smooth transitions.")]
        [SerializeField, Range(0.01f, 0.5f)]
        private float _matchCheckInterval = 0.1f;
        public float MatchCheckInterval { get => _matchCheckInterval; private set => _matchCheckInterval = value; }

        [Header("Progress Settings")]
        [Tooltip("The speed of progress bar fill")]
        [SerializeField] private float _progressBarFillTime = 0.25f;
        public float ProgressBarFillTime { get => _progressBarFillTime; set => _progressBarFillTime = value; }

        /// <summary>
        /// Sets the time scale, controlling the flow of time in the game.
        /// </summary>
        /// <param name="scale">The scale at which time passes. 1 is normal speed.</param>
        public void SetTimeScale(float scale)
        {
            Time.timeScale = scale;
        }
    }
}