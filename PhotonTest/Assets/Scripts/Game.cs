using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections.Generic;

public class Game : MonoBehaviourPunCallbacks
{
	//player related
	public GameObject playerPrefab;
	public GameObject[] players = new GameObject[] {null, null, null, null};
	public GameObject[] playerInfo = new GameObject[4];
	
	public GameObject[] playerNamesInGame = new GameObject[4];
	public GameObject[] potions = new GameObject[4];
	public bool[] playerCheck = new bool[] {false, false, false, false};
	public int[] playerScores = new int[] {0,0,0,0};
	public string[] playerNames = new string[4] {"", "", "", ""};

	//summary screen UI
	public GameObject[] playerNamesSummaryScreen = new GameObject[4];
	public GameObject[] playerScoresSummaryScreen = new GameObject[4];
	int highestScoreIndex = -1;
	int secondHighestScoreIndex = -1;
	int thirdHighestScoreIndex = -1;
	int fourthHighestScoreIndex = -1;


	//timer related
	public static float timer;
	public bool timeStarted = false;
	int minutes;
	int seconds;
	string niceTime;
	private DateTime startingDateTime;
	public GameObject timeSummaryScreen;

	//UI related
	public Text ArePlayersReadyText;
	public GameObject startButton;
	public Text timerUi;

	//used by update function
	GameObject newPlayer;

	//used to determine if start button should be enabled
	public bool gamestarted = false;

	//used for win animation
	public GameObject potionEndAnimation;
	public GameObject modelHolder;

	
	void Start()
	{	
		CheckIfPlayerPrefabExists();
		CheckAndSetMasterClientStartButton();
		SetActivePlayerReadyText();
		SetTimeToZero();
		
	}

	void Update()
	{	
		AssignExistingPlayer();
		AssignNewPlayer();
		UpdateTimer();
		CheckAndSetMasterClientStartButton();
		
	}

	//we need a player prefab to play the game
	private void CheckIfPlayerPrefabExists()
	{
		if (playerPrefab == null){
				playerPrefab = (GameObject)Resources.Load("Player", typeof(GameObject));
				Debug.Log("why the player null tho");
			}
		if (playerPrefab == null)
		{
			Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",this);
		}
		else
		{
			Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
			// we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
			//PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f,5f,0f), Quaternion.identity, 0);
			StartCoroutine("DelayedPlayerInstantiate");	
		}
	}

	//Player Instantiate will not work here without a delay
	public IEnumerator DelayedPlayerInstantiate()
	{
		yield return new WaitForSeconds(0.25f);
		InstantiatePlayer();
		yield return new WaitForSeconds(0.25f);
		SetPlayerReadyText(false);
	}

	private IEnumerator DelayedSetPlayerReadyText()
	{
		yield return new WaitForSeconds(1.0f);
		SetPlayerReadyText(false);
	}

	private void InstantiatePlayer()
	{
			
			GameObject GO = PhotonNetwork.Instantiate("Player", new Vector3(0f,0f,0f), Quaternion.identity, 0);
	}

	//checks if this client is the master client and changes UI accordingly
	private void CheckAndSetMasterClientStartButton()
	{
		if (!gamestarted)
		{
			if (PhotonNetwork.IsMasterClient)
			{	
				startButton.SetActive(true);
			}
		}
		
	}

	private void SetActivePlayerReadyText()
	{
		ArePlayersReadyText.gameObject.SetActive(true);
	}

	

