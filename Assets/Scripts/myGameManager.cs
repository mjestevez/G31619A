using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class myGameManager : Singleton<myGameManager>
{
   
    private float gameTime = 480f;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI materialsText;
    public TextMeshProUGUI passagersText;
    public TextMeshProUGUI deadText;
    public bool pause = false;
    public Image blackPanel;
    public GameObject pauseMenu;

    private void Start()
    {
        Invoke(nameof(ActInterface),0.2f);
    }
    // Update is called once per frame
    void Update()
    {
        if (!pause)
        {
            gameTime += Time.deltaTime;
            int min = (int) (gameTime / 60f);
            int sec = (int) (gameTime % 60);
            timeText.text = min.ToString("00") + ":" + sec.ToString("00");

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                Reanudar();
            }
            
        }
    }

    public void ActInterface()
    {
        dayText.text = EventManager.Instance.day.ToString();
        foodText.text = Variables.Instance.food.ToString();
        materialsText.text = Variables.Instance.materials.ToString();
        passagersText.text = Variables.Instance.passagers.ToString();
        deadText.text = Variables.Instance.dead.ToString();

    }

    public void FundidoNegro()
    {
        Color c = blackPanel.color;
        c.a = 1;

        blackPanel.DOColor(c, 2f).Play();
    }

    public void VueltaNegro()
    {
        Color c = blackPanel.color;
        c.a = 0;

        blackPanel.DOColor(c, 2f).Play();
    }
    
    public void Reanudar()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
    public void SalirJuego()
    {
        Application.Quit();
    }
}
