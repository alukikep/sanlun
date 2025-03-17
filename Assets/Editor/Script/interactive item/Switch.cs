using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.Z;
    public float interactRange;
    public GameObject[] bricksToDesTroy;
    private bool isPlayerNearBy = false;
    private Animator animator;

    private GameObject audio;
    AudioController audioController;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audio = GameObject.FindGameObjectWithTag("Audio");
        audioController = audio.GetComponent<AudioController>();
    }
    private void Update()
    {
        CheckPlayer();
        if(isPlayerNearBy&&Input.GetKeyDown(interactKey))
        {
            animator.Play("Start");
            audioController.PlaySfx(audioController.handleStart);
            Invoke("WallBreak",0.5f);
            DestroyBricks();
        }
    }

    private void WallBreak()
    {
        audioController.PlaySfx(audioController.wallBreak);
    }
    private void CheckPlayer()
    {
        Collider2D[] Player = Physics2D.OverlapCircleAll(transform.position, interactRange);
        foreach(var hit in Player)
        {
            if(hit.CompareTag("Player"))
            {
                isPlayerNearBy = true;
                break;
            }
            else
            {
                isPlayerNearBy = false;
            }
        }

    }
    private void DestroyBricks()
    {
        if(bricksToDesTroy!=null)
        {
            foreach(GameObject brick in bricksToDesTroy)
            {
                if(brick!=null)
                {
                    Destroy(brick);
                }
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