	//check if players already exist in room, if so set them
	private void AssignExistingPlayer()
	{
		//check for players in room and set local game manager & UI accordingly
		if (playerCheck[0] == false)
		{
			newPlayer = GameObject.Find("player1");
			if(newPlayer != null)
			{
				playerCheck[0] = true;
				players[0] = newPlayer;
				playerInfo[0].SetActive(true);
				potions[0].SetActive(true);
				playerNamesInGame[0].SetActive(true);
				playerNamesInGame[0].GetComponent<Text>().text = PhotonNetwork.PlayerList[0].NickName;
				playerInfo[0].GetComponent<Text>().text = PhotonNetwork.PlayerList[0].NickName + " has joined the quest!";
				playerNames[0] = PhotonNetwork.PlayerList[0].NickName;
				
			}
		}

		if (playerCheck[1] == false)
		{
			newPlayer = GameObject.Find("player2");
			if(newPlayer != null)
			{
				playerCheck[1] = true;
				players[1] = newPlayer;
				playerInfo[1].SetActive(true);
				potions[1].SetActive(true);
				playerInfo[1].GetComponent<Text>().text = PhotonNetwork.PlayerList[1].NickName + " has joined the quest!";
				playerNamesInGame[1].SetActive(true);
				playerNamesInGame[1].GetComponent<Text>().text = PhotonNetwork.PlayerList[1].NickName;
				playerNames[1] = PhotonNetwork.PlayerList[1].NickName;

			}
		}

		if (playerCheck[2] == false)
		{
			newPlayer = GameObject.Find("player3");
			if(newPlayer != null)
			{
				playerCheck[2] = true;
				players[2] = newPlayer;
				playerInfo[2].SetActive(true);
				potions[2].SetActive(true);
				playerInfo[2].GetComponent<Text>().text = PhotonNetwork.PlayerList[2].NickName + " has joined the quest!";
				playerNamesInGame[2].SetActive(true);
				playerNamesInGame[2].GetComponent<Text>().text = PhotonNetwork.PlayerList[2].NickName;
				playerNames[2] = PhotonNetwork.PlayerList[2].NickName;

			}
		}

		if (playerCheck[3] == false)
		{
			newPlayer = GameObject.Find("player4");
			if(newPlayer != null)
			{
				playerCheck[3] = true;
				players[3] = newPlayer;
				playerInfo[3].SetActive(true);
				potions[3].SetActive(true);
				playerInfo[3].GetComponent<Text>().text = PhotonNetwork.PlayerList[3].NickName + " has joined the quest!";
				playerNamesInGame[3].SetActive(true);
				playerNamesInGame[3].GetComponent<Text>().text = PhotonNetwork.PlayerList[3].NickName;
				playerNames[3] = PhotonNetwork.PlayerList[3].NickName;

			}
		}
	}

	//check if any new players have joined the room, if so set them
	private void AssignNewPlayer()
	{
		newPlayer = GameObject.Find("Player(Clone)");
		if (newPlayer != null)
		{
			if (players[0] == null)
			{
				newPlayer.GetComponent<PlayerManager>().playerIndex = 0;
				newPlayer.GetComponent<PlayerManager>().playerUiIndex = 0;
				newPlayer.name = "player1";
				players[0] = newPlayer;
				playerInfo[0].SetActive(true);
				potions[0].SetActive(true);
				playerNamesInGame[0].SetActive(true);
				playerNamesInGame[0].GetComponent<Text>().text = PhotonNetwork.PlayerList[0].NickName;
				playerNames[0] = PhotonNetwork.PlayerList[0].NickName;

			}
			else if (players[1] == null)
			{
				newPlayer.GetComponent<PlayerManager>().playerIndex = 1;
				newPlayer.GetComponent<PlayerManager>().playerUiIndex = 1;
				newPlayer.name = "player2";
				players[1] = newPlayer;
				playerInfo[1].SetActive(true);
				potions[1].SetActive(true);
				playerNamesInGame[1].SetActive(true);
				playerNamesInGame[1].GetComponent<Text>().text = PhotonNetwork.PlayerList[1].NickName;
				playerNames[1] = PhotonNetwork.PlayerList[1].NickName;

			}
			else if (players[2] == null)
			{
				newPlayer.GetComponent<PlayerManager>().playerIndex = 2;
				newPlayer.GetComponent<PlayerManager>().playerUiIndex = 2;
				newPlayer.name = "player3";
				players[2] = newPlayer;
				playerInfo[2].SetActive(true);
				potions[2].SetActive(true);
				playerNamesInGame[2].SetActive(true);
				playerNamesInGame[2].GetComponent<Text>().text = PhotonNetwork.PlayerList[2].NickName;
				playerNames[2] = PhotonNetwork.PlayerList[2].NickName;

			}
			else if (players[3] == null)
			{
				newPlayer.GetComponent<PlayerManager>().playerIndex = 3;
				newPlayer.GetComponent<PlayerManager>().playerUiIndex = 3;
				newPlayer.name = "player4";
				players[3] = newPlayer;
				playerInfo[3].SetActive(true);
				potions[3].SetActive(true);
				playerNamesInGame[3].SetActive(true);
				playerNamesInGame[3].GetComponent<Text>().text = PhotonNetwork.PlayerList[3].NickName;
				playerNames[3] = PhotonNetwork.PlayerList[3].NickName;

			}
		}
	}

	private void SetTimeToZero()
	{
		timer = 0.0f;
	}

