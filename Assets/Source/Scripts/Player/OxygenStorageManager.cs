using UnityEngine;
using Zenject;

public class OxygenStorageManager : MonoBehaviour
{
    [SerializeField] private OxygenStorage[] _storages;

    private PlayerOxygenManager _oxygenManger;

    [Inject]
    public void Construct(PlayerFacade player)
    {
        _oxygenManger = player.Oxygen;

        foreach (var storage in _storages)
        {
            storage.OxygenRestored += OnOxygenRestored;
        }
    }

    private void OnDestroy()
    {
        foreach (var storage in _storages)
        {
            storage.OxygenRestored -= OnOxygenRestored;
        }
    }

    private void OnOxygenRestored()
    {
        _oxygenManger.RestoreOxygen();
    }
}
