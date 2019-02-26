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
    public GameObject hintToggle;

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
        //if UnlockedTargets list size is greater or equal to max hints
        if ( unlockedTargets.Count == maxTargets)
        {
            //reset unlockedTargets list
            unlockedTargets.Clear();
            //Set hint text to complete text, go away
            hintText.text = shComplete;
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
        game.IncrementScore();
    }

    private IEnumerator SpawnFoundObject(int paramArIndex){
        hintPanel.SetActive(false);
        hintToggle.GetComponent<Button>().enabled = false;
        arModels[paramArIndex].SetActive(true);
        yield return new WaitForSeconds(3.0f);
        arModels[paramArIndex].SetActive(false);
        hintPanel.SetActive(true);
        hintToggle.GetComponent<Button>().enabled = true;
        //hintToggle.GetComponent<Toggle>().isOn = true;
    }
}