	private void UpdateTimer()
	{
		if (timeStarted == false)
		{
			startingDateTime = DateTime.Now;
		}
		else if (timeStarted == true) 
		{		
			System.TimeSpan timeDifference = DateTime.Now.Subtract(startingDateTime);
			string output = string.Format("{0}:{1:00}", (int)timeDifference.TotalMinutes, timeDifference.Seconds);
			timerUi.text = output;
		}       
		
	}

	public void SetSummaryScreenTime()
	{
		System.TimeSpan timeDifference = DateTime.Now.Subtract(startingDateTime);
		string output = string.Format("{0}:{1:00}", (int)timeDifference.TotalMinutes, timeDifference.Seconds);
		timeSummaryScreen.GetComponent<Text>().text = output;
	}


	public override void OnPlayerEnteredRoom(Player other)
	{
		Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting
		if (PhotonNetwork.IsMasterClient)
		{
			Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
		}
		StartCoroutine(DelayedSetPlayerReadyText());
	}


	public void IncrementScore()
	{

		PhotonView[] foundObjects = FindObjectsOfType<PhotonView>();

		foreach (PhotonView x in foundObjects)
		{
			if (x.IsMine)
			{
				x.RPC("ButtonClicked", RpcTarget.All);
			}
		}
	}

	public void StartIfAllPlayersAreReady()
	{
		PlayerManager[] foundObjects = FindObjectsOfType<PlayerManager>();

		foreach (PlayerManager pms in foundObjects){

				if (!pms.isReady){

					SetPlayerReadyText(false);	
					return;

				}
				else{

					SetPlayerReadyText(true);
					pms.gameObject.GetComponent<PhotonView>().RPC("StartButtonClicked", RpcTarget.All);

				}
			}
		}

	public void SetPlayerReadyText(bool playersReady)
	{
		
		if (playersReady)
		{
			ArePlayersReadyText.text = "All players are ready!";
		}
		else
		{
			ArePlayersReadyText.text = "Not all players are ready!";
		}	
				
	}


    //handles setting the UI for the summary screen
	public void SetSummaryScreenText()
	{
		StartCoroutine(WaitThenSetSummaryScreenText());
		
	}

	private IEnumerator WaitThenSetSummaryScreenText()
	{

		int tempHighScore = 0;
		for (int i = 0; i < 4; i++)
		{
			
			if (playerScores[i] == null)
			{
				
			}
			else if (tempHighScore < playerScores[i])
			{
				tempHighScore = playerScores[i];
				Debug.Log("tempHighScore = " + tempHighScore);
				highestScoreIndex = i;
			}
		}

		//determine second place
		tempHighScore = 0;
		for (int i = 0; i < 4; i++)
		{
			
			if (playerScores[i] == null)
			{
				
			}
			else if (tempHighScore < playerScores[i] && highestScoreIndex != i)
			{
				tempHighScore = playerScores[i];
				secondHighestScoreIndex = i;
			}
		}
		//determine third place
		tempHighScore = 0;
		for (int i = 0; i < 4; i++)
		{
			
			if (playerScores[i] == null)
			{
				
			}
			else if (tempHighScore < playerScores[i] && highestScoreIndex != i && secondHighestScoreIndex != i)
			{
				tempHighScore = playerScores[i];
				thirdHighestScoreIndex = i;
			}
		}
		//determine 4th place
		tempHighScore = 0;
		for (int i = 0; i < 4; i++)
		{
			
			if (highestScoreIndex != i && secondHighestScoreIndex != i  && thirdHighestScoreIndex != i)
			{
				fourthHighestScoreIndex = i;
			}
		}
		yield return new WaitForSeconds(0.5f);

		Debug.Log("highest score index " + highestScoreIndex);
		Debug.Log("second score index " + secondHighestScoreIndex);
		Debug.Log("third score index " + thirdHighestScoreIndex);
		Debug.Log("fourth score index  " + fourthHighestScoreIndex);

		
		playerNamesSummaryScreen[0].GetComponent<Text>().text = playerNames[highestScoreIndex];
		playerNamesSummaryScreen[0].SetActive(true);
		playerScoresSummaryScreen[0].GetComponent<Text>().text = playerScores[highestScoreIndex].ToString();
		playerScoresSummaryScreen[0].SetActive(true);

		if (PhotonNetwork.PlayerList.Length > 1)
		{
			playerNamesSummaryScreen[1].GetComponent<Text>().text = playerNames[secondHighestScoreIndex];
			playerNamesSummaryScreen[1].SetActive(true);
			playerScoresSummaryScreen[1].GetComponent<Text>().text = playerScores[secondHighestScoreIndex].ToString();
			playerScoresSummaryScreen[1].SetActive(true);
		}
		
		if (PhotonNetwork.PlayerList.Length > 2)
		{
			playerNamesSummaryScreen[2].GetComponent<Text>().text = playerNames[thirdHighestScoreIndex];
			playerNamesSummaryScreen[2].SetActive(true);
			playerScoresSummaryScreen[2].GetComponent<Text>().text = playerScores[thirdHighestScoreIndex].ToString();
			playerScoresSummaryScreen[2].SetActive(true);
		}

		if (PhotonNetwork.PlayerList.Length > 3)
		{
			playerNamesSummaryScreen[3].GetComponent<Text>().text = playerNames[fourthHighestScoreIndex];
			playerNamesSummaryScreen[3].SetActive(true);
			playerScoresSummaryScreen[3].GetComponent<Text>().text = playerScores[fourthHighestScoreIndex].ToString();
			playerScoresSummaryScreen[3].SetActive(true);

		}
		
	}

