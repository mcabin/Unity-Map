using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Script.UnitEnum;

namespace Assets.Script
{

    public class UnitData
    {
        private float baseData;
        private float multiplicator;
        private float bonus;

        public float Health => (baseData * multiplicator) + bonus; // Calcul dynamique

        public UnitData()
        {
            Reset();
        }

        public void ChangeBase(float change)
        {
            baseData += change;
        }

        public void ChangeMultiplicator(float change)
        {
            multiplicator *= change;
        }

        public void ChangeBonus(float change)
        {
            bonus += change;
        }

        public void Reset()
        {
            baseData = 1.0f;
            multiplicator = 1.0f;
            bonus = 0;
        }
    }

    public class Unit
    {
        public UnitData maxHealth { get; private set; }
        public float currentHealth { get; private set; }
        public UnitData movementCapacity { get; private set; }

        public Vector2Int coord { get; private set; }

        public Dictionary<TileEnum.BiomeEnum, float> movementCoastByBiome;
        private HashSet<UnitEnum.ModifierEnum> modifiers;
        private List<UnitClass> unitClasses;

        public Unit(Vector2Int coord, UnitClass baseClass)
        {
            this.coord = coord;

            maxHealth = new UnitData();
            movementCapacity = new UnitData();
            movementCoastByBiome = new Dictionary<TileEnum.BiomeEnum, float>();
            modifiers = new HashSet<UnitEnum.ModifierEnum>();
            unitClasses = new List<UnitClass>();

            addClass(baseClass);
            initializeBaseMovementCoast();
        }

        private void initializeBaseMovementCoast()
        {
            foreach (TileEnum.BiomeEnum biome in Enum.GetValues(typeof(TileEnum.BiomeEnum)))
            {
                movementCoastByBiome[biome] = 1f;
            }
        }

        public UnitClass getUnitClass(int index)
        {
            return unitClasses[index];
        }
        private void addClass(UnitClass unitClass)
        {
            unitClasses.Add(unitClass);
            foreach (UnitEnum.ModifierEnum modifierEnum in unitClass.modifiers)
            {
                addModifier(modifierEnum, false);
            }
            applyAllModifiers();
        }

        public void addModifier(UnitEnum.ModifierEnum modifierEnum, bool applyNow = true)
        {
            modifiers.Add(modifierEnum);
            if (applyNow)
            {
                applyAllModifiers();
            }
        }

        public bool hasModifier(UnitEnum.ModifierEnum modifierEnum)
        {
            return modifiers.Contains(modifierEnum);
        }

        private void applyAllModifiers()
        {
            resetAllModifiers();
            foreach (UnitEnum.ModifierEnum modifierEnum in modifiers)
            {
                UnitModifier.applyWithKey(this, modifierEnum);
            }
        }

        private void resetAllModifiers()
        {
            maxHealth.Reset();
            movementCapacity.Reset();
            initializeBaseMovementCoast();
        }
        public void Move(Vector2Int vector)
        {
            coord = vector;
        }

    }

}

