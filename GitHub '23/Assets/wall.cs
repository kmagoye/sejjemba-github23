using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class wall : MonoBehaviour
{
    BoxCollider2D box;
    public SpriteRenderer sprite;
    Rigidbody2D rb2d;
    scaleManager scaleManager;
    public bool requiresExactState;
    public utility.scaleState targetState;
    public List<SpriteRenderer> outlines = new List<SpriteRenderer>();
    public bool Loaded = true;
    public bool onScreen;
    public List<CinemachineVirtualCamera> myCams = new List<CinemachineVirtualCamera>();
    public bool isBox;
    public List<SpriteRenderer> compoundSprites = new List<SpriteRenderer>();

    private void Start() 
    {
        box = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();

        scaleManager = FindObjectOfType<scaleManager>();

        foreach(SpriteRenderer outline in GetComponentsInChildren<SpriteRenderer>())
        {
            if(sprite != outline && !compoundSprites.Contains(outline))
            {
                outlines.Add(outline);

                outline.material = sprite.material;
                outline.enabled = false;
            }
        }
    }

    private void Update() 
    {
        onScreen = false;
        
        if(myCams.Count != 0 && FindObjectOfType<playerController>().currentCam)
        {          
            foreach(CinemachineVirtualCamera cam in myCams)
            {
                if(cam.name == FindObjectOfType<playerController>().currentCam.name)
                {
                    onScreen = true;
                }
            }
        }

        if(Loaded)
        {
            box.enabled = On();  
            if(onScreen)
            {
                sprite.enabled = On();
            }
            else if(!FindObjectOfType<cameraManager>().zoomedIn)
            {
                sprite.enabled = false;
            }
            else
            {
                sprite.enabled = On();
            }

            foreach(SpriteRenderer outline in outlines)
            {
                outline.enabled = false;
            }
        }
        else
        {
            box.enabled = false;
            sprite.enabled = false;

            foreach(SpriteRenderer outline in outlines)
            {
                outline.enabled = true;
            }

            if(onScreen)
            {
                Loaded = !On();
            }
        }

        if(isBox)
        {
            if(box.enabled && onScreen)
            {
                rb2d.gravityScale = 10f;
            }
            else
            {
                rb2d.gravityScale = 0;
            }
        }
    }

    public bool On()
    {
        if(scaleManager.getCurrentState().selectedScale == targetState.selectedScale || 
            targetState.strength == 0)
        {
            if(requiresExactState)
            {
                if(scaleManager.getCurrentState().strength == targetState.strength)
                {
                    return false;    
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if(scaleManager.getCurrentState().strength >= targetState.strength)
                {
                    return false;    
                }
                else
                {
                    return true;
                }
            }
        }
        else
        {
            return true;
        }
    }
}
