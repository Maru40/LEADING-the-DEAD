using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UniRx;

public class GameFocusManager
{
    private class ColorAndFocusObject
    {
        public GameObject focusObject;
        public Color normalColor = new Color();

        public ColorAndFocusObject(GameObject focusObject)
        {
            this.focusObject = focusObject;
        }

        public ColorAndFocusObject(GameObject focusObject,Color normalColor)
        {
            this.focusObject = focusObject;
            this.normalColor = normalColor;
        }
    }

    private static Stack<ColorAndFocusObject> m_colorAndfocusObjectStack = new Stack<ColorAndFocusObject>();

    public static GameObject FocusObject
    {
        set => EventSystem.current.SetSelectedGameObject(value);
        get => EventSystem.current.currentSelectedGameObject;
    }

    public static void PushFocus(GameObject nextObject)
    {
        m_colorAndfocusObjectStack.Push(new ColorAndFocusObject(FocusObject));

        EventSystem.current.SetSelectedGameObject(nextObject);

        var objectAndColor = m_colorAndfocusObjectStack.Peek();

        if(objectAndColor == null || objectAndColor.focusObject == null)
        {
            return;
        }

        var selectable = objectAndColor.focusObject.GetComponent<Selectable>();

        if(!selectable)
        {
            return;
        }

        objectAndColor.normalColor = selectable.colors.normalColor;

        var selectedColorBlock = selectable.colors;
        selectedColorBlock.normalColor = selectedColorBlock.selectedColor;

        selectable.colors = selectedColorBlock;
    }

    public static void PopFocus()
    {
        if(m_colorAndfocusObjectStack.Count == 0)
        {
            return;
        }

        var objectAndColor = m_colorAndfocusObjectStack.Pop();

        EventSystem.current.SetSelectedGameObject(objectAndColor.focusObject);

        var selectable = objectAndColor.focusObject.GetComponent<Selectable>();

        if (!selectable)
        {
            return;
        }

        var selectedColorBlock = selectable.colors;
        selectedColorBlock.normalColor = objectAndColor.normalColor;

        selectable.colors = selectedColorBlock;
    }

    public static void ClearFocus() => m_colorAndfocusObjectStack.Clear();

}
