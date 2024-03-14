using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCoreSpawn : PlayerSpawn
{
    public Transform top;
    public Transform bottom;
    public Transform core;
    bool canInteract = false;
    List<(Vector3 position, Vector3 rotation)> topAnimationQueue = new List<(Vector3, Vector3)>();
    List<(Vector3 position, Vector3 rotation)> bottomAnimationQueue = new List<(Vector3, Vector3)>();
    List<(Vector3 position, Vector3 rotation)> coreAnimationQueue = new List<(Vector3, Vector3)>();
    AnimationState lastTarget;
    public override void StateChanged(bool readyToInteract)
    {
        base.StateChanged(readyToInteract);
        canInteract = readyToInteract;
    }
    protected virtual void Update()
    {
        var target = IsActive ? AnimationState.Active : canInteract ? AnimationState.Ready : AnimationState.Idle;
        if (lastTarget != target)
        {
            topAnimationQueue.Clear();
            bottomAnimationQueue.Clear();
            coreAnimationQueue.Clear();
            if (lastTarget == AnimationState.Active)
            {
                topAnimationQueue.Add((new Vector3(0, top.localPosition.y, 0), default));
                bottomAnimationQueue.Add((new Vector3(0, bottom.localPosition.y, 0), default));
            }
            if (target == AnimationState.Active)
            {
                topAnimationQueue.Add((new Vector3(0, 0.6f, 0), top.localEulerAngles));
                bottomAnimationQueue.Add((new Vector3(0, -0.6f, 0), bottom.localEulerAngles));
            }
            lastTarget = target;
        }
        if (topAnimationQueue.Count == 0)
        {
            switch (target)
            {
                case AnimationState.Idle:
                    topAnimationQueue.Add((default, default));
                    break;
                case AnimationState.Ready:
                    topAnimationQueue.Add((new Vector3(Random.Range(-0.05f, 0.05f), 0.25f + Random.Range(-0.1f, 0.1f), Random.Range(-0.05f, 0.05f)), default));
                    break;
                case AnimationState.Active:
                    topAnimationQueue.Add((new Vector3(0, 0.6f, 0), top.localEulerAngles + new Vector3(0, 135, 0)));
                    break;
            }
        }
        if (bottomAnimationQueue.Count == 0)
        {
            switch (target)
            {
                case AnimationState.Idle:
                    bottomAnimationQueue.Add((default, default));
                    break;
                case AnimationState.Ready:
                    bottomAnimationQueue.Add((new Vector3(Random.Range(-0.05f, 0.05f), -0.25f + Random.Range(-0.1f, 0.1f), Random.Range(-0.05f, 0.05f)), default));
                    break;
                case AnimationState.Active:
                    bottomAnimationQueue.Add((new Vector3(0, -0.6f, 0), bottom.localEulerAngles + new Vector3(0,-135,0)));
                    break;
            }
        }
        if (coreAnimationQueue.Count == 0)
        {
            switch (target)
            {
                case AnimationState.Idle:
                    coreAnimationQueue.Add((default, default));
                    break;
                case AnimationState.Ready:
                    coreAnimationQueue.Add((default, default));
                    break;
                case AnimationState.Active:
                    coreAnimationQueue.Add((core.localPosition.y < 0 ? new Vector3(0, 0.5f, 0) : new Vector3(0, -0.5f, 0), default));
                    break;
            }
        }
        //Debug.Log($"Queues: {topAnimationQueue.Count} {bottomAnimationQueue.Count} {coreAnimationQueue.Count}");
        var t = Time.deltaTime;
        void ProgressQueue(Transform obj, List<(Vector3 position, Vector3 rotation)> queue)
        {
            if (queue.Count > 0)
            {
                var p = queue[0].position;
                var r = Quaternion.Euler(queue[0].rotation);
                //Debug.Log($"{obj.name} at {obj.localPosition} and {obj.localRotation} moving to {p} and rotating to {r}");
                obj.localPosition = Vector3.MoveTowards(obj.localPosition, p, 2 * t);
                obj.localRotation = Quaternion.RotateTowards(obj.localRotation, r, 80 * t);
                if (obj.localPosition == p && obj.localRotation == r)
                {
                    //Debug.Log("removing from queue");
                    queue.RemoveAt(0);
                }
            }
        }
        ProgressQueue(top, topAnimationQueue);
        ProgressQueue(bottom, bottomAnimationQueue);
        ProgressQueue(core, coreAnimationQueue);
    }

    enum AnimationState
    {
        Idle,
        Ready,
        Active
    }
}
