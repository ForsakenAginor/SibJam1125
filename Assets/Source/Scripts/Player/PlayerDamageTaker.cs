using Assets.Source.Scripts.Utility;
using UnityEngine;

public class PlayerDamageTaker : MonoBehaviour
{
    [SerializeField] private Flashlight _flashlight;

    public void TakeDamage()
    {
        DirtPainter.Instance.AddComplexStains();
    }
}
