using Assets.Source.Scripts.Utility;
using UnityEngine;
using Zenject;

public class BreathSwitcher : MonoBehaviour
{
    private const float Zero = 0;

    [SerializeField] private AudioSource _full;
    [SerializeField] private AudioSource _mid;
    [SerializeField] private AudioSource _low;

    [SerializeField] private AudioSource _diying;

    [SerializeField] private float _fullLimit = 0.6f;
    [SerializeField] private float _midLimit = 0.25f;
    private AudioSource _active;
    private IResource _oxygen;
    private bool _isWorking = true;

    public void Init(IResource oxigen)
    {
        _oxygen = oxigen;
    }

    private void Start()
    {
        SwitchAudioBasedOnOxygen();
    }

    private void Update()
    {
        if(_isWorking == false)
            return;

        if (_oxygen.Percent > _fullLimit)
        {
            SwitchToAudio(_full);
        }
        else if (_oxygen.Percent > _midLimit)
        {
            SwitchToAudio(_mid);
        }
        else if(_oxygen.Percent > Zero)
        {
            SwitchToAudio(_low);
        }
        else
        {
            _isWorking = false;
            _low.Stop();
            _mid.Stop();
            _full.Stop();
            _diying.Play();
        }
    }

    private void SwitchToAudio(AudioSource newAudioSource)
    {
        if (_active == newAudioSource)
            return;

        if (_active != null)
        {
            _active.Stop();
        }

        _active = newAudioSource;

        if (_active != null)
        {
            _active.Play();
        }
    }

    private void SwitchAudioBasedOnOxygen()
    {
        if (_oxygen.Percent > _fullLimit)
        {
            _active = _full;
        }
        else if (_oxygen.Percent > _midLimit)
        {
            _active = _mid;
        }
        else
        {
            _active = _low;
        }

        if (_active != null)
        {
            _active.Play();
        }
    }
}