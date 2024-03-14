using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapLine : MonoBehaviour
{
    public LineRenderer target;
    public float MinRadius = 1;
    public float MaxRadius = 1.5f;
    public float MaxStepAngle = 5;
    public float RotationSpeed = 90;
    public float MaxLength = 360;
    Vector3 NextTarget;
    List<Vector3> points = new List<Vector3>();
    Vector3 RandomPoint => Quaternion.Euler(Tools.GetRandomEular()) * Vector3.forward * Random.Range(MinRadius, MaxRadius);
    void Awake()
    {
        points.Add(RandomPoint);
        NextTarget = RandomPoint;
    }
    void Update()
    {
        var dist = RotationSpeed * Time.deltaTime;
        while (dist > 0.001f)
        {
            var angle = Mathf.Min(points.Count > 1 ? MaxStepAngle - Vector3.Angle(points[points.Count - 1], points[points.Count - 2]) : 0, dist);
            var targetAngle = Vector3.Angle(points[points.Count - 1], NextTarget);
            var reached = false;
            if (angle > targetAngle)
            {
                angle = targetAngle;
                reached = true;
            }
            if (angle <= 0.001f)
                points.Add(points[points.Count - 1]);
            else
            {
                
                points[points.Count - 1] = Vector3.RotateTowards(points[points.Count - 1], NextTarget, angle * Mathf.Deg2Rad, float.PositiveInfinity).normalized * Mathf.Lerp(points[points.Count - 1].magnitude, NextTarget.magnitude, angle / targetAngle);
                dist -= angle;
            }
            if (reached)
                NextTarget = RandomPoint;
        }
        var remove = -MaxLength;
        for (int i = 0; i < points.Count - 1; i++)
            remove += Vector3.Angle(points[i], points[i + 1]);
        while (remove > 0.001f)
        {
            var angle = Vector3.Angle(points[0], points[1]);
            if (angle > remove)
            {
                points[0] = Quaternion.RotateTowards(Quaternion.LookRotation(points[0]), Quaternion.LookRotation(points[1]), remove) * Vector3.forward * Mathf.Lerp(points[0].magnitude, points[1].magnitude, remove / angle);
                remove = 0;
            }
            else
            {
                points.RemoveAt(0);
                remove -= angle;
            }
        }
        target.positionCount = points.Count;
        target.SetPositions(points.ToArray());
    }
}
