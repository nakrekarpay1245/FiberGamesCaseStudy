using CS3D.TileSystem;
using System.Collections.Generic;
using UnityEngine;

namespace CS3D.Pathfinding
{
    /// <summary>
    /// Provides A* pathfinding functionality for navigating through tiles.
    /// </summary>
    public class Pathfinding : MonoBehaviour
    {
        // globalbinder or static
        public TileManager TileManager;

        /// <summary>
        /// Finds the shortest path between the start and target tiles using the A* algorithm.
        /// </summary>
        /// <param name="startTile">The starting tile.</param>
        /// <param name="targetTile">The target tile.</param>
        /// <returns>A list of tiles representing the path to the target.</returns>
        public List<Tile> FindPath(Tile startTile, Tile targetTile)
        {
            // The set of nodes to be evaluated
            List<Tile> openSet = new List<Tile>();
            // The set of nodes already evaluated
            HashSet<Tile> closedSet = new HashSet<Tile>();

            // Dictionary to store the cost from start to each tile
            Dictionary<Tile, float> gCost = new Dictionary<Tile, float>();
            // Dictionary to store the cost from start to the target through each tile
            Dictionary<Tile, float> fCost = new Dictionary<Tile, float>();
            // Dictionary to track the most efficient previous step
            Dictionary<Tile, Tile> cameFrom = new Dictionary<Tile, Tile>();

            openSet.Add(startTile);
            gCost[startTile] = 0;
            fCost[startTile] = GetHeuristic(startTile, targetTile);

            while (openSet.Count > 0)
            {
                // Get the tile in the open set with the lowest F cost
                Tile currentTile = GetTileWithLowestFCost(openSet, fCost);

                // If we reached the target, reconstruct and return the path
                if (currentTile == targetTile)
                {
                    return ReconstructPath(cameFrom, currentTile);
                }

                openSet.Remove(currentTile);
                closedSet.Add(currentTile);

                // Evaluate neighbors
                foreach (Tile neighbor in TileManager.GetNeighbors(currentTile))
                {
                    if (closedSet.Contains(neighbor) || neighbor.IsOccupied || !neighbor.IsPlaceable || neighbor.IsReserved)
                    {
                        continue; // Ignore the neighbor which is already evaluated or occupied
                    }

                    float tentativeGCost = gCost[currentTile] + GetDistance(currentTile, neighbor);

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                    else if (tentativeGCost >= gCost[neighbor])
                    {
                        continue; // This is not a better path
                    }

                    // This path is the best so far
                    cameFrom[neighbor] = currentTile;
                    gCost[neighbor] = tentativeGCost;
                    fCost[neighbor] = gCost[neighbor] + GetHeuristic(neighbor, targetTile);
                }
            }

            // Return an empty path if no path is found
            return new List<Tile>();
        }

        /// <summary>
        /// Reconstructs the path from the target tile back to the start tile.
        /// </summary>
        /// <param name="cameFrom">The dictionary of navigated nodes.</param>
        /// <param name="currentTile">The current tile being reconstructed.</param>
        /// <returns>The reconstructed path as a list of tiles.</returns>
        private List<Tile> ReconstructPath(Dictionary<Tile, Tile> cameFrom, Tile currentTile)
        {
            List<Tile> path = new List<Tile>();
            while (cameFrom.ContainsKey(currentTile))
            {
                path.Add(currentTile);
                currentTile = cameFrom[currentTile];
            }
            path.Reverse(); // Optional: reverse the path to start from the start tile
            return path;
        }

        /// <summary>
        /// Gets the tile in the open set with the lowest F cost.
        /// </summary>
        /// <param name="openSet">The set of open tiles.</param>
        /// <param name="fCost">The dictionary of F costs.</param>
        /// <returns>The tile with the lowest F cost.</returns>
        private Tile GetTileWithLowestFCost(List<Tile> openSet, Dictionary<Tile, float> fCost)
        {
            Tile lowestFCostTile = openSet[0];
            foreach (Tile tile in openSet)
            {
                if (fCost[tile] < fCost[lowestFCostTile])
                {
                    lowestFCostTile = tile;
                }
            }
            return lowestFCostTile;
        }

        /// <summary>
        /// Estimates the heuristic cost from the given tile to the target tile (using Manhattan distance).
        /// </summary>
        /// <param name="tileA">The start tile.</param>
        /// <param name="tileB">The target tile.</param>
        /// <returns>The heuristic cost.</returns>
        private float GetHeuristic(Tile tileA, Tile tileB)
        {
            return Mathf.Abs(tileA.TileGridPosition.x - tileB.TileGridPosition.x) +
                Mathf.Abs(tileA.TileGridPosition.y - tileB.TileGridPosition.y);
        }

        /// <summary>
        /// Calculates the distance between two adjacent tiles.
        /// </summary>
        /// <param name="tileA">The first tile.</param>
        /// <param name="tileB">The second tile.</param>
        /// <returns>The distance between the two tiles.</returns>
        private float GetDistance(Tile tileA, Tile tileB)
        {
            return Vector3.Distance(tileA.transform.position, tileB.transform.position);
        }
    }
}