using CS3D._Enums;
using UnityEngine;

namespace CS3D.CoinSystem
{
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