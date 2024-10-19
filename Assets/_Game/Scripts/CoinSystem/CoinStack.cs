using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CS3D._Enums;
using CS3D.Pathfinding;

namespace CS3D.CoinSystem
{
    /// <summary>
    /// Manages a stack of coins using a stack data structure for FILO (First In, Last Out) operations.
    /// Handles adding and removing coins, positioning them with smooth animations using DOTween.
    /// </summary>
    public class CoinStack : Pathfinder
    {
        [Header("Coin Stack Settings")]
        [Tooltip("The vertical distance between each coin in the stack.")]
        [SerializeField, Range(0.1f, 0.25f)] private float _verticalSpacing = 0.2f;

        [Tooltip("Duration for the scale change animation when removing coins.")]
        [SerializeField, Range(0.1f, 1f)] private float _scaleChangeDuration = 0.25f;

        [Header("Coin Stack")]
        [Tooltip("The stack that holds the coins.")]
        private Stack<Coin> _coinStack = new Stack<Coin>();

        // Public list to expose current coins in the stack
        [Header("Current Coins in Stack")]
        [Tooltip("A temporary list to visualize the coins currently in the stack.")]
        public List<Coin> CurrentCoinsInStack = new List<Coin>();

        [Header("Coin Configuration")]
        [Tooltip("List of coin configurations specifying which coin types to create and their counts.")]
        [SerializeField] private List<CoinConfiguration> _coinConfigurations = new List<CoinConfiguration>();

        [Header("Coin Prefab")]
        [Tooltip("The prefab of the coin to instantiate.")]
        [SerializeField] private Coin _coinPrefab; // Prefab to instantiate coins

        private void Awake()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes the coin stack based on the provided configurations.
        /// This method creates the specified number of coins for each coin type.
        /// </summary>
        public void Initialize()
        {
            // Clear the existing stack before initializing
            _coinStack.Clear();

            // Iterate through each coin configuration
            for (int i = 0; i < _coinConfigurations.Count; i++)
            {
                CoinConfiguration config = _coinConfigurations[i];
                for (int j = 0; j < config.Count; j++)
                {
                    // Create a coin of the specified type
                    Coin coin = CreateCoin(config.CoinLevel);
                    if (coin != null)
                    {
                        // Position the coin based on the current stack size
                        Vector3 targetPosition = CalculateTargetPosition();
                        coin.transform.position = targetPosition;

                        // Add the coin to the stack
                        _coinStack.Push(coin);

                        // Update the public list to reflect the current state of the stack
                        UpdateCurrentCoinsInStack();
                    }
                }
            }
        }

        /// <summary>
        /// Creates a coin of the specified level using the predefined coin prefab.
        /// </summary>
        /// <param name="level">The level of the coin to create.</param>
        /// <returns>The created coin instance, or null if creation failed.</returns>
        private Coin CreateCoin(CoinLevel level)
        {
            if (_coinPrefab != null)
            {
                // Instantiate the coin prefab
                Coin coinObject = Instantiate(_coinPrefab, transform);

                if (coinObject != null)
                {
                    coinObject.Level = level; // Set the current level to the coin
                    return coinObject;
                }
            }

            Debug.LogWarning("Coin prefab is not assigned or coin creation failed.");
            return null;
        }

        /// <summary>
        /// Adds a coin to the stack and positions it at the correct height with a smooth animation.
        /// The coin's parent is set to the CoinStack for proper hierarchy management.
        /// </summary>
        /// <param name="coin">The coin to be added to the stack.</param>
        public void AddCoin(Coin coin)
        {
            if (coin == null)
            {
                Debug.LogWarning("Attempted to add a null coin to the stack.");
                return;
            }

            // Set the coin's parent to the CoinStack for hierarchy management
            coin.transform.SetParent(transform);

            // Calculate the target position for the new coin in the stack
            Vector3 targetPosition = CalculateTargetPosition();

            // Move the coin to its position in the stack with a smooth animation
            coin.MoveTo(targetPosition);

            // Add the coin to the stack
            _coinStack.Push(coin);

            // Update the public list to reflect the current state of the stack
            UpdateCurrentCoinsInStack();
        }

        /// <summary>
        /// Removes the coin from the top of the stack, scales it down, and then destroys it.
        /// </summary>
        public void RemoveAndDestroyCoin()
        {
            if (_coinStack.Count == 0)
            {
                Debug.LogWarning("Coin stack is empty. No coin to remove.");
                return;
            }

            // Remove the coin from the top of the stack
            Coin removedCoin = _coinStack.Pop();
            ScaleAndDestroyCoin(removedCoin);

            // Update the public list to reflect the current state of the stack
            UpdateCurrentCoinsInStack();
        }

        /// <summary>
        /// Removes and returns the coin from the top of the stack without scaling it down.
        /// </summary>
        /// <returns>The coin that was removed from the stack, or null if the stack is empty.</returns>
        public Coin RemoveCoin()
        {
            if (_coinStack.Count == 0)
            {
                Debug.LogWarning("Coin stack is empty. No coin to remove.");
                return null;
            }

            // Remove the coin from the top of the stack
            Coin removedCoin = _coinStack.Pop();

            // Update the public list to reflect the current state of the stack
            UpdateCurrentCoinsInStack();

            return removedCoin;
        }

        /// <summary>
        /// Calculates the target position for the next coin based on the current stack size.
        /// </summary>
        /// <returns>The target position for the new coin in the stack.</returns>
        private Vector3 CalculateTargetPosition()
        {
            int coinIndex = _coinStack.Count - 1; // Get the index of the new coin
            return new Vector3(transform.position.x, coinIndex * _verticalSpacing, transform.position.z);
        }

        /// <summary>
        /// Scales down the specified coin over a duration and destroys it once completed.
        /// </summary>
        /// <param name="coin">The coin to be scaled down and destroyed.</param>
        private void ScaleAndDestroyCoin(Coin coin)
        {
            coin.transform.DOScale(Vector3.zero, _scaleChangeDuration).OnComplete(() =>
            {
                // Destroy the coin object after scaling down
                Destroy(coin.gameObject);
            });
        }

        /// <summary>
        /// Updates the public list to reflect the current coins in the stack.
        /// </summary>
        private void UpdateCurrentCoinsInStack()
        {
            CurrentCoinsInStack.Clear();
            CurrentCoinsInStack.AddRange(_coinStack);
        }
    }

    /// <summary>
    /// Represents the configuration for coin creation.
    /// </summary>
    [System.Serializable]
    public class CoinConfiguration
    {
        [Header("Coin Configuration")]
        [Tooltip("The level of the coin type to be created.")]
        public CoinLevel CoinLevel; // Assuming CoinLevel is an enum defined in CS3D._Enums

        [Tooltip("The number of coins to create of this level.")]
        [Range(1, 100)]
        public int Count; // Number of coins to create
    }
}