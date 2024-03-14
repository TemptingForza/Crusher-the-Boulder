using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPlayer : Singleton<MainMenuPlayer>
{
    public Vector3 travelExtents;
    Vector3 last;
    Vector3 target;
    float time;

    protected void Awake()
    {
        transform.localPosition = new Vector3(0,0,travelExtents.z);
        PickTarget();
    }

    void PickTarget()
    {
        last = transform.localPosition;
        time = 0;
        var matching = new HashSet<int>();
        var a1 = last.ToArray();
        var a2 = travelExtents.ToArray();
        for (int i = 0; i < a1.Length; i++)
            if (Mathf.Abs(a1[i]) == a2[i])
                matching.Add(i);
        var axisKeep = matching.Count > 0 ? Random.Range(0, matching.Count) : -1;
        var axisLock = Random.Range(0, a1.Length - matching.Count);
        for (int i = 0; i < a1.Length; i++)
        {
            if (matching.Contains(i))
            {
                if (i != -1 && axisKeep != 0)
                    a1[i] = Random.Range(-a2[i], a2[i]);
                axisKeep--;
            } else
            {
                if (axisLock == 0)
                    a1[i] = a2[i];
                else
                    a1[i] = Random.Range(-a2[i], a2[i]);
                axisLock--;
            }
        }
        target = a1.ToVector3();
    }

    void Update()
    {
        time += Time.deltaTime * 2;
        transform.localPosition = last.MoveTowards(target, time, out bool ended);
        if (ended)
            PickTarget();
    }
}
