using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script
{
    public abstract class UnitModifier
    {
        // Initialisation immédiate pour éviter les NullReferenceException
        protected static Dictionary<UnitEnum.ModifierEnum, List<UnitModifier>> modifierDictionary
            = new Dictionary<UnitEnum.ModifierEnum, List<UnitModifier>>();

        public UnitEnum.ModifierEnum type { get; private set; }

        public UnitModifier(UnitEnum.ModifierEnum type)
        {
            this.type = type;
        }

        protected static void addToDictionary(UnitModifier unitModifier)
        {
            UnitEnum.ModifierEnum modifEnum = unitModifier.type; 

            if (modifierDictionary.ContainsKey(modifEnum))
            {
                modifierDictionary[modifEnum].Add(unitModifier);
            }
            else
            {
                modifierDictionary[modifEnum] = new List<UnitModifier> { unitModifier };
            }
        }

        public abstract void applyModifier(Unit unit);

        public static void applyWithKey(Unit unit, UnitEnum.ModifierEnum key)
        {
            if (modifierDictionary.TryGetValue(key, out List<UnitModifier> modifList))
            {
                foreach (UnitModifier modif in modifList)
                {
                    modif.applyModifier(unit);
                }
            }
            else
            {
                Debug.LogWarning($"Aucun modificateur trouvé pour {key}");
            }
        }
    }
}
