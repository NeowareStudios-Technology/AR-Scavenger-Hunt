using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitThenPlaySound : MonoBehaviour
{
    private AudioSource audioSource;
   
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
    }

    void OnEnable()
    {
        StartCoroutine(WaitAndPlaySound());
    }

    private IEnumerator WaitAndPlaySound()
    {
        yield return new WaitForSeconds(0.8f);
        audioSource.PlayOneShot(audioSource.clip);
    }
}
