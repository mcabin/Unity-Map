using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileEnum 
{
    public enum FeatureEnum
    {
        FOREST = 0,
        MOUNTAIN = 1
    }

    public enum AltitudeEnum
    {
        SEA= 0,
        PLAIN= 1,
        PLATEAU=2
    }
    public enum ElevEnum
    {
        ELEV_PPP = 0,
        ELEV_EPP = 1,
        ELEV_PPE = 2,
        ELEV_EPE= 3,
        ELEV_ONE = 4,
        ELEV_CURVE_EE= 5,
        ELEV_CURVE_EP=6,
        ELEV_CURVE_PE = 7,
        ELEV_CURVE_PP = 8,
        ELEV_BRIDGE=9,
        SOLO_PLATEAU=10,
        CLIFF_MIDDLE=11,
        CLIFF_EDGE=12,
        PLATEAU = 13,
        PLAIN =14,
        COAST_EDGE=15,
        COAST_MIDDLE=16,
        WATER=17,
        UNKNOW =18,
   
    }
    public enum BiomeEnum
    {
        GRASSLAND = 0,
        SWAMP=1,
        DESERT = 2,
        SAVANNA= 3,
        JUNGLE= 4,
        SNOWDESERT = 5,
        TAIGA= 6,
        WATER=7
    }
}
