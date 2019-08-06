using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateMain : MonoBehaviour {

	private Manager man;
	void Start()
	{
		man = GameObject.FindGameObjectWithTag ("Manager").GetComponent<Manager> ();
	}

	void OnEnable()
	{
		man = GameObject.FindGameObjectWithTag ("Manager").GetComponent<Manager> ();
		man.gameTimer = man.levelTimers[man.currentLevel];
		man.hasLogin = true;
		man.levelDone = false;
		man.DisableButtons (true);
		if (man.isMusic) {
			man.backgroundAudio.SetActive (true);
		}
		StartCoroutine (man.Countdown ());
	}

}
