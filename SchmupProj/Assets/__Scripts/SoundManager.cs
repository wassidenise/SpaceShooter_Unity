using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {


    //public static AudioSource efxSource;
    public AudioSource musicSource;
    public static SoundManager instance = null;



    
    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {instance = this;}
        else if (instance != this)
        { Destroy(gameObject); }


       // GameObject[] obj = GameObject.FindGameObjectsWithTag("music");
       // if(obj.Length>1)
       // { Destroy(this.gameObject); }
        //DontDestroyOnLoad(this.gameObject);
    }

    //public static void PlaySingle(AudioClip clip)
    //{

    //efxSource.clip = clip;
    // efxSource.Play();

    //}


}