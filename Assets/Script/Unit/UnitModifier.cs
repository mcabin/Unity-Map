using UnityEditor;
using UnityEngine;

namespace Assets.Script
{
    public abstract class UnitModifier 
    {
        public UnitEnum.ModifierEnum name; 
        public abstract void applyModifier(Unit unit);
        public abstract void removeModifier(Unit unit);

    }
}