using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintPanelToggle : MonoBehaviour
{
    
    public GameObject hintPanel;
    public bool canToggle = true;

    public void ToggleHintPanel()
    {
        if (canToggle){
            if (hintPanel.activeInHierarchy == true){
                Debug.Log("I Am active!");
                hintPanel.SetActive(false);
            }
            else{
                hintPanel.SetActive(true);
            }
        }   
    }
}
