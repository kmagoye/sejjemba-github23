using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class apron : MonoBehaviour
{
    private void Start() 
    {
        foreach(SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.enabled = true;
        }    
    }
    
    void Update()
    {
        if(!FindObjectOfType<cameraManager>().blending && FindObjectOfType<cameraManager>().zoomedIn)
        {
            float newScale = Camera.main.orthographicSize / 4.5f;

            transform.localScale = new Vector3(newScale, newScale, transform.localScale.z);
        }
    }
}
