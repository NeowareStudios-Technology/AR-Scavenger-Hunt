﻿using System.Collections;
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

    //public GameObject endGameDragonAnimation;
    private GameObject winnerText;
    private GameObject winCanvas;
    private bool canFindClues = true;
    private GameObject storyCanvas;
    private GameObject playerNamePanel;

    //audio references
    public GameObject gameplayTheme;
    public GameObject storyTheme;
    public DragonAmbience dragonAmbiance;


    void Start()
    {

        game =  GameObject.Find("GameManager").GetComponent<Game>();
        
        //winnerText.SetActive(false);
        //endGameDragonAnimation = GameObject.Find("Antler");
        //endGameDragonAnimation.SetActive(false);
        gameplayTheme = GameObject.Find("GameplayTheme");
        if (gameplayTheme != null)
        {
            gameplayTheme.SetActive(false);
        }
        playerNamePanel = GameObject.Find("PlayerNamePanel");
        storyTheme = GameObject.Find("StoryTheme");
        ScavengerHuntAR scavengerHuntAr = GameObject.FindObjectOfType<ScavengerHuntAR>();
        winnerText = scavengerHuntAr.winnerText;
        winCanvas = scavengerHuntAr.winCanvas;
        if (winCanvas != null)
        {
            winCanvas.SetActive(false);
        }
        storyCanvas = GameObject.Find("StoryandReadyCanvas");
        dragonAmbiance = GameObject.Find("DragonAmbience").GetComponent<DragonAmbience>();
    }

    
    [PunRPC]
    public void ButtonClicked()
    {
        if (canFindClues)
        {
            localScore++;
            
            dragonAmbiance.PlayRandomClip(localScore);
        }

        if (localScore >= 10)
        {
            StartCoroutine(EndGame());
        }
        FindPlayerName();
        game.playerInfo[playerUiIndex].GetComponent<Text>().text = playerName + " has "+localScore+" points";
        game.potions[playerUiIndex].GetComponent<Transform>().GetChild(0).GetComponent<Image>().fillAmount = (float)((localScore * 1.0f)/10.0f);
        game.playerScores[playerUiIndex] = localScore;
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

    


    private IEnumerator EndGame()
    {
       
        game.SetSummaryScreenText();
        winCanvas.SetActive(true);
        ScavengerHuntAR scavengerHuntAr = GameObject.FindObjectOfType<ScavengerHuntAR>();
        scavengerHuntAr.hintPanel.SetActive(false);
        //endGameDragonAnimation.SetActive(true);
       
        //endGameDragonAnimation.SetActive(false);
        winnerText.GetComponent<Text>().text = playerName + " won the game!";
        yield return new WaitForSeconds(1.0f);
        //game.LeaveRoom();
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
        game.gamestarted = true;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        //GameObject.Find("StartButton").SetActive(false);
        if (storyCanvas != null)
        {
            storyCanvas.SetActive(false);
        }
        if (playerNamePanel != null)
        {
            playerNamePanel.SetActive(false);
        }
        
        if (gameplayTheme != null)
        {
            gameplayTheme.SetActive(true);
        }
        if (gameplayTheme != null)
        {
            storyTheme.SetActive(false);
        }
        
    }
}
