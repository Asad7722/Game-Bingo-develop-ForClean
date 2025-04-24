using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace Games.Bingo
{
public class ImageSlideAnimator : MonoBehaviour
{
    public RectTransform imageToAnimate;
    public Canvas canvas;
    public float animationDuration = 0.5f;
    public float animationDurationDown = 1.5f;
    public float pauseAtCenterDuration = 2f;
    private Vector2 topPos;
    private Vector2 centerPos;
    private Vector2 bottomPos;
    public static event Action OnGameResumed;
    void OnEnable()
    {
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        float screenHeight = canvasRect.rect.height;
        centerPos = new Vector2(0, 0);
        topPos = new Vector2(0, screenHeight);
        bottomPos = new Vector2(0, -screenHeight);
        imageToAnimate.anchoredPosition = topPos;
        PlaySlideAnimation();
        Time.timeScale = 0;
    }
    public void PlaySlideAnimation()
    {
        imageToAnimate.DOAnchorPos(centerPos, animationDuration).SetUpdate(true)
            .OnComplete(() =>
            {
                DOVirtual.DelayedCall(pauseAtCenterDuration, () =>
                {
                    imageToAnimate.DOAnchorPos(bottomPos, animationDurationDown).SetUpdate(true)
                        .OnComplete(() =>
                        {
                            Time.timeScale = 1;
                            OnGameResumed?.Invoke();
                        });
                }).SetUpdate(true);
            });
    }
}

}