using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    public Button selectPrev;
    public Button selectNext;
    public Text skinName;
    public Image skinDisplay;
    List<Appearance> skins;
    int selected;
    public Button closeButton;
    void Awake()
    {
        selectNext.onClick.AddListener(SelectNext);
        selectPrev.onClick.AddListener(SelectPrev);
    }
    void OnEnable()
    {
        AudioManager.Instance.PlayOneShot("click");
        skins = new List<Appearance>();
        foreach (var appearance in AppearanceManager.Instance.Appearances.Values)
            if (appearance.Unlocked)
            {
                if (appearance == AppearanceManager.Instance.SelectedAppearance)
                    selected = skins.Count;
                skins.Add(appearance);
            }
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
        AppearanceManager.Instance.SelectedAppearanceName = skins[selected].name;
        ProgressManager.Instance.WriteDataToFile();
    }
    void SelectPrev()
    {
        selected--;
        UpdateSelection();
        AppearanceManager.Instance.SelectedAppearanceName = skins[selected].name;
        ProgressManager.Instance.WriteDataToFile();
    }
    void UpdateSelection()
    {
        skinName.text = skins[selected].Name;
        skinDisplay.sprite = skins[selected].Image;
        selectPrev.gameObject.SetActive(selected != 0);
        selectNext.gameObject.SetActive(selected != skins.Count - 1);
    }
}
