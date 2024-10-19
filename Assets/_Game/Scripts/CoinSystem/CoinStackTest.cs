using UnityEngine;
using CS3D._Enums;

namespace CS3D.CoinSystem
{
    /// <summary>
    /// A test class for validating the functionality of the CoinStack.
    /// This class allows adding and removing coins from the stack based on their levels.
    /// </summary>
    public class CoinStackTest : MonoBehaviour
    {
        [Header("Test Settings")]
        [Tooltip("The first CoinStack to which the coins will be added and removed.")]
        [SerializeField] private CoinStack _coinStack1;

        [Tooltip("The second CoinStack to which the coins will be added and removed.")]
        [SerializeField] private CoinStack _coinStack2;

        [Tooltip("The coin prefab to instantiate when adding new coins to the stack.")]
        [SerializeField] private GameObject _coinPrefab;

        [Header("Add Coin Settings")]
        [Tooltip("The level of the coin to add to the stack.")]
        [SerializeField] private CoinLevel _coinLevelToAdd = CoinLevel.Level1;

        private void Update()
        {
            HandleInput();
        }

        /// <summary>
        /// Handles user input for adding, removing, and transferring coins between CoinStacks.
        /// </summary>
        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                AddCoin();
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                RemoveAndDestroyCoin();
            }
            else if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                RemoveCoin();
            }

            // Transfer coin from CoinStack2 to CoinStack1
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                TransferCoin(_coinStack2, _coinStack1);
            }

            // Transfer coin from CoinStack1 to CoinStack2
            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                TransferCoin(_coinStack1, _coinStack2);
            }
        }

        /// <summary>
        /// Adds a coin of the specified level to the CoinStack.
        /// </summary>
        public void AddCoin()
        {
            if (_coinPrefab == null || _coinStack1 == null)
            {
                Debug.LogWarning("CoinPrefab or CoinStack1 is not assigned.");
                return;
            }

            // Instantiate a new coin from the coin prefab
            GameObject coinObject = Instantiate(_coinPrefab);

            // Get the Coin component from the instantiated object
            Coin coin = coinObject.GetComponent<Coin>();

            if (coin != null)
            {
                // Set the coin level based on the selected enum
                coin.Level = _coinLevelToAdd;

                // Add the coin to the first stack
                _coinStack1.AddCoin(coin);
            }
            else
            {
                Debug.LogWarning("The instantiated object does not contain a Coin component.");
            }
        }

        /// <summary>
        /// Removes a coin from the CoinStack without destroying it.
        /// </summary>
        public void RemoveCoin()
        {
            if (_coinStack1 == null)
            {
                Debug.LogWarning("CoinStack1 is not assigned.");
                return;
            }

            Coin removedCoin = _coinStack1.RemoveCoin();

            if (removedCoin == null)
            {
                Debug.LogWarning("No coin was removed; CoinStack1 may be empty.");
            }
        }

        /// <summary>
        /// Removes a coin from the CoinStack and destroys it.
        /// </summary>
        public void RemoveAndDestroyCoin()
        {
            if (_coinStack1 == null)
            {
                Debug.LogWarning("CoinStack1 is not assigned.");
                return;
            }

            _coinStack1.RemoveAndDestroyCoin();
        }

        /// <summary>
        /// Transfers a coin from one CoinStack to another.
        /// </summary>
        /// <param name="fromStack">The CoinStack to transfer a coin from.</param>
        /// <param name="toStack">The CoinStack to transfer a coin to.</param>
        private void TransferCoin(CoinStack fromStack, CoinStack toStack)
        {
            if (fromStack == null || toStack == null)
            {
                Debug.LogWarning("One of the CoinStacks is not assigned.");
                return;
            }

            Coin transferredCoin = fromStack.RemoveCoin();
            if (transferredCoin != null)
            {
                toStack.AddCoin(transferredCoin);
                Debug.Log($"Transferred coin from {fromStack.name} to {toStack.name}");
            }
            else
            {
                Debug.LogWarning($"No coin was removed; {fromStack.name} may be empty.");
            }
        }
    }
}