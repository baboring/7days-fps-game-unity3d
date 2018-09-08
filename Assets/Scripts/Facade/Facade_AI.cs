/* *************************************************
*  Created:  7/21/2017, 12:52:56 AM
*  File:     Facade_AI.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SB {

    public enum RaycastResult
    {
        Nothing = 0,
        FoundTarget = 1,
        BlockedTarget = 2,
    }

    public static class Facade_AI  {

        static bool debug_on = false;

        // detecting target
        static public bool DetectTarget(ObjectProperty property,float range, out ObjectProperty target ) {

            Debug.Assert(property != null,"owener property is null");
            target = null;
            Quaternion startingAngle = Quaternion.AngleAxis(-60, Vector3.up);
            Quaternion stepAngle = Quaternion.AngleAxis(property.tb.stepAngle, Vector3.up);


            RaycastHit hit;
            var angle = property.transform.rotation * startingAngle;
            var pos = property.transform.position;
            var direction = angle * Vector3.forward;

            // adjust height for eyes level
            pos.y += property.tb.eyeLevel;

            for (var i = 0; i < 24; i++)
            {
                if (Physics.Raycast(pos, direction, out hit, range))
                {
                    if(debug_on)
                        Debug.DrawRay(pos, direction * hit.distance, Color.green);
                    var unit_info = hit.collider.GetComponent<ObjectProperty>();
                    if (unit_info && unit_info.ally != property.ally) {
                        //Enemy was seen
                        //Debug.Log("- Found out taret:" + unit_info.gameObject.name);
                        target = unit_info;
                    }
                }
                else 
                    if(debug_on)
                        Debug.DrawRay(pos, direction * property.tb.sightRange, Color.green);
                
                direction = stepAngle * direction;
            }

            return (target != null);
        }


        // raycast test
        public static RaycastResult IsRaycastHit(ObjectProperty origin, ObjectProperty target, float range, int mask = int.MaxValue) {
            RaycastHit hit;
            if (Physics.Raycast(origin.Position, origin.DistanceFrom(target).normalized, out hit, range, mask)) {
                if (hit.collider.transform == target.transform)
                    return RaycastResult.FoundTarget;
                return RaycastResult.BlockedTarget;
            }
            return RaycastResult.Nothing;
        }

        // raycast test
        public static RaycastResult IsRaycastHit(Transform origin, Transform target, float range, int mask = int.MaxValue) {
            RaycastHit hit;
            if(Physics.Raycast(origin.position, origin.DistanceFrom(target).normalized, out hit, range, mask)) {
                if(hit.collider.transform == target)
                    return RaycastResult.FoundTarget;
                return RaycastResult.BlockedTarget;
            }
            return RaycastResult.Nothing;
        }        
	       
    }
}

