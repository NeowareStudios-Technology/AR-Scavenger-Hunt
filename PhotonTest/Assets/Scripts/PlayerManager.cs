using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class PlayerManager : MonoBehaviourPunCallbacks {
    

    //Number of clues found by player
    public int localScore = 0;


    public int playerIndex;
    public int playerUiIndex;

    private string playerName;
    
    public bool isReady;

    private Game game;

    private PlayerManager[] playerManagers = new PlayerManager[4];

   


    void Start()
    {

        game =  GameObject.Find("GameManager").GetComponent<Game>();
        
        
        
    }

    
    [PunRPC]
    public void ButtonClicked()
    {

        localScore++;
        FindPlayerName();
        game.playerInfo[playerUiIndex].GetComponent<Text>().text = playerName + " has "+localScore+" points";

    }
            
    
    [PunRPC]
    public void SetPlayerUi()
    {
       
        FindPlayerName();
        game.playerInfo[playerIndex].GetComponent<Text>().text = playerName + " has "+localScore+" points";

    }


    [PunRPC]
    public void SetPlayerReady()
    {

        isReady = true; 

    }


    [PunRPC]
    public void StartButtonClicked()
    {

        FindPlayerManagers();

        foreach (PlayerManager playerManager in playerManagers)
        {
            if (!playerManager.isReady)
            {
                Debug.Log("Not all players are ready!");
                return;
            }
            else
            {
                StartGame();
            }
        }

    }


    [PunRPC]
    public void SetUiIfPlayersAreReady()
    {
        FindPlayerManagers();

        foreach (PlayerManager playerManager in playerManagers)
        {
            if (!playerManager.isReady)
            {
                game.ArePlayersReadyText.text = "not all players are ready!";
                return;
            }
            else
            {
                game.ArePlayersReadyText.text = "All players are ready!";
            }
        }
    }


    public void SetPlayerNameUI()
    {
                
        FindPlayerName();
        game.playerInfo[playerUiIndex].GetComponent<Text>().text = playerName + " has "+localScore+" points";

    }


    private void FindPlayerManagers()
    {

        playerManagers = FindObjectsOfType<PlayerManager>();

    }


    private void FindPlayerName()
    {

        playerName = PhotonNetwork.PlayerList[playerIndex].NickName;

    }


    private void StartGame()
    {

        
        game.timeStarted = true;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        GameObject.Find("StoryandReadyCanvas").SetActive(false);
    }
}
