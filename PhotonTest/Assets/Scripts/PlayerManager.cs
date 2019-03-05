﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    

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

    //hint panel reference
    ScavengerHuntAR scavengerHuntAr;

    // used as Observed component in a PhotonView, this only reads/writes the position
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isReady);
        }
        else
        {
            this.isReady = (bool)stream.ReceiveNext();
        }
    }

    void Start()
    {

        game =  GameObject.Find("GameManager").GetComponent<Game>();
        scavengerHuntAr = GameObject.FindObjectOfType<ScavengerHuntAR>();
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
                Debug.Log("this many player manager are not ready!");
                game.ArePlayersReadyText.text = "Not all players are ready!";
                return;
            }
            else
            {
                Debug.Log("this many player manager are ready");
                game.ArePlayersReadyText.text = "All players are ready!";
            }
        }
    }

    


    private IEnumerator EndGame()
    {
        game.SetSummaryScreenTime();
        game.potionEndAnimation.SetActive(true);
        scavengerHuntAr.hintPanel.SetActive(false);
        yield return new WaitForSeconds(4.0f);
        winnerText.GetComponent<Text>().text = playerName + " won the game!";
        game.SetSummaryScreenText();
        winCanvas.SetActive(true);

    }

    private IEnumerator WaitThenSetHintPanelActive()
    {
        yield return new WaitForSeconds(6.0f);
        scavengerHuntAr.hintPanel.SetActive(true);

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
