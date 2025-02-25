using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource Sfx;
    [SerializeField] private AudioSource BGM;
    private string currentScene;

    [Header("BGM")]
    public AudioClip castleHall;
    public AudioClip sewers;
    public AudioClip village;
    public AudioClip arsenal;
    public AudioClip clocktower;
    public AudioClip castleKeep;


    [Header("Sfx")]
    public AudioClip attack;
    public  AudioClip playerHurt;
    public AudioClip enemyHurt;
    public AudioClip enemyDie;
    public AudioClip jump;
    public AudioClip fallGround;
    public AudioClip handleStart;
    public AudioClip wallBreak;
    public AudioClip block;
    public AudioClip blockStart;

    [Header("EnemySfx")]
    public AudioClip minotaurAttack;
    public AudioClip lightningSkeletonAttack;
    void Start()
    {
        currentScene = "123";
    }

    // Update is called once per frame
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != currentScene)
        {
            switch (scene.name)
            {
                case "CastleHall":
                    if (castleHall != null)
                    {
                        BGM.clip = castleHall;
                        BGM.Play();
                    }
                    break;
                case "Sewers":
                    if (sewers != null)
                    {
                        BGM.clip = sewers;
                        BGM.Play();
                    }
                    break;
                case "Village":
                    if (village != null)
                    {
                        BGM.clip = village;
                        BGM.Play();
                    }
                    break;
                case "Arsenal":
                    if (arsenal != null)
                    {
                        BGM.clip = arsenal;
                        BGM.Play();
                    }
                    break;
                case "ClockTower":
                    if (clocktower != null)
                    {
                        BGM.clip = clocktower;
                        BGM.Play();
                    }
                    break;
                case "CastleKeep":
                    if (castleKeep != null)
                    {
                        BGM.clip = castleKeep;
                        BGM.Play();
                    }
                    break;
            }
            currentScene = scene.name;
        }
    }



    public void PlaySfx(AudioClip sfx)
    {
        Sfx.PlayOneShot(sfx);
    }
}
