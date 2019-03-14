using System.Collections;
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

    

    private bool canFindClues = true;

    private GameObject playerNamePanel;

    //audio references
    private AudioReferences audioReferences;

    //UI references
    private UIReferences uIReferences;

    //hint panel reference
    ScavengerHuntAR scavengerHuntAr;


    //Photon (IPunObservable), reads and writes the streamable variable so all clients are updated.
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
        CacheGameObjects();
        SetUIElementsForStart();
        audioReferences.gameplayTheme.SetActive(false);
        
    }

    private void CacheGameObjects()
    {
        audioReferences = GameObject.Find("AudioReferences").GetComponent<AudioReferences>();
        game =  GameObject.Find("GameManager").GetComponent<Game>();
        scavengerHuntAr = FindObjectOfType<ScavengerHuntAR>();
        uIReferences = FindObjectOfType<UIReferences>();
    }

    //This ensures the correct UI is activated on start in case some UI element is left on or off during development
    private void SetUIElementsForStart()
    {
        uIReferences.potionsAndPlayerNameCanvas.SetActive(true);
        uIReferences.quitButton.SetActive(true);
        uIReferences.helpButton.SetActive(true);
        uIReferences.hintPanel.SetActive(true);
        uIReferences.hintButton.SetActive(false);
        uIReferences.toggleHintButton.SetActive(true);
        uIReferences.winCanvas.SetActive(false);
        uIReferences.summaryCanvas.SetActive(false);
        uIReferences.storyCanvas.SetActive(true);
        uIReferences.playerNamePanel.SetActive(true);
        uIReferences.leaveCanvas.SetActive(false);
        uIReferences.helpCanvas.SetActive(false);
        
    }

    [PunRPC]
    public void IncrementScore()
    {
        if (canFindClues)
        {
            localScore++;      
            audioReferences.dragonAmbience.PlayRandomClip(localScore);
        }

        if (localScore >= scavengerHuntAr.maxTargets)
        {
            StartCoroutine(EndGame());
        }
        FindPlayerName();
        game.playerInfo[playerUiIndex].GetComponent<Text>().text = playerName + " has "+localScore+" points";
        game.potions[playerUiIndex].GetComponent<Transform>().GetChild(0).GetComponent<Image>().fillAmount = (float)((localScore * 1.0f)/10.0f);
        game.playerScores[playerUiIndex] = localScore;
        //uIReferences.mainCanvas.GetComponent<HintPanelAnimatorController>().SetStateOfAnimator(true);
    }

    [PunRPC]  
    public void SetScores()
    {
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
                game.SetPlayerReadyText(false);
                return;
            }
            else
            {
                game.SetPlayerReadyText(true);
            }
        }
    }

    


    private IEnumerator EndGame()
    {
        
        game.SetSummaryScreenTime();
        uIReferences.hintPanel.SetActive(false);
        uIReferences.winnerText.GetComponent<Text>().text = playerName + " has made the hero’s brew! The dragon has been put into a deep slumber.";
        game.SetSummaryScreenText();

        yield return new WaitForSeconds(8.0f);
        uIReferences.winCanvas.SetActive(true);

        //setting the win audio on here
        //turning off the win audio with a delegate on the "Next" button on the win canvas
        audioReferences.gameplayTheme.SetActive(false);
        audioReferences.storyTheme.SetActive(false);
        audioReferences.winAudio.SetActive(true);
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

        //closes the room so no more players can join once the game has been started
        PhotonNetwork.CurrentRoom.IsOpen = false;

        //switches audio to gameplayTheme
        audioReferences.storyTheme.SetActive(false);
        audioReferences.gameplayTheme.SetActive(true);
        
        //turns off the storyCanvas, camera view is now visible
        uIReferences.storyCanvas.SetActive(false);
        uIReferences.playerNamePanel.SetActive(false);
        
    }
}
