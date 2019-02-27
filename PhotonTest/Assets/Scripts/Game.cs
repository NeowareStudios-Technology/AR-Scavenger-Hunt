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


	//timer related
	public static float timer;
	public bool timeStarted = false;
	int minutes;
	int seconds;
	string niceTime;

	//UI related
	public Text ArePlayersReadyText;
	public GameObject startButton;
	public Text timerUi;

	//used by update function
	GameObject newPlayer;

	void Start()
	{	
		CheckIfPlayerPrefabExists();
		CheckAndSetMasterClientUi();
		SetTimeToZero();
	}

	//we need a player prefab to play the game
	private void CheckIfPlayerPrefabExists()
	{
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
	}

	private void InstantiatePlayer()
	{
			GameObject GO = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f,0f,0f), Quaternion.identity, 0);
	}

	//checks if this client is the master client and changes UI accordingly
	private void CheckAndSetMasterClientUi()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			ArePlayersReadyText.gameObject.SetActive(true);
			startButton.SetActive(true);
		}
	}

	void Update()
	{	
		AssignExistingPlayer();
		AssignNewPlayer();
		UpdateTimer();
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
			}
		}

		if (playerCheck[2] == false)
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
				playerNamesInGame[1].GetComponent<Text>().text = PhotonNetwork.PlayerList[0].NickName;
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
				playerNamesInGame[2].GetComponent<Text>().text = PhotonNetwork.PlayerList[0].NickName;
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
				playerNamesInGame[3].GetComponent<Text>().text = PhotonNetwork.PlayerList[0].NickName;
			}
		}
	}

	private void SetTimeToZero()
	{
		timer = 0.0f;
	}

	private void UpdateTimer()
	{
		if (timeStarted == true) 
		{
			timer += Time.deltaTime;
		}       

		int minutes = Mathf.FloorToInt(timer / 60F);
		int seconds = Mathf.FloorToInt(timer - minutes * 60);
		string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
		timerUi.text = niceTime;
	}


	public override void OnPlayerEnteredRoom(Player other)
	{
		Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting
		if (PhotonNetwork.IsMasterClient)
		{
			Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
		}
	}


	public void IncrementScore(){
		PhotonView[] foundObjects = FindObjectsOfType<PhotonView>();
		foreach (PhotonView x in foundObjects){
			Debug.Log("Length of FoundObjects = " + foundObjects.Length);
			if (x.IsMine){
				Debug.Log("This many objects are 'Mine'");
				x.RPC("ButtonClicked", RpcTarget.All);
			}
		}
	}

	public void StartIfAllPlayersAreReady(){
		PlayerManager[] foundObjects = FindObjectsOfType<PlayerManager>();
		foreach (PlayerManager pms in foundObjects){
				if (!pms.isReady){
					ArePlayersReadyText.text = "Not all players are ready!";
					Debug.Log("Not all players are ready!");
					return;
				}
				else{
					ArePlayersReadyText.text = "All players are ready!";
					pms.gameObject.GetComponent<PhotonView>().RPC("StartButtonClicked", RpcTarget.All);
				}
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
		}
	}
}
