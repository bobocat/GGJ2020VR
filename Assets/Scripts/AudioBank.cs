using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioBank : MonoBehaviour
{

    public AudioClip[] clipList;
    AudioSource audioSource;
    public static AudioBank instance;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayClip(int index)
    {
        audioSource.PlayOneShot(clipList[index]);
    }
}
