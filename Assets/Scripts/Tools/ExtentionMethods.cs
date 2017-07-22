/* *************************************************
*  Created:  7/22/2017, 2:55:10 PM
*  File:     ExtentionMethods.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SB {


	public static class AnimatorExtensionMethods
	{
        public static void ResetParameters(this Animator animator) {
            AnimatorControllerParameter[] parameters = animator.parameters;
            for (int i = 0; i < parameters.Length; i++) {
                AnimatorControllerParameter parameter = parameters[i];
                switch (parameter.type) {
                    case AnimatorControllerParameterType.Int:
                        animator.SetInteger(parameter.name, parameter.defaultInt);
                        break;
                    case AnimatorControllerParameterType.Float:
                        animator.SetFloat(parameter.name, parameter.defaultFloat);
                        break;
                    case AnimatorControllerParameterType.Bool:
                        animator.SetBool(parameter.name, parameter.defaultBool);
                        break;
                }
            }
        }
	}

}