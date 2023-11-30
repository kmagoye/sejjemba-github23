using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class scaleManager : MonoBehaviour
{
    public cameraManager camMan;
    public List<valueScale> scales = new List<valueScale>();
    public List<utility.scaleState> scaleStates = new List<utility.scaleState>();
    int progress = 1; //serialize feild is wack so it has to start at one
    public int currentScale = 1;
    int maxScale = 1; 
    public bool disabled;
    private void Start() 
    {
        applyState(scaleStates[progress]);    
    }

    private void Update() 
    {
        tryMove();

        if(!camMan.blending && FindObjectOfType<cameraManager>().zoomedIn)
        {
            float newScale = Camera.main.orthographicSize / 4.5f;

            transform.localScale = new Vector3(newScale, newScale, transform.localScale.z);
        }
    }

    void tryMove()
    {
        if(camMan.zoomedIn || disabled)
        {
            return;
        }
        
        //evaluate max scale
        maxScale = 0;
        
        foreach(valueScale scale in scales)
        {
            if(scale.on)
            {
                maxScale++;
            }
        }

        //up and down switch between scales
        if((Input.GetKeyDown("up") || Input.GetKeyDown("w")) && currentScale < maxScale)
        {
            currentScale++;
            moveScales(false);
        }
        if((Input.GetKeyDown("down") || Input.GetKeyDown("s")) && currentScale > 1)
        {
            currentScale--;
            moveScales(true);
        }
    }

    void moveScales(bool up)
    {
        Vector2 movement; 

        if(up)
        {   
            movement = new Vector2(0,1);
        }
        else
        {
            movement = new Vector2(0,-1);
        }

        foreach(valueScale scale in scales)
        {
            scale.smoothMovePublic(movement);

            if(scales.IndexOf(scale) + 1 == currentScale)
            {
                scale.selected = true;
            }
            else
            {
                scale.selected = false;
            }
        }
    }

    public void Advance()
    {
        progress++;
        applyState(scaleStates[progress]);
    }

    public void applyState(utility.scaleState state)
    {
        currentScale = state.selectedScale + 1;
        
        foreach(valueScale scale in scales)
        {
            int scaleIndex = scales.IndexOf(scale);

            //enabling/disabling scales
            if(state.enabledScales.Contains(scaleIndex))
            {
                scale.on = true;
            }
            else
            {
                scale.on = false;
            }

            //selecting selected scale and setting its position
            if(state.selectedScale == scaleIndex)
            {
                scale.setPosition(state.strength, state.selectedScale);
                scale.selected = true;
            }
            else
            {
                scale.setPosition(0, state.selectedScale);
                scale.selected = false;
            }
        }
    }

    public utility.scaleState getCurrentState()
    {
        utility.scaleState state = new utility.scaleState();

        foreach(valueScale scale in scales)
        {
            int scaleIndex = scales.IndexOf(scale);

            if(scale.selected)
            {
                state.selectedScale = scaleIndex;

                state.strength = scale.value;
            }

            if(scale.on)
            {
                state.enabledScales.Add(scaleIndex);
            }
        }

        return state;
    }
}
