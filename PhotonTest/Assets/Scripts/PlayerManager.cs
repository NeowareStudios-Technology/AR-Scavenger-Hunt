using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;




public class PlayerManager : MonoBehaviourPunCallbacks {
    
    #region  Public Fields

    public int localScore = 0;

    public int playerIndex;
    public int playerUiIndex;

    public GameManager gameManager;

    [Tooltip("The Player's UI GameObject Prefab")]
    public GameObject PlayerUiPrefab;

    public int Health;
    public bool isReady;
    #endregion

    #region Private Fields

    [Tooltip("The Beams GameObject to control")]
    [SerializeField]
    private GameObject beams;
    //True, when the user is firing
    bool IsFiring;
    #endregion

    #region MonoBehaviour CallBacks

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    /// </summary>
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity on every frame.
    /// </summary>
    void Update()
    {
    }
    void Start(){
        if (PlayerUiPrefab!=null)
        {
            GameObject _uiGo =  Instantiate(PlayerUiPrefab) as GameObject;
            _uiGo.SendMessage ("SetTarget", this, SendMessageOptions.RequireReceiver);
        }
        else
        {
            Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
        }
    }
    #endregion

    #region Custom

        /// <summary>
        /// Processes the inputs. Maintain a flag representing when the user is pressing Fire.
        /// </summary>
        void ProcessInputs()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (!IsFiring)
                {
                    IsFiring = true;
                    //PhotonView photonView = this.GetComponent<PhotonView>();
                    //photonView.RPC("ButtonClicked", RpcTarget.All, "yo");	
                   // GameObject.Find("GameManager").GetComponent<GameManager>().ButtonClicked();
                    
                }
            }
            if (Input.GetButtonUp("Fire1"))
            {
                if (IsFiring)
                {
                    IsFiring = false;
                }
            }
        }

        #endregion

        [PunRPC]
        public void ButtonClicked()
        {
            string playerName;
            GameManager gm =  GameObject.Find("GameManager").GetComponent<GameManager>();
            
            //gm.numClicked++;
            localScore++;
            Debug.Log("<b> Num clicked = </b>" + this.localScore + " By: " + this.gameObject.name);
            Debug.Log("player index is: " + playerIndex);
            Debug.Log(PhotonNetwork.PlayerList[playerIndex].NickName);
            
            playerName = PhotonNetwork.PlayerList[playerIndex].NickName;

            gm.playerInfo[playerUiIndex].GetComponent<Text>().text = playerName + " has "+localScore+" points";
        }
        

        #region PunCallbacks
        void CalledOnLevelWasLoaded(){
            GameObject _uiGo = Instantiate(this.PlayerUiPrefab) as GameObject;
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }
            
        
        [PunRPC]
        public void ReadyButtonClicked()
        {
            string playerName;
            GameManager gm =  GameObject.Find("GameManager").GetComponent<GameManager>();
            
            //gm.numClicked++;
            localScore++;
            Debug.Log("<b> Num clicked = </b>" + this.localScore + " By: " + this.gameObject.name);
            Debug.Log("player index is: " + playerIndex);
            
            playerName = PhotonNetwork.PlayerList[playerIndex].NickName;

            gm.playerInfo[playerIndex].GetComponent<Text>().text = playerName + " has "+localScore+" points";
        }

        [PunRPC]
        public void GoButtonClicked()
        {
           Debug.Log("Ready button clicked player ready test");
           isReady = true;
           
        }

        [PunRPC]
        public void StartButtonClicked()
        {
           Debug.Log("Start button clicked player ready test");
           isReady = true;
           PlayerManager[] foundObjects = FindObjectsOfType<PlayerManager>();
           foreach (PlayerManager pms in foundObjects){
				if (!pms.isReady){
					Debug.Log("Not all players are ready!");
					return;
				}
				else{
					GameObject.Find("BigButton").GetComponent<Button>().interactable = true;
                    gameManager.timeStarted = true;
				}
			}
           
        }

        [PunRPC]
        public void CheckIfPlayersAreReady(){
		PlayerManager[] foundObjects = FindObjectsOfType<PlayerManager>();
		foreach (PlayerManager x in foundObjects){
				if (!x.isReady){
                    GameObject.Find("GameManager").GetComponent<GameManager>().ArePlayersReadyText.text = "not all players are ready!";
					//ArePlayersReadyText.text = "Not all players are ready!";
					Debug.Log("Not all players are ready!");
					return;
				}
				else{
                    GameObject.Find("GameManager").GetComponent<GameManager>().ArePlayersReadyText.text = "All players are ready!";
					//ArePlayersReadyText.text = "All players are ready!";
				}
			}
	}

        #endregion

    

    }
