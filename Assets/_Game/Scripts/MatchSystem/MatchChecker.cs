using System.Collections.Generic;
using UnityEngine;
using _Game._helpers;
using CS3D.CoinSystem;
using CS3D.TileSystem;
using DG.Tweening;
using static UnityEngine.Rendering.DebugUI.Table;

namespace CS3D.MatchSystem
{
    /// <summary>
    /// The MatchChecker class is responsible for checking matches in the game's tile grid.
    /// It verifies if a given tile's coin type matches with its neighboring tiles (up, down, left, and right).
    /// </summary>
    public class MatchChecker : MonoBehaviour
    {
        [Header("Match Checker Settings")]
        [Tooltip("The delay between each coin transfer animation during a match.")]
        [SerializeField, Range(0.01f, 1f)]
        private float _coinTransferInterval = 0.05f;

        [Tooltip("The delay before initiating the match processing animation.")]
        [SerializeField, Range(0.1f, 1f)]
        private float _matchProcessingDelay = 0.35f;

        [Tooltip("The delay between consecutive match checks to ensure smooth transitions.")]
        [SerializeField, Range(0.01f, 0.5f)]
        private float _matchCheckInterval = 0.1f;

        /// <summary>
        /// Checks for matches in the entire tile grid and stops if a match is found.
        /// </summary>
        public bool CheckForMatches()
        {
            Tile[,] tileGrid = GlobalBinder.singleton.TileManager.TileGrid;
            int rows = tileGrid.GetLength(0);
            int cols = tileGrid.GetLength(1);

            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < cols; y++)
                {
                    // If a match is found, break out of both loops
                    if (CheckTileMatches(tileGrid[x, y]))
                    {
                        Debug.Log("Match found, stopping further checks.");
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the specified tile's coin matches with its four neighboring tiles.
        /// </summary>
        /// <param name="tile">The tile to check for matching neighbors.</param>
        /// <returns>True if a match is found, false otherwise.</returns>
        private bool CheckTileMatches(Tile tile)
        {
            if (tile == null || !tile.CoinStack) return false;

            Coin coin = tile.CoinStack.GetCoin();
            if (coin == null) return false;

            // Check the four neighboring tiles for a match
            if (CheckNeighbor(tile, Vector2Int.up)) return true;
            if (CheckNeighbor(tile, Vector2Int.down)) return true;
            if (CheckNeighbor(tile, Vector2Int.left)) return true;
            if (CheckNeighbor(tile, Vector2Int.right)) return true;

            return false; // No match found
        }

        /// <summary>
        /// Checks if a neighboring tile has the same coin type as the specified tile.
        /// </summary>
        /// <param name="tile">The tile whose neighbor will be checked.</param>
        /// <param name="direction">The direction in which to check the neighbor.</param>
        /// <returns>True if a match is found with the neighbor, false otherwise.</returns>
        private bool CheckNeighbor(Tile tile, Vector2Int direction)
        {
            Vector2Int neighborPosition = tile.TileGridPosition + direction;
            Tile neighborTile = GlobalBinder.singleton.TileManager.GetTileAt(neighborPosition.x, neighborPosition.y);

            if (neighborTile != null && neighborTile.CoinStack != null &&
                neighborTile.Level == tile.Level)
            {
                HandleMatch(tile, neighborTile);
                return true;
            }

            return false; // No match found with the neighbor
        }

        /// <summary>
        /// Handles the logic when a match is found.
        /// </summary>
        /// <param name="firstTile">The first tile in the match.</param>
        /// <param name="neighborTile">The neighboring tile in the match.</param>
        private void HandleMatch(Tile firstTile, Tile neighborTile)
        {
            Sequence matchSequence = DOTween.Sequence();

            //Debug.Log($"Match found between {firstTile} and {neighborTile} tiles!");

            bool firstIsGreater = firstTile.Weight > neighborTile.Weight;

            matchSequence.AppendInterval(_matchProcessingDelay);

            if (firstIsGreater)
            {
                int tileCount = neighborTile.Weight;
                for (int i = 0; i < tileCount; i++)
                {
                    matchSequence.AppendCallback(() =>
                    {
                        Coin coin = neighborTile.CoinStack.RemoveCoin();
                        firstTile.CoinStack.AddCoin(coin);
                    });
                    matchSequence.AppendInterval(_coinTransferInterval);
                }
            }
            else
            {
                int tileCount = firstTile.Weight;
                for (int i = 0; i < tileCount; i++)
                {
                    matchSequence.AppendCallback(() =>
                    {
                        Coin coin = firstTile.CoinStack.RemoveCoin();
                        neighborTile.CoinStack.AddCoin(coin);
                    });
                    matchSequence.AppendInterval(_coinTransferInterval);
                }
            }

            matchSequence.AppendInterval(_matchCheckInterval);

            matchSequence.OnComplete(() =>
            {
                firstTile.EnsureCoinWeightLimit();
                neighborTile.EnsureCoinWeightLimit();
                Debug.Log("Match handling completed.");
            });

            // Append the individual match sequence to the global match checking sequence
            DOTween.Sequence().Append(matchSequence);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                TileControl();
            }
        }

        private void TileControl()
        {
            Tile[,] tileGrid = GlobalBinder.singleton.TileManager.TileGrid;
            int rows = tileGrid.GetLength(0);
            int cols = tileGrid.GetLength(1);

            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < cols; y++)
                {
                    tileGrid[x, y].EnsureCoinWeightLimit();
                }
            }
        }
    }
}