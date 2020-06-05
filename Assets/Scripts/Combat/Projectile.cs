using RPG.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed;
        [SerializeField] bool IsHoming = false;
        [SerializeField] float LifeTime = 1.5f;
        [SerializeField] float lifeAfterImpact = 2f;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] GameObject[] DestroyOnHit = null;

        [SerializeField] UnityEvent onHit;
        
        [SerializeField] bool isAoe = false;
        [SerializeField] Transform aoeCenter = null;
        [SerializeField] float aoeRadius = 4f;
        [SerializeField] float splashDamage = 5f;
        [SerializeField] GameObject aoeEffect = null;

        GameObject WhoLaunched = null;
        Health target = null;
        float damage = 0;

        private void Start()
        {

            transform.LookAt(GetAimLocation());
            Destroy(LifeTime);
        }


        void Update()
        {
            if (target == null) { return; }
            if (IsHoming && !target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }

            if (transform.parent == null)
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }


        }

        public void SetTarget(Health target, GameObject WhoLaunched, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.WhoLaunched = WhoLaunched;
        }
        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * (targetCapsule.height / 2);


        }

        private void OnTriggerEnter(Collider other)
        {

            Health health = other.GetComponent<Health>();
            if (health != target) return;
            else
            {
                if (target.IsDead()) { return; }
                if (other.gameObject.tag != "Player")
                {
                    Destroy(gameObject);
                }
                target.TakeDamage(WhoLaunched,damage);
                speed = 0;
                onHit.Invoke();
                // ProjectileStop(other);
                if (hitEffect != null)
                {
                    Instantiate(hitEffect, GetAimLocation(), transform.rotation);
                }
                

                foreach (GameObject obj in DestroyOnHit)
                {
                    Destroy(obj);
                }

                Destroy(gameObject, lifeAfterImpact); ;

            }


        }
        /* private void OnTriggerStay(Collider other)
         {
             Health health = other.GetComponent<Health>();
            // if (health != target) return;
             if (health.isDead())
             {
                 Destroy(this.gameObject);
             }
             else Destroy(this.gameObject,2f);
         }*/

        private void ProjectileStop(Collider other)
        {
            transform.parent = other.transform;
            transform.Translate(Vector3.zero);
            TrailRenderer trail = GetComponentInChildren<TrailRenderer>();
            trail.enabled = false;
        }

        private void Destroy(float lifeTIme)
        {
            Destroy(this.gameObject, lifeTIme);
        }

      //  public void aoeHit()
    //   {
           
        //    RaycastHit[] hits =  Physics.SphereCastAll(aoeCenter.position, aoeRadius,- Vector3.up, 0);
         //   foreach(var hit in hits)
         //   {
        //       if(hit.transform.gameObject.tag != "Enemy")
         //       {
           //         continue;
          //      }
         //       Health enemy = hit.collider.GetComponent<Health>();
         //       enemy.TakeDamage(gameObject, splashDamage);
         //       Instantiate(aoeEffect, aoeCenter);
         //   }
     //   }

       
    }

}