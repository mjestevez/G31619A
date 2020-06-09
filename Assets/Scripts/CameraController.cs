using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : Singleton<CameraController>
{
    public float viewportSizeOriginal=5.3f;
    public float viewportSizeFocus=3;
    private float duration = 3f;
    

    public void FocusCamera(Vector3 p)
    {
        transform.DOMove(p, duration).SetEase(Ease.InOutQuad).Play();
        Camera.main.DOOrthoSize(viewportSizeFocus, duration).SetEase(Ease.InOutQuad).Play();
    }

    public void ResetCamera()
    {
        transform.DOMove(new Vector3(0, 0, -10), duration).SetEase(Ease.InOutQuad).Play();
        Camera.main.DOOrthoSize(viewportSizeOriginal, duration).SetEase(Ease.InOutQuad).Play();
    }
}
