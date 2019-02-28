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
    private string [,]hints = new string[10,3];
    public GameObject[] arModels;
    public string shComplete = "Thank you for playing!";
    public Text hintText;
    public int arIndex ;
    public int maxTargets = 10;
    public Game game;
    public GameObject hintPanel;
    public GameObject hintButton;
    public GameObject mainCanvas;
    public GameObject winCanvas;
    public GameObject winnerText;

    //keeps track of unlocked targets
    public List<int> unlockedTargets = new List<int>();


    void Start()
    {
        SetHintText();
        
        Random.seed = (int)System.DateTime.Now.Ticks; 
        arIndex = Random.Range(0,maxTargets);
        unlockedTargets.Add(arIndex);
        //Set initial ar hint to the current index, 0
        //hintText.text = arHints[arIndex];
        do 
        {
            hintText.text = hints[arIndex, Random.Range(0,3)];
        } while (hintText.text == "");
        Debug.Log("hints length is "+ hints.GetLength(0));
        Debug.Log("hints[0] legth is " + hints.GetLength(1));

        
    }

    private void SetHintText(){
        
        //antlers
        hints[0,0] = "I am a trophy for some, but for others I can be the key ingredient with the right tools to crush me. (Insert line about where it is located in the real world)"; 
        hints[0,1] = "I am a trophy only obtainable by the most skilled of hunter";
        hints[0,2] = "";
        //ashes
        hints[1,0] = "From fire I am born. When touched I turn your fingers silvery, grey. (Insert line about where it is located in the real world)";
        hints[1,1] = "I am the end of a Phoenix living, leaving me behind in order to create his next life.";
        hints[1,2] = "I am both silver and grey and all that remains of the city burnt in flames. (Insert line about where it is located in the real worl";
        //dagger
        hints[2,0] = "I have a hilt to hold and sharp edges for you to cut what you will need (Insert line about where it is located in the real world)";
        hints[2,1] = "I am a small blade forged in cold steel.";
        hints[2,2] = "";
        //book
        hints[3,0] = "I have no voice and yet I speak to you, I tell of all things in the world that people do. I have a spine and hinges, but I am not a man or a door. (Insert line about where it is located in the real world)";
        hints[3,1] = "I have leaves, but I am not a tree, I have pages, but I am not a bride or royalty. (Insert line about where it is located in the real world) ";
        hints[3,2] = "My life is often a volume of grief, your help is needed to turn a new leaf. Stiff is my spine and my body is pale, but I'm always ready to tell a tale. (Insert line about where it is located in the real world) ";
        //egg        
        hints[4,0] = "I cannot be used until I am broken. (Insert line about where it is located in the real world). ";
        hints[4,1] = "A box without no opening, yet golden treasure is hidden inside. (Insert line about where it is located in the real world). ";
        hints[4,2] = "I have no bones or legs but if you keep me warm I will soon walk. (Insert line about where it is located in the real world).";
        //diamond
        hints[5,0] =  "Sometimes called a rock, but I am often worth a great deal. A bauble or a precious stone, so tempting for some to steal. (Insert line about where it is located in the real world)";
        hints[5,1] = "I am a thing that is treasured, worthless or valuable and thus guarded by the most dangerous of creatures";
        hints[5,2] = "I am clear and small. I am hidden underground, worthless until found.";
        //mortar pestle
        hints[6,0] = "Separate I am useless but together I can crush ingredients into powder. (Insert line about where it is located in the real world)";
        hints[6,1] = "I am deep but not a basin. I hold and crush but do not have hands. I am made of stone. (Insert line about where it is located in the real world";
        hints[6,2] = "";
        //mushroom
        hints[7,0] = "I am a type of room you cannot enter or leave. Raised from the ground below, I could be poisonous or a delicious treat. (Insert line about where it is located in the real world)";
        hints[7,1] = "I can be grown without sun or soil and can either provide nourishment or deliver poison. (Insert line about where it is located in the real world)";
        hints[7,2] = "";
        //potion bottle
        hints[8,0] = "You will need to use me to contain the strength to put the dragon into his slumber (Insert line about where it is located in the real world)";
        hints[8,1] = "When I am made I will fill up, when I am used I will be empty inside. (Insert line about where it is located in the real world";
        hints[8,2] = "";
        //snake venom
        hints[9,0] = "If I am removed from my host, he will become safe to the touch. (Insert line about where it is located in the real world)";
        hints[9,1] = " come from the most dangerous of serpents. One unaltered drop could be fatal. (Insert line about where it is located in the real wo";
        hints[9,2] = "";
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
        //hintText.text = arHints[arIndex];
        do 
        {
            hintText.text = hints[arIndex, Random.Range(0,3)];
        } while (hintText.text == "");
       
    }

    private IEnumerator SpawnFoundObject(int paramArIndex){
        //turn off the hint panel
        hintPanel.SetActive(false);
        //turn off the panels ability to turn on for the duration of showing the model
        mainCanvas.GetComponent<HintPanelAnimatorController>().canToggle = false;
        //ensuring no usage of button
        hintButton.GetComponent<Button>().enabled = false;
        arModels[paramArIndex].SetActive(true);
        yield return new WaitForSeconds(3.0f);
        arModels[paramArIndex].SetActive(false);
        //enable the hint panel
        hintPanel.SetActive(true);
        //enable the ability to toggle the panel
        mainCanvas.GetComponent<HintPanelAnimatorController>().canToggle = true;
        hintButton.GetComponent<Button>().enabled = true;
        
    }
}