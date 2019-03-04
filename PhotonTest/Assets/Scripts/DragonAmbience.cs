using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAmbience : MonoBehaviour
{
    
    public AudioClip[] audioClips;
    private AudioClip clipToPlay;

    //the audioSource attached to this gameObject
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRandomClip(float volumeParam)
    {
        //set volume from a scale of 1 to 10, to a scale of 0 to 1
        float volume = volumeParam/10.0f;
        
        StartCoroutine(WaitThenPlayClip(volume));
    }

    private IEnumerator WaitThenPlayClip(float volumeParam)
    {

        float secondsToWait = Random.Range(3.0f,5.0f);
        yield return new WaitForSeconds(secondsToWait);
        //choose a random clip index
        int randomClipIndex = Random.Range(0,audioClips.Length);
        clipToPlay = audioClips[randomClipIndex];
        audioSource.clip = clipToPlay;
        audioSource.volume = volumeParam + 0.4f;
        audioSource.PlayOneShot(clipToPlay, volumeParam);


    }
}
