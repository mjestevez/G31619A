using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavPoint : MonoBehaviour
{
    public float yCoords;

    private void Update()
    {
        if (!DialogueManager.Instance.active)
        {
            if (Input.GetMouseButtonDown(1))
            {
            
                RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                foreach(RaycastHit2D h in hits)
                if (h.collider == GetComponent<BoxCollider2D>())
                {
                    Vector3 pos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, yCoords);
                    NavManager.Instance.Move(pos);
                }
            }

        }
    }
}
