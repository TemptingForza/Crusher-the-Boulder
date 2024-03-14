using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerInteractionHandler : MonoBehaviour, IPlayerDeathReciever
{
    public KeyPair inputInteract;
    protected void Awake()
    {
        inputInteract = InputManager.GetOrCreateButton("interact", KeyCode.E);
    }
    List<IInteractable> interactions = new List<IInteractable>();
    void OnTriggerEnter(Collider other)
    {
        var interactables = other.GetComponents<IInteractable>();
        interactions.AddRange(interactables);
    }
    void OnTriggerExit(Collider other)
    {
        var interactables = other.GetComponents<IInteractable>();
        interactions.RemoveAll(x => interactables.Contains(x));
    }
    public IInteractable lastSelected;
    void Update()
    {
        IInteractable current = null;
        for (int i = interactions.Count - 1; i >= 0; i--)
            if (interactions[i] == null)
                interactions.RemoveAt(i);
            else if (interactions[i].CanInteract())
            {
                current = interactions[i];
                break;
            }
        if (lastSelected != current)
        {
            if (lastSelected != null)
                lastSelected.StateChanged(false);
            if (current != null)
                current.StateChanged(true);
            lastSelected = current;
        }
        if (current != null && inputInteract.JustPressed)
            current.OnInteract();
    }

    void IPlayerDeathReciever.OnDeath()
    {
        if (lastSelected != null)
            lastSelected.StateChanged(false);
    }
}

public interface IInteractable
{
    bool CanInteract();
    void StateChanged(bool readyToInteract);
    void OnInteract();
}
