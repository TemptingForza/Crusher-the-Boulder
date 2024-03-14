using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MainMenu : MenuBase
{
    public GameObject levelSelector;
    public GameObject characterSelector;
    public GameObject settings;
    public GameObject credits;
    public Button creditsClose;
    void Start()
    {
        AddMenuItem("Play", () => levelSelector.SetActive(true));
        AddMenuItem("Character", () => characterSelector.SetActive(true));
        AddMenuItem("Settings", () => settings.SetActive(true));
        AddMenuItem("Credits", () => credits.SetActive(true));
        AddMenuItem("Exit", () => Application.Quit(0));
        levelSelector.GetComponent<LevelSelector>().closeButton.onClick.AddListener(() => levelSelector.SetActive(false));
        characterSelector.GetComponent<CharacterSelector>().closeButton.onClick.AddListener(() => characterSelector.SetActive(false));
        settings.GetComponent<SettingsMenu>().closeButton.onClick.AddListener(() => settings.SetActive(false));
        creditsClose.onClick.AddListener(() => credits.SetActive(false));
    }
}
