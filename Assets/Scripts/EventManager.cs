using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class EventManager : Singleton<EventManager>
{
    public int day=0;
    public Vector3 panelOffset;
    public Vector3 cameraOffset;
    private GameObject player;
    public GameObject mujer;
    public GameObject nino;
    public Sprite ninoSinMuñeco;
    public GameObject anciano;
    public Sprite ancianoSinProtesis;
    public GameObject criminal;
    public Sprite criminalSprite;
    public GameObject infectado;
    public Sprite infectadoSprite;
    public GameObject IA;
    public int historyPoint = 0;
    

    [Header("Day 0")]
    public Dialogue day0_InitialEvent;
    public Dialogue day0_EndingA;
    public Dialogue day0_EndingB;
    public int day0_EndPoint = 5;

    [Header("Day 1")]
    public Dialogue day1_InitialEvent;
    public Dialogue day1_EndingA;
    public Dialogue day1_EndingB;

    [Header("Day 2")]
    public Dialogue day2_InitialEvent;
    public Dialogue day2_EndingA;
    public Dialogue day2_EndingB;
    

    [Header("Day 3")]
    public Dialogue day3_InitialEvent;
    public Dialogue day3_EndingA;
    public Dialogue day3_EndingB;
    public Dialogue day3_End;

    [Header("Day 4")]
    public Dialogue day4_InitialEvent;
    public Dialogue day4_EndingA;
    public Dialogue day4_EndingB;
    public Dialogue day4_Dead;

    [Header("Day 5")]
    public Dialogue day5_InitialEvent;
    public Dialogue day5_EndingA;
    public Dialogue day5_EndingB;
    public Dialogue day5_Dead;

    [Header("Day 6")]
    public Dialogue day6_InitialEvent;
    public Dialogue day6_Ending;
    public int day6_EndPoint = 6;

    public SpriteRenderer nave;
    public Sprite naveSinBano;
    public Sprite naveNormal;
    public ParticleSystem humoMotor;
    public ParticleSystem humoMotorP;

    [Header("Audio")]
    public AudioClip IAnotification;
    public AudioClip Cof;
    public AudioClip Shoot;
    public AudioClip Golpe;
    private AudioSource aSr;

    // Start is called before the first frame update
    void Start()
    {
        if (Variables.Instance.fallos >= 2) GameOver();
        else
        {
            if (Variables.Instance.fallos == 1)
            {
                GameObject.FindGameObjectWithTag("Nave").GetComponent<Animator>().SetBool("isDamage_lv1", true);
                humoMotor.Play();
                humoMotorP.Play();
            }

            else
            {
                GameObject.FindGameObjectWithTag("Nave").GetComponent<Animator>().SetBool("isDamage_lv1", false);
                humoMotor.Stop();
                humoMotorP.Stop();
            }
                

            player = GameObject.FindGameObjectWithTag("Player");
            day = SceneManager.GetActiveScene().buildIndex-1;
            aSr = GetComponent<AudioSource>();
            historyPoint = 0;
            if (!Variables.Instance.muneco)
                Day2ChangeSprite();
            if (!Variables.Instance.protesis)
                Day3ChangeSprite();
            if (!Variables.Instance.inhalador)
                infectado.gameObject.SetActive(false);
            Invoke(nameof(StartEvent), Time.deltaTime);
        }
        
    }

    private void StartEvent()
    {
        switch (day)
        {
            case 0:
                DialogueManager.Instance.StarConversation(day0_InitialEvent, player.transform.position + panelOffset);
                CameraController.Instance.FocusCamera(player.transform.position + cameraOffset);
                break;
            case 1:
                DialogueManager.Instance.StarConversation(day1_InitialEvent, player.transform.position + panelOffset);
                CameraController.Instance.FocusCamera(player.transform.position + cameraOffset);
                Variables.Instance.food=3;
                myGameManager.Instance.ActInterface();
                break;
            case 2:
                DialogueManager.Instance.StarConversation(day2_InitialEvent, player.transform.position + panelOffset);
                CameraController.Instance.FocusCamera(player.transform.position + cameraOffset);
                break;
            case 3:
                DialogueManager.Instance.StarConversation(day3_InitialEvent, player.transform.position + panelOffset);
                CameraController.Instance.FocusCamera(player.transform.position + cameraOffset);
                break;
            case 4:
                if (Variables.Instance.protesis)
                {
                    anciano.SetActive(false);
                    Variables.Instance.passagers--;
                    Variables.Instance.dead++;
                    myGameManager.Instance.ActInterface();
                    Dialogue d = criminal.GetComponent<DialogueTrigger>().dialogue[0];
                    criminal.GetComponent<DialogueTrigger>().dialogue[0] = day4_Dead;
                    criminal.GetComponent<DialogueTrigger>().dialogue[1] = d;
                }
                    
                DialogueManager.Instance.StarConversation(day4_InitialEvent, player.transform.position + panelOffset);
                CameraController.Instance.FocusCamera(player.transform.position + cameraOffset);
                break;
            case 5:
                if (Variables.Instance.protesis)
                    anciano.SetActive(false);

                if (!Variables.Instance.inhalador)
                {
                    infectado.SetActive(false);
                    Variables.Instance.passagers--;
                    Variables.Instance.dead++;
                    myGameManager.Instance.ActInterface();
                    Dialogue d = criminal.GetComponent<DialogueTrigger>().dialogue[0];
                    criminal.GetComponent<DialogueTrigger>().dialogue[0] = day5_Dead;
                    criminal.GetComponent<DialogueTrigger>().dialogue[1] = d;
                }
                DialogueManager.Instance.StarConversation(day5_InitialEvent, player.transform.position + panelOffset);
                CameraController.Instance.FocusCamera(player.transform.position + cameraOffset);
                break;
            case 6:
                if (Variables.Instance.protesis)
                {
                    anciano.SetActive(false);
                    day6_EndPoint--;
                }
                if (!Variables.Instance.inhalador)
                {
                    infectado.SetActive(false);
                    day6_EndPoint--;
                }
                DialogueManager.Instance.StarConversation(day6_InitialEvent, player.transform.position + panelOffset);
                CameraController.Instance.FocusCamera(player.transform.position + cameraOffset);
                break;
        }
    }

    public void AdvancePoint()
    {
        historyPoint++;
        switch (day)
        {
            case 0:
                if (historyPoint == day0_EndPoint)
                    Invoke(nameof(EndDay),0.01f);
                break;
            case 6:
                if (historyPoint == day6_EndPoint)
                    Invoke(nameof(EndDay), 0.01f);
                break;
            
        }
    }

    private void EndDay()
    {
        switch (day)
        {
            case 0:
                DialogueManager.Instance.StarConversation(day0_EndingA, player.transform.position + panelOffset);
                CameraController.Instance.FocusCamera(player.transform.position + cameraOffset);
                break;
            case 1:
                RepairEvent.Instance.Repair();
                SubtractMaterials();
                Invoke(nameof(Day1EndA), 3f);
                break;
            case 2:
                RepairEvent.Instance.Repair();
                SubtractMaterials();
                Invoke(nameof(Day2EndA), 3f);
                break;
            case 3:
                RepairEvent.Instance.Repair();
                SubtractMaterials();
                Invoke(nameof(Day3EndA), 3f);
                break;
            case 4:
                RepairEvent.Instance.Repair();
                SubtractMaterials();
                Invoke(nameof(Day4EndA), 3f);
                break;
            case 5:
                RepairEvent.Instance.Repair();
                SubtractMaterials();
                Invoke(nameof(Day5EndA), 3f);
                break;
            case 6:
                DialogueManager.Instance.StarConversation(day6_Ending, player.transform.position + panelOffset);
                CameraController.Instance.FocusCamera(player.transform.position + cameraOffset);
                break;

        }
        SleepEvent.Instance.active = true;
        RemoveConversations();
    }

    private void RemoveConversations()
    {
        anciano.GetComponent<DialogueTrigger>().enabled = false;
        criminal.GetComponent<DialogueTrigger>().enabled = false;
        infectado.GetComponent<DialogueTrigger>().enabled = false;
        mujer.GetComponent<DialogueTrigger>().enabled = false;
        IA.GetComponent<DialogueTrigger>().enabled = false;
    }

    public void InvokeMethod(string name)
    {
        Invoke(name, 0.01f);
    }

    private void Sleep()
    {
        SleepEvent.Instance.EndDay();
    }

    private void Temblor()
    {
       Camera.main.transform.DOShakePosition(2f).Play();
        aSr.clip = Golpe;
        aSr.Play();
    }
    
    private void SubtractFood()
    {
        Variables.Instance.food--;
        myGameManager.Instance.ActInterface();
    }
    private void SubtractMaterials()
    {
        Variables.Instance.materials--;
        myGameManager.Instance.ActInterface();
    }

    private void AddMaterials()
    {
        Variables.Instance.materials++;
        myGameManager.Instance.ActInterface();
    }

    private void ActiveBed()
    {
        SleepEvent.Instance.active = true;
    }

    private void ActiveRepair()
    {
        RepairEvent.Instance.active = true;
    }

    private void Day1EndA()
    {
        
        player.transform.position = new Vector3(1.384f, -0.044f, 0);
        mujer.transform.position = new Vector3(-1.549f, -0.209f, 0);
        nino.transform.position = new Vector3(-1.855f, -0.592f, 0);
        criminal.transform.position = new Vector3(2.131f, -0.181f, 0);
        DialogueManager.Instance.StarConversation(day1_EndingA, transform.position + panelOffset);
        CameraController.Instance.FocusCamera(transform.position + cameraOffset);
        SleepEvent.Instance.active = true;

    }

    private void Day2EndA()
    {

        player.transform.position = new Vector3(0.62f, -0.044f, 0);
        mujer.transform.position = new Vector3(-1.549f, -0.209f, 0);
        nino.transform.position = new Vector3(-1.855f, -0.592f, 0);
        criminal.transform.position = new Vector3(2.131f, -0.181f, 0);
        IA.SetActive(false);
        DialogueManager.Instance.StarConversation(day2_EndingA, transform.position + panelOffset);
        CameraController.Instance.FocusCamera(transform.position + cameraOffset);
        SleepEvent.Instance.active = true;
        Variables.Instance.muneco = false;

    }

    private void Day1Nino()
    {
        nino.GetComponent<SpriteRenderer>().flipX = false;
        nino.transform.DOMoveX(-2.5f, 2f).SetRelative().SetEase(Ease.InOutQuad).OnComplete(()=> { nino.GetComponent<SpriteRenderer>().flipX = true; }).Play();
        
    }

    private void Day2ChangeSprite()
    {
        nino.GetComponent<SpriteRenderer>().sprite = ninoSinMuñeco;
    }
    private void Day3ChangeSprite()
    {
        anciano.GetComponent<SpriteRenderer>().sprite = ancianoSinProtesis;
    }
    private void IdaNegro()
    {
        myGameManager.Instance.FundidoNegro();
    }
    private void VueltaNegro()
    {
        myGameManager.Instance.VueltaNegro();
    }
    private void Day3InitialEvent()
    {
        anciano.GetComponent<SpriteRenderer>().sprite = ancianoSinProtesis;
        anciano.transform.position = new Vector3(1.63f, -0.497f, 0);
        criminal.GetComponent<SpriteRenderer>().sprite = null;
        criminal.transform.position = new Vector3(5.31f, -0.21f, 0);
        anciano.GetComponent<DialogueTrigger>().enabled = true;
        nave.sprite = naveSinBano;
    }

    private void Day3CriminalEvent()
    {
        infectado.GetComponent<DialogueTrigger>().enabled = true;
        criminal.GetComponent<DialogueTrigger>().enabled = true;
    }

    private void Day3CriminalAppear()
    {
        nave.sprite = naveNormal;
        criminal.GetComponent<SpriteRenderer>().sprite = criminalSprite;
    }
    private void Day3EndingA()
    {
        Debug.Log("EndingA");
        anciano.GetComponent<DialogueTrigger>().enabled = true;
        anciano.GetComponent<DialogueTrigger>().iterator = 0;
        anciano.GetComponent<DialogueTrigger>().dialogue[0] = day3_EndingA;
        Variables.Instance.protesis = false;
    }
    private void Day3EndingB()
    {
        anciano.GetComponent<DialogueTrigger>().enabled = true;
        anciano.GetComponent<DialogueTrigger>().iterator = 0;
        anciano.GetComponent<DialogueTrigger>().dialogue[0] = day3_EndingB;
        
    }
    private void Day3EndA()
    {
        player.transform.position = new Vector3(0.62f, -0.044f, 0);
        DialogueManager.Instance.StarConversation(day3_End, transform.position + panelOffset);
        CameraController.Instance.FocusCamera(transform.position + cameraOffset);
        ActiveBed();
    }
    private void Day4Event()
    {
        infectado.transform.DOScaleX(-1, 0.01f).Play();
        infectado.transform.position = new Vector3(1.649f, -2.825f, 0);
        infectado.GetComponent<DialogueTrigger>().enabled = true;
    }

    private void Day4EndA()
    {
        player.transform.position = new Vector3(0.62f, -0.044f, 0);
        mujer.transform.position = new Vector3(-1.549f, -0.209f, 0);
        nino.transform.position = new Vector3(-1.855f, -0.592f, 0);
        criminal.transform.position = new Vector3(2.131f, -0.181f, 0);
        infectado.transform.position = new Vector3(-0.496f, -0.309f, 0);
        nino.SetActive(true);
        infectado.transform.DOScaleX(1, 0.01f).Play();
        criminal.transform.DOScaleX(-1, 0.01f).Play();
        mujer.transform.DOScaleX(1, 0.01f).Play();
        DialogueManager.Instance.StarConversation(day4_EndingA, transform.position + panelOffset);
        CameraController.Instance.FocusCamera(transform.position + cameraOffset);
        ActiveBed();
        Variables.Instance.inhalador = false;
    }


    private void Day4EndB()
    {
        RepairEvent.Instance.Repair();
        Invoke(nameof(Day4EndB2), 3f);
    }

    private void Day4EndB2()
    {
        player.transform.position = new Vector3(0.62f, -0.044f, 0);
        mujer.transform.position = new Vector3(-1.549f, -0.209f, 0);
        nino.transform.position = new Vector3(-1.855f, -0.592f, 0);
        criminal.transform.position = new Vector3(2.131f, -0.181f, 0);
        infectado.transform.position = new Vector3(-0.496f, -0.309f, 0);
        nino.SetActive(true);
        infectado.transform.DOScaleX(1, 0.01f).Play();
        criminal.transform.DOScaleX(-1, 0.01f).Play();
        mujer.transform.DOScaleX(1, 0.01f).Play();
        DialogueManager.Instance.StarConversation(day4_EndingB, transform.position + panelOffset);
        CameraController.Instance.FocusCamera(transform.position + cameraOffset);
        ActiveBed();
    }

    private void Day5EndA()
    {
        player.transform.position = new Vector3(0.62f, -0.044f, 0);
        criminal.transform.position = new Vector3(2.131f, -0.181f, 0);
        infectado.transform.position = new Vector3(-0.496f, -0.309f, 0);
        DialogueManager.Instance.StarConversation(day5_EndingA, transform.position + panelOffset);
        CameraController.Instance.FocusCamera(transform.position + cameraOffset);
        ActiveBed();
    }

    private void GameOver()
    {
        SceneManager.LoadScene("Ending_Bad");
    }
    private void GoodEnding()
    {
        Invoke(nameof(Final), 3f);
        
    }
    private void Final()
    {
        SceneManager.LoadScene("Ending_Good");
    }

    private void AddFallos()
    {
        Variables.Instance.fallos++;
    }

    private void IAAudio()
    {
        aSr.clip = IAnotification;
        aSr.Play();
    }
    private void Toser()
    {
        aSr.clip = Cof;
        aSr.Play();
    }

    private void Disparo()
    {
        aSr.clip = Shoot;
        aSr.Play();
    }
    
}
