using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSetup : MonoBehaviour
{
    private Canvas CanvasObject;
    void Awake()
    {
        CanvasObject = this.GetComponent<Canvas>();
    }

    void Update()
    {

    }
}
