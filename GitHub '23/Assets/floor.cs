using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class floor : MonoBehaviour
{
    public CinemachineVirtualCamera myCam;
    SpriteRenderer sprite;
    public bool isFloor;
    bool on = false;
    private void Start() 
    {
        sprite = GetComponent<SpriteRenderer>();   

        if(isFloor)
        {
            transform.localScale = new Vector3((myCam.m_Lens.OrthographicSize * 2f) * (10f / 9f), transform.localScale.y, transform.localScale.z);
        } 

        sprite.enabled = false;
    }

    private void Update() 
    {
        if(FindObjectOfType<playerController>().currentCam)
        {
            if(myCam.name == FindObjectOfType<playerController>().currentCam.name)
            {
                sprite.enabled = true;
                on = true;
            }
            else
            {
                if(on)
                {
                    StartCoroutine(WaitForScroll());
                    on = false;
                }
            }
        }
    }

    IEnumerator WaitForScroll()
    {
        yield return new WaitForSeconds(FindObjectOfType<cameraManager>().blendTime);

        sprite.enabled = false;
    }
}
