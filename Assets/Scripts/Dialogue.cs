using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[CreateAssetMenu(fileName ="newDialogue",menuName ="Crear Dialogo",order =1)]

[System.Serializable]
public class Dialogue : ScriptableObject
{
    public Message[] menssages;
}

[System.Serializable]
public class Message
{
    [TextArea]
    public string message="";
    public string npcName="";
    public float slowText= 0.04f;
    public bool thinking = false;
    public string methodName = "";
    public Options[] options;
}

[System.Serializable]
public class Options
{
    public string text;
    public int number;
    public string methodName="";
    public Dialogue response;
    public bool needFood = false;
    
}
