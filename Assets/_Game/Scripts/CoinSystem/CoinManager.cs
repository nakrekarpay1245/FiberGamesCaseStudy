using _Game._helpers;
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
    public Coin GetCoin(Vector3 position, Transform parent)
    {
        Coin coin = _coinPool.GetObject();
        coin.transform.parent = parent;
        return coin;
    }

    public void DeactivateCoin(Coin coin)
    {
        coin.transform.parent = null;
        _coinPool.ReturnObject(coin);
    }
}
