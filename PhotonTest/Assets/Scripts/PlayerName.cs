using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI; 
using Photon.Realtime;

public class PlayerName : MonoBehaviourPunCallbacks, IPunObservable {

	private GameObject[] players = new GameObject[4];


	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
		if (stream.IsWriting)
		{
			// We own this player: send the others our data
			stream.SendNext(playerName);
		}
		else
		{
			// Network player, receive data
			this.playerName = (string)stream.ReceiveNext();
		}
    }


	public string playerName;
	// Use this for initialization
	void Start () {
		//playerName = PhotonNetwork.LocalPlayer.NickName;
		players = GameObject.FindGameObjectsWithTag("PlayerName");
	}
	
	// Update is called once per frame
	void Update () {
		
			for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++){
				players[i].GetComponent<Text>().text = PhotonNetwork.PlayerList[i].NickName;
			}

		
		//playerName = PhotonNetwork.LocalPlayer.NickName;
		//this.GetComponent<Text>().text = playerName;
	}

	public override void OnPlayerEnteredRoom(Player other)
		{
			players = GameObject.FindGameObjectsWithTag("PlayerName");
		}
}
