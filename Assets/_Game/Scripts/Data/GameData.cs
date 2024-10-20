using UnityEngine;
using CS3D.Save;
using CS3D.LevelSystem;
using System.Collections.Generic;

namespace CS3D.Data
{
    /// <summary>
    /// Holds game data, including levels and game progression.
    /// Manages loading and saving of data using the SaveManager for persistent storage.
    /// </summary>
    [CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData")]
    public class GameData : ScriptableObject
    {
        [Header("Game Configuration")]
        [Tooltip("List of levels available in the game.")]
        [SerializeField]
        private List<LevelDataSO> _levelList;

        [Tooltip("The index of the currently active level.")]
        [SerializeField] // HideInInspector because this value is managed through the property
        private int _currentLevelIndex;

        /// <summary>
        /// Gets or sets the index of the current level.
        /// Ensures that the index is loaded from and saved to persistent storage.
        /// </summary>
        public int CurrentLevelIndex
        {
            get
            {
                _currentLevelIndex = SaveManager.LoadLevelIndex();
                return _currentLevelIndex % _levelList.Count;
            }
            set
            {
                _currentLevelIndex = value;
                SaveManager.SaveLevelIndex(_currentLevelIndex);
            }
        }

        /// <summary>
        /// Gets the data for the currently active level.
        /// </summary>
        public LevelDataSO CurrentLevel => _levelList[CurrentLevelIndex];

        // Uncomment and use the below code when economy features are needed in the game

        //[Header("Economy Configuration")]
        //[Tooltip("The player's current amount of in-game currency.")]
        //[SerializeField]
        //private int _coins;

        ///// <summary>
        ///// Gets or sets the player's coin count.
        ///// This value is loaded from and saved to persistent storage.
        ///// </summary>
        //public int Coins
        //{
        //    get
        //    {
        //        // Lazy load the coin amount from the SaveManager when accessed
        //        if (_coins == 0)
        //        {
        //            _coins = SaveManager.LoadCoins();
        //        }
        //        return _coins;
        //    }
        //    set
        //    {
        //        _coins = Mathf.Max(0, value); // Ensure the coin count is non-negative
        //        SaveManager.SaveCoins(_coins);
        //    }
        //}
    }
}