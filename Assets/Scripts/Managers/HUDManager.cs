using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HUDManager : Singleton<HUDManager>
{
    public RectTransform HealthBar;
    public PowerUpUI PowerUp;
    public PowerUpUI PowerUpWaiting;
    public GameObject pauseMenu;
    public KeyPair inputPause;
    public RectTransform InfoCard;
    public Text InfoText;
    bool infoClosing = false;
    float infoTime = 0;
    public delegate string GetObjectInformation<T>(T obj);
    public static event GetObjectInformation<PlayerAbility> getAbilityInformation;
    public delegate bool CloseInformationCondition(float timePassed);
    void Update()
    {
        HealthBar.anchorMin = new Vector2(PlayerController.Instance ? 1 - PlayerController.Instance.Health / PlayerController.Instance.MaxHealth : 1, HealthBar.anchorMin.y);
        PowerUp.Update(PlayerController.Instance?.heldPowerup);
        PowerUpWaiting.Update(PowerUpSpawner.ready?.powerUp);
        if (infoClosing)
        {
            InfoCard.pivot = InfoCard.pivot.MoveTowards(new Vector2(0, InfoCard.pivot.y), 2f * Time.deltaTime, out var ended);
            if (ended)
            {
                if (messages.Count > 0)
                    InfoText.text = messages[0].message;
                infoClosing = false;
            }
        }
        else if (messages.Count > 0)
        {
            infoTime += Time.deltaTime;
            var m = messages[0];
            if (m.condition(infoTime))
            {
                infoTime = 0;
                infoClosing = true;
                messages.RemoveAt(0);
            }
            else
            {
                InfoText.text = m.message;
                InfoCard.pivot = Vector2.MoveTowards(InfoCard.pivot, new Vector2(1, InfoCard.pivot.y), 2f * Time.deltaTime);
            }
        }
        if (inputPause.JustPressed)
            Pause();
    }
    public void Pause()
    {
        Time.timeScale = 0;
        (PlayerCamera.Instance as PlayerCamera).TakeMouse = false;
        pauseMenu.SetActive(true);
        IEnumerator CheckForUnpause() {
            {
                yield return null;
                while (pauseMenu?.activeSelf ?? false)
                {
                    if (inputPause.JustPressed && !pauseMenu.GetComponentInChildren<PauseMenu>().settings.activeSelf)
                        Resume();
                    yield return null;
                }
            }
        }
        StartCoroutine(CheckForUnpause());
    }
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        (PlayerCamera.Instance as PlayerCamera).TakeMouse = true;
    }

    string GetInformation<T>(GetObjectInformation<T> resolver, T obj)
    {
        if (resolver != null)
            foreach (var d in resolver.GetInvocationList())
            {
                var r = d?.DynamicInvoke(obj) as string;
                if (r != null)
                    return r;
            }
        return null;
    }
    public void ShowInformation(PlayerAbility obj, float time = 3, bool priority = false) => ShowInformation(obj, CloseAfterTime(time));
    public void ShowInformation(string message, float time = 3, bool priority = false) => ShowInformation(message, CloseAfterTime(time));
    static CloseInformationCondition CloseAfterTime(float time) => x => x >= time;

    public void ShowInformation(PlayerAbility obj, CloseInformationCondition close, bool priority = false) => ShowInformation(GetInformation(getAbilityInformation, obj), close);
    List<(string message, CloseInformationCondition condition)> messages = new List<(string,CloseInformationCondition)>();
    public void ShowInformation(string message, CloseInformationCondition close, bool priority = false)
    {
        if (priority)
        {
            infoTime = 0;
            messages.Insert(0, (message, close));
        }
        else
            messages.Add((message, close));
    }
    public void ClearInformation()
    {
        if (messages.Count > 0)
        {
            infoClosing = true;
            messages.Clear();
        }
    }

}

[Serializable]
public class PowerUpUI
{
    public RectTransform Back;
    public Image Image;
    public Text Text;
    public void Update(PowerUp powerUp)
    {
        if (powerUp)
        {
            Back.offsetMin = Vector2.MoveTowards(Back.offsetMin, new Vector2(Back.offsetMin.x, Back.offsetMin.x - Back.offsetMax.x ), 1000 * Time.deltaTime);
            Image.sprite = powerUp.Image;
            Text.text = powerUp.Name;
        }
        else
            Back.offsetMin = Vector2.MoveTowards(Back.offsetMin, new Vector2(Back.offsetMin.x, 0), 1000 * Time.deltaTime);
    }
}