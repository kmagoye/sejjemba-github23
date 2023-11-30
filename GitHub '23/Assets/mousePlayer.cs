using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mousePlayer : MonoBehaviour
{
    int scaleLayer = 1 << 10; 

    void Update()
    {
        if(!FindObjectOfType<cameraManager>().zoomedIn)
        {
            if(Input.GetMouseButtonDown(0))
            {
                RaycastHit2D scale = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                                                       transform.position, 0.1f, scaleLayer);
                
                valueScale[] scales = FindObjectsOfType<valueScale>();

                foreach(valueScale valueScale in scales)
                {
                    valueScale.selected = false;
                }

                if(scale)
                {
                    scale.transform.GetComponent<valueScale>().selected = true;
                }
            }
        }
    }
}
