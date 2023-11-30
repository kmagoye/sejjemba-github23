using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class valueScale : MonoBehaviour
{
    public bool selected = false;
    public bool on = true;
    public int value = 0;
    public int maxValue = 4;
    public Sprite highlightSprite;
    public Sprite regularSprite;
    public Rigidbody2D rb2d;
    [HideInInspector] public SpriteRenderer sprite;
    public int color;   // C - 0
                        // M - 1
                        // Y - 2
                        // K - 3
    int moveDistanceX = 10;
    int moveDistanceY = 12;
    Vector2 defaultPosition = new Vector2(-194, 20000); //position of the slider when it is "blank"
    cameraManager camMan;
    public scaleManager scaleMan;

    private void Start() 
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        camMan = FindObjectOfType<cameraManager>();
    }

    private void Update() 
    {
        tryMove();

        if(!on)
        {
            sprite.enabled = false;
        }
        else
        {
            sprite.enabled = true;

            if(selected)
            {
                sprite.sprite = highlightSprite;
            }
            else
            {
                sprite.sprite = regularSprite;
            }
        }
    }

    private void tryMove()
    {
        if(!selected || camMan.zoomedIn || camMan.blending)
        {
            return;
        }

        if(Input.GetKey("up") || Input.GetKey("down"))
        {
            return;
        }

        //left and right movemements manage different values
        if((Input.GetKeyDown("right") || Input.GetKeyDown("d")) && value < maxValue)
        {
            StartCoroutine(smoothMove(new Vector2(-1,0), moveDistanceX, 20));
            value++;
        }
        if((Input.GetKeyDown("left") || Input.GetKeyDown("a")) && value > 0)
        {
            StartCoroutine(smoothMove(new Vector2(1,0), moveDistanceX, 20));
            value--;
        }

    }

    public void smoothMovePublic(Vector2 direction)
    {
        StartCoroutine(smoothMove(direction, moveDistanceY, 20));
    }

    IEnumerator smoothMove(Vector2 direction, int distance, int speed)
    {
        Vector2 endPosition = new Vector2(direction.x * distance * scaleMan.transform.localScale.x + transform.position.x,
                                          direction.y * distance * scaleMan.transform.localScale.y + transform.position.y); 
        
        int count = 1;

        while(count <= speed)
        {
            Vector2 translation = direction * (distance * scaleMan.transform.localScale.x / Mathf.Pow(2,count));
            
            transform.Translate(translation);

            yield return new WaitForFixedUpdate();

            count++;
        }

        rb2d.position = endPosition;
    }

    public void setPosition(int desiredStrength, int selected)
    {
        if(defaultPosition == new Vector2(-194, 20000))
        {
            defaultPosition = transform.localPosition;
        }
        
        float x = Camera.main.transform.position.x + (defaultPosition.x - 10 * desiredStrength) * scaleMan.transform.localScale.x;

        float y = Camera.main.transform.position.y + (defaultPosition.y - 12 * selected) * scaleMan.transform.localScale.y;
        
        value = desiredStrength;

        rb2d.position = new Vector2(x, y);
    }
}
