﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections.Generic;
    public class GameManager : MonoBehaviourPunCallbacks
    {
		
		public int numClicked;
		[Tooltip("The prefab to use for representing the player")]
		public GameObject playerPrefab;

		public GameObject playerUIPrefab;
		public Transform PlayerUIParent;

		public GameObject[] players = new GameObject[4];
		
		public GameObject[] playerInfo = new GameObject[4];

		public int player1score = 0;
		public int player2score = 0;
		public int player3score = 0;
		public int player4score = 0;

        #region Photon Callbacks


        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }


        #endregion

		void Start(){
			if (playerPrefab == null)
			{
				Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",this);
			}
			else
			{
				Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
				// we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
				//PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f,5f,0f), Quaternion.identity, 0);
				
				InstantiatePlayer();
			
				
				
			}
		}
		private void InstantiatePlayer(){
				GameObject GO = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f,0f,0f), Quaternion.identity, 0);
				
				//GO.GetComponent<RectTransform>().SetParent(PlayerUIParent);
				//GO.GetComponent<RectTransform>().localScale = new Vector3(1.0F, 1.0f, 1.0f);
				//GO.GetComponent<Text>().text = PhotonNetwork.LocalPlayer.NickName;
				
				if (players[0] == null)
				{
					GO.GetComponent<PlayerManager>().playerIndex = 0;
					GO.name = "player1";
					players[0] = GO;
					playerInfo[0].SetActive(true);
				}
				else if (players[1] == null)
				{
					GO.GetComponent<PlayerManager>().playerIndex = 1;
					GO.name = "player2";
					players[1] = GO;
					playerInfo[1].SetActive(true);
				}
				else if (players[2] == null)
				{
					GO.GetComponent<PlayerManager>().playerIndex = 2;
					GO.name = "player3";
					players[2] = GO;
					playerInfo[2].SetActive(true);
				}
				else if (players[3] == null)
				{
					GO.GetComponent<PlayerManager>().playerIndex = 3;
					GO.name = "player4";
					players[3] = GO;
					playerInfo[3].SetActive(true);
				}
		}

		void Update(){
			/* 
			for (int i = 0; i< players.Count; i++){
				players[i].GetComponent<Transform>().GetChild(1).GetComponent<Text>().text = PhotonNetwork.PlayerList[i].NickName;
			}
			*/

		}
        #region Public Methods


        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }


        #endregion

		#region Photon Callbacks

		public override void OnPlayerEnteredRoom(Player other)
		{

			Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting
 
			
			

			if (PhotonNetwork.IsMasterClient)
			{
				Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


				//LoadArena();
			}
		}


		public override void OnPlayerLeftRoom(Player other)
		{
			Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

			if ((GameObject.Find("player1") == null)&&(players[0] != null))
			{
				players[0] = null;
				playerInfo[0].SetActive(false);
			}
			if ((GameObject.Find("player2") == null)&&(players[1] != null))
			{
				players[1] = null;
				playerInfo[1].SetActive(false);
			}
			if ((GameObject.Find("player3") == null)&&(players[2] != null))
			{
				players[2] = null;
				playerInfo[2].SetActive(false);
			}
			if ((GameObject.Find("player4") == null)&&(players[3] != null))
			{
				players[3] = null;
				playerInfo[3].SetActive(false);
			}


			if (PhotonNetwork.IsMasterClient)
			{
				Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


				//LoadArena();
			}


		}


		#endregion


		void LoadArena()
		{
			if (!PhotonNetwork.IsMasterClient)
			{
				Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
			}
			Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
			PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
		}

	public void ClickThis(){
		//PhotonView photonView = this.GetComponent<PhotonView>();
		PhotonView[] foundObjects = FindObjectsOfType<PhotonView>();
		foreach (PhotonView x in foundObjects){
			Debug.Log("Length of FoundObjects = " + foundObjects.Length);
			if (x.IsMine){
				Debug.Log("This many objects are 'Mine'");
				x.RPC("ButtonClicked", RpcTarget.All);
			}
		}
		//foundObjects.RPC("ButtonClicked", RpcTarget.All, "yo");
    	//photonView.RPC("ButtonClicked", RpcTarget.All, "yo");	
	}
	
    #region RPC Calls

    [PunRPC]
    void ButtonClicked(string message)
    {
        //numClicked++;
       // Debug.Log("<b> Num clicked = </b>" + numClicked);
    }
    #endregion
    }

