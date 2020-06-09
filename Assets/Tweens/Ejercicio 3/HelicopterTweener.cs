using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HelicopterTweener : MonoBehaviour
{
    public Camera Camera;
    public List<Transform> PositionsCamera;
    public ParticleSystem ExplosionPS;

    public Transform Head;
    public Transform Body;

    public void Start()
    {
        PlayFullSequence();
    }

    private void PlayFullSequence()
    {
        Sequence cameraSeq = LoadCameraSequence();
        Sequence helixSeq = LoadHelixSequence();
        Sequence flySeq = LoadFlySequence();

        helixSeq.Insert(3, flySeq);


        //cameraSeq.OnComplete(() => HelixSeq.Play());
        cameraSeq.Append(helixSeq); // Otra forma de hacer lo de arriba.

        cameraSeq.Play();
    }

    private Sequence LoadFlySequence()
    {
        Sequence flySeq = DOTween.Sequence();

        flySeq.Append(transform.DOMoveY(5, 7).SetEase(Ease.InOutQuad));

        flySeq.Append(Body.transform.DOShakePosition(1, 0.3f));

        flySeq.AppendCallback(() => ExplosionPS.Play());

        flySeq.Join(Body.transform.DOMoveY(1, 1).SetEase(Ease.OutBounce));// Al mismo timepo que comienza el shake comenzariamos a bajar

        flySeq.Join(Head.DOMoveY(30, .5f).SetEase(Ease.Linear));

        return flySeq;
    }

    public Sequence LoadCameraSequence()
    {
        Sequence cameraSeq = DOTween.Sequence();

        cameraSeq.Append(Camera.transform.DOMove(PositionsCamera[0].position, 3)
                      .From()
                      .SetEase(Ease.InOutQuad));

        return cameraSeq;
    }

    public Sequence LoadHelixSequence()
    {
        Sequence helixSequence = DOTween.Sequence();

        helixSequence.Append(Head.DORotate(new Vector3(0, 360, 0), 3f)
                                 .SetRelative()
                                 .SetEase(Ease.InQuad))
                                 .OnPlay(()=> Debug.Log("Append01"));

        helixSequence.Append(Head.DORotate(new Vector3(0, 360, 0), 1f)
                                 .SetRelative()
                                 .SetEase(Ease.Linear))
                                 .OnPlay(() => Debug.Log("Append02"));

        helixSequence.Append(Head.DORotate(new Vector3(0, 360, 0), 0.5f)
                                 .SetRelative()
                                 .SetLoops(9999999, LoopType.Incremental)
                                 .SetEase(Ease.Linear))
                                 .OnPlay(() => Debug.Log("Append03"));

        return helixSequence;
    }

}
