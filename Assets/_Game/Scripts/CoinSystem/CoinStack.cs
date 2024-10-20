using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CS3D._Enums;
using CS3D.Pathfinding;
using CS3D.TileSystem;

namespace CS3D.CoinSystem
{
    /// <summary>
    /// Manages a list of coins using a List data structure to maintain LIFO (Last In, First Out) behavior.
    /// Handles adding and removing coins, and positions them with smooth animations using DOTween.
    /// </summary>
    public class CoinStack : Pathfinder
    {
        [Header("Coin Stack Settings")]
        [Tooltip("The vertical distance between each coin in the stack.")]
        [SerializeField, Range(0.1f, 0.25f)] private float _verticalSpacing = 0.2f;

        [Tooltip("Duration for the scale change animation when removing coins.")]
        [SerializeField, Range(0.1f, 1f)] private float _scaleChangeDuration = 0.25f;

        [Header("Coin Stack")]
        [Tooltip("The list that holds the coins.")]
        [SerializeField] private List<Coin> _coinList = new List<Coin>();
        public List<Coin> CoinList { get => _coinList; private set => _coinList = value; }

        [Header("Coin Configuration")]
        [Tooltip("List of coin configurations specifying which coin types to create and their counts.")]
        [SerializeField] private List<CoinConfiguration> _coinConfigurations = new List<CoinConfiguration>();
        public List<CoinConfiguration> CoinConfigurations
        {
            get => _coinConfigurations;
            set => _coinConfigurations = value;
        }

        [Header("Coin Prefab")]
        [Tooltip("The prefab of the coin to instantiate.")]
        [SerializeField] private string _coinPrefabResourceKey = "Coin/Coin";

        [SerializeField] private Tile _tile;
        public Tile Tile
        {
            get => _tile;
            set => _tile = value;
        }

        /// <summary>
        /// Initializes the coin stack based on the provided configurations.
        /// This method creates the specified number of coins for each coin type.
        /// </summary>
        public void Initialize()
        {
            // Clear the existing list before initializing
            _coinList.Clear();

            // Iterate through each coin configuration
            foreach (CoinConfiguration config in _coinConfigurations)
            {
                for (int j = 0; j < config.Count; j++)
                {
                    // Create a coin of the specified type
                    Coin coin = CreateCoin(config.CoinLevel);
                    if (coin != null)
                    {
                        // Position the coin based on the current list size
                        Vector3 targetPosition = CalculateTargetPosition();
                        coin.transform.localPosition = targetPosition;

                        // Add the coin to the list
                        _coinList.Add(coin);
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
            Coin coinPrefab = Resources.Load<Coin>(_coinPrefabResourceKey);

            if (coinPrefab != null)
            {
                // Instantiate the coin prefab
                Coin coinObject = Instantiate(coinPrefab, transform);

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
        /// Adds a coin to the list and positions it at the correct height with a smooth animation.
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

            // Calculate the target position for the new coin in the list
            Vector3 targetPosition = CalculateTargetPosition();

            // Move the coin to its position in the stack with a smooth animation
            coin.MoveTo(targetPosition);

            // Add the coin to the list
            _coinList.Add(coin);
        }

        /// <summary>
        /// Removes the coin from the top of the list, scales it down, and then destroys it.
        /// </summary>
        public void RemoveAndDestroyCoin()
        {
            if (_coinList.Count == 0)
            {
                Debug.LogWarning("Coin list is empty. No coin to remove.");
                return;
            }

            // Remove the coin from the top of the list
            Coin removedCoin = _coinList[_coinList.Count - 1];
            _coinList.RemoveAt(_coinList.Count - 1);
            ScaleAndDestroyCoin(removedCoin);
        }

        /// <summary>
        /// Removes and returns the coin from the top of the list without scaling it down.
        /// </summary>
        /// <returns>The coin that was removed from the list, or null if the list is empty.</returns>
        public Coin RemoveCoin()
        {
            if (_coinList.Count == 0)
            {
                Debug.LogWarning("Coin list is empty. No coin to remove.");
                return null;
            }

            // Remove the coin from the top of the list
            Coin removedCoin = _coinList[_coinList.Count - 1];
            _coinList.RemoveAt(_coinList.Count - 1);

            return removedCoin;
        }

        /// <summary>
        /// Gets the coin from the top of the list without removing it.
        /// </summary>
        /// <returns>The coin at the top of the list, or null if the list is empty.</returns>
        public Coin GetCoin()
        {
            if (_coinList.Count == 0)
            {
                Debug.LogWarning("Coin list is empty. No coin to return.");
                return null;
            }

            return _coinList[_coinList.Count - 1];
        }

        /// <summary>
        /// Calculates the target position for the next coin based on the current list size.
        /// </summary>
        /// <returns>The target position for the new coin in the stack.</returns>
        private Vector3 CalculateTargetPosition()
        {
            int coinIndex = _coinList.Count; // Get the index of the new coin
            return new Vector3(0, coinIndex * _verticalSpacing, 0);
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
        /// Finds and returns a list of coins in the list that match the specified coin level.
        /// </summary>
        /// <param name="level">The coin level to search for in the list.</param>
        /// <returns>A list of coins that have the specified level.</returns>
        public List<Coin> GetCoinsByLevel(CoinLevel level)
        {
            List<Coin> matchingCoins = new List<Coin>();

            foreach (Coin coin in _coinList)
            {
                if (coin.Level == level)
                {
                    matchingCoins.Add(coin);
                }
            }

            return matchingCoins;
        }
    }
}