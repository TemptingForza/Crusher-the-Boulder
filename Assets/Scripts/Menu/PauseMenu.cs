using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MenuBase
{
    public GameObject characterSelector;
    public GameObject settings;
    void Awake()
    {
        characterSelector.GetComponent<CharacterSelector>().closeButton.onClick.AddListener(() => characterSelector.SetActive(false));
        settings.GetComponent<SettingsMenu>().closeButton.onClick.AddListener(() => settings.SetActive(false));
        AddMenuItem("Resume", HUDManager.Instance.Resume);
        AddMenuItem("Character", () => characterSelector.SetActive(true));
        AddMenuItem("Settings", () => settings.SetActive(true));
        AddMenuItem("Respawn",() => { (PlayerController.Instance as IDamageable)?.Kill(); HUDManager.Instance.Resume(); });
        AddMenuItem("Back To Menu", () => { Time.timeScale = 1; SceneManager.LoadScene(Scenes.MainMenuScene); });
    }
}
