using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarAndPestleSound : MonoBehaviour
{

    public GameObject mortarAndPestleAudio;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "mortar_low")
        {
            mortarAndPestleAudio.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "mortar_low")
        {
            mortarAndPestleAudio.SetActive(false);
        }
    }
}
