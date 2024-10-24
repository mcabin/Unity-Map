using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Assets.Script
{
    public class UnitClass
    {
        public List< UnitEnum.ModifierEnum > modifiers { get; private set; }

        public UnitEnum.ClassEnum type { get; private set; }

        public UnitClass(List<UnitEnum.ModifierEnum> modifiers, UnitEnum.ClassEnum type)
        {
            this.modifiers = modifiers;
            this.type = type;
        }

        private static Dictionary<UnitEnum.ClassEnum, UnitClass> allUnitClasses;
        private static void initialize()
        {
            XmlDocument xmlDoc = new XmlDocument();
            allUnitClasses = new Dictionary<UnitEnum.ClassEnum, UnitClass>();
            xmlDoc.Load(XMLAsset.getPathXml("UnitClass.xml"));
            XmlNodeList classesXML = xmlDoc.SelectNodes("//UnitClass");
            foreach (XmlNode classXML in classesXML)
            {
                string name = classXML.SelectSingleNode("Name")?.InnerText;
                if (Enum.TryParse<UnitEnum.ClassEnum>(name, true, out UnitEnum.ClassEnum classEnum))
                {
                    List<UnitEnum.ModifierEnum> modifierList = new List<UnitEnum.ModifierEnum>();
                    XmlNodeList modifiersXML = classXML.SelectNodes("./Modifier");
                    foreach (XmlNode modifierXML in modifiersXML)
                    {
                        if (Enum.TryParse<UnitEnum.ModifierEnum>(modifierXML?.InnerText, true, out UnitEnum.ModifierEnum modifierEnum)
                        )
                        {
                            Debug.Log(modifierEnum);
                            modifierList.Add(modifierEnum);
                        }
                        else throw new Exception("Invalid Modifier"+ modifierXML?.InnerText);
                    }
                    allUnitClasses.Add(classEnum,new UnitClass(modifierList,classEnum));
                }
                else throw new Exception("Enum doesn't exist for " + name);
            }
        }

    }
}