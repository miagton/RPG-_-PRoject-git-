using GameDevTV.Inventories;
using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = ("RPG/Inventory/Equipable Item"))]
    public class StatsEquipableItem : EquipableItem,IModifierProvider
    {
        [SerializeField]
        Modifier[] additiveModifiers;
        [SerializeField]
        Modifier[] percentageModifier;

        public IEnumerable<float> GetAdditiveModifier(Stat stat)
        {
            foreach (var modifier in additiveModifiers)
            {
                if(modifier.stat == stat)
                {
                    yield return modifier.value;
                }
            }
        }

        public IEnumerable<float> GetPercentageModifier(Stat stat)
        {
            foreach (var modifier in percentageModifier)
            {
                if (modifier.stat == stat)
                {
                    yield return modifier.value;
                }
            }
        }

        [System.Serializable]
      struct Modifier
        {
            public Stat stat;
            public float value;
        }
    }
}
