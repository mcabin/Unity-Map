using System;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace Assets.Script.TileMap
{
    public class TileFeatureType
    {
        public float movDifficulty { get;private set;}
        public TileEnum.FeatureEnum type { get; private set; }
        private static Dictionary<TileEnum.FeatureEnum,TileFeatureType> typesDictionary;
        public TileFeatureType(TileEnum.FeatureEnum type,float movDifficulty )
        {
            this.movDifficulty = movDifficulty;
            this.type = type;
        }


        static TileFeatureType()
        {
            Initialise();
        
        }

        static void Initialise()
        {
            typesDictionary=new Dictionary<TileEnum.FeatureEnum, TileFeatureType> ();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(XMLAsset.getPathXml("Feature.xml"));
            XmlNodeList featuresXML = xmlDoc.SelectNodes("//Feature");
            foreach (XmlNode featureXML in featuresXML)
            {
                string typeStr = featureXML.SelectSingleNode("Type")?.InnerText;
                if (Enum.TryParse<TileEnum.FeatureEnum>(typeStr, true, out TileEnum.FeatureEnum featureEnum))
                {
                    if (int.TryParse(featureXML.SelectSingleNode("MovDifficulty")?.InnerText, out int MovDifficulty)) {
                        typesDictionary.Add(featureEnum, new TileFeatureType(featureEnum, MovDifficulty));
                    }
                    else
                        throw new Exception("Invalid MovDifficulty");
                }
                else
                {
                    throw new Exception("Type "+typeStr+" invalid");
                }
            }
        }
        public static TileFeatureType getType(TileEnum.FeatureEnum key)
        {
            return typesDictionary[key];
        }
    }
}