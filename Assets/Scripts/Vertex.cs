using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Vertex : MonoBehaviour {

    [SerializeField]
    private Text vertexTitle;
    public bool used = false;//для bfs
    public List<Edge> edges;
    private Image img;
    private Color normalVertexColour = Color.white, currentVertexColour = Color.grey, usedVertexColour = Color.cyan;

    private void Start()
    {
        img = transform.FindChild("Canvas/Image").GetComponent<Image>();
    }

    /// <summary>
    /// Подсветка вершины как текущей просматриаемой в поиске в ширину
    /// </summary>
    public void HightlightAsCurrent()
    {
        img.color = currentVertexColour;
    }

    public void HightlightAsUsed()
    {
        img.color = usedVertexColour;
    }
    public void HightlightAsNormal()
    {
        img.color = normalVertexColour;
    }


    public void SetText(string text)
    {
        vertexTitle.text = text;
    }
    public void RemoveEdge(Edge edge)
    {
        edges.Remove(edge);
    }
    /// <summary>
    /// Удаление всех ребер связаных с вершиной
    /// </summary>
    public void RemoveAllLinkedEdges()
    {
        Debug.Log("Удаление ребер в вершине :" + gameObject.name);
        for (int i = 0; i < edges.Count; i++)
        {
            GameManager.Instance.RemoveLinkedEdges(edges[i].Label);
        }
        edges.Clear();
        
    }
    /// <summary>
    /// Удаляет все избыточные записи из списка дуг
    /// </summary>
    public void RemoveRedudanciesEdges()
    {
        List<Edge> tempEdges = new List<Edge>();
        foreach (var item in edges)
        {
            if (item != null) tempEdges.Add(item);
        }
        edges = tempEdges;
    }
}
