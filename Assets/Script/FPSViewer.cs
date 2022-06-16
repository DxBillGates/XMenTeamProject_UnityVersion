using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSViewer : MonoBehaviour
{
    [SerializeField] Text displayFpsText;
    private int frameCount;
    private float lastViewTime;

    // Start is called before the first frame update
    void Start()
    {
        frameCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        frameCount++;

        if (displayFpsText == null) { return; }

        if (Time.time < lastViewTime + 1) { return; }
        displayFpsText.text = "FPS:" + frameCount;
        frameCount = 0;

        lastViewTime = Time.time;
    }
}
