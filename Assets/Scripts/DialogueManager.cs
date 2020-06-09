using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
    public Queue<Message> conversation;
    public TextMeshProUGUI npcName;
    public TextMeshProUGUI messageText;
    private bool waitAnswer = false;
    private Message currentMessage;
    public GameObject[] optionButtons;
    public GameObject dialoguePanel;
    public bool active = false;
    public Color normalMessage;
    public Color thinkingMessage;
    public Animator hud;
    private bool writing = false;

    private void Start()
    {
        conversation = new Queue<Message>();
        dialoguePanel.SetActive(false);
        ResetOptions();
    }

    private void Update()
    {
        if (active && Time.timeScale==1)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                if (!writing)
                    NextDialogue();
                else
                    WriteText();
            }
        }
        
    }

    private void WriteText()
    {
        writing = false;
        messageText.text = currentMessage.message;

    }

    internal void StarConversation(Dialogue dialogue, Vector3 canvasPosition)
    {
        hud.SetBool("HUD_Oculto", true);
        if (dialogue.menssages.Length!=0)
        {
            if (canvasPosition != Vector3.zero)
            {
                RectTransform rt = dialoguePanel.GetComponent<RectTransform>();
                rt.anchoredPosition = canvasPosition;
            }
            Invoke(nameof(ActiveConversation), 0.5f);
            Queue<Message> copy = new Queue<Message>();
            for (int i = 0; i < conversation.Count; i++)
            {
                copy.Enqueue(conversation.Dequeue());
            }
            conversation.Clear();
            foreach (Message d in dialogue.menssages)
            {
                conversation.Enqueue(d);
            }
            for (int i = 0; i < copy.Count; i++)
            {
                conversation.Enqueue(copy.Dequeue());
            }
            dialoguePanel.SetActive(true);
            NextDialogue();
        }
        else
        {
            EndDialogue();
        }
        
    }

    private void NextDialogue()
    {
        if (!waitAnswer)
        {
            StopAllCoroutines();
            if (conversation.Count != 0) DisplayDialogue();
            else EndDialogue();
        }
        
    }

    public void EndDialogue()
    {
        hud.SetBool("HUD_Oculto", false);
        npcName.text = "";
        messageText.text = "";
        dialoguePanel.SetActive(false);
        active = false;
        Debug.Log("End Conversation");
        CameraController.Instance.ResetCamera();
        EventManager.Instance.AdvancePoint();
    }

    private void DisplayDialogue()
    {
        

        currentMessage = conversation.Dequeue();
        if(currentMessage.npcName!="")
        npcName.text = currentMessage.npcName;

        if (currentMessage.methodName!="")
            EventManager.Instance.InvokeMethod(currentMessage.methodName);

        if (currentMessage.thinking)
            messageText.color = thinkingMessage;
        else
            messageText.color = normalMessage;

        StartCoroutine(TypeMessage(currentMessage.message));
    }

    IEnumerator TypeMessage (string message)
    {
        writing = true;
        messageText.text = "";

        if (currentMessage.options.Length != 0) waitAnswer = true;

        foreach (char letter in message.ToCharArray())
        {
            if (writing)
            {
                messageText.text += letter;
                if (currentMessage.slowText != 0)
                    yield return new WaitForSeconds(currentMessage.slowText);
                else
                    yield return new WaitForSeconds(0.03f);
            }
            
        }
        if (currentMessage.options.Length != 0)
        {
            for (int i = 0; i < currentMessage.options.Length; i++)
            {
                if(!currentMessage.options[i].needFood || (currentMessage.options[i].needFood && Variables.Instance.food > 0))
                {
                    yield return new WaitForSeconds(0.5f);
                    optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentMessage.options[i].text;
                    optionButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
                    int n = currentMessage.options[i].number;
                    string name = currentMessage.options[i].methodName;
                    optionButtons[i].GetComponent<Button>().onClick.AddListener(() =>
                    {
                        SelectOption(n);
                        if (name != "")
                            EventManager.Instance.InvokeMethod(name);
                    }
                    );
                    optionButtons[i].SetActive(true);
                }
                
            }
        }
        writing = false;
    }

    public void SelectOption(int selected)
    {
        Debug.Log("hola");
        foreach (Options option in currentMessage.options)
        {
            if(option.number == selected)
            {
                if (option.response)
                {
                    Debug.Log(selected);
                    waitAnswer = false;
                    StarConversation(option.response,Vector3.zero);
                   
                }
            }
        }
        ResetOptions();
    }

    private void ResetOptions()
    {
        foreach (GameObject g in optionButtons)
        {
            g.SetActive(false);
        }
    }

    private void ActiveConversation()
    {
        active = true;
    }
}
