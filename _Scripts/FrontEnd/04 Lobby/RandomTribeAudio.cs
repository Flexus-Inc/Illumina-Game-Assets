using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Illumina;

public class RandomTribeAudio : MonoBehaviour
{
    public AudioClip earthClip;
    public AudioClip waterClip;
    public AudioClip fireClip;
    public AudioClip voidClip;
    Player player;
    void Start()
    {
        
        AudioSource earthSound = GetComponent<AudioSource>();
        AudioSource waterSound = GetComponent<AudioSource>();
        AudioSource fireSound = GetComponent<AudioSource>();
        AudioSource voidSound = GetComponent<AudioSource>();
        int tribeInt = (int)player.tribe;
        switch(tribeInt)
        {
            case 0: voidSound.PlayOneShot(voidClip);
                    break;
            case 1: earthSound.PlayOneShot(earthClip);
                    break;
            case 2: fireSound.PlayOneShot(fireClip);
                    break;
            case 3: waterSound.PlayOneShot(waterClip);
                    break;
            default: Debug.Log("404 Error, tribe not found");
                    break;
        }

    }
}
