using UnityEngine;
using CS3D.Save;

namespace CS3D.Data
{
    /// <summary>
    /// Holds game data including levels, economy, and car system.
    /// Ensures data is loaded from and saved to persistent storage using SaveManager.
    /// </summary>
    [CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData")]
    public class GameData : ScriptableObject
    {
        [Header("Game Configuration")]
        [Tooltip("List of levels in the game.")]
        //[SerializeField]
        //private List<Level> _levelList;
        [SerializeField]
        private int _currentLevelIndex;

        /// <summary>
        /// Gets or sets the current level index, loading/saving it from/to persistent storage.
        /// </summary>
        public int CurrentLevelIndex
        {
            get
            {
                _currentLevelIndex = SaveManager.LoadLevelIndex();
                //return _currentLevelIndex % _levelList.Count;
                return _currentLevelIndex;
            }
            set
            {
                _currentLevelIndex = value;
                SaveManager.SaveLevelIndex(_currentLevelIndex);
            }
        }

        ///// <summary>
        ///// Gets the current level based on the level index.
        ///// </summary>
        //public Level CurrentLevel => _levelList[CurrentLevelIndex];

        //[Header("Economy Configuration")]
        //[Tooltip("The player's current amount of coins.")]
        //[SerializeField]
        //private int _coins;

        ///// <summary>
        ///// Gets or sets the player's coin count, loading/saving it from/to persistent storage.
        ///// </summary>
        //public int Coins
        //{
        //    get
        //    {
        //        _coins = SaveManager.LoadCoins();
        //        return _coins;
        //    }
        //    set
        //    {
        //        _coins = value;
        //        SaveManager.SaveCoins(_coins);
        //    }
        //}
    }
}