using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Script
{
    public abstract class UnitModifier 
    {
        protected static Dictionary<UnitEnum.ModifierEnum, List<UnitModifier>> modifierDictionary;
        protected static void addToDictionary(UnitModifier unitModifier)
        {
            UnitEnum.ModifierEnum modifEnum = unitModifier.name;
            if (modifierDictionary.ContainsKey(modifEnum))
            {
                modifierDictionary[modifEnum].Add(unitModifier);
            }
            else
            {
                modifierDictionary.Add(modifEnum, new List<UnitModifier>() { unitModifier });
            }
        }
        public UnitEnum.ModifierEnum name; 
        public abstract void applyModifier(Unit unit);
        public abstract void removeModifier(Unit unit);

        public static void applyWithKey(Unit unit,UnitEnum.ModifierEnum key)
        {
            List<UnitModifier>  modifList=modifierDictionary[key];
            foreach(UnitModifier modif in modifList)
            {
                modif.applyModifier(unit);
            }
        }

        public static void removeWithKey(Unit unit, UnitEnum.ModifierEnum key)
        {
            List<UnitModifier> modifList = modifierDictionary[key];
            foreach (UnitModifier modif in modifList)
            {
                modif.removeModifier(unit);
            }
        }
    }
}