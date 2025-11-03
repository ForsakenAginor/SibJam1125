using Assets.Source.Scripts.DI.Services.Global;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class VoiceMessagePlayer : SerializedMonoBehaviour
{
    [ShowInInspector, OdinSerialize] private Dictionary<VoiceMessage, AudioClip> _clips;
    [SerializeField] private AudioSource _audioSource;

    [Header("Triggers")]
    [SerializeField] private StopVoiceTrigger _stopVoiceTrigger;
    [SerializeField] private MiddleVoiceTrigger _middleVoiceTrigger;
    [SerializeField] private FinishVoiceTrigger _finishTrigger;

    private ICoroutineRunner _coroutineRunner;
    private CoroutineQueue _queue;

    [Inject]
    public void Construct(ICoroutineRunner coroutineRunner)
    {
        _coroutineRunner = coroutineRunner;
        _queue = _coroutineRunner.StartCorotineQueue();
        _queue.StartLoop();

        _stopVoiceTrigger.PlayerEnter += OnStop;
        _middleVoiceTrigger.PlayerEnter += OnMiddle;
        _finishTrigger.PlayerEnter += OnFinish;
    }

    private void OnDestroy()
    {
        _queue.StopLoop();
        _stopVoiceTrigger.PlayerEnter -= OnStop;
        _middleVoiceTrigger.PlayerEnter -= OnMiddle;
        _finishTrigger.PlayerEnter -= OnFinish;
    }

    public void Play(VoiceMessage message)
    {
        _queue.EnqueueCoroutine(PlayVoiceMessage(message));
    }

    private void OnStop()
    {
        Play(VoiceMessage.StopMove);
        _stopVoiceTrigger.PlayerEnter -= OnStop;

    }

    private void OnMiddle()
    {
        Play(VoiceMessage.KeepMove);
        _middleVoiceTrigger.PlayerEnter -= OnMiddle;
    }

    private void OnFinish()
    {
        Play(VoiceMessage.SoClose);
        _finishTrigger.PlayerEnter -= OnFinish;
    }

    private IEnumerator PlayVoiceMessage(VoiceMessage message)
    {
        WaitForSeconds delay = new WaitForSeconds(_clips[message].length);
        WaitForSeconds pause = new WaitForSeconds(2f);
        yield return pause;

        _audioSource.clip = _clips[message];
        _audioSource.Play();

        yield return delay;
        _audioSource.Stop();
    }
}
