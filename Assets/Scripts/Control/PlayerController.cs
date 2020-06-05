
using UnityEngine;
using RPG.Movement;

using RPG.Combat;
using RPG.Attributes;
using System;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;
using UnityEngine.AI;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;


        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float maxNavMeshProjectionDIstance = 1f;
        [SerializeField] float raycastRadius = 0.8f;

        bool isDraggingUi = false;
        private void Awake()
        {
            health = GetComponent<Health>();
        }
        void Update()
        {
            if(InteractWithUi())  return;
            if (health.IsDead()) 
            {
                SetCursor(CursorType.None);
                return;
            }
            if (InteractWithComponent()) return;
            
            if (InteractWithMovement()) { return; }
            SetCursor(CursorType.None);

        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (var hit in hits)
            {
                IRaycastable[] raycastables= hit.transform.GetComponents<IRaycastable>();
                foreach(IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());// need set sto something else l8er
                        return true;
                    }
                   
                }
            }
            return false;
        }

        
        RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), raycastRadius);

            float[] distances = new float[hits.Length];

            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);// rearenging the array
            return hits;
        }
           

        private bool InteractWithUi()
        {
            if (Input.GetMouseButtonUp(0)) isDraggingUi = false;
            if (EventSystem.current.IsPointerOverGameObject())
            {
               if(Input.GetMouseButtonDown(0)) isDraggingUi = true;

                SetCursor(CursorType.UI);
                return true;
            }
            if (isDraggingUi) return true;
           
            else return false;
                
        }

       

       

        private bool InteractWithMovement()
        {

            //  Ray ray = GetMouseRay();//casting a RAy
            // RaycastHit hit;//creating variable to store what a Ray hits
            Vector3 target;
            bool hashit = RaycastNavmesh(out target);// Raycast returns bool
            if (hashit == true)
            {
                if(!GetComponent<Mover>().CanMoveTo(target)) return false;

                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(target, 1f);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;


        }
        private bool RaycastNavmesh(out Vector3 target)
        {
            //raycast to terain
            target = new Vector3();
            RaycastHit hit;
            bool hashit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hashit) return false;
            //Find nearest navmeshpoint 
            NavMeshHit navMeshHit;
            bool hasCastToNavmesh= NavMesh.SamplePosition(hit.point, out navMeshHit, maxNavMeshProjectionDIstance, NavMesh.AllAreas);
            //return true if found
            if (!hasCastToNavmesh) return false;
          

            
            target = navMeshHit.position;
           

            return true;
        }

      

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture,mapping.hotspot,CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping maping in cursorMappings)
            {
                if(maping.type== type)
                {
                    return maping;
                }
                
            }
            return cursorMappings[0];
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
