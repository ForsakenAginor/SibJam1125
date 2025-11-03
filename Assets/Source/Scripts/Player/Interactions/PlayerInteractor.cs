using Assets.Source.Scripts.Utility;
using UnityEngine;
using Zenject;

public class PlayerInteractor : MonoBehaviour, IPlayerInteractor
{
    [SerializeField] private SwitchableElement _tooltip;
    [SerializeField] private FillableImage _interactButton;

    private IInteractable _interactable;
    private bool _canInteract = false;
    private IPlayerInput _inputProvider;

    [Inject]
    public void Construct(IPlayerInput inputProvider)
    {
        _inputProvider = inputProvider;

        _interactButton.FillComplete += OnInteract;
    }

    private void OnDestroy()
    {
        _interactButton.FillComplete -= OnInteract;
        UnsubscribeFromInputEvents();
    }

    public void SetInteractable(IInteractable interactable)
    {
        if (_interactable != null)
            return;

        UnsubscribeFromInputEvents();
        _canInteract = true;
        _interactable = interactable;
        _tooltip.Enable();

        if (interactable.IsInstant == false)
        {
            _inputProvider.OnInteractStart += OnInteractStarted;
            _inputProvider.OnInteractCancel += OnInteractCanceled;
        }
        else
        {
            _inputProvider.OnInteractStart += OnInteract;
        }

    }

    public void RemoveInteractable()
    {
        _canInteract = false;
        _interactable = null;
        UnsubscribeFromInputEvents();
        OnInteractCanceled();
        _tooltip.Disable();
    }

    private void UnsubscribeFromInputEvents()
    {
        _inputProvider.OnInteractStart -= OnInteractStarted;
        _inputProvider.OnInteractCancel -= OnInteractCanceled;
        _inputProvider.OnInteractStart -= OnInteract;
    }

    private void OnInteract()
    {
        UnsubscribeFromInputEvents();
        _interactable.Interact();
    }

    private void OnInteractCanceled()
    {
        if (_canInteract == false)
            return;

        _interactButton.Unfill();
    }

    private void OnInteractStarted()
    {
        if (_canInteract == false)
            return;

        _interactButton.Fill();
    }
}
