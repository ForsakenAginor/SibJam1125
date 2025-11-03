using Assets.Source.Scripts.Utility;
using System;
using UnityEngine;

public class FlashlightObject : InteractableObject
{
    [SerializeField] private SwitchableElement _view;

    private bool _isEnabled = true;

    public event Action Pickuped;

    public override bool IsInstant => true;

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
