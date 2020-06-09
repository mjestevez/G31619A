using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue[] dialogue;
    public int iterator = 0;
    private GameObject player;
    public Vector3 cameraOffset;
    public Vector3 canvasOffset;
    public int tipo = 0;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
       if (CheckPlayerPosition())
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].collider == GetComponent<BoxCollider2D>())
                    {
                        TriggerConversation();
                    }
                }
                
            }

        }
    }

    private bool CheckPlayerPosition()
    {
        float distance = Vector2.Distance(player.transform.position, transform.position);
        return distance <= 1;
    }

    public void TriggerConversation()
    {
        
        if (iterator >= dialogue.Length)
        {
            this.enabled = false;
        }
        else
        {
            if (dialogue[iterator] != null)
            {
                DialogueManager.Instance.StarConversation(dialogue[iterator], transform.position + canvasOffset);
                CameraController.Instance.FocusCamera(transform.position + cameraOffset);
                iterator++;
            }
            if (iterator >= dialogue.Length)
            {
                this.enabled = false;
            }

        }
            
        

    }
    
    
}
