using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class compoundWall : MonoBehaviour
{
    BoxCollider2D box;
    SpriteRenderer sprite;
    public BoxCollider2D parentBox;
    public SpriteRenderer parentSprite;

    private void Start() 
    {
        box = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();    
    }

    private void Update() 
    {
        box.enabled = parentBox.enabled;
        sprite.enabled = parentBox.enabled;    
    }
}
