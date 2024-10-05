using Assets.Script;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
namespace Assets.Script
{

    public class MapViewAsset
    {
        const int yBaseCoord = 0;
        static private int widthTileSize = 2;
        static private int heightTileSize = 2;

        static private Dictionary<TileEnum.ElevEnum, GameObject> tileModels;
        static private Dictionary<TileEnum.BiomeEnum, Material> tileMaterial;

        static private string tileModelPath = "Prefab/Tile";
        static private string shaderPath = "Shader Graphs/TileShader";
        static MapViewAsset()
        {
            initializeTileModel();
            initializeBiomeMaterial();
        }


        public static Vector3 getTilePosition(int w, int h, int z = yBaseCoord)
        {
            return new Vector3(h * heightTileSize, z, w * widthTileSize);
        }

        private static void initializeTileModel()
        {
            tileModels = new Dictionary<TileEnum.ElevEnum, GameObject>();
            GameObject[] modelsTab = Resources.LoadAll<GameObject>(tileModelPath);
            for (int i = 0; i < modelsTab.Length; i++)
            {
                string name = modelsTab[i].name;
                if (Enum.TryParse(name, true, out TileEnum.ElevEnum elevEnum))
                {
                    tileModels.Add(elevEnum, modelsTab[i]);
                }
                else throw new Exception("Model :" + name + " doesn't exist.");
            }
        }

        private static void initializeBiomeMaterial()
        {
            tileMaterial = new Dictionary<TileEnum.BiomeEnum, Material>();
            Material material = new Material(Shader.Find(shaderPath));
            foreach (TileEnum.BiomeEnum e in Enum.GetValues(typeof(TileEnum.BiomeEnum)))
            {
                Color color = ColorList.getBiomeColor(e);
                Material newMat = new Material(material);
                newMat.SetColor("_NewColor", color);

                tileMaterial.Add(e, newMat);
            }
        }
        public static GameObject getTileModel(TileEnum.ElevEnum elevEnum)
        {
            return tileModels[elevEnum];
        }
        public static Material getTileMaterial(TileEnum.BiomeEnum biomeEnum)
        {
            return tileMaterial[biomeEnum];
        }

    }
}