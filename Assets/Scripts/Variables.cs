using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Variables : Singleton<Variables>
{
    public int materials = 0;
    public int food = 3;
    public int passagers = 6;
    public int dead = 0;
    public bool muneco;
    public bool protesis;
    public bool inhalador;
    public float fallos = 0;

    private void Awake()
    {
        if (Variables.Instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
}
