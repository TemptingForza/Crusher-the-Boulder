using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Json;

public class Settings : SavedSingleton<Settings>
{
    public bool autoPowerup = true;
    public bool autoJump = true;
    public bool toggleSprint = false;
    public bool toggleBlade = true;
    public float mouseSensitivity = 2;
    public bool invertMouseX = false;
    public bool invertMouseY = false;
    public int FOV = 50;
    [SerializeField]
    KeyPair[] keyPairs;

    protected void Awake()
    {
        fileName = Path.Combine(Application.persistentDataPath, "settings.json");
        var inputs = InputManager.GetAllInputs();
        foreach (var key in keyPairs)
            inputs[key.name] = key;
    }

    protected override void ReadData(JsonObject savedData)
    {
        savedData.TryGetValue("autoPowerup", ref autoPowerup);
        savedData.TryGetValue("autoJump", ref autoJump);
        savedData.TryGetValue("toggleSprint", ref toggleSprint);
        savedData.TryGetValue("toggleBlade", ref toggleBlade);
        savedData.TryGetValue("mouseSensitivity", ref mouseSensitivity);
        savedData.TryGetValue("invertMouseX", ref invertMouseX);
        savedData.TryGetValue("invertMouseY", ref invertMouseY);
        savedData.TryGetValue("FOV", ref FOV);
        if (savedData["inputs"] is JsonObject inputs)
            foreach (var item in inputs)
                if (item.Value is JsonObject keyData)
                {
                    var keyPair = InputManager.GetOrCreateButton(item.Key,KeyCode.None);
                    keyData.TryGetValue("main", ref keyPair.Main);
                    keyData.TryGetValue("alt", ref keyPair.Alt);
                }
    }
    protected override JsonObject WriteData()
    {
        var data = new JsonObject();
        data["autoPowerup"] = autoPowerup;
        data["autoJump"] = autoJump;
        data["toggleSprint"] = toggleSprint;
        data["toggleBlade"] = toggleBlade;
        data["mouseSensitivity"] = mouseSensitivity;
        data["invertMouseX"] = invertMouseX;
        data["invertMouseY"] = invertMouseY;
        data["FOV"] = FOV;
        var inputs = data.CreateChild("inputs");
        foreach (var key in InputManager.GetAllInputs())
        {
            var keyData = inputs.CreateChild(key.Key);
            keyData["main"] = (int)key.Value.Main;
            keyData["alt"] = (int)key.Value.Alt;
        }
        return data;
    }
}