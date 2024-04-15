using UnityEngine;
using UnityEngine.EventSystems;

public abstract class CustomBtn : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    // ボタンを押して離したときに実行するアクション
    public abstract void OnPush();

    // ボタンを押した瞬間に実行するアクション
    public abstract void OnPushDown();

    // ボタンを離した瞬間に実行するアクション
    public abstract void OnPullUp();

    // タップ  
    public void OnPointerClick(PointerEventData eventData)
    {
        OnPush();
    }

    // タップダウン  
    public void OnPointerDown(PointerEventData eventData)
    {
        OnPushDown();
    }

    // タップアップ  
    public void OnPointerUp(PointerEventData eventData)
    {
        OnPullUp();
    }
}
