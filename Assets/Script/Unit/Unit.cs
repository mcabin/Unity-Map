using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script
{
    public class Unit
    {
        public int life { get; private set; }
        private int coordW, coordH;

        public Dictionary<TileEnum.BiomeEnum, float> movementCoastByBiome;
        private List<UnitModifier> modifiers;
        public float movementCapacity;

        public void move(int w, int h)
        {
            coordH = h;
            coordW = w;
        }

        public (int w, int h) getPosition()
        {
            return (coordW, coordH);
        }

        
    }
}

