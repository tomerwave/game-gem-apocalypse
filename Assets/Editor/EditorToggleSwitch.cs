using UnityEngine;
using UnityEditor;
public static class EditorToggleSwitch
{
    public static bool Draw(Rect rect, bool value)
    {
        // Background
        Color bgOn = new Color(0.25f, 0.75f, 0.35f);
        Color bgOff = new Color(0.55f, 0.55f, 0.55f);

        EditorGUI.DrawRect(rect, value ? bgOn : bgOff);

        // Knob
        float knobSize = rect.height - 4;
        float knobX = value ? rect.xMax - knobSize - 2 : rect.xMin + 2;

        EditorGUI.DrawRect(new Rect(knobX, rect.y + 2, knobSize, knobSize), Color.white);

        // Click handling
        if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
        {
            value = !value;
            GUI.changed = true;
            Event.current.Use();
        }

        return value;
    }

}
