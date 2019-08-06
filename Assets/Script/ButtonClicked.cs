using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonClicked : MonoBehaviour {

	Manager man;
	GameObject buttomPanel;
	void Start()
	{
		man = GameObject.FindGameObjectWithTag ("Manager").GetComponent<Manager> ();
		buttomPanel = GameObject.FindGameObjectWithTag ("Manager").GetComponent<Manager>().buttomPanel;
	}

	public void OnButtonClicked()
	{
		if (man.txtHelper.text.ToString ().Equals (this.gameObject.name.ToString ())) {// picked correct item
			if (man.counter > 0)
			{
				man.counter -= 1;

				man.allTools [man.ReturnOriginalIndex (this.gameObject.transform.transform.name.ToString ()), 1] = "true";
				man.SetCorrectTool ();
				StartCoroutine(man.PlayAudio(2)); //play good audio
				man.txtScore.text=(man.score+1).ToString();
				man.score = man.score + 1;
			}
			else 
			{
				man.levelDone = true;
				StartCoroutine(man.PlayAudio(2)); 
			}
		} 
		else {
			StartCoroutine(man.PlayAudio(0)); 
		}

		if (man.counter == 0) 
		{
			try
			{
				AudioSource background = GameObject.FindGameObjectWithTag ("BackGroundAudio").GetComponent<AudioSource> ();
				background.Stop ();
			}
			catch{}

			man.backgroundAudio.SetActive (false);
			man.DisableButtons (false);
			man.mainMenuPanel.SetActive (true);
			StartCoroutine(man.PlayAudio(6));
			man.levelDone=true;
			NextLevel (false);
		}
	}

	public void RestartGame()
	{
		StartCoroutine(man.PlayAudio(5));
		man.mainCanvas.SetActive (false);
		man.mainMenuPanel.SetActive (false);
		man.nextLevelPanel.SetActive (false);
		man.mainCanvas.SetActive (true);
		man.txtScore.text=(0).ToString();
		man.score = 0;
		man.gameTimer = man.levelTimers [man.currentLevel];
		man.RandomizeTools(false);
		buttomPanel.SetActive (true);
		man.paused = false;
		man.levelDone = false;
		Time.timeScale = 1;
	}

	public void ReplayLevel()
	{
		StartCoroutine(man.PlayAudio(5));
		man.mainCanvas.SetActive (false);
		man.mainMenuPanel.SetActive (false);
		man.nextLevelPanel.SetActive (false);
		man.mainCanvas.SetActive (true);

		if (man.currentLevel > 0) {
			man.score = man.levelScored [man.currentLevel-1];
		}
		else {
			man.txtScore.text=(0).ToString();
			man.score = 0;
		}

		man.gameTimer = man.levelTimers [man.currentLevel];
		man.RandomizeTools(false);
		buttomPanel.SetActive (true);
		man.paused = false;
		man.levelDone = false;
		Time.timeScale = 1;
	
	}

	public void Help()
	{
		//man.backgroundAudio.SetActive (true);
		StartCoroutine(man.PlayAudio(5));
		if (man.settingsPanel.activeSelf) {
			man.settingsPanel.SetActive (false);
		} else if (man.mainMenuPanel.activeSelf) {
			man.mainMenuPanel.SetActive (false);
		} else if (man.helpPanel.activeSelf) {
			man.helpPanel.SetActive (false);
		} else if (man.aboutPanel.activeSelf) {
			man.helpPanel.SetActive (false);
		}
		buttomPanel.SetActive (false);
		man.helpPanel.SetActive (true);

	}

	public void ExitGame()
	{
		man.backgroundAudio.SetActive (true);
		StartCoroutine(man.PlayAudio(5));
		Application.Quit ();
	}

	public void ExitHelpPanel()
	{
		if (man.isFX) {
			StartCoroutine(man.PlayAudio(5));
		}
		man.playerAvatar.GetComponent<Image> ().sprite = man.currentAvar.sprite;
		man.helpPanel.SetActive (false);
		man.mainMenuPanel.SetActive (true);
	}

	public void ExitMainMenuPanel()
	{
		man.playerAvatar.GetComponent<Image> ().sprite = man.currentAvar.sprite;

		if (man.isFX) {
			StartCoroutine(man.PlayAudio(5));
		}
		man.mainMenuPanel.SetActive (false);
		buttomPanel.SetActive (true);
		if (man.isMusic) {
			man.backgroundAudio.SetActive (true); 
		}
		man.paused = false;
		Time.timeScale = 1; 
	}

	public void ExitLearderBoardPanel(bool exitLeaderBoard)
	{
		StartCoroutine(man.PlayAudio(5));
		if (!exitLeaderBoard) {
			man.leaderBoardImage.sprite = man.currentAvar.sprite;
			man.leaderBoardName.text = man.playerName.text.ToString ();
			man.leaderBoardScore.text = man.txtScore.text.ToString ();
			man.mainMenuPanel.SetActive (false);
			man.leaderBoardPanel.SetActive (true);
		} else {
			man.leaderBoardPanel.SetActive (false);
			man.mainMenuPanel.SetActive (true);
		}
	}

	public void ShowCloseSettingsPanel(bool closeShow)
	{
		if (man.aboutPanel.activeSelf) {
			man.aboutPanel.SetActive (false);
		} else if (man.helpPanel.activeSelf) {
			man.helpPanel.SetActive (false);
		} else if (man.mainMenuPanel.activeSelf) {
			man.mainMenuPanel.SetActive (false);
		}

		StartCoroutine(man.PlayAudio(5));
		man.backgroundAudio.SetActive (true);
		AudioSource background = GameObject.FindGameObjectWithTag ("BackGroundAudio").GetComponent<AudioSource> ();
		man.settingsPanel.SetActive (closeShow);
		buttomPanel.SetActive (false);

		if (closeShow) {
			man.paused = true;
			man.settingsPanel.SetActive (closeShow);
			Time.timeScale = 0;
			background.Pause ();
			man.backgroundAudio.SetActive (false);
			buttomPanel.SetActive (false);

		} else {
			man.playerAvatar.GetComponent<Image> ().sprite = man.currentAvar.sprite;
			man.paused = false;
			man.settingsPanel.SetActive (closeShow);
			Time.timeScale = 1;  
			man.backgroundAudio.SetActive (closeShow);
			buttomPanel.SetActive (true);
			if (man.isMusic) {
				man.backgroundAudio.SetActive (true);
				background.Play();
				buttomPanel.SetActive (true);
			}
		}
	}

	public void ShowCloseAboutPanel(bool closeShow)
	{
		if (man.settingsPanel.activeSelf) {
			man.settingsPanel.SetActive (false);
		} else if (man.mainMenuPanel.activeSelf) {
			man.mainMenuPanel.SetActive (false);
		} else if (man.helpPanel.activeSelf) {
			man.helpPanel.SetActive (false);
		}
		
		if (man.isFX) {
			StartCoroutine(man.PlayAudio(5));
		}
		man.backgroundAudio.SetActive (true);
		AudioSource background = GameObject.FindGameObjectWithTag ("BackGroundAudio").GetComponent<AudioSource> ();
		man.playerAvatar.GetComponent<Image> ().sprite = man.currentAvar.sprite;
		man.aboutPanel.SetActive (closeShow);

		if (closeShow) {
			man.paused = true;
			man.aboutPanel.SetActive (closeShow);
			Time.timeScale = 0;
			background.Pause ();
			buttomPanel.SetActive (false);
			//man.backgroundAudio.SetActive (false);

		} else {
			//man.playerAvatar.GetComponent<Image> ().sprite = man.currentAvar.sprite;
			man.paused = false;
			man.aboutPanel.SetActive (closeShow);
			Time.timeScale = 1;  
			buttomPanel.SetActive (true);
			man.backgroundAudio.SetActive (closeShow);
			if (man.isMusic) {
				man.backgroundAudio.SetActive (true);
				background.Play();
			}
		}
	}

	public void CloseSettingShowMenu()
	{
		if (man.isFX) {
			StartCoroutine(man.PlayAudio(5));
		}

		man.settingsPanel.SetActive (false);
		man.mainMenuPanel.SetActive (true);
	}

	public void Login()
	{
		if (man.txtUsername.text.ToString () != null && man.txtPassword.text.ToString () != null) {
			StartCoroutine(man.PlayAudio(5));
			man.playerName.text = man.txtUsername.text.ToString ();
			man.loginCanvas.SetActive (false);
			man.mainCanvas.SetActive (true);
		}
		else
		{
			// show message box here
		}
	}

	public void AvatarSelection(bool next)
	{
		  if (next) {
			
			if (man.avatar.Length != man.avatarIndex) {
				StartCoroutine (man.PlayAudio (5));
				man.currentAvar.sprite = man.avatar [man.avatarIndex];
				man.avatarIndex += 1;
			} else {
				StartCoroutine(man.PlayAudio(1));
			}
			   
		} else {
				
			if (man.avatarIndex > 0) {
				man.avatarIndex -= 1;	
				StartCoroutine (man.PlayAudio (5));
				man.currentAvar.sprite = man.avatar [man.avatarIndex];
			} else {
				StartCoroutine(man.PlayAudio(1));
			}
		}
	}

	public void ManageSettings(bool isMusic)
	{
		if (isMusic) {

			if (man.musicToggle.isOn) {
				man.isMusic = true;
			} else {
				man.isMusic = false;
			}
		} else {
			if (man.fxToggle.isOn) {
				man.isFX = true;
			} else {
				man.isFX = false;
			}
		}
	}

	public void NextLevel(bool isButtonClicked)
	{
		man.DisableButtons (false);
		man.nextLevelPanel.SetActive (true);
		man.levelScored [man.currentLevel] = man.score;
		Debug.Log ("Current Level  " + man.currentLevel + "     Current Scored    " + man.score);


		for (int i = 0; i < man.hideOnGameOver.Length; i++) {
			man.hideOnGameOver [i].SetActive (false);
		}
		//man.mainCanvas.SetActive (true);
		man.mainMenuPanel.SetActive (false);

		if (isButtonClicked) {
			
			man.currentLevel += 1;
			StartCoroutine(man.PlayAudio(5));
			man.Start ();
			man.levelDone = false;

			man.DisableButtons (true);
			man.backgroundAudio.SetActive (true);
			AudioSource background = GameObject.FindGameObjectWithTag ("BackGroundAudio").GetComponent<AudioSource> ();
			if (background.isPlaying && man.isMusic) {background.Play ();}
			man.mainCanvas.SetActive (false);
			man.nextLevelPanel.SetActive (false);
			man.mainCanvas.SetActive (true);
			for (int i = 0; i < man.hideOnGameOver.Length; i++) {man.hideOnGameOver [i].SetActive (true);}
		}
	}
}
