using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapGenerator : MonoBehaviour
{
    public Texture2D map;

    public ColorToTile[] ColorMappings;

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        for (int i = 0; i < map.width; i++)
        {
            for (int j = 0; j < map.height; j++)
            {
                GenerateTile(i, j);
            }
        }
    }

    private void GenerateTile(int x, int y)
    {
        Color pixelColor = map.GetPixel(x, y);

        if (pixelColor.a == 0)
        {
            return;
        }

        foreach (ColorToTile colorMapping in ColorMappings)
        {
            if (colorMapping.Color.Equals(pixelColor))
            {
                Vector3 position = new Vector3(x, 0, y);
                Instantiate(colorMapping.Prefab, position, Quaternion.identity, transform);
            }
        }
    }
}
