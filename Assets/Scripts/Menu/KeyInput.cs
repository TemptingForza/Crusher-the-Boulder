using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class KeyInput : Setting<KeyCode>, IPointerClickHandler
{
    IEnumerator Editing()
    {
        var l = Cursor.lockState;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        var s = text.text;
        text.text = "...";
        KeyCode key;
        while (!InputManager.AnyKeyDown(out key))
            yield return null;
        RaiseEvent(key);
        text.text = InputManager.GetKeyName(key);
        Cursor.lockState = l;
        Cursor.visible = true;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            RaiseEvent(KeyCode.None);
            text.text = InputManager.GetKeyName(KeyCode.None);
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
            StartCoroutine(Editing());
        if (eventData.button != PointerEventData.InputButton.Middle)
            eventData.Use();
    }
}
