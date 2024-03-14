using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public Button selectPrev;
    public Button selectNext;
    public Text levelName;
    public Image levelImg;
    public Button playLevel;
    List<LevelSettings> levels;
    int selected;
    public Button closeButton;
    void Awake()
    {
        selectNext.onClick.AddListener(SelectNext);
        selectPrev.onClick.AddListener(SelectPrev);
        playLevel.onClick.AddListener(Play);
    }
    void OnEnable()
    {
        AudioManager.Instance.PlayOneShot("click");
        levels = new List<LevelSettings>();
        foreach (var level in ProgressManager.Instance.Levels.Values)
            if (level.Unlocked)
                levels.Add(level);
        levels.Sort();
        selected = 0;
        UpdateSelection();
    }
    void OnDisable()
    {
        AudioManager.Instance.PlayOneShot("click");
    }
    void SelectNext()
    {
        selected++;
        UpdateSelection();
    }
    void SelectPrev()
    {
        selected--;
        UpdateSelection();
    }
    void Play()
    {
        LevelManager.CurrentLevel = levels[selected];
        SceneManager.LoadScene("InGame");
    }
    void UpdateSelection()
    {
        levelName.text = levels[selected].Name;
        levelImg.sprite = levels[selected].Image;
        selectPrev.gameObject.SetActive(selected != 0);
        selectNext.gameObject.SetActive(selected != levels.Count - 1);
    }
}
