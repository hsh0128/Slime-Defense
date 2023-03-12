using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonModel : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            OnMiddleClick();
        }
    }

    protected virtual void OnLeftClick()
    {
        PlayClickSound();
    }

    protected virtual void OnRightClick()
    {

    }

    protected virtual void OnMiddleClick()
    {

    }

    /// <summary>
    /// 요건 override할 때 base.PlayClickSound 쓰지 말기(요걸 쓰면 기본 소리가 출력됨)
    /// </summary>
    protected virtual void PlayClickSound()
    {
        //AudioManager.instance.PlaySFX("place");
    }

    // 근데 OnMouseEnter로 하면 될걸 굳이 이걸 써야될까
    protected virtual void OnMousePointEnter()
    {

    }

    // 이것도 OnMouseExit 그리면 되는데
    protected virtual void OnMousePointExit()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMousePointEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMousePointExit();
    }
}