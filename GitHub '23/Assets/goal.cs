using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using Unity.Mathematics;

public class goal : MonoBehaviour
{
    public CinemachineVirtualCamera myCam;
    public CinemachineVirtualCamera targetCam;
    [HideInInspector] public CircleCollider2D box;
    Rigidbody2D rb2d;
    public float speed;
    public float targetSize;
    cameraManager camMan;
    playerController player;
    scaleManager scaleMan;
    SpriteRenderer sprite;
    public bool lastGoal;
    bool dead = false;
    public bool winning = false;
    void Start()
    {
        camMan = FindObjectOfType<cameraManager>();
        player = FindObjectOfType<playerController>();
        scaleMan = FindObjectOfType<scaleManager>();
        box = GetComponent<CircleCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update() 
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z + 1);  

        sprite.enabled = false;

        if(myCam.gameObject == player.currentCam)
        {
            sprite.enabled = true;
        }

        if(targetCam.gameObject == player.currentCam)
        {
            sprite.enabled = true;
        }

        if(dead)
        {
            sprite.enabled = false;
            box.enabled = false;
        }
    }

    public void Win()
    {
        box.enabled = false;
        FindObjectOfType<musicManager>().advance();
        StartCoroutine(winCoRoutine());
    }

    IEnumerator winCoRoutine()
    {
        player.CanMove = false;
        winning = true;

        if(lastGoal)
        {  
            scaleMan.enabled = false;

            foreach(SpriteRenderer sprite in player.GetComponentsInChildren<SpriteRenderer>())
            {
                sprite.enabled = false;
            }

            player.enabled = false;
            camMan.enabled = false;
        }

        while(transform.localScale.x < targetSize)
        {
            float newScale = transform.localScale.x + speed;

            transform.localScale = new Vector3(newScale, newScale, transform.localScale.z);

            transform.Rotate(0, 0, transform.rotation.z + 3);

            yield return new WaitForFixedUpdate();
        }

        if(lastGoal)
        {
            yield break;
        }

        player.CanMove = true;
        transform.position = targetCam.transform.position;

        scaleMan.Advance();
        camMan.Advance();

        player.rb2d.position = targetCam.transform.position;

        while(transform.localScale.x > 0)
        {
            float newScale = transform.localScale.x - speed;

            transform.localScale = new Vector3(newScale, newScale, transform.localScale.z);

            transform.Rotate(0, 0, transform.rotation.z - 3);

            yield return new WaitForFixedUpdate();
        }

        dead = true;
        winning = false;
        camMan.brain.m_DefaultBlend.m_Time = camMan.blendTime;
    }
}
