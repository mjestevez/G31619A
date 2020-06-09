using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairEvent : Singleton<RepairEvent>
{
    public Dialogue dialogue;
    public bool active = false;
    private GameObject player;
    public Vector3 cameraOffset;
    private AudioSource aSr;
    
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        aSr = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (active)
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
                            DialogueManager.Instance.StarConversation(dialogue, transform.position + cameraOffset);
                            CameraController.Instance.FocusCamera(player.transform.position + cameraOffset);
                        }
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

    public void Repair()
    {
        aSr.Play();
        myGameManager.Instance.FundidoNegro();
        Variables.Instance.fallos = 0;
        Invoke(nameof(VueltaNegro), 4f);
        this.enabled = false;
    }

    private void VueltaNegro()
    {
        aSr.Stop();
        myGameManager.Instance.VueltaNegro();
    }
}
