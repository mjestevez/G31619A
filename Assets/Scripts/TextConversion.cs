using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextConversion : MonoBehaviour
{
    public string Protagonista;
    public string Anciano;
    public string Mujer;
    public string Niño;
    public string Enfermo;
    public string Criminal;
    public string IA;

    public Dialogue[] dialogos;

    private void Start()
    {
        TextConvert();
    }

    private void TextConvert()
    {
        for (int i = 0; i < dialogos.Length; i++)
        {
            foreach(Message m in dialogos[i].menssages)
            {
                m.message = Verify(m.message);
                m.npcName = Verify(m.npcName);
            }
        }
        Debug.Log("Finihs");
    }

    private string Verify(string message)
    {
        String[] palabras = message.Split(' ');
        String result="";
        for (int i = 0; i < palabras.Length; i++)
        {
            switch (palabras[i])
            {
                case "*Protagonista*":
                    palabras[i] = Protagonista;
                    break;
                case "*P*":
                    palabras[i] = Protagonista;
                    break;
                case "*Anciano*":
                    palabras[i] = Anciano;
                    break;
                case "*A*":
                    palabras[i] = Anciano;
                    break;
                case "*Mujer*":
                    palabras[i] = Mujer;
                    break;
                case "*M*":
                    palabras[i] = Mujer;
                    break;
                case "*Niño*":
                    palabras[i] = Niño;
                    break;
                case "*N*":
                    palabras[i] = Niño;
                    break;
                case "*Enfermo*":
                    palabras[i] = Enfermo;
                    break;
                case "*Criminal*":
                    palabras[i] = Criminal;
                    break;
                case "*IA*":
                    palabras[i] = IA;
                    break;
                default:
                    break;
            }
            result += palabras[i] +" ";
        }
        return result;
    }
}
