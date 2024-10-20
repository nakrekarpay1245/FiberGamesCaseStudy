using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CS3D.TileSystem
{
    public static class ImageToTileMapData
    {
        public static Color placeableColor = new Color(255, 255, 255);
        public static Color nonPlaceableColor = new Color(255, 255, 255);

        public static TileMapData GenerateLevel(Texture2D map)
        {
            Vector2Int gridSize = new Vector2Int(map.width, map.height);
            TileMapData tileMapData = new TileMapData(gridSize);

            for (int i = 0; i < map.width; i++)
            {
                for (int j = 0; j < map.height; j++)
                {
                    if (!IsTilePlaceable(i, j, map))
                    {
                        Vector2Int position = new Vector2Int(i, j);
                        tileMapData.NonPlaceableTilePositions.Add(position);
                    }
                }
            }

            return tileMapData;
        }

        private static bool IsTilePlaceable(int x, int y, Texture2D map)
        {
            Color pixelColor = map.GetPixel(x, y);

            if (pixelColor.a == 0)
            {
                return false;
            }

            if (pixelColor.Equals(placeableColor))
            {
                return true;
            }
            return false;
        }
    }
}