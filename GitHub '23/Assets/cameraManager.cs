using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class cameraManager : MonoBehaviour
{
    public bool zoomedIn = true;
    List<CinemachineVirtualCamera> levelCams = new List<CinemachineVirtualCamera>();
    public CinemachineVirtualCamera zoomedOutCam;
    public float blendTime = 1f;
    public CinemachineBrain brain;
    public bool blending = false;
    goal[] goals;

    private void Start() 
    {
        goals = FindObjectsOfType<goal>();
        
        foreach(CinemachineVirtualCamera camera in FindObjectsOfType<CinemachineVirtualCamera>())
        {
            if(camera != zoomedOutCam)
            {
                levelCams.Add(camera);
            }
        }
    }

    private void Update() 
    {
        zoomedOutCam.m_Lens.OrthographicSize = 8 * FindObjectOfType<scaleManager>().transform.localScale.x;
        zoomedOutCam.m_Lens.FarClipPlane = FindObjectOfType<playerController>().currentCam.GetComponent<CinemachineVirtualCamera>().m_Lens.FarClipPlane;

        int x = 0;

        foreach(goal goal in goals)
        {
            if(goal.winning)
            {
                x++;
            }
        }

        if(Input.GetKeyDown("z") && !brain.IsBlending && x == 0)
        {
            brain.m_DefaultBlend.m_Time = blendTime;
            zoomedIn = !zoomedIn;
            StartCoroutine(camMovingDelay());
        }    

        FindObjectOfType<playerController>().GetComponent<playerController>().CanMove = zoomedIn;

        foreach(CinemachineVirtualCamera camera in levelCams)
        {
            if(camera.gameObject == FindObjectOfType<playerController>().currentCam)
            {
                camera.Priority = 1;
            }
            else
            {
                camera.Priority = 0;
            }
        }

        if(zoomedIn)
        {
            zoomedOutCam.Priority = 0;
        }
        else
        {
            zoomedOutCam.Priority = 2;
        }
    }

    public void Advance()
    {
        brain.m_DefaultBlend.m_Time = 0f;
    }

    IEnumerator camMovingDelay()
    {
        blending = true;
        
        yield return new WaitForSeconds(blendTime);

        blending = false;
    }
}
