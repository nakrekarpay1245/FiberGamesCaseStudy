using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CS3D._Enums;
using CS3D.Pathfinding;
using CS3D.TileSystem;
using _Game._helpers;
using _Game._helpers.TimeManagement;

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

        [Tooltip("The maximum weight limit for coins in the stack before triggering removal.")]
        [SerializeField] private int _maxWeightLimit = 10;

        [SerializeField] private Vector3 _popUpTextOffset = new Vector3(0, 1, -1);

        [Header("Coin Stack")]
        [Tooltip("The list that holds the coins.")]
        [SerializeField] private List<Coin> _coinList = new List<Coin>();

        [Header("Coin Configuration")]
        [Tooltip("List of coin configurations specifying which coin types to create and their counts.")]
        [SerializeField] private List<CoinConfiguration> _coinConfigurations = new List<CoinConfiguration>();
        public List<CoinConfiguration> CoinConfigurations
        {
            get => _coinConfigurations;
            set => _coinConfigurations = value;
        }

        /// <summary>
        /// Gets the CoinLevel of the coin at the top of the stack.
        /// Returns null if the stack is empty.
        /// </summary>
        public CoinLevel? Level
        {
            get => _coinList.Count > 0 ? _coinList[_coinList.Count - 1].Level : (CoinLevel?)null;
        }

        /// <summary>
        /// Gets the count of coins in the stack that have the same level as the top coin.
        /// If the stack is empty, returns 0.
        /// </summary>
        public int Weight
        {
            get
            {
                if (_coinList.Count == 0)
                    return 0;

                int count = 0;

                // Count the number of coins with the same level as the top coin
                foreach (Coin coin in _coinList)
                {
                    if (coin.Level == Level)
                        count++;
                }

                return count;
            }
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
                Debug.Log("Config Count: " + config.Count);

                for (int j = 0; j < config.Count; j++)
                {
                    // Create a coin of the specified type
                    Coin coin = GetCoin(config.CoinLevel);
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
        private Coin GetCoin(CoinLevel level)
        {
            if (GlobalBinder.singleton.CoinManager != null)
            {
                Coin coin = GlobalBinder.singleton.CoinManager.GetCoin(transform.position, transform);
                if (coin != null)
                {
                    coin.Level = level; // Set the current level to the coin
                    return coin;
                }
            }

            Debug.LogWarning("Coin Manager is not assigned or coin pool is failed.");
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
            coin.transform.DOScale(Vector3.zero, GlobalBinder.singleton.TimeManager.CoinScaleChangeDuration).OnComplete(() =>
            {
                // Destroy the coin object after scaling down
                Destroy(coin.gameObject);
            });
        }

        /// <summary>
        /// Checks if the Weight of the coin stack is greater than or equal to 10.
        /// If true, removes and destroys the coins from the top of the stack until the Weight is less than 10.
        /// </summary>
        public void CheckAndDestroyTopCoinsIfWeightExceedsLimit()
        {
            int weightTileCount = Weight;

            // Only proceed if the weight exceeds the limit
            if (weightTileCount >= _maxWeightLimit)
            {
                LogScoreForTopCoins();
                Sequence coinRemovalSequence = DOTween.Sequence();

                // Append the coin removal actions to the sequence
                for (int i = 0; i < weightTileCount; i++)
                {
                    coinRemovalSequence.AppendCallback(() => RemoveAndDestroyCoin());
                    // Add an interval (adjust as necessary)
                    coinRemovalSequence.AppendInterval(GlobalBinder.singleton.TimeManager.CoinRemovalInterval); // Adjust the interval as needed
                }

                // Optionally, you can add a completion callback to handle any logic after removal
                coinRemovalSequence.OnComplete(() =>
                {
                    GlobalBinder.singleton.MatchChecker.CheckForMatches();
                    //Debug.Log("Coin removal complete. Weight is now below the limit.");
                });
            }
        }

        /// <summary>
        /// Logs the score based on the levels of the top coins in the stack.
        /// </summary>
        private void LogScoreForTopCoins()
        {
            // Calculate score based on the levels of the top coins
            int score = 0;
            int weightTileCount = Weight;

            // Append the coin removal actions to the sequence
            for (int i = 0; i < weightTileCount; i++)
            {
                score += ((int)Level + 1);
            }

            string popUpString = $"+{score}";
            Vector3 popUpTextPosition = CalculateTargetPosition() + transform.position + _popUpTextOffset;
            GlobalBinder.singleton.PopUpTextManager.ShowPopUpText(popUpTextPosition, popUpString);

            GlobalBinder.singleton.ProgressManager.AddScore(score);
        }
    }
}