using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Script.UnitEnum;

namespace Assets.Script
{

    public struct UnitData
    {
        private float baseData;
        private float multiplicator;
        private float bonus;
        public float health { get; private set; }

        private void updateHealth()
        {
            health = (baseData * multiplicator) + bonus;
        }
        public void changeBase(float change)
        {
            baseData += change;
            updateHealth();
        }
        public void changeMultiplicator(float change)
        {
            multiplicator = multiplicator * change;
            updateHealth();
        }
        public void changeBonus(float change)
        {
            bonus += change;
            updateHealth();
        }

        public void reset()
        {
            baseData = 1.0f;
            multiplicator = 1.0f;
            bonus = 0;
            updateHealth();
        }
    }
    public class Unit
    {
        public UnitData maxHealth { get; private set; }
        public float currentHealth { get; private set; }
        public UnitData movementCapacity { get; private set; }
        
        private int coordW, coordH;

        public Dictionary<TileEnum.BiomeEnum, float> movementCoastByBiome;
        private HashSet<UnitEnum.ModifierEnum> modifiers;
        private List<UnitClass> unitClasses;

        private void initializeBaseMovementCoast()
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
            initializeBaseMovementCoast();
            
        }

        private void addClass(UnitClass unitClass)
        {
            unitClasses.Add(unitClass);
            foreach(UnitEnum.ModifierEnum modifierEnum in unitClass.modifiers)
            {
                addModifier(modifierEnum,false);
            }
            applyAllModifiers();
        }

        
        public void addModifier(UnitEnum.ModifierEnum modifierEnum,bool applyNow=true)
        {
            modifiers.Add(modifierEnum);
            if (applyNow)
            {
                applyAllModifiers();
            }
        }

        private void applyAllModifiers()
        {
            resetAllModifiers();
            foreach(UnitEnum.ModifierEnum modifierEnum in modifiers)
            {
                UnitModifier.applyWithKey(this, modifierEnum);
            }
        }
        private void resetAllModifiers()
        {
            maxHealth.reset();
            movementCapacity.reset();
            initializeBaseMovementCoast();
        }

        private void removeOneModifier(UnitEnum.ModifierEnum modifEnum)
        {
            modifiers.Remove(modifEnum);
            applyAllModifiers();
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

