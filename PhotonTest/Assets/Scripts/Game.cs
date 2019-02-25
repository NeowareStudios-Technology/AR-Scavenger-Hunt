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
	public int player1score = 0;
	public int player2score = 0;
	public int player3score = 0;
	public int player4score = 0;

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
		StartCoroutine("LoadPlayerName");	
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
		yield return new WaitForSeconds(1);
		InstantiatePlayer();
		
	
	}

	private IEnumerator LoadPlayerName(){
		yield return new WaitForSeconds(1.1f);
		LoadPlayerNameUi();
	}

	private void InstantiatePlayer()
	{
			GameObject GO = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f,0f,0f), Quaternion.identity, 0);
			
	}
	
	private void LoadPlayerNameUi(){
		PhotonView photonView = FindObjectOfType<PhotonView>();
		photonView.RPC("SetPlayerUi", RpcTarget.All);
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
		newPlayer = GameObject.Find("player1");
		if(newPlayer != null)
		{
			players[0] = newPlayer;
			playerInfo[0].SetActive(true);
		}

		newPlayer = GameObject.Find("player2");
		if(newPlayer != null)
		{
			players[1] = newPlayer;
			playerInfo[1].SetActive(true);
		}

		newPlayer = GameObject.Find("player3");
		if(newPlayer != null)
		{
			players[2] = newPlayer;
			playerInfo[2].SetActive(true);
		}

		newPlayer = GameObject.Find("player4");
		if(newPlayer != null)
		{
			players[3] = newPlayer;
			playerInfo[3].SetActive(true);
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
			}
			else if (players[1] == null)
			{
				newPlayer.GetComponent<PlayerManager>().playerIndex = 1;
				newPlayer.GetComponent<PlayerManager>().playerUiIndex = 1;
				newPlayer.name = "player2";
				players[1] = newPlayer;
				playerInfo[1].SetActive(true);
			}
			else if (players[2] == null)
			{
				newPlayer.GetComponent<PlayerManager>().playerIndex = 2;
				newPlayer.GetComponent<PlayerManager>().playerUiIndex = 2;
				newPlayer.name = "player3";
				players[2] = newPlayer;
				playerInfo[2].SetActive(true);
			}
			else if (players[3] == null)
			{
				newPlayer.GetComponent<PlayerManager>().playerIndex = 3;
				newPlayer.GetComponent<PlayerManager>().playerUiIndex = 3;
				newPlayer.name = "player4";
				players[3] = newPlayer;
				playerInfo[3].SetActive(true);
			}
		}
		
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
		StartCoroutine("DelayedDeactivateUI");
		Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects
		if (PhotonNetwork.IsMasterClient)
		{
			Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
		}
	}

/* 
	private IEnumerator DelayedDeactivateUI()
	{
		yield return new WaitForSeconds(1);

		//if player left, decrease index of all following players (so no indexing error)
		if (GameObject.Find("player1") == null)
		{
			players[0] = null;

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

			if (GameObject.Find("player3") != null)
				players[2].GetComponent<PlayerManager>().playerIndex --;
			if (GameObject.Find("player4") != null)
				players[3].GetComponent<PlayerManager>().playerIndex --;
		}
		if (GameObject.Find("player3") == null)
		{
			players[2] = null;

			if (GameObject.Find("player4") != null)
				players[3].GetComponent<PlayerManager>().playerIndex --;
			
		}
		if (GameObject.Find("player4") == null)
		{
			players[3] = null;
		}
	}
	*/
}
