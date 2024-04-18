using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleImg : MonoBehaviour
{
    public Vector3 step;
    bool isZoom;
    RectTransform rect;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (isZoom)
        {
            rect.position += step * Time.deltaTime;
        }
    }

    public void Zoom()
    {
        isZoom = true;
    }
}
