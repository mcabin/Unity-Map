using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Script.UnitEnum;

namespace Assets.Script
{
    public class Unit
    {
        public int currentHealth { get; private set; }

        
        public int maxHealth { get; private set; }
        private int coordW, coordH;

        public Dictionary<TileEnum.BiomeEnum, float> movementCoastByBiome;
        private HashSet<UnitEnum.ModifierEnum> modifiers;
        public float movementCapacity;
        private List<UnitClass> unitClasses;

        private void InitializeBaseMovementCoast()
        {
            foreach(TileEnum.BiomeEnum biome in Enum.GetValues(typeof(TileEnum.BiomeEnum)))
            {
                movementCoastByBiome.Add(biome, 1);
            }
        }
        public Unit(int coordW,int coordH,UnitClass baseClass)
        {
            this.coordW = coordW;
            this.coordH = coordH;
            unitClasses = new List<UnitClass>();
            addClass(baseClass);
            InitializeBaseMovementCoast();
            
        }

        private void addClass(UnitClass unitClass)
        {
            unitClasses.Add(unitClass);
            foreach(UnitEnum.ModifierEnum modifierEnum in unitClass.modifiers)
            {
                addModifier(modifierEnum);
            }
            recalculateHealth();
        }

        private void recalculateHealth()
        {
            int newHealth = 0;
            int divide = 0;
            foreach(UnitClass unitClass in unitClasses)
            {
                divide++;
                newHealth += unitClass.defaultHealth;
            }
            maxHealth=newHealth/divide;
        }
        private void addModifier(UnitEnum.ModifierEnum modifierEnum)
        {
            modifiers.Add(modifierEnum);
            UnitModifier.applyWithKey(this, modifierEnum);
        }

        private void applyAllModifiers()
        {
            foreach(UnitEnum.ModifierEnum modifierEnum in modifiers)
            {
                UnitModifier.applyWithKey(this, modifierEnum);
            }
        }

        private void removeAllModifiers()
        {
            resetAllModifiers();
            modifiers.Clear();
        }
        private void resetAllModifiers()
        {
            foreach (UnitEnum.ModifierEnum modifierEnum in modifiers)
            {
                UnitModifier.removeWithKey(this, modifierEnum);
            }
        }

        private void removeOneModifier(UnitEnum.ModifierEnum modifEnum)
        {
            modifiers.Remove(modifEnum);
            UnitModifier.removeWithKey(this, modifEnum);
        }
        public void move(int w, int h)
        {
            coordH = h;
            coordW = w;
        }

        public (int w, int h) getPosition()
        {
            return (coordW, coordH);
        }

        
    }
}

