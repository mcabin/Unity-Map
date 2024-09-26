using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorList 
{
    private static readonly Dictionary<TileEnum.BiomeEnum, Color> biomeColors = new Dictionary<TileEnum.BiomeEnum, Color> {
        {TileEnum.BiomeEnum.GRASSLAND,createColor(43, 128, 24) },
        {TileEnum.BiomeEnum.SWAMP,createColor(38, 117, 72) },
        {TileEnum.BiomeEnum.DESERT,createColor(222, 209, 98) },
        {TileEnum.BiomeEnum.SAVANNA,createColor(169, 181, 40) },
        {TileEnum.BiomeEnum.JUNGLE,createColor(33, 181, 55) },
        {TileEnum.BiomeEnum.SNOWDESERT,createColor(235, 242, 236) },
        {TileEnum.BiomeEnum.TAIGA,createColor(23, 66, 29) },
        {TileEnum.BiomeEnum.WATER,createColor(20,50,200) }
    };

    private static Color createColor(int r,int g,int b)
    {
        return new Color((float)r /255, (float)g /255,(float)b/255);
    }

    public static Color getBiomeColor(TileEnum.BiomeEnum biomeEnum)
    {
        if(biomeColors.TryGetValue(biomeEnum, out Color value))
        {
            return value;
        }
        else
        {
            return biomeColors[TileEnum.BiomeEnum.GRASSLAND];
        }
    }
}
