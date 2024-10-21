using _Game._helpers.Audios;
using _Game._helpers.Particles;
using _Game._helpers.PopUp;
using _Game._helpers.TimeManagement;
using _Game.InputHandling;
using CS3D.CoinSystem;
using CS3D.LevelSystem;
using CS3D.MatchSystem;
using CS3D.Pathfinding;
using CS3D.ProgressSystem;
using CS3D.TileSystem;
using CS3D.UI;
using UnityEngine;

namespace _Game._helpers
{
    /// <summary>
    /// Acts as a central hub for accessing various managers in the game.
    /// Inherits from MonoSingleton to ensure a single instance across the game.
    /// </summary>
    public class GlobalBinder : MonoSingleton<GlobalBinder>
    {
        [Header("Managers")]
        [Tooltip("Handles audio functionalities like playing sounds and managing music.")]
        [SerializeField] private AudioManager _audioManager;

        [Tooltip("Manages particle effects used throughout the game.")]
        [SerializeField] private ParticleManager _particleManager;

        [Tooltip("Manages pop-up text display throughout the game.")]
        [SerializeField] private PopUpTextManager _popUpTextManager;

        [Tooltip("Handles time management including countdowns, timers, and related functions.")]
        [SerializeField] private TimeManager _timeManager;

        [Tooltip("")]
        [SerializeField] private TileGrid _tileGrid;

        [Tooltip("Handles pathfinding operations for navigating the grid.")]
        [SerializeField] private Pathfinding _pathfinding;

        [Tooltip("")]
        [SerializeField] private CoinStackManager _coinStackManager;

        [Tooltip("")]
        [SerializeField] private MatchChecker _matchChecker;

        [Tooltip("")]
        [SerializeField] private ProgressManager _progressManager;

        [Tooltip("")]
        [SerializeField] private CoinManager _coinManager;

        [Tooltip("")]
        [SerializeField] private UIManager _uiManager;

        [Tooltip("")]
        [SerializeField] private LevelManager _levelManager;
        
        [Tooltip("")]
        [SerializeField] private InputHandler _inputHandler;

        /// <summary>
        /// Provides public access to the AudioManager instance.
        /// </summary>
        public AudioManager AudioManager
        {
            get => _audioManager;
            private set => _audioManager = value;
        }

        /// <summary>
        /// Provides public access to the ParticleManager instance.
        /// </summary>
        public ParticleManager ParticleManager
        {
            get => _particleManager;
            private set => _particleManager = value;
        }

        /// <summary>
        /// Provides public access to the PopUpTextManager instance.
        /// </summary>
        public PopUpTextManager PopUpTextManager
        {
            get => _popUpTextManager;
            private set => _popUpTextManager = value;
        }

        /// <summary>
        /// Provides public access to the TimeManager instance.
        /// </summary>
        public TimeManager TimeManager
        {
            get => _timeManager;
            private set => _timeManager = value;
        }

        /// <summary>
        /// Provides public access to the TileGrid instance.
        /// </summary>
        public TileGrid TileGrid
        {
            get => _tileGrid;
            private set => _tileGrid = value;
        }

        /// <summary>
        /// Provides public access to the Pathfinder instance.
        /// </summary>
        public Pathfinding Pathfinding
        {
            get => _pathfinding;
            private set => _pathfinding = value;
        }

        /// <summary>
        /// Provides public access to the CoinStackManager instance.
        /// </summary>
        public CoinStackManager CoinStackManager
        {
            get => _coinStackManager;
            private set => _coinStackManager = value;
        }

        /// <summary>
        /// Provides public access to the MatchChecker instance.
        /// </summary>
        public MatchChecker MatchChecker
        {
            get => _matchChecker;
            private set => _matchChecker = value;
        }

        /// <summary>
        /// Provides public access to the ProgressManager instance.
        /// </summary>
        public ProgressManager ProgressManager
        {
            get => _progressManager;
            private set => _progressManager = value;
        }

        /// <summary>
        /// Provides public access to the CoinManager instance.
        /// </summary>
        public CoinManager CoinManager
        {
            get => _coinManager;
            private set => _coinManager = value;
        }

        /// <summary>
        /// Provides public access to the UIManager instance.
        /// </summary>
        public UIManager UIManager
        {
            get => _uiManager;
            private set => _uiManager = value;
        }

        /// <summary>
        /// Provides public access to the LevelManager instance.
        /// </summary>
        public LevelManager LevelManager
        {
            get => _levelManager;
            private set => _levelManager = value;
        }

        /// <summary>
        /// Provides public access to the LevelManager instance.
        /// </summary>
        public InputHandler InputHandler
        {
            get => _inputHandler;
            private set => _inputHandler = value;
        }
    }
}