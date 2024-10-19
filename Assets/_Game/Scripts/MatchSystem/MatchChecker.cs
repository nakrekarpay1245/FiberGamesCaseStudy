using System.Collections.Generic;
using UnityEngine;
using _Game._helpers;
using CS3D.CoinSystem;
using CS3D.TileSystem;
using DG.Tweening;

namespace CS3D.MatchSystem
{
    /// <summary>
    /// The MatchChecker class is responsible for checking matches in the game's tile grid.
    /// It verifies if a given tile's coin type matches with its neighboring tiles (up, down, left, and right).
    /// </summary>
    public class MatchChecker : MonoBehaviour
    {
        /// <summary>
        /// Checks for matches in the entire tile grid and stops if a match is found.
        /// </summary>
        public void CheckForMatches()
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
                        return;
                    }
                }
            }
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
                neighborTile.CoinStack.GetCoin().Level == tile.CoinStack.GetCoin().Level)
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

            Debug.Log($"Match found between {firstTile} and {neighborTile} tiles!");
            bool firstIsGreater = firstTile.CoinStack.GetCoinsByLevel(firstTile.CoinStack.GetCoin().Level).Count >
                 neighborTile.CoinStack.GetCoinsByLevel(firstTile.CoinStack.GetCoin().Level).Count;
            matchSequence.AppendInterval(0.35f);

            if (firstIsGreater)
            {
                int tileCount = neighborTile.CoinStack.GetCoinsByLevel(firstTile.CoinStack.GetCoin().Level).Count;
                for (int i = 0; i < tileCount; i++)
                {
                    matchSequence.AppendCallback(() =>
                    {
                        Coin coin = neighborTile.CoinStack.RemoveCoin();
                        firstTile.CoinStack.AddCoin(coin);
                    });
                    matchSequence.AppendInterval(.05f);
                }
            }
            else
            {
                int tileCount = firstTile.CoinStack.GetCoinsByLevel(firstTile.CoinStack.GetCoin().Level).Count;
                for (int i = 0; i < tileCount; i++)
                {
                    matchSequence.AppendCallback(() =>
                    {
                        Coin coin = firstTile.CoinStack.RemoveCoin();
                        neighborTile.CoinStack.AddCoin(coin);
                    });
                    matchSequence.AppendInterval(.05f);
                }
            }

            matchSequence.OnComplete(() =>
            {
                firstTile.ControlCoinStack();
                neighborTile.ControlCoinStack();
                CheckForMatches();
            });
            // TODO: Implement match handling logic such as removing coins or triggering effects.
        }
    }
}