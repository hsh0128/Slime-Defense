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
    /// ��� override�� �� base.PlayClickSound ���� ����(��� ���� �⺻ �Ҹ��� ��µ�)
    /// </summary>
    protected virtual void PlayClickSound()
    {
        //AudioManager.instance.PlaySFX("place");
    }

    // �ٵ� OnMouseEnter�� �ϸ� �ɰ� ���� �̰� ��ߵɱ�
    protected virtual void OnMousePointEnter()
    {

    }

    // �̰͵� OnMouseExit �׸��� �Ǵµ�
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