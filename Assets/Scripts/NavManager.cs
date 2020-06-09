using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NavManager : Singleton<NavManager>
{
    public float xStairs=3.34f;
    public float speed = 3;
    private GameObject player;
    private Sequence seq;
    private bool flip;
    private Animator animator;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        seq = DOTween.Sequence();
        if (player.transform.localScale.x >= 0)
            flip = false;
        else
            flip = true;

        animator = player.GetComponent<Animator>();
    }

    public void Move(Vector3 position)
    {
        seq.Kill();
        seq = DOTween.Sequence();
        animator.SetBool("isWalking", true);
        float t = Mathf.Abs(position.x - player.transform.position.x) / speed;
        if (position.y != player.transform.position.y)
        {
            MoveToStairs(position);
            t= Mathf.Abs(position.x - xStairs) / speed;
        }
        seq.AppendCallback(() => Flip(position.x));
        seq.Append(player.transform.DOMoveX(position.x, t).SetEase(Ease.Linear)).OnComplete(()=> { player.transform.position = position; animator.SetBool("isWalking", false); });
        seq.Play();

    }

    private void MoveToStairs(Vector3 position)
    {
        float t = Mathf.Abs(xStairs - player.transform.position.x) / speed;
        seq.AppendCallback(()=>Flip(xStairs));
        seq.Append(player.transform.DOMoveX(xStairs, t).SetEase(Ease.Linear));
        seq.AppendInterval(0.1f);
        seq.Append(player.transform.DOMoveY(position.y, 0.01f).SetEase(Ease.Linear));
        seq.AppendInterval(0.1f);
    }

    private void Flip(float dest)
    {
        if ((!flip && dest > player.transform.position.x) || (flip && dest < player.transform.position.x))
        {
            flip = !flip;
            if (flip)
                player.transform.DOScaleX(-1, 0.01f).Play();
            else
                player.transform.DOScaleX(1, 0.01f).Play();
            
        }
    }
}
