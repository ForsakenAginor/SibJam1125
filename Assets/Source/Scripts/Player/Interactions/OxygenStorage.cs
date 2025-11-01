using System;
using UnityEngine;

public class OxygenStorage : InteractableObject
{
    private bool _isUsed = false;

    public event Action OxygenRestored;

    public override void Interact()
    {
        base.Interact();
        _isUsed = true;
        OxygenRestored?.Invoke();
    }

    protected override bool CanInteract()
    {
        return _isUsed == false;
    }
}
