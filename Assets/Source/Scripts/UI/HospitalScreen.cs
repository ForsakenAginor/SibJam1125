using Assets.Source.Scripts.Utility;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HospitalScreen : SwitchableElement
{
    [SerializeField] private Image blackImage;
    [SerializeField] private Image hospitalImage;
    [SerializeField] private SwitchableElement winScreen;

    public void Start()
    {
        //.OnComplete(()=> hospitalImage.enabled = true)
        blackImage.color = new Color(0, 0, 0, 0);
        Sequence sequence = DOTween.Sequence();
        blackImage.DOColor(new Color(0, 0, 0, 1), 2f).SetUpdate(true)
            .OnComplete(
            () =>
                {
                    hospitalImage.gameObject.SetActive(true);
                    blackImage.color = new Color(0, 0, 0, 1);
                    blackImage.DOColor(new Color(0, 0, 0, 0), 20f).SetUpdate(true)
                        .OnComplete(() =>
                        {
                            winScreen.Enable();
                            //gameObject.SetActive(false);
                        });
                });
    }
}
