using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Vertex : MonoBehaviour {

    [SerializeField]
    private Text vertexTitle;
    
    public List<Edge> edges;
    
    
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
