using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AltitudeType
{
    public TileEnum.AltitudeEnum type { get; private set; }
    public int minAltitude { get; private set; }
    public int maxAltitude { get; private set; }

    public int level => (int)type;
    
    public AltitudeType(TileEnum.AltitudeEnum type, int minAltitude, int maxAltitude)
    {
        this.type = type;
        this.minAltitude = minAltitude;
        this.maxAltitude = maxAltitude;
    }
    public bool isType(int altitude) {
        return altitude >= minAltitude && altitude <= maxAltitude;
    }
}
