/******************************************************
*Project: AR Scavenger Hunt
*Created by: Colton Spruill
*Date: 20190118
*Description: Controls the flow of the tips and ar objects unlocking in order.
*Copyright 2019 LeapWithAlice,LLC. All rights reserved
 ******************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScavengerHuntAR : MonoBehaviour
{
    public GameObject map;
    public GameObject[] arMarkers;
    public string[] arHints;
    public GameObject[] arModels;
    public string shComplete = "Thank you for playing!";
    public Text hintText;
    public int arIndex ;
    public int maxTargets = 10;
    public Game game;
    public GameObject hintPanel;
    public GameObject hintButton;

    //keeps track of unlocked targets
    public List<int> unlockedTargets = new List<int>();


    void Start()
    {
        Random.seed = (int)System.DateTime.Now.Ticks; 
        arIndex = Random.Range(0,maxTargets);
        unlockedTargets.Add(arIndex);
        //Set initial ar hint to the current index, 0
        hintText.text = arHints[arIndex];
    }

    //randomly unlock next target
    public void UnlockNextTarget()
    {
        //Win Condition
        //if UnlockedTargets list size is greater or equal to max hints
        game.IncrementScore();
        if ( unlockedTargets.Count == maxTargets)
        {
            //reset unlockedTargets list
            //unlockedTargets.Clear();
            //Set hint text to complete text, go away
            //hintText.text = shComplete;
            
            return;
        }
        
        
        //randomly get index of next target to hunt
        int randomSelection = Random.Range(0,maxTargets);
        //ensure that the target is not already unlocked
        while (unlockedTargets.Contains(randomSelection) == true)
        {
            randomSelection = Random.Range(0,maxTargets);
        }

        StartCoroutine(SpawnFoundObject(arIndex));

        //save the target index in the unlockedTargets list
        unlockedTargets.Add(randomSelection);
        arIndex = randomSelection;

        //Else just update the hint text
        hintText.text = arHints[arIndex];
       
    }

    private IEnumerator SpawnFoundObject(int paramArIndex){
        //turn off the hint panel
        hintPanel.SetActive(false);
        //turn off the panels ability to turn on for the duration of showing the model
        hintButton.GetComponent<HintPanelToggle>().canToggle = false;
        //ensuring no usage of button
        hintButton.GetComponent<Button>().enabled = false;
        arModels[paramArIndex].SetActive(true);
        yield return new WaitForSeconds(3.0f);
        arModels[paramArIndex].SetActive(false);
        //enable the hint panel
        hintPanel.SetActive(true);
        //enable the ability to toggle the panel
        hintButton.GetComponent<HintPanelToggle>().canToggle = false;
        hintButton.GetComponent<Button>().enabled = true;
        
    }
}