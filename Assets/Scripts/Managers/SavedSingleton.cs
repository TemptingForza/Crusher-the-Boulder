using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Json;

public abstract class SavedSingleton<T> : Singleton<T> where T : Component
{
    public string fileName;
    protected abstract void ReadData(JsonObject savedData);
    protected abstract JsonObject WriteData();

    protected virtual void Start()
    {
        try
        {
            if (File.Exists(fileName))
                ReadDataFromFile();
            else
                WriteDataToFile();
        }
        catch { }
    }

    public void ReadDataFromFile()
    {
        ReadData(JsonValue.Parse(File.ReadAllText(fileName)) as JsonObject);
    }

    public void WriteDataToFile()
    {
        File.WriteAllText(fileName, WriteData().ToString());
    }
}
