﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;




public class PlayerManager : MonoBehaviourPunCallbacks {
    
    public int localScore = 0;

    public int playerIndex;
    
    #region  Public Fields

    [Tooltip("The Player's UI GameObject Prefab")]
    public GameObject PlayerUiPrefab;

    public int Health;

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
        if (beams == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> Beams Reference.", this);
        }
        else
        {
            beams.SetActive(false);
        }
    }

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity on every frame.
    /// </summary>
    void Update()
    {
        ProcessInputs ();
        // trigger Beams active state
        if (beams != null && IsFiring != beams.activeSelf)
        {
            beams.SetActive(IsFiring);
        }
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
            
            GameManager gm =  GameObject.Find("GameManager").GetComponent<GameManager>();
            
            //gm.numClicked++;
            localScore++;
            Debug.Log("<b> Num clicked = </b>" + this.localScore + " By: " + this.gameObject.name);
            Debug.Log("player index is: " + playerIndex);
            gm.playerInfo[playerIndex].GetComponent<Text>().text = PhotonNetwork.LocalPlayer.NickName + " has "+localScore+" points";
        }
        

        #region PunCallbacks
        void CalledOnLevelWasLoaded(){
            GameObject _uiGo = Instantiate(this.PlayerUiPrefab) as GameObject;
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }
            

        #endregion

    

    }
