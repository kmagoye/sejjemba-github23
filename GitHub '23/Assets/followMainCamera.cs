using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followMainCamera : MonoBehaviour
{
    private void Update() 
    {
        GetComponent<Rigidbody2D>().position = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);    
    }
}
