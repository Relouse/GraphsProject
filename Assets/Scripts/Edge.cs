using UnityEngine;
using System.Collections;

public class Edge : MonoBehaviour {

    public Vertex firstLinkedVertex, secondLinkedVertex;
    private TextMesh textObject;
    private LineRenderer lineRenderer;

    /// <summary>
    /// Подпись на дуге
    /// </summary>
    public string Label
    {
        set
        {
            textObject.text = value;
        }
        get
        {
            return textObject.text;
        }
    }

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        textObject = transform.FindChild("Text").GetComponent<TextMesh>();
    } 
    private void Start()
    {
        Vector3 pos = (lineRenderer.GetPosition(lineRenderer.numPositions - 1) + lineRenderer.GetPosition(0))/2;
        textObject.transform.position = pos + Vector3.one/2;
    }
    
 
}
