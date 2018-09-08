/* *************************************************
*  Created:  7/22/2017, 10:09:28 PM
*  File:     HUDDevDisplay.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;

namespace SB {
	public class HUDDevDisplay : ManualSingletonMB<HUDDevDisplay> {

		public Text TextOut;

		Dictionary<string,string> Watch = new Dictionary<string,string>();

		// Use this for initialization
		void Start () {
			instance = this;
		}

		// UI debug
		public void Show(string key, string format, params object[] args) {
			Watch[key] = string.Format(format,args);
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