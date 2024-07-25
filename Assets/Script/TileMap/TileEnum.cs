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
        PLATEAU= 3,
        MOUNTAIN= 4
    }
    public enum ElevEnum
    {
        ELEV_BASIC_SLOPE= 0,
        ELEV_SLOP_RIGHT = 1,
        ELEV_SLOP_LEFT = 2,
        ELEV_EE_VIRAGE = 3,
        ELEV_EP_VIRAGE=4,
        ELEV_PE_VIRAGE = 5,
        ELEV_PP_VIRAGE = 6,
        ELEV_3_PLAT= 7,
        ELEV_NO_NEIGHBOOR= 8,
        ELEV_PARA_EE=9,
        ERROR=10
   
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
