/* *************************************************
*  Created:  7/24/2017, 3:08:39 AM
*  File:     HUD.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace SB {

	public enum eNotifyHUD {
		Damage,
		Kill,
		Die,
	} 
	public class HUD : ManualSingletonMB<HUD> {

		public Image			hDamageFlash;
        public Text     		hScore;
        public Text     		hTimer;
        public RectTransform    hGameOver;

        public delegate void Callback(object v);

        public Dictionary<eNotifyHUD, Callback> notifyMap =  new Dictionary<eNotifyHUD, Callback>();

		// Use this for initialization
		void Awake () {
			instance = this;

            hDamageFlash.enabled = false;

            // HUD Damage
            notifyMap.Add(eNotifyHUD.Damage, (val) => {
                Color backup = hDamageFlash.color;
                Color color = backup;
                hDamageFlash.enabled = true;
                Facade_Coroutine.While(this, () => {
                    if (color.a < 0.5) {
                        color.a += ((Time.deltaTime > 0) ? Time.deltaTime : 0.03f);
                        hDamageFlash.color = color;
                        return true;
                    }
                    Debug.Log("Flash Done");
                    hDamageFlash.color = backup;
                    hDamageFlash.enabled = false;
                    return false;
                });
            });

            // HUD Kill Enemy
            notifyMap.Add(eNotifyHUD.Kill, (val) => {
                HUDDevDisplay.instance.Show("Score", GameData.instance.Score.ToString());
                hScore.text = GameData.instance.Score.ToString();
            });
		}

        // Event Notifyer
		public void Event(eNotifyHUD id, object val ) {
            Callback callback;
			if(notifyMap.TryGetValue(id, out callback)) {
                callback(val);
			}
		}

		string FormatTime (float time){
			int intTime = (int)time;
			int minutes = intTime / 60;
			int seconds = intTime % 60;
			float fraction = time * 1000;
			fraction = (fraction % 1000);
			string timeText = String.Format ("{0:00}:{1:00}:{2:000}", minutes, seconds, fraction);
			return timeText;
		}

		
		// Update is called once per frame
		void Update () {
            // timer
            hTimer.text = FormatTime(GameData.instance.Timer);
            // game over
            hGameOver.gameObject.SetActive(GameData.instance.IsShowGameOver);
		}
	}
}