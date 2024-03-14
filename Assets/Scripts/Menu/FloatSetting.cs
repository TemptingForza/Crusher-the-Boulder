using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatSetting : Setting<float>
{
    public Slider slider;
    public Text numberDisplay;
    public delegate string UpdateNumber(float newValue);
    public event UpdateNumber getNewNumberText;
    void Start()
    {
        numberDisplay.text = getNewNumberText?.Invoke(slider.value) ?? slider.value.ToString();
        slider.onValueChanged.AddListener(x => {
            numberDisplay.text = getNewNumberText?.Invoke(x) ?? x.ToString();
            RaiseEvent(x);
        });
    }
}
