using CS3D._Enums;
using UnityEngine;

namespace CS3D.CoinSystem
{
    /// <summary>
    /// Serializable class that defines the coin level and its associated color.
    /// </summary>
    [System.Serializable]
    public class CoinLevelData
    {
        public CoinLevel Level;
        public Color CoinColor;

        public CoinLevelData(CoinLevel level, Color color)
        {
            Level = level;
            CoinColor = color;
        }
    }
}