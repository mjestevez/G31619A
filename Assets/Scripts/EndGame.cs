using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public Image panel;
    public bool good = true;
    [TextArea]
    public string message;
    public Text messageText;
    // Start is called before the first frame update
    void Start()
    {
        if (good)
            StartCoroutine(GoodEnding());
        else
            StartCoroutine(BadEnding());
    }

    IEnumerator GoodEnding()
    {
        yield return new WaitForSeconds(30f);
        Color c = panel.color;
        c.a = 1;
        panel.DOColor(c, 3f).OnComplete(WriteText).Play();
    }

    IEnumerator BadEnding()
    {
        yield return new WaitForSeconds(30f);
        Color c = panel.color;
        c.a = 1;
        panel.DOColor(c, 3f).OnComplete(ChangeScene).Play();
    }
    private void WriteText()
    {
        StartCoroutine(Letter());
    }
    IEnumerator Letter()
    {
        foreach (char letter in message.ToCharArray())
        {
            messageText.text += letter;
            yield return new WaitForSeconds(0.04f);
        }
        yield return new WaitForSeconds(5f);
        ChangeScene();
    }
    private void ChangeScene()
    {
        SceneManager.LoadScene(0);
    }
}
