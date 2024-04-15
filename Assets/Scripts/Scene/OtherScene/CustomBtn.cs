using UnityEngine;
using UnityEngine.EventSystems;

public abstract class CustomBtn : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    // �{�^���������ė������Ƃ��Ɏ��s����A�N�V����
    public abstract void OnPush();

    // �{�^�����������u�ԂɎ��s����A�N�V����
    public abstract void OnPushDown();

    // �{�^���𗣂����u�ԂɎ��s����A�N�V����
    public abstract void OnPullUp();

    // �^�b�v  
    public void OnPointerClick(PointerEventData eventData)
    {
        OnPush();
    }

    // �^�b�v�_�E��  
    public void OnPointerDown(PointerEventData eventData)
    {
        OnPushDown();
    }

    // �^�b�v�A�b�v  
    public void OnPointerUp(PointerEventData eventData)
    {
        OnPullUp();
    }
}
