public interface IInteractable
{
    public bool IsInstant { get; }

    public void Interact();

    public void StopInteract();
}
