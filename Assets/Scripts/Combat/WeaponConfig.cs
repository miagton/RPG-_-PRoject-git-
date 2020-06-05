
using GameDevTV.Inventories;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class WeaponConfig : EquipableItem,IModifierProvider
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] Weapon EquipedWeaponPrefab = null;// Weapon we gona equip
        [SerializeField] float WeaponRange = 1f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float WeaponDamage = 5f;
        [SerializeField] float percentageBonus = 0;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;
        [SerializeField] GameObject[] Skills = null;
        [SerializeField]bool isOFFhand = false;

        const string WeaponName = "Weapon";

        public Weapon Spawn(Transform Righthand, Transform Lefthand, Animator animator)
        {
            DestroyOldWeapon(Righthand, Lefthand);

            Weapon Weapon = null;
            if(EquipedWeaponPrefab != null)
            {
                Transform handTransform = GetTransformHand(Righthand, Lefthand);
                Weapon =  Instantiate(EquipedWeaponPrefab, handTransform);
                Weapon.gameObject.name = WeaponName;
            }
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;// overriding Animator controller with Weapon overrride
            }
            else
            {
                var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
                if(overrideController!= null)
                {
                    animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
                }
            }
            return Weapon;
        }
        public float GetDamage()
        {
            return WeaponDamage;
        }

        private void DestroyOldWeapon(Transform righthand,Transform lefthand)
        {
            Transform oldWeapon = righthand.Find(WeaponName);
            if(oldWeapon == null)
            {
               
                oldWeapon = lefthand.Find(WeaponName);
                if (isOFFhand) return;
            }
            if(oldWeapon == null) { return; }
            oldWeapon.name = "DESTROYING!";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetTransformHand(Transform Righthand, Transform Lefthand)
        {
            Transform handTransform;
            if (isRightHanded) handTransform = Righthand;
            else handTransform = Lefthand;
            return handTransform;
        }

        public bool HasProjectile()
        {
          return  projectile != null;
        }

        public void LaunchProjectile(Transform righthand,Transform leftHand,Health target, GameObject WhoLaunched,float calculateDMG)
        {
            Projectile projectileInstance = Instantiate(projectile, GetTransformHand(righthand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target,WhoLaunched, calculateDMG) ;
        }

        public float GetWeaponDamage()
        {
            return WeaponDamage;
        }
        public float GetWeaponRange()
        {
            return WeaponRange;
        }
        public float GetPercentageBonus()
        {
            return percentageBonus;
        }
        public float GetTimeBetweenAttacks()
        {
            return timeBetweenAttacks;
        }

        public IEnumerable<float> GetAdditiveModifier(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return WeaponDamage;
            }
        }

        public IEnumerable<float> GetPercentageModifier(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return percentageBonus;
            }
        }
    }
}