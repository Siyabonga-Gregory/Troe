using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Manager : MonoBehaviour
{


	//Private Variables
	bool isMouseDragging;
	Vector3 offsetValue;
	Vector3 positionOfScreen;
	GameObject getTarget;
	bool positionSet=false;
	int done=0;
	public int gameTimer;
	public int score;

	//Public variables
	public Text txtHelper;
	public Text txtScore;
	public Text txtTimer;
	public Text txtLevel;
	public Vector3 orgPosition;
	public int correctToolIndex;
	public GameObject fadeCanvas;
	public GameObject collectedTools;
	public int counter;

	//Arrays
	public string[] tools;
	public string[] status;
	public string[,] allTools;

	public string[] newTools;
	//public string[] newStatus;

	public GameObject[] placeHolder;
	public GameObject[] newPlaceHolder;

	public GameObject[]toolsMap;
	public GameObject[] newToolsMap;


	//Sprites array
	public Sprite[]sprites;
	public Sprite[] avatar;
	public int avatarIndex = 0;
	public Image currentAvar;

	public bool inspected = false;
	public GameObject centerImage;
	public bool levelDone = false;
	public bool hasLogin = false;

	public AudioClip[]gameSounds;
	private AudioSource gameAudioSource;
	public AudioSource backGroundAudioSource;

	// canvas
	public GameObject loginCanvas;
	public GameObject mainCanvas;

	//Panels
	public GameObject mainMenuPanel;
	public GameObject helpPanel;
	public GameObject leaderBoardPanel;
	public GameObject nextLevelPanel;
	public GameObject settingsPanel;
	public GameObject aboutPanel;


	//login username and password
	public Text txtUsername;
	public Text txtPassword;

	public GameObject backgroundAudio;

	public int[] spriteStartIdex;
	public int currentLevel=0;
	public int[] levelTimers;
	public int[] levelScored;

	public bool paused;
	public bool isMusic=true;
	public bool isFX=true;

	public Toggle musicToggle;
	public Toggle fxToggle;

	public Text playerName;
	public GameObject playerAvatar;

	//Learderboard
	public Image leaderBoardImage;
	public Text leaderBoardName;
	public Text leaderBoardScore;


	public GameObject buttomPanel;
	public GameObject[] hideOnGameOver;


	// Initialization
	public void Start()
	{
		allTools = new string[placeHolder.Length, 2];
		status   = new string[placeHolder.Length];
		tools = new string[placeHolder.Length];
		newTools = new string[placeHolder.Length];
		newToolsMap = new GameObject[placeHolder.Length];
		newPlaceHolder = new GameObject[placeHolder.Length];
		if (currentLevel == 0) {
			levelScored=new int[levelTimers.Length];
		}
		RandomizeTools (true);
		gameAudioSource = GameObject.FindGameObjectWithTag ("Manager").GetComponent<AudioSource> ();
		mainMenuPanel.SetActive (false);
		helpPanel.SetActive (false);
		levelDone = false;
	}

	// Update
	void Update()
	{
		//pause the game
		if (Input.GetKeyDown (KeyCode.Escape)) {
			settingsPanel.SetActive (true);
			Time.timeScale = 0;
			paused = true;
			backgroundAudio.SetActive (false);
		}

		if (!paused) {
			if (!hasLogin && !isMusic) {
				backgroundAudio.SetActive (false);
			} else if (counter > 0 && !levelDone && isMusic && hasLogin) {
				backgroundAudio.SetActive (true);
			}

		}

		try
		{
			//Mouse Button Press Down
			if (Input.GetMouseButtonDown(0))
			{  
				RaycastHit hitInfo;
				getTarget = ReturnClickedObject(out hitInfo);

				if (!positionSet)  // get gameobject 'specific tool' original position
				{
					orgPosition = getTarget.transform.position;
					positionSet = true;
				}

				if (getTarget != null )
				{
					if (SearchArray (getTarget) >=0) 
					{
						isMouseDragging = true;
						positionOfScreen = Camera.main.WorldToScreenPoint (getTarget.transform.position);
						offsetValue = getTarget.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, positionOfScreen.z));
					}    
				}
			}

			//Mouse Button Up
			if (Input.GetMouseButtonUp(0))
			{
				isMouseDragging = false;
				positionSet = false;

				if (SearchArray (getTarget) == correctToolIndex) // You have picked up a correct tool
				{
					placeHolder[correctToolIndex].SetActive(false);
					ChangeToolMap (SearchArray (getTarget));
					allTools [correctToolIndex, 1]= "True";
					SetCorrectTool ();

					if(correctToolIndex==14)  // All tools collected  //   OR for dynamic coding  ***   if(correctToolIndex==allTools.Length-1) *** 
					{
						if(done==1)
						{
							txtHelper.text = "All tools collected.";
							//GameObject orgFade;

							//StartCoroutine(HideCanvas());
						}
						done+=1;
					}
				} 
				else
				{
					getTarget.transform.position = orgPosition;  //  Object droped in a wrong 'position' place holder
					if(SearchArray(getTarget) >=0)
					{

					}
				}
			}
			//Moving a drag object
			if (isMouseDragging)
			{return ;}
		}
		catch 
		{}
	}

	//Method to Return Clicked Object ' Tool '
	GameObject ReturnClickedObject(out RaycastHit hit)
	{
		GameObject target = null;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
		{
			target = hit.collider.gameObject;
		}
		return target;
	}

	//Method to randomize tools
	public void RandomizeTools(bool replay)
	{
		for(int i=0;i<placeHolder.Length;i++)
		{
			status [i] = "False";
			tools [i] = placeHolder [i].transform.name;

			allTools [i, 0] = tools [i];
			allTools [i, 1] = "False";
		}


		if (!replay) {
			txtScore.text = score.ToString();
			levelDone=true;
			counter = newToolsMap.Length;
			Time.timeScale = 1;  

		} else {

			txtLevel.text = (currentLevel+1).ToString ();
			int placeHolderRandomIndex;


			for(int i=0;i<placeHolder.Length;i++)
			{
				placeHolderRandomIndex = Random.Range(0, placeHolder.Length);

				if (!NodeExist(newPlaceHolder, placeHolder[placeHolderRandomIndex].transform.name.ToString())) 
				{	
					newPlaceHolder [i] = placeHolder [placeHolderRandomIndex];
					newToolsMap [i] = toolsMap [placeHolderRandomIndex];
				} 
				else
				{
					i -= 1;
				} 

				if(i >= 0)
				{
					allTools [i, 1] = "False";
				}
			}

			for (int i = 0; i < newPlaceHolder.Length; i++) 
			{

				allTools [i, 0] = newPlaceHolder [i].transform.name.ToString ();
				newTools [i] = newPlaceHolder [i].transform.name.ToString ();
				tools [i] = newPlaceHolder [i].transform.name.ToString ();
				placeHolder [i] = newPlaceHolder [i];

				Vector3 holder = new Vector3 (toolsMap[i].transform.localPosition.x, toolsMap[i].transform.localPosition.y, toolsMap[i].transform.localPosition.z);

				toolsMap [i].transform.localPosition = new Vector3 (newToolsMap[i].transform.localPosition.x, newToolsMap[i].transform.localPosition.y, newToolsMap[i].transform.localPosition.z);

				newToolsMap [i].transform.localPosition = new Vector3 (holder.x, holder.y, holder.z);
				AssignSprites (spriteStartIdex[currentLevel]);
				toolsMap [i] = newToolsMap [i];

			}
		}



		SetCorrectTool ();
		counter = newToolsMap.Length;
		for (int i = 0; i < hideOnGameOver.Length; i++) {
			hideOnGameOver [i].SetActive (true);
		}
	}

	/*assign sprites for specific level

      (Level 1) : startIndex = 0 
 
   */
	public void AssignSprites(int startIndex)
	{
		for(int i=0;i<newToolsMap.Length;i++)
		{
			Sprite image=sprites[startIndex];
			newToolsMap [i].GetComponent<Image>().sprite=image;
			startIndex += 1;
		}
	}

	//Method to search a specific tool from a array
	int SearchArray(GameObject target)
	{
		for (int i = 0; i < placeHolder.Length; i++) 
		{
			if (target.transform.name.Equals (allTools [i,0].ToString()) && allTools[i,1].ToString().Equals("False"))
			{return i;} 	
		}
		return -1;
	}

	//Method to update a next tool to be pick 
	public void SetCorrectTool()
	{
		try
		{
			for (int i = 0; i < placeHolder.Length; i++) 
			{
				if (allTools [i, 1].ToString ().Equals ("False")) 
				{
					correctToolIndex = i;
					//helper.text = placeHolder [i].transform.name.ToString ();
					txtHelper.text = newToolsMap [i].transform.name.ToString ();
					Sprite image=newToolsMap[i].GetComponent<Image>().sprite;
					centerImage.GetComponent<Image>().sprite=image;
					i = placeHolder.Length;
				}
			}	
		}
		catch 
		{}
	}

	public int ReturnOriginalIndex(string objectName)
	{
		int index=0;  

		for (int i = 0; i < toolsMap.Length; i++) 
		{
			if (toolsMap [i].transform.name.ToString ().Equals (objectName.ToString ())) 
			{
				if (i > 0) {return i;}else {return i;}
			}
		}  
		return index;
	}

	//Method to change tool map icon for a correct picked tool
	void ChangeToolMap(int index)
	{
		string correctPickedTool="#00FF0CFF";

		Color buttonColor = toolsMap [index].GetComponent<Image> ().color;
		//Color imageColor = toolsMap [index].transform.GetChild (0).GetComponent<Image> ().color;
		Color imageColor = toolsMap [index].GetComponent<Image> ().color;

		buttonColor = new Color ();
		imageColor = new Color ();

		ColorUtility.TryParseHtmlString (correctPickedTool, out buttonColor);
		ColorUtility.TryParseHtmlString (correctPickedTool, out imageColor);

		toolsMap [index].GetComponent<Image> ().color = buttonColor;
		//toolsMap [index].transform.GetChild (0).GetComponent<Image> ().color = imageColor;
		toolsMap [index].GetComponent<Image> ().color = imageColor;
		toolsMap [index].GetComponent<Button> ().interactable = true;
		//MapButton (toolsMap [index].GetComponent<Button> ());
	}

	//Method to fade and hide canvas for tool map
	IEnumerator HideCanvas()
	{
		yield return new WaitForSeconds (1.2f);
		fadeCanvas.SetActive (false);
		collectedTools.SetActive (false);
	}

	bool NodeExist(GameObject[]newArray, string value)
	{
		try
		{
			for (int i = 0; i < newArray.Length; i++) 
			{

				if (newArray[i].name.ToString ().Equals (value.ToString ()))
				{
					return true;
				}
			}	
		}
		catch
		{}
		//}
		return false;
	}


	public IEnumerator PlayAudio(int soundIndex)
	{
		if (isFX) 
		{
			if (gameSounds [soundIndex] != null && gameAudioSource != null) {
				gameAudioSource.clip = gameSounds [soundIndex];
				gameAudioSource.Play ();

				while (gameAudioSource.isPlaying) {
					yield return null;
				}
			}
		}

	}

	public IEnumerator Countdown()
	{
		if (isMusic) {
			backgroundAudio.SetActive (true);
			backGroundAudioSource = GameObject.FindGameObjectWithTag ("BackGroundAudio").GetComponent<AudioSource> ();
		}

		while (!levelDone) {
			if (gameTimer > 0 && counter > 0) {

				if (!backGroundAudioSource.isPlaying && isMusic) {
					backgroundAudio.SetActive (true);
					backGroundAudioSource.Play ();
				}
				txtTimer.text = (gameTimer - 1).ToString();
				gameTimer = gameTimer - 1;
			}
			else if(!levelDone && counter>0) {

				backGroundAudioSource.Stop ();
				DisableButtons (true);
				StartCoroutine(PlayAudio(4));// game over time-up
				levelDone=true;
				mainCanvas.SetActive (true);
				mainMenuPanel.SetActive (true);
				counter = newToolsMap.Length;
				for (int i = 0; i < hideOnGameOver.Length; i++) {
					hideOnGameOver [i].SetActive (false);
				}


				// will show main menu here for retry , new game , help and  or exit
			}
			yield return new WaitForSeconds (1f);
		}


	}

	public void DisableButtons(bool controller)
	{
		for (int i = 0; i < toolsMap.Length; i++) {
			toolsMap [i].gameObject.GetComponent<Button> ().interactable = controller;
		}
	}
}
