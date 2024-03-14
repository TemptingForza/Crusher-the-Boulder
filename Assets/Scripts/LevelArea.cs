using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelArea : MonoBehaviour
{
    LevelSubArea[] subAreas;
    public GameObject activeObject;
    public GameObject inactiveObject;
    bool wasActive;
    void Awake()
    {
        subAreas = GetComponentsInChildren<LevelSubArea>(true);
    }
    void Update()
    {
        var current = subAreas.Any(x => x.ContainsPlayer);
        if (wasActive != current)
        {
            wasActive = current;
            activeObject.SetActive(current);
            inactiveObject.SetActive(!current);
        }
    }
}
