using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{

    public RawImage fundido;

    public GameObject MenuPrincipal;
    public GameObject MenuInstrucciones;
    public GameObject MenuCreditos;

    public GameObject robot;


    // Start is called before the first frame update
    void Start()
    {
        loadMenu("MenuPrincipal");
        
    }

    public void loadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void activeMenu(string MenuName)
    {
        switch (MenuName)
        {
            case "MenuPrincipal":
                robot.SetActive(true);
                MenuPrincipal.SetActive(true);
                MenuInstrucciones.SetActive(false);
                MenuCreditos.SetActive(false);
                break;
            case "MenuInstrucciones":
                robot.SetActive(false);
                MenuPrincipal.SetActive(false);
                MenuCreditos.SetActive(false);
                MenuInstrucciones.SetActive(true);
                break;
            case "MenuCreditos":
                robot.SetActive(false);
                MenuPrincipal.SetActive(false);
                MenuInstrucciones.SetActive(false);
                MenuCreditos.SetActive(true);
                break;
            default:
                break;
        }
    }
    public void loadMenu(string MenuName)
    {
        Color c = fundido.color;
        c.a = 1;
        if(!fundido.gameObject.activeInHierarchy)
            fundido.gameObject.SetActive(true);
        fundido.DOColor(c, 2f).OnComplete(() => { activeMenu(MenuName); VueltaNegro();}).Play();
    }

    public void VueltaNegro()
    {
        Color c = fundido.color;
        c.a = 0;

        fundido.DOColor(c, 2f).OnComplete(()=> { fundido.gameObject.SetActive(false); }).Play();
    }

    public void exitGame()
    {
        Application.Quit();
    }

}
