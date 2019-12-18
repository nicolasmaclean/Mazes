using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraZoom : MonoBehaviour
{
    public static int width = 11;
    public static int height = 9;

    public float div = 3.5f;

    void Start()
    {
        updateGridSize();
    }

    void updateGridSize()
    {
        width = (int) Mathf.Abs(transform.position.z * div);
        height = (int) Mathf.Abs(transform.position.z * div);
    }

    // Update is called once per frame
    void Update()
    {
        updateGridSize();
    }
}
