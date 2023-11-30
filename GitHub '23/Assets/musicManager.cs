using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class musicManager : MonoBehaviour
{
    //first dimension is level
    //second dimension is the acutal music
    [SerializeField] public List<utility.song> clipArray = new List<utility.song>();
    AudioSource source;
    public int levelProgress;
    public int songProgress = 0;
    bool isPlaying = false;

    private void Start() 
    {
        source = GetComponent<AudioSource>();    
        playNext();
    }

    private void Update() 
    {
        if(isPlaying)
        {
            if(!source.isPlaying)
            {
                playNext();
            }
        }
    }

    public void advance()
    {
        levelProgress++;
    }

    void playNext()
    {
        isPlaying = false;

        source.clip = clipArray[levelProgress].clips[songProgress];
        
        songProgress++;

        songProgress = songProgress % 4;


        source.Play();

        isPlaying = true;
    }
}
