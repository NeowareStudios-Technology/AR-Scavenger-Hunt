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
    //2d array of hints, First layer are the objects. Second layer are the hints.
    private string [,]hints = new string[10,3];
    public GameObject[] arModels;
    public int arIndex ;
    public int maxTargets = 10;
    public Game game;
    public modelAnimationController mainCamera;
    
    //UI references
    public UIReferences uIReferences;

    //keeps track of unlocked targets
    public List<int> unlockedTargets = new List<int>();


    void Start()
    {
        SetHintText();
        
        //determine a random index to start from
        Random.seed = (int)System.DateTime.Now.Ticks; 
        arIndex = Random.Range(0,maxTargets);

        unlockedTargets.Add(arIndex);
 
        PickRandomHint();
        
    }


    private void SetHintText(){


         //antlers
        hints[0, 0] = "Stary Night painting in Alfonso's office";
        hints[0, 1] = "";
        hints[0, 2] = "";
        //ashes
        hints[1, 0] = "ViewStub Marketing and Design sign";
        hints[1, 1] = "";
        hints[1, 2] = "";
        //dagger
        hints[2, 0] = "Thor Poster in the Lobby";
        hints[2, 1] = "";
        hints[2, 2] = "";
        //book
        hints[3, 0] = "Caution: High Noise Area sign in the break room";
        hints[3, 1] = "";
        hints[3, 2] = "";
        //egg        
        hints[4, 0] = "Florida Heat Map near the restrooms";
        hints[4, 1] = "";
        hints[4, 2] = "";
        //diamond
        hints[5, 0] = "Moana poster in the game room";
        hints[5, 1] = "";
        hints[5, 2] = "";
        //mortar pestle
        hints[6, 0] = "Space Jam paster in the hallway";
        hints[6, 1] = "";
        hints[6, 2] = "";
        //mushroom
        hints[7, 0] = "Deadpool poster in the hallway";
        hints[7, 1] = "";
        hints[7, 2] = "";
        //potion bottle
        hints[8, 0] = "Octopus in Zack's office";
        hints[8, 1] = "";
        hints[8, 2] = "";
        //snake venom
        hints[9, 0] = "Stranger Things poster in the hallway";
        hints[9, 1] = "";
        hints[9, 2] = "";
        
        /*
        //antlers
        hints[0, 0] = "I am a trophy for some, but for others I can be the key ingredient with the right tools to crush me. You will find me in the large room where everyone meets, at the place where you stand and speak";
        hints[0, 1] = "";
        hints[0, 2] = "";
        //ashes
        hints[1, 0] = "From fire I am born. When touched I turn your fingers silvery, grey. You will find me in the large room where everyone meets, on the wall incase of an emergency";
        hints[1, 1] = "I am the end of a Phoenix living, leaving me behind in order to create his next life. You will find me in the room where everyone meets, on the wall incase of an emergency";
        hints[1, 2] = "";
        //dagger
        hints[2, 0] = "I have a hilt to hold and sharp edges for you to cut what you will need. You will find me in the lobby on the orange cone";
        hints[2, 1] = "";
        hints[2, 2] = "";
        //book
        hints[3, 0] = "I have no voice and yet I speak to you, I tell of all things in the world that people do. I have a spine and hinges, but I am not a man or a door. I am in the lobby and I was created from this";
        hints[3, 1] = "I have leaves, but I am not a tree, I have pages, but I am not a bride or royalty. I am in the lobby and I was created from this";
        hints[3, 2] = "";
        //egg        
        hints[4, 0] = "I cannot be used until I am broken. I am on the fridge in the breakroom";
        hints[4, 1] = "A box without no opening, yet golden treasure is hidden inside. I am on the fridge in the breakroom";
        hints[4, 2] = "";
        //diamond
        hints[5, 0] = "Sometimes called a rock, but I am often worth a great deal. A bauble or a precious stone, so tempting for some to steal. I am in the hall where you walk, chillin with Boss Ross";
        hints[5, 1] = "I am a thing that is treasured, worthless or valuable and thus guarded by the most dangerous of creatures. I am in the hall where you walk, chillin with Boss Ross";
        hints[5, 2] = "";
        //mortar pestle
        hints[6, 0] = "Separate I am useless but together I can crush ingredients into powder. Find me at the designated Party Animal Parking";
        hints[6, 1] = "";
        hints[6, 2] = "";
        //mushroom
        hints[7, 0] = "I am a type of room you cannot enter or leave. Raised from the ground below, I could be poisonous or a delicious treat. Find me on the Marvelous poster";
        hints[7, 1] = "";
        hints[7, 2] = "";
        //potion bottle
        hints[8, 0] = "You will need to use me to contain the strength to put the dragon into his slumber. I am in the hall where you extinguish fire";
        hints[8, 1] = "";
        hints[8, 2] = "";
        //snake venom
        hints[9, 0] = "If I am removed from my host, he will become safe to the touch. I am in the lobby on the mini fridge, don't confuse me for a drink";
        hints[9, 1] = "";
        hints[9, 2] = "";
        */

        /*
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
        */
    }

    //randomly unlock next target
    public void UnlockNextTarget()
    {
        
        game.IncrementScore();

        //Win Condition : if UnlockedTargets list size is greater or equal to max hints
        //Note: Win Condition is handled in the Player Manger within "IncrementScore" 
        if ( unlockedTargets.Count == maxTargets)
        {   
            //spawn the last object then return.
            StartCoroutine(SpawnFoundObject(arIndex));
            return;
        }
        
        
        //randomly get index of next target to hunt
        int randomSelection = Random.Range(0,maxTargets);

        //ensure that the target is not already unlocked
        while (unlockedTargets.Contains(randomSelection) == true)
        {
            randomSelection = Random.Range(0,maxTargets);
        }

        //spawn the model for the player to see
        StartCoroutine(SpawnFoundObject(arIndex));

        //save the target index in the unlockedTargets list
        unlockedTargets.Add(randomSelection);
        arIndex = randomSelection;
        
        PickRandomHint();
         
    }


    //Find a random hint, between index 0 and 2. if index has an empty hint, find another
    private void PickRandomHint()
    {
        do 
        {
            uIReferences.hintText.text = hints[arIndex, Random.Range(0,3)];
        } while (uIReferences.hintText.text == "");
    }

    private IEnumerator SpawnFoundObject(int paramArIndex)
    {
        //turn off the hint panel
        uIReferences.hintPanel.SetActive(false);

        //turn off the panels ability to turn on for the duration of showing the model
        uIReferences.mainCanvas.GetComponent<HintPanelAnimatorController>().canToggle = false;

        //ensuring no usage of hint button
        uIReferences.hintButton.GetComponent<Button>().enabled = false;

        //set the found model active
        arModels[paramArIndex].SetActive(true);

        //enable rising animation for model
        mainCamera.ChangeStateOfAnimator();

        yield return new WaitForSeconds(6.0f);

        //enable fall animation for model
        mainCamera.ChangeStateOfAnimator();

        yield return new WaitForSeconds(2.0f);

        //turn off model
        arModels[paramArIndex].SetActive(false);

        //enable the hint panel
        uIReferences.hintPanel.SetActive(true);

        //enable the ability to toggle the panel
        uIReferences.mainCanvas.GetComponent<HintPanelAnimatorController>().canToggle = true;

        //enable the ability to use the hint button again
        uIReferences.hintButton.GetComponent<Button>().enabled = true;
        
    }
}