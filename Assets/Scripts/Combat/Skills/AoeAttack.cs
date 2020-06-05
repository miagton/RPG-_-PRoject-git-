using RPG.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Skills
{
    public class AoeAttack : MonoBehaviour
    {
        Collider[] hits = null;
        [SerializeField] float radius = 3f;
        [SerializeField] float damage = 15f;
        [SerializeField] ParticleSystem effect = null;
        [SerializeField] float skillDistance = 10f;

       
        Ray ray;
        RaycastHit hit;
        void Start()
        {
        }


        // Update is called once per frame
        void Update()
        {
            
               
            if (Input.GetKeyDown(KeyCode.Q))
            {
                CastSkill();

            }
        }

      /* private void AoeDmg()
        {

            if (GetSkillPosition() == transform.position) return;
            hits = Physics.OverlapSphere(GetSkillPosition(), radius);
            if (effect != null)
            {
                Instantiate(effect, GetSkillPosition(), Quaternion.identity);
            }
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].gameObject.tag == "Enemy")
                {
                    hits[i].gameObject.GetComponent<Health>().TakeDamage(this.gameObject, 15f);
                }
                else continue;
            }
        }*/
        
       /* private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position + Vector3.up, transform.forward);
            
        }*/

        private Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
            
          
        }

        private void CastSkill()
        {

            Ray ray = new Ray(transform.position + new Vector3(0, 1, 0), transform.forward);
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green);
            if (Physics.Raycast(ray))
            {
                if(hit.distance <= skillDistance)
                {
                    print(hit.collider.gameObject.tag);
                    if (hit.collider.gameObject.tag == "Enemy")
                    {
                        DamageAoe();
                    }
                    else return;
                }
            }


        }

        private void DamageAoe()
        {
            hits = Physics.OverlapSphere(hit.collider.transform.position, radius);
            if (effect != null)
            {
                Instantiate(effect, hit.collider.transform.position, Quaternion.identity);
            }
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].gameObject.tag == "Enemy")
                {
                    hits[i].gameObject.GetComponent<Health>().TakeDamage(this.gameObject, 15f);
                }
                else continue;
            }
        }
    }
}

   

               
            
