using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioSound
{
    Click,
    ClickError
}

public class SoundManager : MonoBehaviour {
    private static SoundManager _instance;
    public static SoundManager Instance { get { return _instance; } }
    public AudioSource audioSource;
    public AudioClip[] audioClips;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public void PlaySound(AudioSound audioSound)
    {
        audioSource.PlayOneShot(audioClips[(int)audioSound]);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
