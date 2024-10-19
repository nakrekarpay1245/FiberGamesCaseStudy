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
        [Tooltip("The movement speed of the enemy (time to move between tiles in seconds).")]
        [Range(0.001f, 0.25f)]
        [SerializeField] private float _tileMovementTime = 1f;

        private List<Tile> _pathTiles = new List<Tile>();

        //GlobalBinder or static
        public Pathfinding Pathfinding;

        //GlobalBinder or static
        public TileManager TileManager;

        /// <summary>
        /// Moves the soldier to a specified tile using A* pathfinding.
        /// </summary>
        /// <param name="targetTile">The tile to move the soldier to.</param>
        public void MoveToTile(Tile targetTile)
        {
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
                return;
            }

            Tile startTile = TileManager.GetClosestTile(transform.position);
            if (startTile == null)
            {
                Debug.LogWarning("Start tile is invalid.");
                return;
            }

            List<Tile> path = Pathfinding.FindPath(startTile, targetTile);
            if (path.Count == 0)
            {
                Debug.LogWarning("No path found to the target tile.");
                return;
            }

            StopAllCoroutines();
            StartCoroutine(MoveAlongPath(path));

            foreach (var tile in path)
            {
                _pathTiles.Add(tile);
                tile.PathVisible();
            }
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

                // Smoothly move the enemy to the next tile using DOTween
                transform.DOMove(targetPosition, _tileMovementTime).SetEase(Ease.Linear);

                // Wait for the movement to complete before proceeding to the next tile
                yield return new WaitForSeconds(_tileMovementTime);
            }

            OccupyTile(path.LastOrDefault());
        }

        public virtual void OccupyTile(Tile tile)
        {
            tile.SetOccupied(true);
        }

        public virtual void ReserveTile(Tile tile)
        {
            tile.SetReserved(true);
        }
    }
}