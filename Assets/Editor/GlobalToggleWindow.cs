using UnityEngine;
using UnityEditor;

public class GlobalToggleWindow : EditorWindow {
    private static bool globalToggle;

    [MenuItem("Tools/Global toggle")]
    private static void ShowWindow() {
        var window = GetWindow<GlobalToggleWindow>();
        window.titleContent = new GUIContent("Apocalipse toggle");
        window.position = new Rect(100, 100, 200, 30);
        window.Show();
    }

    private void OnGUI() {
        GUILayout.Label("Future Toggle", EditorStyles.boldLabel);

        Rect r = GUILayoutUtility.GetRect(20, 20);
        bool newValue = EditorToggleSwitch.Draw(r, globalToggle);

        if (newValue != globalToggle)
        {
            globalToggle = newValue;
            CallOnAllItems(globalToggle);
        }

    }
    private void CallOnAllItems(bool value)
    {
        ToggleTimeScript[] items = FindObjectsOfType<ToggleTimeScript>();

        foreach (var item in items)
        {
            item.OnGlobalToggleChanged(value);
        }

        Debug.Log($"Called OnGlobalToggleChanged({value}) on {items.Length} items");
    }

}