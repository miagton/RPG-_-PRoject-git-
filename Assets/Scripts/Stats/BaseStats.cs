using GameDevTV.Utils;
using RPG.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] ProgressionSystem progression = null;
        [SerializeField] GameObject LvLUpPArticleEffect = null;
        [SerializeField] bool shouldUseModifires = false;

        LazyValue<int> currentLvL;
        public event Action OnLvLUp;
        Experience experience;

        int CurrentLvL;

        private void Awake()
        {
            experience = GetComponent<Experience>();
            currentLvL = new LazyValue<int>(CalculateLevel);
        }
        private void Start()
        {
            currentLvL.ForceInit();



        }

        private void OnEnable()
        {
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }
        private void OnDisable()
        {
            if (experience != null)
            {
                experience.onExperienceGained -= UpdateLevel;
            }
        }




        private void UpdateLevel()
        {
          int  newLvL = CalculateLevel();
            if (newLvL > currentLvL.value)
            {
                currentLvL.value = newLvL;
                LevelUpEffect();
                OnLvLUp();
            }
        }

        private void LevelUpEffect()
        {
            if (LvLUpPArticleEffect == null) return;
            
            Instantiate(LvLUpPArticleEffect, transform);
          

        }

        public float GetStat(Stat stat)
        {

            return (GetBaseStat(stat) + GetAdditiveModifier(stat))*(1+GetPercentageModifier(stat)/100);

        }

        

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifires) return 0;
            float total = 0;
            foreach(IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach(float modifier in provider.GetAdditiveModifier(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifires) return 0;
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifier(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }
        public int GetLevel()
        {
            return currentLvL.value;
        }
       
        
        private int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();

            if (experience == null) return startingLevel;
              
            
            float currentXP= experience.GetXPValue();

           
            int PenultimalLevel = progression.GetLevels(Stat.ExperienceToLvlUp, characterClass);
            for (int level = 1; level <= PenultimalLevel; level++)
            {
                float XPtoLevelUp = progression.GetStat(Stat.ExperienceToLvlUp, characterClass, level);
                if (XPtoLevelUp > currentXP)
                {
                    return level;
                }
                
            }
            return PenultimalLevel + 1;
        }

       


    }
}
