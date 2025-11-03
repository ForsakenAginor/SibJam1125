using System;
using UnityEngine;

public class OxygenStorage : InteractableObject
{
    public event Action OxygenRestored;

    public override void Interact()
    {
        base.Interact();
        OxygenRestored?.Invoke();
    }
}
