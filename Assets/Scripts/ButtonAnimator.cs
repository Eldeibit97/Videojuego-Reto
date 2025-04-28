// ✅ Script: ButtonAnimator.cs
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimator : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    private Animator anim;

    void Start() => anim = GetComponent<Animator>();

    public void OnPointerEnter(PointerEventData eventData)
    {
        anim.SetTrigger("Hover");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        anim.SetTrigger("Click");
    }
}
