using _Game._helpers;
using CS3D._Enums;
using CS3D.CoinSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [Header("Coin Manager Settings")]
    [Header("Coin Prefab")]
    [Tooltip("The prefab of the coin to instantiate.")]
    [SerializeField] private string _coinPrefabResourceKey = "Coin/Coin";

    [Tooltip("Size of the object pool.")]
    [SerializeField, Range(50, 500)] private int _poolSize = 10;

    [Header("Coin Levels and Colors")]
    [Tooltip("List of coin levels and their corresponding colors.")]
    [SerializeField]
    private List<CoinLevelData> _coinLevelDataList = new List<CoinLevelData>
                {
                    new CoinLevelData(CoinLevel.Level1, Color.red),
                    new CoinLevelData(CoinLevel.Level2, Color.blue),
                    new CoinLevelData(CoinLevel.Level3, Color.green),
                    new CoinLevelData(CoinLevel.Level4, Color.yellow),
                    new CoinLevelData(CoinLevel.Level5, Color.magenta),
                    new CoinLevelData(CoinLevel.Level6, Color.cyan),
                    new CoinLevelData(CoinLevel.Level7, Color.gray),
                    new CoinLevelData(CoinLevel.Level8, Color.white)
                };

    private ObjectPool<Coin> _coinPool;

    private void Awake()
    {
        InitializeObjectPool();
    }

    /// <summary>
    /// Initializes the object pool with the specified size.
    /// </summary>
    private void InitializeObjectPool()
    {
        _coinPool = new ObjectPool<Coin>(InstantiateCoin, _poolSize);
    }

    /// <summary>
    /// Instantiates a new PopUpText instance and adds it to the pool.
    /// </summary>
    /// <returns>A new PopUpText instance.</returns>
    private Coin InstantiateCoin()
    {
        Coin coinPrefab = Resources.Load<Coin>(_coinPrefabResourceKey);

        if (coinPrefab != null)
        {
            // Instantiate the coin prefab
            Coin coinObject = Instantiate(coinPrefab, transform);

            coinObject.gameObject.SetActive(false);

            return coinObject;
        }
        return null;
    }

    /// <summary>
    /// Displays a pop-up text at the specified position with optional custom duration.
    /// </summary>
    public Coin GetCoin(Vector3 position, Transform parent, CoinLevel coinLevel)
    {
        Coin coin = _coinPool.GetObject();

        int score = (int)coinLevel + 1;

        CoinLevelData data = _coinLevelDataList.Find(item => item.Level == coinLevel);
        Color coinColor = data.CoinColor;

        coin.Initialize(coinLevel, score, coinColor);

        coin.transform.parent = parent;
        coin.transform.localScale = Vector3.one;
        coin.transform.rotation = Quaternion.identity;
        coin.gameObject.SetActive(true);
        return coin;
    }

    public void DeactivateCoin(Coin coin)
    {
        coin.transform.parent = null;
        _coinPool.ReturnObject(coin);
    }
}
