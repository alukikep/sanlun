using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource Sfx;
    [SerializeField] private AudioSource BGM;

    public AudioClip bgm;
    public AudioClip attack;
    public  AudioClip playerHurt;
    public AudioClip enemyHurt;
    public AudioClip enemyDie;
    public AudioClip jump;
    public AudioClip fallGround;
    public AudioClip handleStart;
    public AudioClip wallBreak;
    void Start()
    {
        BGM.clip=bgm;
        BGM.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySfx(AudioClip sfx)
    {
        Sfx.PlayOneShot(sfx);
    }
}
