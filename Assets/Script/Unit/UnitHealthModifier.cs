using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Assets.Script
{
    public class UnitHealthModifier :UnitModifier
    {
        private float baseHealth;
        private float multiplicatorHealth;
        private float bonusHealth;
        public UnitHealthModifier(UnitEnum.ModifierEnum type, float baseHealth, float multiplicatorHealth, float bonusHealth) : base(type)
        {
            this.baseHealth = baseHealth;
            this.multiplicatorHealth = multiplicatorHealth;
            this.bonusHealth = bonusHealth;
        }

        public override void applyModifier(Unit unit)
        {
            unit.maxHealth.changeMultiplicator(multiplicatorHealth);
            unit.maxHealth.changeBonus(bonusHealth);
            unit.maxHealth.changeBase(baseHealth);
        }

        private static void initialize()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(XMLAsset.getPathXml("HealthModifier.xml"));
            XmlNodeList movModifiersXML = xmlDoc.SelectNodes("//HealthModifier");
            foreach (XmlNode movModifierXML in movModifiersXML)
            {
                string nameStr = movModifierXML.SelectSingleNode("Name")?.InnerText;
                if (Enum.TryParse<UnitEnum.ModifierEnum>(nameStr, true, out UnitEnum.ModifierEnum modifierEnum))
                {
                    
                    if (int.TryParse(movModifierXML.SelectSingleNode("Base")?.InnerText, out int baseHealtht) &&
                        int.TryParse(movModifierXML.SelectSingleNode("Bonus")?.InnerText, out int bonusHealths) &&
                        int.TryParse(movModifierXML.SelectSingleNode("Multiplicator")?.InnerText, out int multiplicator))
                    {

                        addToDictionary(new UnitHealthModifier(modifierEnum, baseHealtht, multiplicator, bonusHealths));
                    }
                    else throw new Exception("Invalid health ");
                }
                else throw new Exception("Enum doesn't exist for " + nameStr);
            }
        }
    }
}