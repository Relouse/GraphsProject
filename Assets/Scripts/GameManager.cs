using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    // Для singleton
    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }
    //Ссылки на контроллеры
    private InputManager IM;
    private UIManager UM;
    //Список вершин и ребер
    public List<Vertex> vertexes = new List<Vertex>();
    public List<Edge> edges = new List<Edge>();
    //Первая вершина новосоздаваемого ребра и вторая
    private Vertex firstVertexOFEdge, secondVertexOfEdge;
    public bool mouseOnUI = false;//перенести это в Input Manager
    void Awake()
    {
        //Singleton
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    /// <summary>
    /// Очистка выбранных вершин
    /// </summary>
    public void ClearFirstAndSecondChoosedVertexes()
    {
        firstVertexOFEdge = null;
        secondVertexOfEdge = null;
    }

    void Start()
    {
        IM = new GameObject().AddComponent<InputManager>();
        UM = new GameObject().AddComponent<UIManager>();
        IM.gameObject.name = "InputManager";
        UM.gameObject.name = "UIManager";
    }
    void Update()
    {
        //Если нажали левый или правый ctrl
        if(Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
        {
            ClearFirstAndSecondChoosedVertexes();
        }
    }
    /// <summary>
    /// Режимы построения
    /// </summary>
    public enum mode
    {
        edgeDraw,
        vertexDraw,
        vertexRemove
    }
    
    private mode curMode = mode.vertexDraw;
    /// <summary>
    /// Текущий режим построения
    /// </summary>
    public mode CurMode
    {
        get
        {
            return curMode;
        }
    }

    /// <summary>
    /// Добавление вершины в массив для мониторинга всех вершин
    /// </summary>
    /// <param name="vert">Созданная вершина</param>
    public void AddVertexToList(Vertex vert)
    {
        vertexes.Add(vert);
        Debug.Log("Добавлена вершина: " + vertexes.Count);
        vert.name = vertexes.Count.ToString();
        vert.SetText(vertexes.Count.ToString());//Даем подпись вершине
    }

    public void RemoveVertexFromList(Vertex vert)
    {
        vert.RemoveAllLinkedEdges();
        vertexes.Remove(vert);
        Debug.Log("Удалена вершина: " + vert.name);
        Destroy(vert.gameObject);
    }
    /// <summary>
    /// Удаление ребер которые связаны с удаляемой вершиной
    /// <param name="edgeLabel">Передавать edge.Label</param>
    /// </summary>
    public void RemoveLinkedEdges(string edgeLabel)
    {
        Debug.Log("Запросили удаление " + edgeLabel);
        foreach (var vertex in vertexes)
        {
            for (int i = 0; i < vertex.edges.Count; i++)
            {
                if (vertex.edges[i].Label == edgeLabel)
                {
                    Debug.Log("Нашел " + edgeLabel + " в вершине " + vertex.name);
                    Destroy(vertex.edges[i].gameObject);
                    vertex.Invoke("RemoveRedudanciesEdges",0.1f);//УБРАТЬ инвок
                }
            }
        }
        Invoke("RemoveRedudanciesEdges", 0.1f);//УБРАТЬ инвок
    }
    /// <summary>
    /// Удаляет все избыточные записи из списка дуг
    /// </summary>
    public void RemoveRedudanciesEdges()
    {
        Debug.Log("Удаление излишеств");
        List<Edge> tempEdges = new List<Edge>();
        foreach (var item in edges)
        {
            if (item != null)
            {
                tempEdges.Add(item);
            }
            else
            {
                Debug.Log("Нашел null");
            }
        }
        edges = tempEdges;
    }
    /// <summary>
    /// Добавление ребра в массив для мониторинга всех ребер
    /// </summary>
    /// <param name="edge"></param>
    public void AddEdgeToList(Edge edge)
    {
        edges.Add(edge);
        edge.Label = "e" + edges.Count;
        CalculateAdjacencyAndIncidentnostMatrix();//После добавления нового ребра пересчитываем матрицу смежности
    }
    /// <summary>
    /// Выбор режима построения
    /// </summary>
    /// <param name="choosedMode">Номер режима, 0 - ребра, 1 - вершины</param>
    public void ChooseMode(int choosedMode)
    {
        curMode = (mode)choosedMode;
        Debug.Log("Выбран режим отрисовки: " + curMode);
    }
    /// <summary>
    /// Метод, который вызывается при тыке по вершине
    /// </summary>
    /// <param name="vert"></param>
    public void ClickOnVertex(Vertex vert)
    {
        Debug.Log("Нажал на вершину " + vert.name);
        if (curMode != mode.vertexRemove)
        {
            if (firstVertexOFEdge == null)//если до этого не было выбрано первой вершины
            {
                firstVertexOFEdge = vert;//выбираем
            }
            else//иначе выбираем вторую вершину
            {
                secondVertexOfEdge = vert;
                UM.DrawEdge(firstVertexOFEdge, secondVertexOfEdge);//если выбирается 2я вершина то отрисовываем ребро
            }
        }
        else
        {
            RemoveVertexFromList(vert);
            RemoveRedudanciesEdges();
            Invoke("CalculateAdjacencyAndIncidentnostMatrix",0.2f);//УБРАТЬ инвок
        }
    }
    
    
    //Вычисление матрицы смежности
    public void CalculateAdjacencyAndIncidentnostMatrix()
    {
        Debug.Log("CalculateAdjacencyMatrix");
        string[,] lines = new string[vertexes.Count, vertexes.Count];
        Debug.Log(vertexes.Count);
        for (int i = 0; i < edges.Count; i++)
        {
            try
            {
                lines[(int.Parse(edges[i].firstLinkedVertex.name) - 1), (int.Parse(edges[i].secondLinkedVertex.name)) - 1] = "1";
                lines[(int.Parse(edges[i].secondLinkedVertex.name) - 1), (int.Parse(edges[i].firstLinkedVertex.name)) - 1] = "1";
            }
            catch(System.Exception exp)
            {
                Debug.LogError(exp.ToString());
            }
        }
        for (int i = 0; i < lines.GetLength(0); i++)
        {
            for (int j = 0; j < lines.GetLength(1); j++)
            {
                if (string.IsNullOrEmpty(lines[i, j])) lines[i, j] = "0";
            }
        }
        
        string textWithVertNumbers = "", matrixText = "";
        string vertexNumbers = "    ";
        for (int i = 0; i < lines.GetLength(1); i++)
        {
            vertexNumbers += "<b>" + (i + 1) + "</b> ";
        }
        vertexNumbers += "\n";
        for (int i = 0; i < lines.GetLength(0); i++)
        {
            textWithVertNumbers += "<b>" + (i + 1) + "</b>  ";
            for (int j = 0; j < lines.GetLength(1); j++)
            {
                textWithVertNumbers += lines[i, j] + " ";
                matrixText += lines[i, j] + " ";
            }
            textWithVertNumbers += "\n";
            matrixText += "\n";//если это последняя строчка в матрице, то не добавляем \n
        }
        UM.DrawAdjacencyMatrixText(vertexNumbers + textWithVertNumbers);
        CalculateIncidentnostMatrix(lines);
    }

    void FillAllZeroInMantrix(string[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int k = 0; k < matrix.GetLength(1); k++)
            {
                matrix[i, k] = "0";
            }
        }
    }
    private void CalculateIncidentnostMatrix(string[,] matrix)
    {
        Debug.Log("CalculateIncidentnostMatrix");
        string edgesNumbers = "    ";
        int edgesCount = 0;
        for (int i = 0; i < matrix.GetLength(0); i++)
        {   
            for (int k = 0; k < matrix.GetLength(1); k++)
            {
                if (matrix[i, k] == "1" && i < k)
                {
                    edgesNumbers += "(" + (i + 1) + "," + (k + 1) + ") ";//Если нашли 1 и мы не выводили еще это пересечение
                    edgesCount++;
                }
            }
        }
        
        string[,] incidentnostMatrix = new string[matrix.GetLength(0), edgesCount];
        FillAllZeroInMantrix(incidentnostMatrix);

        int column = 0;
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int k = 0; k < matrix.GetLength(1); k++)
            {
                if (matrix[i, k] == "1" && i < k)
                {
                    incidentnostMatrix[i, column] = "1";
                    incidentnostMatrix[k, column] = "1";
                    column++;
                }
            }
        }
        
        edgesNumbers += "\n";
        string incidMatrixComplete = "";
        for (int i = 0; i < incidentnostMatrix.GetLength(0); i++)
        {
            incidMatrixComplete += "<b>" + (i + 1) + "</b>  ";
            for (int j = 0; j < incidentnostMatrix.GetLength(1); j++)
            {
                incidMatrixComplete += incidentnostMatrix[i, j] + "       ";
            }
            incidMatrixComplete += "\n";
        }
        UM.DrawIncidentnostMatrixText("<b>" + edgesNumbers +"</b>" + incidMatrixComplete);
        CalculateVertexesPows();
    }
    public void CalculateVertexesPows()
    {
        string text = "";
        for (int i = 0; i < vertexes.Count; i++)
        {
            text += "V" + vertexes[i].name + ":" + vertexes[i].edges.Count + "\n";
            //if (i % 3 == 1) text += "\n";
        }
        UIManager.Instance.DrawVertexesPowMatrixText(text);
    }

    //Два нижних метода перенести в InputManager
    /// <summary>
    /// Мышка залезла на окошки UI
    /// </summary>
    public void MouseIsOnUI()
    {
        mouseOnUI = true;
    }
    /// <summary>
    /// Мышка вышла из окошек UI
    /// </summary>
    public void MouseIsNotOnUI()
    {
        mouseOnUI = false;
    }
}
