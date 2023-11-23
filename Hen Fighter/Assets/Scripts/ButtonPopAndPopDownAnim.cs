using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPopAndPopDownAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Animator _Animator;

    void Start() => _Animator.Play("LoveButtonOff");

    public void OnPointerEnter(PointerEventData eventData) => _Animator.Play("LoveButtonOn");

    public void OnPointerExit(PointerEventData eventData) => _Animator.Play("LoveButtonOff");
}





