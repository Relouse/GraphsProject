using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {


    private Rect GameWindowRect;
    void Start () {
        GameWindowRect = new Rect(Screen.width / 5.33f, 0, Screen.width / 1.232f, Screen.height / 1.49f);

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0) && !GameManager.Instance.mouseOnUI)
        {
            if (GameManager.Instance.CurMode == GameManager.mode.vertexDraw)
            {
                UIManager.Instance.DrawVertex(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
                if (hit.collider != null && hit.collider.gameObject.GetComponent<Vertex>())
                {
                    GameManager.Instance.ClickOnVertex(hit.collider.gameObject.GetComponent<Vertex>());
                }

            }
        }
    }
   
    
   
}
