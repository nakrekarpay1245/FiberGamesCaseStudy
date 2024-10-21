using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using _Game._helpers;
using CS3D.CoinSystem;
using CS3D.TileSystem;

namespace CS3D.MatchSystem
{
    /// <summary>
    /// This class handles match checking and coin transferring between tiles in the game.
    /// </summary>
    public class MatchCheck : MonoBehaviour
    {
        private Tile[,] _tileGrid;
        private bool _matchFound;

        /// <summary>
        /// Initializes the tile grid from the GlobalBinder and starts the match check.
        /// </summary>
        public void CheckForMatch()
        {
            _tileGrid = GlobalBinder.singleton.TileGrid.Grid;
            StartMatchCheck();
        }

        /// <summary>
        /// Starts the match checking process.
        /// </summary>
        private void StartMatchCheck()
        {
            _matchFound = false;
            CheckAndTransferMatches();
        }

        /// <summary>
        /// Checks for matches in the grid and handles coin transfers using DOTween.
        /// If matches are found, it repeats the check after a delay.
        /// </summary>
        private void CheckAndTransferMatches()
        {
            Sequence matchSequence = DOTween.Sequence();
            _matchFound = false;

            // Loop through each tile in the grid
            for (int x = 0; x < _tileGrid.GetLength(0); x++)
            {
                for (int y = 0; y < _tileGrid.GetLength(1); y++)
                {
                    Tile currentTile = _tileGrid[x, y];
                    if (IsTileValid(currentTile))
                    {
                        if (CheckAndProcessNeighborMatches(currentTile, x, y, matchSequence))
                        {
                            _matchFound = true;
                        }
                    }
                }
            }

            // If a match was found, add a delay and repeat the check
            if (_matchFound)
            {
                matchSequence.AppendInterval(GlobalBinder.singleton.TimeManager.MatchCheckInterval);
                matchSequence.OnComplete(CheckAndTransferMatches);
                matchSequence.Play();
            }
        }

        /// <summary>
        /// Checks for matches with neighboring tiles and processes the transfers.
        /// </summary>
        private bool CheckAndProcessNeighborMatches(Tile currentTile, int x, int y, Sequence sequence)
        {
            bool matchProcessed = false;
            List<Tile> neighbors = GlobalBinder.singleton.TileGrid.GetNeighbors(currentTile);

            foreach (Tile neighbor in neighbors)
            {
                if (IsNeighborValid(neighbor, currentTile))
                {
                    Tile primaryTile = DeterminePrimaryTile(currentTile, neighbor);
                    Tile secondaryTile = (primaryTile == currentTile) ? neighbor : currentTile;

                    // Mark tiles as busy
                    MarkTilesAsBusy(primaryTile, secondaryTile);

                    // Transfer all coins from secondary to primary tile
                    TransferAllCoins(primaryTile, secondaryTile, sequence);
                    matchProcessed = true;
                }
            }
            return matchProcessed;
        }

        /// <summary>
        /// Transfers all coins from the secondary tile to the primary tile.
        /// </summary>
        private void TransferAllCoins(Tile primaryTile, Tile secondaryTile, Sequence sequence)
        {
            int transferCount = Mathf.Min(secondaryTile.Weight, primaryTile.Weight);
            int completedTransfers = 0;

            for (int i = 0; i < transferCount; i++)
            {
                sequence.AppendCallback(() =>
                {
                    Coin coin = secondaryTile.CoinStack.RemoveCoin();
                    primaryTile.CoinStack.AddCoin(coin);

                    completedTransfers++;
                    if (completedTransfers == transferCount)
                    {
                        HandleTransferCompletion(primaryTile, secondaryTile);
                    }
                });
            }
        }

        /// <summary>
        /// Handles completion of the coin transfer.
        /// </summary>
        private void HandleTransferCompletion(Tile primaryTile, Tile secondaryTile)
        {
            TileFree(primaryTile, secondaryTile);
            primaryTile.EnsureCoinWeightLimit();
            secondaryTile.EnsureCoinWeightLimit();
            CheckForMatch();
        }

        /// <summary>
        /// Frees the tiles after a delay.
        /// </summary>
        private void TileFree(Tile primaryTile, Tile secondaryTile)
        {
            DOVirtual.DelayedCall(GlobalBinder.singleton.TimeManager.MatchCheckInterval, () =>
            {
                primaryTile.IsBusy = false;
                secondaryTile.IsBusy = false;
            });
        }

        /// <summary>
        /// Determines the primary tile based on neighbor count and weight.
        /// </summary>
        private Tile DeterminePrimaryTile(Tile tileA, Tile tileB)
        {
            int tileANeighborCount = GetMatchingNeighborCount(tileA);
            int tileBNeighborCount = GetMatchingNeighborCount(tileB);

            return (tileANeighborCount > tileBNeighborCount ||
                    (tileANeighborCount == tileBNeighborCount && tileA.Weight >= tileB.Weight))
                ? tileA
                : tileB;
        }

        /// <summary>
        /// Returns the count of matching neighbors for a given tile.
        /// </summary>
        private int GetMatchingNeighborCount(Tile tile)
        {
            int matchCount = 0;
            List<Tile> neighbors = GlobalBinder.singleton.TileGrid.GetNeighbors(tile);

            foreach (Tile neighbor in neighbors)
            {
                if (neighbor != null && neighbor.IsOccupied && neighbor.Level == tile.Level)
                {
                    matchCount++;
                }
            }
            return matchCount;
        }

        /// <summary>
        /// Checks if the tile is valid for processing.
        /// </summary>
        private bool IsTileValid(Tile tile)
        {
            return tile != null && tile.IsOccupied && !tile.IsBusy;
        }

        /// <summary>
        /// Checks if the neighbor tile is valid for processing.
        /// </summary>
        private bool IsNeighborValid(Tile neighbor, Tile currentTile)
        {
            return neighbor != null && neighbor.IsOccupied &&
                   neighbor.Level == currentTile.Level && !neighbor.IsBusy;
        }

        /// <summary>
        /// Marks the given tiles as busy.
        /// </summary>
        private void MarkTilesAsBusy(Tile primaryTile, Tile secondaryTile)
        {
            primaryTile.IsBusy = true;
            secondaryTile.IsBusy = true;
        }
    }
}