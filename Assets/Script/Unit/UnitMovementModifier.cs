using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace Assets.Script
{
    public class UnitMovementModifier : UnitModifier
    {
        public UnitEnum.ModifierEnum type { get; private set; }
        private float movementCapacityModifier;
        private Dictionary<TileEnum.BiomeEnum, float> biomesCostsDictionary;

        public UnitMovementModifier(UnitEnum.ModifierEnum type, float movementCapacityModifier, Dictionary<TileEnum.BiomeEnum, float> biomeCosts)
        {
            this.type = type;
            this.movementCapacityModifier = movementCapacityModifier;
            this.biomesCostsDictionary = biomeCosts;
        }

        public override void applyModifier(Unit unit)
        {
            foreach(KeyValuePair< TileEnum.BiomeEnum,float> biomeCost in biomesCostsDictionary)
            {
                unit.movementCoastByBiome[biomeCost.Key] *= biomeCost.Value;
            }
            unit.movementCapacity *= movementCapacityModifier;

        }

        public override void removeModifier(Unit unit)
        {
            foreach (KeyValuePair<TileEnum.BiomeEnum, float> biomeCost in biomesCostsDictionary)
            {
                unit.movementCoastByBiome[biomeCost.Key] /= biomeCost.Value;
            }
            unit.movementCapacity /= movementCapacityModifier;
        }

        static private Dictionary<UnitEnum.ModifierEnum, UnitMovementModifier> allMovementModifiers;
        private static void initialize()
        {
            XmlDocument xmlDoc = new XmlDocument();
            allMovementModifiers = new Dictionary<UnitEnum.ModifierEnum, UnitMovementModifier>();
            xmlDoc.Load(XMLAsset.getPathXml("MovementModifier.xml"));
            XmlNodeList movModifiersXML = xmlDoc.SelectNodes("//MovementModifier");
            foreach (XmlNode movModifierXML in movModifiersXML)
            {
                string nameStr = movModifierXML.SelectSingleNode("Name")?.InnerText;
                if (Enum.TryParse<UnitEnum.ModifierEnum>(nameStr, true, out UnitEnum.ModifierEnum modifierEnum))
                {
                    Dictionary<TileEnum.BiomeEnum, float> newDictionary = new Dictionary<TileEnum.BiomeEnum, float>();
                    XmlNodeList biomesCostsXML = movModifierXML.SelectNodes("//BiomeCosts");
                    foreach(XmlNode biomeCostXML in biomesCostsXML)
                    {
                        if (int.TryParse(biomeCostXML.SelectSingleNode("Cost")?.InnerText, out int cost) &&
                            Enum.TryParse<TileEnum.BiomeEnum>(biomeCostXML.SelectSingleNode("Biome")?.InnerText, true, out TileEnum.BiomeEnum biomeEnum)
                        )
                        {
                            newDictionary.Add(biomeEnum, cost);
                        }
                        else throw new Exception("Invalid Biome Cost");
                    }
                    if(int.TryParse(movModifierXML.SelectSingleNode("MovementCapacity")?.InnerText, out int moveCapacity)){
                        allMovementModifiers.Add(modifierEnum, new UnitMovementModifier(modifierEnum, moveCapacity, newDictionary));
                    }
                    else throw new Exception("Invalid move Capacity");
                }
                else throw new Exception("Enum doesn't exist for " + nameStr);
            }
        }
    }
}