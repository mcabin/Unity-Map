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
        ELEVATION=2,
        MOUNTAIN= 3
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
        PLAIN=9,
        PLATEAU= 10,
        UNKNOW=11
   
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
        WATER=7,
    }
}
