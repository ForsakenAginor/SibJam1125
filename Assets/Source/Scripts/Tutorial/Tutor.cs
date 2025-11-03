using UnityEngine;

public class Tutor : MonoBehaviour
{
    [SerializeField] private OxygenDrainTrigger _oxygenDrainTrigger;
    private PlayerOxygenManager _oxygenManager;

    private bool _isOxyDraining = false;

    private void OnDestroy()
    {
        _oxygenManager.OxygenRestored -= OnOxygenRestored;
        _oxygenDrainTrigger.PlayerEnter -= OnOxygenRestored;
    }

    public void Init(PlayerOxygenManager oxygenManager)
    {
        _oxygenManager = oxygenManager;

        _oxygenManager.OxygenRestored += OnOxygenRestored;
        _oxygenDrainTrigger.PlayerEnter += OnOxygenRestored;
    }

    private void OnOxygenRestored()
    {
        if (_isOxyDraining == false)
        {
            _isOxyDraining = true;
            _oxygenManager.StartOxygenDrain();
        }
    }
}
