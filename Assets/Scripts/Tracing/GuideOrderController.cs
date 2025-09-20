using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GuideOrderController : MonoBehaviour
{
    //This script ensures that the strokes are drawn in an order.

    public List<GameObject> guides;
    public GameObject nextLevelPanel;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject guide in guides)
        {
            SetComponentInactive(guide);
        }
        nextLevelPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Contineously destroy element on index 0, untill the list is empty.
        if (guides.Count > 0)
        {
            SetComponentActive(guides[0]);
            if (guides[0].GetComponent<TracingSystem>().traceComplete)
            {
                Destroy(guides[0]);
                guides.RemoveAt(0);
            }
        }
        AllGuidesDone();

    }

    void SetComponentActive(GameObject gameObject)  //Enable the tracing logic script on the guides.
    {
        TracingSystem tracingSystem = gameObject.GetComponent<TracingSystem>();
        if (tracingSystem)
        {
            tracingSystem.enabled = true;
        }
    }
    void SetComponentInactive(GameObject gameObject)  //Disable the tracing logic script on guides.
    {
        TracingSystem tracingSystem = gameObject.GetComponent<TracingSystem>();
        if (tracingSystem)
        {
            tracingSystem.enabled = false;
        }
    }
    void AllGuidesDone() //When all guides are done, enable next letter panel.
    {
        if(guides.Count <= 0){
            nextLevelPanel.gameObject.SetActive(true);
        }
        
    }
}
