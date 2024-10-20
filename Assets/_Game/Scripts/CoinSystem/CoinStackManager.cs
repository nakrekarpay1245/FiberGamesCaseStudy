using CS3D.Data;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace CS3D.CoinSystem
{
    /// <summary>
    /// Manages the generation and configuration of coin stacks in the game.
    /// </summary>
    public class CoinStackManager : MonoBehaviour
    {
        [Header("Game Data")]
        [Tooltip("The current game data")]
        [SerializeField] private GameData _gameData;

        [Header("Coin Stack Prefab")]
        [Tooltip("Prefab to instantiate new coin stacks.")]
        [SerializeField] private CoinStack _coinStackPrefab;

        [Header("Coin Stack Positioning")]
        [Tooltip("Point in the scene where the coin stacks will be instantiated.")]
        [SerializeField] private Transform _coinStackPoint;

        [Header("Horizontal Spacing")]
        [Tooltip("Distance between each generated coin stack.")]
        [SerializeField][Range(1f, 5f)] private float _verticalSpace = 2.5f;

        private List<CoinStack> _generatedCoinStackList = new List<CoinStack>();

        private int _generatedCoinStackCount = 0; // Track the number of generated coin stacks

        /// <summary>
        /// Initializes the coin stacks based on the predefined configurations.
        /// </summary>
        private void Start()
        {
            GenerateAllCoinStacks();
        }

        /// <summary>
        /// Generates all coin stacks according to the configurations.
        /// </summary>
        private void GenerateAllCoinStacks()
        {
            _generatedCoinStackCount = 0;

            List<CoinStackConfig> coinStackList = new List<CoinStackConfig>();
            coinStackList = _gameData.CurrentLevel.LevelConfigurationData.CoinStackList;

            for (int i = 0; i < coinStackList.Count; i++)
            {
                GenerateCoinStack(i, coinStackList);
            }
        }

        /// <summary>
        /// Generates a single coin stack at a specified position and configures it.
        /// </summary>
        /// <param name="index">The index of the configuration to use for this coin stack.</param>
        private void GenerateCoinStack(int index, List<CoinStackConfig> coinStackList)
        {
            // Calculate the position for the new coin stack
            Vector3 coinStackPosition = _coinStackPoint.position + (Vector3.back * (_generatedCoinStackCount * _verticalSpace));

            // Instantiate the coin stack prefab at the calculated position
            CoinStack generatedCoinStack = Instantiate(_coinStackPrefab, coinStackPosition, Quaternion.identity);

            // Retrieve the current configuration
            CoinStackConfig currentCoinStackConfig = coinStackList[index];

            // Add configurations to the generated coin stack
            foreach (CoinConfiguration coinStackConfig in currentCoinStackConfig.ConfigList)
            {
                generatedCoinStack.CoinConfigurations.Add(coinStackConfig);
            }

            // Initialize the coin stack with the added configurations
            generatedCoinStack.Initialize();

            _generatedCoinStackList.Add(generatedCoinStack);

            // Increment the count of generated coin stacks
            _generatedCoinStackCount++;
        }

        /// <summary>
        /// Retrieves the first coin stack and removes it from the list of generated stacks.
        /// </summary>
        /// <returns>The first coin stack, or null if there are no stacks left.</returns>
        public CoinStack GetCoinStack()
        {
            if (_generatedCoinStackList.Count <= 0)
            {
                Debug.Log("RegenerateCoinStack!");
                GenerateAllCoinStacks();
            }

            CoinStack coinStackToReturn = _generatedCoinStackList[0]; // Get the first coin stack
            return coinStackToReturn; // Return the retrieved stack
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveCoinStack(CoinStack coinStack)
        {
            _generatedCoinStackList.Remove(coinStack); // Remove it from the list
            UpdateCoinStackPositions();
        }

        private void UpdateCoinStackPositions()
        {
            for (int i = 0; i < _generatedCoinStackList.Count; i++)
            {
                Vector3 coinStackPosition = _coinStackPoint.position + (Vector3.back * (i * _verticalSpace));
                _generatedCoinStackList[i].transform.DOMove(coinStackPosition, 1);
            }
        }
    }
}