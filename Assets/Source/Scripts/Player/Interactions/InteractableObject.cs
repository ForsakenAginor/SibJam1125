using UnityEngine;
using Zenject;

public abstract class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private LayerMask _playerLayer;

    private IPlayerInteractor _player;
    private SphereCollider _collider;

    public virtual bool IsInstant => false;

    [Inject]
    public void Construct(IPlayerInteractor player)
    {
        _player = player;

        if (TryGetComponent(out _collider) == false)
        {
            throw new System.Exception();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (CanInteract() == false || enabled == false)
            return;

        if ((_playerLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            _player.SetInteractable(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((_playerLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            _player.RemoveInteractable();
        }
    }

    public virtual void Interact()
    {
        _player.RemoveInteractable();
    }

    public void StopInteract()
    {
        if (CanInteract() == false || enabled == false)
            return;

        var colliders = Physics.OverlapSphere(transform.position + _collider.center, _collider.radius * transform.localScale.x);

        foreach (var collider in colliders)
        {
            if ((_playerLayer.value & (1 << collider.gameObject.layer)) != 0)
            {
                _player.SetInteractable(this);
                return;
            }
        }
    }

    protected virtual bool CanInteract()
    {
        return true;
    }
}
