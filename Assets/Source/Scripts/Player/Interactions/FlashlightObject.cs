using Assets.Source.Scripts.Utility;
using System;
using UnityEngine;

public class FlashlightObject : InteractableObject
{
    [SerializeField] private SwitchableElement _view;

    public event Action Pickuped;
    private bool _isEnabled = true;

    public override void Interact()
    {
        base.Interact();
        _view.Disable();
        _isEnabled = false;
        Pickuped?.Invoke();
    }

    protected override bool CanInteract()
    {
        return _isEnabled;
    }
}
