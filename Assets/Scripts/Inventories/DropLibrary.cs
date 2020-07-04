﻿using GameDevTV.Inventories;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
   [CreateAssetMenu(menuName ="RPG/Inventory/Drop Library")]
    public class DropLibrary : ScriptableObject
    {
        [SerializeField]
        DropConfig[] potentialDrops;
        
        [SerializeField] float[] dropChancePercentage;
        [SerializeField] int[] minDrops;
        [SerializeField] int[] maxDrops;

        [System.Serializable]
        class DropConfig
        {
            public InventoryItem item;
            public float[] relativeChance;
            public int[] minNumber;
            public int[] maxNumber;

            public int GetRandomNumber(int level)
            {
                if (!item.IsStackable())
                {
                    return 1;
                }
                int min = GetByLevel(minNumber,level);
                int max = GetByLevel(maxNumber,level);

                return UnityEngine.Random.Range(min, max+1);
            }
        }
        public struct Dropped
        {
            public InventoryItem item;
            public int number;
        }

        public IEnumerable<Dropped> GetRandomDrops(int level)
        {
            if (!ShouldDropRandom(level))
            {
                yield break;
            }

            for(int i =0; i< GetRandomNumberOfDrops(level); i++)
            {
                yield return GetRandomDrop(level);
            }
        }

        private Dropped GetRandomDrop(int level)
        {
            var drop = SelectRandomItem(level);
            var result = new Dropped();
            result.item = drop.item;
            result.number = drop.GetRandomNumber(level);

            return result;
        }

        private int GetRandomNumberOfDrops(int level)
        {
            int min = GetByLevel(minDrops, level);
            int max = GetByLevel(maxDrops, level);
            return UnityEngine.Random.Range(min, max);
        }

        private bool ShouldDropRandom(int level)
        {
          return  UnityEngine.Random.Range(0,100) < GetByLevel(dropChancePercentage, level);
        }

        DropConfig SelectRandomItem(int level)
        {
            float totalChance = GetTotalChance(level);
            float randomroll = UnityEngine.Random.Range(0,totalChance);
            float chanceTotal = 0;
            foreach(var drop in potentialDrops)
            {
                chanceTotal +=GetByLevel( drop.relativeChance,level);
                if (chanceTotal > randomroll)
                {
                    return drop;
                }
            }
            return null;
        }

        private float GetTotalChance(int level)
        {
            float total = 0;
            foreach(var drop in potentialDrops)
            {
                total += GetByLevel( drop.relativeChance,level);
            }
            return total;
        }


        static T GetByLevel<T>(T[] values, int level)
        {
            if (values.Length == 0)
            {
                return default;
            }
            if (level > values.Length)
            {
                return values[values.Length - 1];
            }
            if(level <= 0)
            {
                return default;
            }
            return values[level - 1];
        }
    }

}