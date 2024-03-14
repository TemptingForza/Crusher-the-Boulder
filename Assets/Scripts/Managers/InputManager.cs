using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class InputManager
{
    static Dictionary<string, KeyPair> keys = new Dictionary<string, KeyPair>();

    public static KeyPair GetButton(string button) => keys.TryGetValue(button, out var v) ? v : null;
    public static KeyPair GetOrCreateButton(string button, KeyCode defaultKey) => keys.TryGetValue(button, out var v) && v ? v : keys[button] = KeyPair.Create(defaultKey);
    public static bool RemoveButton(string button)
    {
        if (keys.TryGetValue(button, out var v) && v)
            UnityEngine.Object.DestroyImmediate(v);
        return keys.Remove(button);
    }
    public static IEnumerable<string> GetAllButtons() => keys.Keys;
    public static IDictionary<string,KeyPair> GetAllInputs() => keys;


    public static Dictionary<KeyCode, string> nickname = new Dictionary<KeyCode, string> // Stores a list of nicknames for the KeyCodes
    {
        [KeyCode.Semicolon] = ";",
        [KeyCode.Quote] = "'",
        [KeyCode.Slash] = "/",
        [KeyCode.Backslash] = "\\",
        [KeyCode.Insert] = "Ins",
        [KeyCode.Delete] = "Del",
        [KeyCode.PageDown] = "PgDn",
        [KeyCode.PageUp] = "PgUp",
        [KeyCode.BackQuote] = "`",
        [KeyCode.LeftArrow] = "Left",
        [KeyCode.RightArrow] = "Right",
        [KeyCode.UpArrow] = "Up",
        [KeyCode.DownArrow] = "Down",
        [KeyCode.LeftParen] = "(",
        [KeyCode.LeftBracket] = "[",
        [KeyCode.LeftCurlyBracket] = "{",
        [KeyCode.RightParen] = ")",
        [KeyCode.RightBracket] = "]",
        [KeyCode.RightCurlyBracket] = "}",
        [KeyCode.Mouse0] = "LMB",
        [KeyCode.Mouse1] = "RMB",
        [KeyCode.Mouse2] = "MMB",
        [KeyCode.Mouse3] = "EMB1",
        [KeyCode.Mouse4] = "EMB2",
        [KeyCode.Mouse5] = "EMB3",
        [KeyCode.Mouse6] = "EMB4"
    };
    static InputManager()
    {
        Dictionary<string, string> nameReplace = new Dictionary<string, string> // Used for dynamically generating KeyCode nicknames
        {
            ["Alpha"] = "",
            ["Keypad"] = "Num",
            ["Plus"] = "+",
            ["Minus"] = "-",
            ["Divide"] = "/",
            ["Multiply"] = "*",
            ["Period"] = ".",
            ["Equals"] = "=",
            ["Left"] = "Lf",
            ["Right"] = "Rt",
            ["Windows"] = "Win",
            ["Control"] = "Ctrl"
        };
        foreach (KeyCode k in Enum.GetValues(typeof(KeyCode)))
        {
            if (nickname.ContainsKey(k))
                continue;
            bool flag = false;
            string name = k.ToString();
            foreach (var pair in nameReplace)
                if (name.Contains(pair.Key))
                {
                    name = name.Replace(pair.Key, pair.Value);
                    flag = true;
                }
            if (flag)
                nickname.Add(k, name);
        }
    }
    public static string GetKeyName(KeyCode key)
    {
        if (nickname.TryGetValue(key, out var s))
            return s;
        return key.ToString();
    }
    static bool AnyKey(Predicate<KeyCode> conditiion, out KeyCode key)
    {
        foreach (KeyCode k in Enum.GetValues(typeof(KeyCode)))
            if (conditiion(k))
            {
                key = k;
                return true;
            }
        key = KeyCode.None;
        return false;
    }
    public static bool AnyKeyDown(out KeyCode key) => AnyKey(x => GetKeyDown(x), out key);
    public static bool AnyKey(out KeyCode key) => AnyKey(x => GetKey(x), out key);
    public static bool AnyKeyUp(out KeyCode key) => AnyKey(x => GetKeyUp(x), out key);
    public static bool GetKeyDown(KeyCode key) => Input.GetKeyDown(key);
    public static bool GetKey(KeyCode key) => Input.GetKey(key);
    public static bool GetKeyUp(KeyCode key) => Input.GetKeyUp(key);
}
