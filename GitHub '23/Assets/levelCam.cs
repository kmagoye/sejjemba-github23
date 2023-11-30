using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class levelCam : MonoBehaviour
{
    CinemachineVirtualCamera cam;
    BoxCollider2D box;
    void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        box = GetComponent<BoxCollider2D>();

        box.size = new Vector2(cam.m_Lens.OrthographicSize * 2, cam.m_Lens.OrthographicSize * 2);
        
        cam.m_Lens.FarClipPlane += 2 * cam.m_Lens.OrthographicSize;
    }
}
