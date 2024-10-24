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
        private float movementCapacityBase;
        private float movementCapacityMultiplicator;
        private float movementCapacityBonus;
        private Dictionary<TileEnum.BiomeEnum, float> biomesCostsDictionary;

        public UnitMovementModifier(UnitEnum.ModifierEnum type, float movementCapacityBase,float movementCapacityMultiplicator,float movementCapacityBonus, Dictionary<TileEnum.BiomeEnum, float> biomeCosts)
        :base(type)
        {
            this.movementCapacityBonus = movementCapacityBonus;
            this.movementCapacityBase = movementCapacityBase;
            this.movementCapacityMultiplicator = movementCapacityMultiplicator;
            this.biomesCostsDictionary = biomeCosts;
        }

        static UnitMovementModifier()
        {
            initialize();  
        }
        public override void applyModifier(Unit unit)
        {
            foreach(KeyValuePair< TileEnum.BiomeEnum,float> biomeCost in biomesCostsDictionary)
            {
                unit.movementCoastByBiome[biomeCost.Key] *= biomeCost.Value;
            }
            unit.movementCapacity.changeMultiplicator(movementCapacityMultiplicator);
            unit.movementCapacity.changeBase(movementCapacityBase);
            unit.movementCapacity.changeBonus(movementCapacityBonus);

        }

        private static void initialize()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(XMLAsset.getPathXml("MovementModifier.xml"));
            XmlNodeList movModifiersXML = xmlDoc.SelectNodes("//MovementModifier");
            foreach (XmlNode movModifierXML in movModifiersXML)
            {
                string nameStr = movModifierXML.SelectSingleNode("Name")?.InnerText;
                if (Enum.TryParse<UnitEnum.ModifierEnum>(nameStr, true, out UnitEnum.ModifierEnum modifierEnum))
                {
                    Dictionary<TileEnum.BiomeEnum, float> newDictionary = new Dictionary<TileEnum.BiomeEnum, float>();
                    XmlNodeList biomesCostsXML = movModifierXML.SelectNodes("//BiomeCost");
                    foreach(XmlNode biomeCostXML in biomesCostsXML)
                    {
                        if (float.TryParse(biomeCostXML.SelectSingleNode("Cost")?.InnerText, out float cost) &&
                            Enum.TryParse<TileEnum.BiomeEnum>(biomeCostXML.SelectSingleNode("Biome")?.InnerText, true, out TileEnum.BiomeEnum biomeEnum)
                        )
                        {
                            newDictionary.Add(biomeEnum, cost);
                        }
                        else throw new Exception("Invalid Biome Cost");
                    }
                    if(int.TryParse(movModifierXML.SelectSingleNode("MovementCapacity").SelectSingleNode("Base")?.InnerText, out int baseMovement)&&
                        int.TryParse(movModifierXML.SelectSingleNode("MovementCapacity").SelectSingleNode("Bonus")?.InnerText, out int bonusMovement)&&
                        int.TryParse(movModifierXML.SelectSingleNode("MovementCapacity").SelectSingleNode("Multiplicator")?.InnerText, out int multiplicator))
                    {

                        addToDictionary( new UnitMovementModifier(modifierEnum, baseMovement,multiplicator,bonusMovement, newDictionary));
                    }
                    else throw new Exception("Invalid move Capacity");
                }
                else throw new Exception("Enum doesn't exist for " + nameStr);
            }
        }

    }
}