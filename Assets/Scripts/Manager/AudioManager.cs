using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    
    public static AudioManager instance;

    private AudioSource managerAudio;

    private AudioClip[] audioClips = new AudioClip [10];

    public AudioClip walkSE, jumpSE, extractionSE, rotationSE, switchSE, cardSE, winSE, dieSE, UISE;

    public enum SoundEffect {
        Walk = 0,
        Jump = 1,
        Extraction = 2,
        Rotation = 3,
        Switch = 4,
        Card = 5,
        Win = 6,
        Die = 7,
        UISound = 8
    }

    private void Awake() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }


    private void Start() {
        managerAudio = this.GetComponent<AudioSource>();
        audioClips[0] = walkSE;
        audioClips[1] = jumpSE;
        audioClips[2] = extractionSE;
        audioClips[3] = rotationSE;
        audioClips[4] = switchSE;
        audioClips[5] = cardSE;
        audioClips[6] = winSE;
        audioClips[7] = dieSE;
        audioClips[8] = UISE;
        Invoke("PlayCard", 0.1f);
    }

    public void PlayCard() {
        PlaySE(SoundEffect.Card);

    }

    public void PlaySE(SoundEffect se) {
        managerAudio.PlayOneShot(audioClips[(int) se]);
    }
}

[System.Serializable]
public class SoundSet {
    public AudioClip walkSE, jumpSE, extractionSE, rotationSE, SwitchSE, WinSE, DieSE, UISE;
}