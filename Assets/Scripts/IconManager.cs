using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IconManager : Singleton<IconManager>
{
    public Texture2D[] icons;
    public LayerMask layer;
    public Vector2 offset;

    private void Awake()
    {
        if (IconManager.Instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        
    }
    private void Update()
    {

        CheckIcon();
    }
    private void CheckIcon()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 10f, layer);
        if (hit)
        {
            DialogueTrigger dt = hit.collider.gameObject.GetComponent<DialogueTrigger>();
            if (dt && dt.enabled)
            {
                ChagueMouseIcon(0);
                return;
            }

            SleepEvent se = hit.collider.gameObject.GetComponent<SleepEvent>();
            if (se && se.active)
            {
                ChagueMouseIcon(1);
                return;
            }
            RepairEvent re = hit.collider.gameObject.GetComponent<RepairEvent>();
            if (re && re.active)
            {
                ChagueMouseIcon(2);
                return;
            }
        }
        else
        {
            ResetMouse();
        }
    }
    public void ChagueMouseIcon(int n)
    {
        Cursor.SetCursor(icons[n],offset , CursorMode.ForceSoftware);
    }
    public void ResetMouse()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    
}