	public void SetThisPlayerReady(){
		PlayerManager[] foundObjects = FindObjectsOfType<PlayerManager>();
		foreach (PlayerManager x in foundObjects){
			Debug.Log("# of player managers SetThisPlayerReady " + foundObjects.Length);
			if (x.gameObject.GetComponent<PhotonView>().IsMine){
				x.gameObject.GetComponent<PhotonView>().RPC("SetPlayerReady", RpcTarget.All);
				x.gameObject.GetComponent<PhotonView>().RPC("SetUiIfPlayersAreReady", RpcTarget.All);
			}
		}
	}

	

	public void LeaveRoom()
	{
			PhotonNetwork.LeaveRoom();
	}

	//When local player leaves room, load launch scene
	public override void OnLeftRoom()
	{
			SceneManager.LoadScene(0);
	}

	public override void OnPlayerLeftRoom(Player other)
	{
		
		StartCoroutine("DelayedRemovePlayerFromGameLogic");
		Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects
		if (PhotonNetwork.IsMasterClient)
		{
			Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
		}
	}

	private IEnumerator DelayedRemovePlayerFromGameLogic()
	{
		yield return new WaitForSeconds(0.25f);

		//if player left, decrease index of all following players (so no indexing error)
		if (GameObject.Find("player1") == null)
		{
			players[0] = null;
			playerCheck[0] = false;
			playerInfo[0].GetComponent<Text>().text = "";
			potions[0].SetActive(false);
			playerNamesInGame[0].SetActive(false);
			playerNamesInGame[0].GetComponent<Text>().text = "";
			playerNames[0] = "";


			if (GameObject.Find("player2") != null)
				players[1].GetComponent<PlayerManager>().playerIndex --;
			if (GameObject.Find("player3") != null)
				players[2].GetComponent<PlayerManager>().playerIndex --;
			if (GameObject.Find("player4") != null)
				players[3].GetComponent<PlayerManager>().playerIndex --;
		}
		if (GameObject.Find("player2") == null)
		{
			players[1] = null;
			playerCheck[1] = false;
			playerInfo[1].GetComponent<Text>().text = "";
			potions[1].SetActive(false);
			playerNamesInGame[1].SetActive(false);
			playerNamesInGame[1].GetComponent<Text>().text = "";
			playerNames[1] = "";

			if (GameObject.Find("player3") != null)
				players[2].GetComponent<PlayerManager>().playerIndex --;
			if (GameObject.Find("player4") != null)
				players[3].GetComponent<PlayerManager>().playerIndex --;
		}
		if (GameObject.Find("player3") == null)
		{
			players[2] = null;
			playerCheck[2] = false;
			playerInfo[2].GetComponent<Text>().text = "";
			potions[2].SetActive(false);
			playerNamesInGame[2].SetActive(false);
			playerNamesInGame[2].GetComponent<Text>().text = "";
			playerNames[2] = "";

			if (GameObject.Find("player4") != null)
				players[3].GetComponent<PlayerManager>().playerIndex --;
			
		}
		if (GameObject.Find("player4") == null)
		{
			players[3] = null;
			playerCheck[3] = false;
			playerInfo[3].GetComponent<Text>().text = "";
			potions[3].SetActive(false);
			playerNamesInGame[3].SetActive(false);
			playerNamesInGame[3].GetComponent<Text>().text = "";
			playerNames[3] = "";
		}
	}
}
