/* *************************************************
*  Created:  7/22/2017, 10:09:28 PM
*  File:     DevDisplay.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;

namespace SB {
	public class DevDisplay : ManualSingletonMB<DevDisplay> {

		public Text TextOut;

		public Dictionary<string,string> Watch = new Dictionary<string,string>();

		// Use this for initialization
		void Start () {
			instance = this;
		}
		private StringBuilder sb;
		
		// Update is called once per frame
		void Update () {
			sb = new StringBuilder();
			foreach(var info in Watch) {
				sb.AppendFormat("{0} : {1}\n",info.Key,info.Value);
			}
			TextOut.text = sb.ToString();
		}
	}
}