
using UnityEngine;
using RPG.Movement;
using RPG.Core;

using GameDevTV.Saving;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using GameDevTV.Utils;
using System;
using GameDevTV.Inventories;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable,IModifierProvider
    {
        
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null;
       
        Health target;
        Equipment equipment;
        
        float timeSinceLastAttack = Mathf.Infinity;
       
        
        WeaponConfig currentWeaponConfig;
        LazyValue<Weapon> currentWeapon;
        private void Awake()
        {
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetDefaultWeapon);
            equipment = GetComponent<Equipment>();
            if (equipment)
            {
                equipment.equipmentUpdated += UpdateWeapon;
            }
        }

        private Weapon SetDefaultWeapon()
        {
            
            return AttachWeapon(defaultWeapon);
        }

        private void Start()
        {
            currentWeapon.ForceInit();
        }
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target == null) return;
            if (target.IsDead())// trying to cancel attack after target is killed
            {
                Cancel();
                return;
            } 
            if (!GetIsInRange(target.transform))
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }
        public void EquipWeapon(WeaponConfig Weapon)
        {
            currentWeaponConfig = Weapon;
          currentWeapon.value=  AttachWeapon(Weapon);
        }

        private void UpdateWeapon()
        {
         var weapon =   equipment.GetItemInSlot(EquipLocation.Weapon) as WeaponConfig;
            if(weapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
            else
            {
                EquipWeapon(weapon);
            }
        }

        private Weapon AttachWeapon(WeaponConfig Weapon)
        {
            Animator animator = GetComponent<Animator>();
            return Weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public Health GetTarget()
        {
            return target;
        }
        private void AttackBehaviour()
        {
             
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > currentWeaponConfig.GetTimeBetweenAttacks())
            {
                // This will trigger the Hit() event.
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }
        private void TriggerAttack()
        {
            if (target.IsDead()) return;
            GetComponent<Animator>().ResetTrigger("StopAttack");
            GetComponent<Animator>().SetTrigger("Attack");
           // currentWeapon.value.OnAttack();//particle system for swing
        }
        // Animation Event
        void Hit()
        {
            if (target == null) { return; }


            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            if (currentWeapon.value != null)
            {
                currentWeapon.value.OnHit();
            }
            
            if (currentWeaponConfig.HasProjectile())
            {
                //currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject);
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            }
            else
            {
               // target.TakeDamage(gameObject, currentWeapon.GetDamage());
                target.TakeDamage(gameObject, damage);
            }
        }

        void Shoot()
        {
            Hit();
        }
        private bool GetIsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) < currentWeaponConfig.GetWeaponRange();
        }
        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }
            if (!GetComponent<Mover>().CanMoveTo(combatTarget.transform.position) && !GetIsInRange(combatTarget.transform)  )  return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }
        public void Attack(GameObject combatTarget)
        {
          
            GetComponent<ActionSchedular>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
            
            
        }
        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }
        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("Attack");
            GetComponent<Animator>().SetTrigger("StopAttack");
        }
        public object CaptureState()
        {
            string Weaponname = currentWeaponConfig.name;
            return Weaponname;
        }
        public void RestoreState(object state)
        {
            string WeaponName = (string)state;
            WeaponConfig Weapon = UnityEngine.Resources.Load<WeaponConfig>(WeaponName);
            EquipWeapon(Weapon);
        }

        public IEnumerable<float> GetAdditiveModifier(Stat stat)
        {
            if(stat== Stat.Damage)
            {
                yield return currentWeaponConfig.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifier(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetPercentageBonus();
            }
        }
    }
}