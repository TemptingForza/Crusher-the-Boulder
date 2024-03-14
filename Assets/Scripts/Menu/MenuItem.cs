using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class MenuItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler
{
    public Text text;
    bool isHighlighted = false;
    Vector2 baseSize;
    bool sleeping;
    public Action OnClick;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlayOneShot("click");
        eventData.Use();
        isHighlighted = true;
        sleeping = false;
    }
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        AudioManager.Instance.PlayOneShot("click");
        eventData.Use();
        isHighlighted = false;
        sleeping = false;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            AudioManager.Instance.PlayOneShot("click_sharp");
            eventData.Use();
            OnClick?.Invoke();
        }
    }

    private void Update()
    {
        if (sleeping)
            return;
        var s = text.rectTransform.sizeDelta;
        if (s.x < baseSize.x)
            s = baseSize;
        if (s.x > baseSize.x + 80)
            s = baseSize + new Vector2(80, 0);
        text.rectTransform.sizeDelta = s.MoveTowards(baseSize + (isHighlighted ? new Vector2(80,0) : Vector2.zero), 500 * Time.deltaTime,out sleeping);
    }

    public void SetText(string newText)
    {
        text.text = newText;
        baseSize = new Vector2(text.preferredWidth + 20, text.preferredHeight);
        sleeping = false;
    }
}
