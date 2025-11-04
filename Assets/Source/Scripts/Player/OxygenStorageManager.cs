using UnityEngine;
using Zenject;

public class OxygenStorageManager : MonoBehaviour
{
    [SerializeField] private OxygenStorage[] _storages;

    private PlayerOxygenManager _oxygenManger;

    [Inject]
    public void Construct(PlayerOxygenManager player)
    {
        _oxygenManger = player;

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
