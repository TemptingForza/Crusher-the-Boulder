using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public LayoutGroup itemParent;
    public BoolSetting boolItemPrefab;
    public FloatSetting floatItemPrefab;
    public KeyPairSetting keyPairItemPrefab;
    public Button closeButton;
    void Start()
    {
        Setup(Settings.Instance.autoPowerup, x => Settings.Instance.autoPowerup = x, "Auto Collect Powerup");
        Setup(Settings.Instance.autoJump, x => Settings.Instance.autoJump = x, "Auto Jump");
        Setup(Settings.Instance.toggleSprint, x => Settings.Instance.toggleSprint = x, "Toggle Sprint");
        Setup(Settings.Instance.toggleBlade, x => Settings.Instance.toggleBlade = x, "Toggle Blade Form");
        Setup(Settings.Instance.mouseSensitivity,0.5f,10, x => Settings.Instance.mouseSensitivity = x, "Mouse Sensitivity").getNewNumberText += x => (Mathf.Round(x * 10) / 10).ToString();
        Setup(Settings.Instance.invertMouseX, x => Settings.Instance.invertMouseX = x, "Invert Mouse X");
        Setup(Settings.Instance.invertMouseY, x => Settings.Instance.invertMouseY = x, "Invert Mouse Y");
        Setup(Settings.Instance.FOV, 20, 120, x => Settings.Instance.FOV = x, "FOV");
        foreach (var k in InputManager.GetAllInputs().Values)
            Setup(k, (x, y) =>
                {
                    if (x)
                        k.Main = y;
                    else
                        k.Alt = y;
                }, k.Name);
    }
    void OnDisable()
    {
        Settings.Instance.WriteDataToFile();
    }
    public void RebuildSettings()
    {
        foreach (Transform child in itemParent.transform)
            Destroy(child.gameObject);
        Start();
    }
    BoolSetting Setup(bool value, Action<bool> setValue, string text)
    {
        var b = Instantiate(boolItemPrefab, itemParent.transform);
        b.text.text = text;
        b.checkbox.isOn = value;
        b.onValueChange += x => setValue(x);
        return b;
    }
    FloatSetting Setup(float value, float minValue, float maxValue, Action<float> setValue, string text)
    {
        var f = Instantiate(floatItemPrefab, itemParent.transform);
        f.text.text = text;
        f.slider.minValue = minValue;
        f.slider.maxValue = maxValue;
        f.slider.value = value;
        f.onValueChange += x => setValue(x);
        return f;
    }
    FloatSetting Setup(int value, int minValue, int maxValue, Action<int> setValue, string text)
    {
        var f = Instantiate(floatItemPrefab, itemParent.transform);
        f.text.text = text;
        f.slider.wholeNumbers = true;
        f.slider.minValue = minValue;
        f.slider.maxValue = maxValue;
        f.slider.value = value;
        f.onValueChange += x => setValue(Mathf.RoundToInt(x));
        return f;
    }
    KeyPairSetting Setup(KeyPair value, Action<bool,KeyCode> setValue, string text)
    {
        var k = Instantiate(keyPairItemPrefab, itemParent.transform);
        k.text.text = text;
        k.mainInput.text.text = InputManager.GetKeyName(value.Main);
        k.altInput.text.text = InputManager.GetKeyName(value.Alt);
        k.onValueChange += x => setValue(x.isMain,x.newValue);
        return k;
    }
}
