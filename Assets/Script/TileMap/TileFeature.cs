using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class TileFeature
{
    public TileEnum.FeatureEnum type;
    public System.Func<float, float> movDifficutltyModifier;

    public TileFeature(TileEnum.FeatureEnum type, System.Func<float, float> movDifficulty)
    {
        this.type = type;
        this.movDifficutltyModifier = movDifficulty;
    }
}
