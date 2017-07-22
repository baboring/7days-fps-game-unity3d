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

    public static class Facade_AI  {

        static bool debug_on = false;

        // detecting target
        static public bool DetectTarget(UnitProperty owner, out UnitProperty target ) {

            Debug.Assert(owner != null,"unit owener is null");
            target = null;
            Quaternion startingAngle = Quaternion.AngleAxis(-60, Vector3.up);
            Quaternion stepAngle = Quaternion.AngleAxis(owner.stepAngle, Vector3.up);


            RaycastHit hit;
            var angle = owner.transform.rotation * startingAngle;
            var direction = angle * Vector3.forward;
            var pos = owner.transform.position;

            pos.y += owner.eyeLevel;

            for (var i = 0; i < 24; i++)
            {
                
                if (Physics.Raycast(pos, direction, out hit, owner.sightRange))
                {
                    if(debug_on)
                        Debug.DrawRay(pos, direction * hit.distance, Color.green);
                    var unit_info = hit.collider.GetComponent<UnitProperty>();
                    if (unit_info && unit_info.ally != owner.ally) {
                        //Enemy was seen
                        Debug.Log("Find taret:" + unit_info.gameObject.name);
                        target = unit_info;
                    }
                }
                else 
                    if(debug_on)
                        Debug.DrawRay(pos, direction * owner.sightRange, Color.green);
                
                direction = stepAngle * direction;
            }

            return (target != null);
        }
    }
}

