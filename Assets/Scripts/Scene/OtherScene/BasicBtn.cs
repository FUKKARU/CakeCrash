using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BasicBtn : CustomBtn
{
    [SerializeField] Sprite normal;
    [SerializeField] Sprite hover;
    [SerializeField] ActInfo[] actInfos;
    Image img;

    private void Start()
    {
        img = GetComponent<Image>();
    }

    private void OnMouseEnter()
    {
        if (hover != null)
        {
            img.sprite = hover;
        }
    }

    private void OnMouseExit()
    {
        if (normal != null)
        {
            img.sprite = normal;
        }
    }

    public override void OnPush()
    {
        StartCoroutine(Act());
    }

    public override void OnPushDown()
    {

    }

    public override void OnPullUp()
    {

    }

    private IEnumerator Act()
    {
        for (int i = 0; i < actInfos.Length; i++)
        {
            yield return new WaitForSecondsRealtime(actInfos[i].delay);
            actInfos[i].act?.Invoke();
        }   
    }
}

[System.Serializable]
struct ActInfo
{
    public float delay;
    public UnityEvent act;
}
