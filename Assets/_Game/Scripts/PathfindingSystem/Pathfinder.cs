using _Game._helpers;
using CS3D.TileSystem;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CS3D.Pathfinding
{
    public class Pathfinder : MonoBehaviour
    {
        private List<Tile> _pathTiles = new List<Tile>();

        /// <summary>
        /// Moves the soldier to a specified tile using A* pathfinding.
        /// </summary>
        /// <param name="targetTile">The tile to move the soldier to.</param>
        public bool TryMoveToTile(Tile targetTile)
        {
            bool success = false;
            if (_pathTiles.Count > 0)
            {
                foreach (var tile in _pathTiles)
                {
                    tile.PathHidden();
                }

                _pathTiles.Clear();
            }

            // Ensure the target tile is not occupied and is within the grid
            if (targetTile == null || targetTile.IsOccupied || targetTile.IsReserved || !targetTile.IsPlaceable)
            {
                Debug.LogWarning("Target tile is invalid or occupied or reserved or placed");
                success = false;
                return success;
            }

            Tile startTile = GlobalBinder.singleton.TileManager.GetClosestTile(transform.position);
            if (startTile == null)
            {
                Debug.LogWarning("Start tile is invalid.");
                success = false;
                return success;
            }

            List<Tile> path = GlobalBinder.singleton.Pathfinding.FindPath(startTile, targetTile);
            if (path.Count == 0)
            {
                Debug.LogWarning("No path found to the target tile.");
                success = false;
                return success;
            }

            StopAllCoroutines();
            StartCoroutine(MoveAlongPath(path));

            foreach (var tile in path)
            {
                _pathTiles.Add(tile);
                tile.PathVisible();
            }

            success = true;
            return success;
        }

        /// <summary>
        /// Moves the soldier along the specified path with smooth animation for each tile.
        /// </summary>
        /// <param name="path">The list of tiles representing the path.</param>
        /// <returns>An IEnumerator for coroutine execution.</returns>
        private IEnumerator MoveAlongPath(List<Tile> path)
        {
            ReserveTile(path.LastOrDefault());

            foreach (Tile tile in path)
            {
                Vector3 targetPosition = tile.transform.position;

                transform.DOKill();

                // Smoothly move the enemy to the next tile using DOTween
                transform.DOMove(targetPosition, GlobalBinder.singleton.TimeManager.TileMovementTime).SetEase(Ease.Linear);

                // Wait for the movement to complete before proceeding to the next tile
                yield return new WaitForSeconds(GlobalBinder.singleton.TimeManager.TileMovementTime);
            }
        }

        public virtual void ReserveTile(Tile tile)
        {
            tile.SetReserved(true);
        }
    }
}