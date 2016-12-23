using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {


    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            return instance;
        }
    }
    [SerializeField]
    private Text AdjacencyMatrixText,IncidentnostMatrixText, vertexesPowsText;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
        AdjacencyMatrixText = GameObject.Find("AdjacencyMatrixText").GetComponent<Text>();
        IncidentnostMatrixText = GameObject.Find("IncidentnostMatrixText").GetComponent<Text>();
        vertexesPowsText = GameObject.Find("VertexesPowsText").GetComponent<Text>();

    }
    public void DrawVertex(Vector2 position)
    {
        GameObject vertex = Instantiate(Resources.Load("Vertex"), position, Quaternion.identity) as GameObject;
        GameManager.Instance.AddVertexToList(vertex.GetComponent<Vertex>());
    }
    public void DrawEdge(Vertex first, Vertex second)
    {
        GameObject edge = GameObject.Instantiate(Resources.Load("Edge"), transform.position, Quaternion.identity) as GameObject;
        edge.GetComponent<LineRenderer>().SetPosition(0, first.transform.position);
        edge.GetComponent<LineRenderer>().SetPosition(1, second.transform.position);
        GameManager.Instance.ClearFirstAndSecondChoosedVertexes();
        first.edges.Add(edge.GetComponent<Edge>());
        second.edges.Add(edge.GetComponent<Edge>());
        edge.GetComponent<Edge>().firstLinkedVertex = first;
        edge.GetComponent<Edge>().secondLinkedVertex = second;
        GameManager.Instance.AddEdgeToList(edge.GetComponent<Edge>());
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            GameManager.Instance.ClickOnVertex(second);
        }
    }
    
    public void DrawAdjacencyMatrixText(string text)
    {
        AdjacencyMatrixText.text = text;
    }
    public void DrawIncidentnostMatrixText(string text)
    {
        IncidentnostMatrixText.text = text;
    }
    public void DrawVertexesPowMatrixText(string pows)
    {
        vertexesPowsText.text = pows;
    }
}
