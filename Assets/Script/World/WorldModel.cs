using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Script.World
{
    public class WorldModel : ScriptableObject
    {
        private TileMapModel tileMap;
        private Dictionary<Vector2Int, Unit> unitsByPos;

        public WorldModel(TileMapModel tileMap,Dictionary<Vector2Int,Unit> unitsByPos) {
            this.tileMap = tileMap;
            this.unitsByPos = unitsByPos;
        }

        public WorldModel(TileMapModel tileMap) {
            this.tileMap = tileMap;
        }

        public void AddUnit(Vector2Int coord,Unit unit)
        {
            this.unitsByPos.Add(coord, unit);
        }
        
        public Unit GetUnit(Vector2Int coord)
        {
            return this.unitsByPos[coord];
        }
    }
}