using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Script
{
    public class UnitViewAsset : ScriptableObject
    {
        const int yBaseCoord = 1;
        static private Dictionary<UnitEnum.ClassEnum, GameObject> tileModels;
        static private string tileModelPath = "Prefab/Unit";

        static UnitViewAsset() {
            initializeUnitModel();
        }
        public static Vector3 getUnitPostion(Vector2Int vec, int z = yBaseCoord)
        {
            return MapViewAsset.getTilePosition(vec,z);
        }

        private static void initializeUnitModel()
        {
            tileModels = new Dictionary<UnitEnum.ClassEnum, GameObject>();
            GameObject[] modelsTab = Resources.LoadAll<GameObject>(tileModelPath);
            for (int i = 0; i < modelsTab.Length; i++)
            {
                string name = modelsTab[i].name;
                if (Enum.TryParse(name, true, out UnitEnum.ClassEnum classEnum))
                {
                    tileModels.Add(classEnum, modelsTab[i]);
                }
                else throw new Exception("Model :" + name + " doesn't exist.");
            }
        }

        public static GameObject getUnitModel(UnitEnum.ClassEnum classEnum)
        {
            return tileModels[classEnum];
        }
    }
}