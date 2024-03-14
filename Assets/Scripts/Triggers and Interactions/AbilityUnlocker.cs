using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUnlocker : MonoBehaviour, IInteractable
{
    public Transform containerTop;
    public Transform containerBottom;
    public Transform core;
    public PlayerAbility unlock;

    bool unlocked => ProgressManager.Instance.IsAbilityUnlocked(unlock.Id);
    bool waitingToInteract = false;

    bool IInteractable.CanInteract() => !unlocked;
    void IInteractable.StateChanged(bool readyToInteract) => waitingToInteract = readyToInteract;
    void IInteractable.OnInteract() 
    {
        ProgressManager.Instance.UnlockAbility(unlock.Id);
        HUDManager.Instance.ShowInformation(unlock,priority: true);
    }

    void Awake()
    {
        if (unlocked)
            core.gameObject.SetActive(false);
    }

    bool unlock1 = false;
    bool unlock2 = false;
    bool unlock3 => !core.gameObject.activeSelf;
    Quaternion containerRotation;
    void Update()
    {
        if (unlocked)
        {
            if (!unlock1)
                containerTop.localPosition = containerTop.localPosition.MoveTowards(Vector3.up * 0.4f, Time.deltaTime, out unlock1);
            else if (!unlock2)
            {
                if (PlayerController.Instance)
                {
                    var target = Quaternion.LookRotation(Quaternion.LookRotation(core.position - PlayerController.Instance.transform.position) * Vector3.down);
                    core.localRotation = Quaternion.RotateTowards(core.localRotation, target, 360 * Time.deltaTime);
                    if (Quaternion.Angle(core.localRotation, target) < 5)
                        unlock2 = true;
                }
            } else if (!unlock3)
            {
                if (PlayerController.Instance)
                {
                    var targetRot = Quaternion.LookRotation(Quaternion.LookRotation(core.position - PlayerController.Instance.transform.position) * Vector3.down);
                    if (Quaternion.Angle(core.localRotation, targetRot) * 0.05f < 360 * Time.deltaTime)
                        core.localRotation = Quaternion.RotateTowards(core.localRotation, targetRot, 360 * Time.deltaTime);
                    else
                        core.localRotation = Quaternion.Lerp(core.localRotation, targetRot, 0.05f);
                    var posDiff = (core.position - PlayerController.Instance.transform.position).magnitude;
                    bool ended = false;
                    if (posDiff * 0.05f < Time.deltaTime)
                        core.position = core.position.MoveTowards(PlayerController.Instance.transform.position, Time.deltaTime, out ended);
                    else
                        core.position = Vector3.Lerp(core.position, PlayerController.Instance.transform.position, 0.05f);
                    if (ended || (core.position - PlayerController.Instance.transform.position).magnitude < 0.5f)
                        core.gameObject.SetActive(false);
                }
            }
        } else if (waitingToInteract)
        {
            core.localEulerAngles += Vector3.up * 90 * Time.deltaTime;
            containerTop.localPosition = Vector3.MoveTowards(containerTop.localPosition, Vector3.up * 0.2f, Time.deltaTime);
            containerRotation = Quaternion.Euler(containerRotation.eulerAngles + Vector3.up * 45 * Time.deltaTime);
        }
        else
        {
            containerTop.localPosition = Vector3.MoveTowards(containerTop.localPosition, Vector3.zero, Time.deltaTime);
            containerRotation = Quaternion.Euler(containerRotation.eulerAngles + Vector3.up * 90 * Time.deltaTime);
        }
        containerBottom.localPosition = -containerTop.localPosition;
        var r = Vector3.up * containerTop.localPosition.y * 450;
        containerTop.localEulerAngles = containerRotation.eulerAngles + r;
        containerBottom.localEulerAngles = containerRotation.eulerAngles - r;
    }
}
