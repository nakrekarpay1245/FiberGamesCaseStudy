using System.Collections.Generic;
using UnityEngine;

namespace CS3D.CoinSystem
{
    [System.Serializable]
    public class CoinStackConfig
    {
        [Header("Coin Configuration List")]
        [Tooltip("List of configurations for different coin types in this stack.")]
        [SerializeField] private List<CoinConfiguration> configList = new List<CoinConfiguration>();

        /// <summary>
        /// Exposes the list of coin configurations for enumeration.
        /// </summary>
        public List<CoinConfiguration> ConfigList => configList;
    }
}