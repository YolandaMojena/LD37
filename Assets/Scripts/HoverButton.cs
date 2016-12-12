using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private float scale = 0.0f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<RectTransform>().localScale *= 1.2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<RectTransform>().localScale /= 1.2f;
    }
}