using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitParent : MonoBehaviour
{
    public int orbitCount;
    public float maxSpeed = 360;
    public float minSpeed = 90;
    public int layers = 1;
    List<Transform> children = new List<Transform>();
    List<Vector3> speeds = new List<Vector3>();

    void Awake()
    {
        children.Add(transform.GetChild(0));
        var rand = new System.Random(orbitCount * layers);
        for (int i = 0; i < orbitCount; i++)
        {
            if (i > 0)
                children.Add(Instantiate(children[0], transform));
            var layer = i % layers;
            var index = i / layers;
            speeds.Add(Quaternion.LookRotation( new Vector3((float)(rand.NextDouble() * 2 - 1), (float)(rand.NextDouble() * 2 - 1), (float)(rand.NextDouble() * 2 - 1))).eulerAngles.normalized * ((maxSpeed - minSpeed) * layer / layers + minSpeed));
            children[i].rotation = Quaternion.Euler((float)(rand.NextDouble() * 360), index / ((float)orbitCount / layers) * 360, (float)(rand.NextDouble() * 360));
        }
    }

    void Update()
    {
        for (var i = 0; i < children.Count; i++)
            children[i].localRotation *= Quaternion.Euler(speeds[i] * Time.deltaTime);
    }
}
