using System.Collections.Generic;
using UnityEngine;
using Practice.QuadTreePractice;
using Rect = Practice.QuadTreePractice.Rect;
public partial class VerletIntegration : MonoBehaviour
{
    List<Line> lines = new List<Line>();
    int[] order;
    public Vector2Int size;
    [SerializeField]
    [Range(.5f, 1f)]
    float windSpeed = .5f;
    Wind wind = new Wind();
    bool windBlow;

    QuadTree quadTree;
    Rect r = new Rect(Vector2.zero, Vector2.one * 10);
    Rect qRect = new Rect(Vector2.zero, Vector2.one);
    SpatialHash hash;
    LineRenderer l;

    void Start()
    {
        l = GetComponent<LineRenderer>();
        quadTree = new QuadTree(r, 4);
        hash = new SpatialHash(1);
        int x = size.x >> 1;
        int y = size.y >> 1;
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Vector3 p = new Vector3(x - i, y - j, 0);

                Point point = new Point(p, p, i % 4 == 0 && j == 0);
                points.Add(point);
                quadTree.Insert(point);
                hash.Insert(point.position, i * size.x + j);
            }
        }
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                int a = Get2DIndex(i, j),
                    b = Get2DIndex(i + 1, j),
                    c = Get2DIndex(i, j + 1);
                if (i + 1 < size.x)
                    lines.Add(new Line(points[a], points[b]));
                if (j + 1 < size.y)
                    lines.Add(new Line(points[a], points[c]));

            }
        }

        CreateOrderArray();

        //cam = Camera.main;

        int Get2DIndex(int i, int j)
        {
            return (i * size.y) + j;
        }
    }

    void Update()
    {
        hash.Trash();
        for (int i = 0; i < points.Count; i++)
        {
            hash.Insert(points[i].position, i);
        }
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        wind.windSpeed = windSpeed;

        if (Input.GetKeyDown(KeyCode.Space)) editMode = !editMode;
        if (Input.GetKeyDown(KeyCode.W))
        {
            windBlow = !windBlow;
        }

        if (editMode)
        {
            EditMode();
        }
        for (int i = 0; i < Mathf.Min(l.positionCount, points.Count); i++)
        {
            l.SetPosition(i, points[i].position);
        }
        qRect.x = mousePos.x;
        qRect.y = mousePos.y;
        print(quadTree.Query(qRect).Count);

        if (!editMode)
        {
            UpdatePoint();
            foreach (var point in points)
            {
                if (windBlow && !point.locked)
                    point.position += wind.wind * Time.deltaTime * Time.deltaTime;
            }
            UpdateLine();
            //UpdateFluid();
        }
    }
    private void UpdatePoint()
    {
        quadTree = new QuadTree(r, 4);
        for (int i = 0; i < points.Count; i++)
        {
            points[i].UpdatePoint();
            quadTree.Insert(points[i]);
        }
    }
    private void UpdateLine()
    {
        for (int it = 0; it < 1; it++)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                lines[order[i]].UpdateLine();
            }
        }
    }
    private void UpdateFluid()
    {
        for (int it = 0; it < 5; it++)
        {
            for (int i = 0; i < points.Count; i++)
            {
                hash.GetGridXY(points[i].position,out int x,out int y);
                foreach (var item in hash.GetCellElements(x,y,1))
                {
                    if (points[item] == points[i]) continue;
                    points[i].Fluid(points[item]);
                }
            }
        }
    }
    public static T[] ShuffleArray<T>(T[] array, System.Random prng)
    {

        int elementsRemainingToShuffle = array.Length;
        int randomIndex = 0;

        while (elementsRemainingToShuffle > 1)
        {

            // Choose a random element from array
            randomIndex = prng.Next(0, elementsRemainingToShuffle);
            T chosenElement = array[randomIndex];

            // Swap the randomly chosen element with the last unshuffled element in the array
            elementsRemainingToShuffle--;
            array[randomIndex] = array[elementsRemainingToShuffle];
            array[elementsRemainingToShuffle] = chosenElement;
        }

        return array;
    }

    protected void CreateOrderArray()
    {
        order = new int[lines.Count];
        for (int i = 0; i < order.Length; i++)
        {
            order[i] = i;
        }
        ShuffleArray(order, new System.Random());
    }

    private void OnDrawGizmosSelected()
    {
        //for (int i = 0; i < lines.Count; i++)
        //{
        //    Gizmos.color = editMode ? Color.red : Color.green;
        //    Gizmos.DrawSphere(Vector3.one * 9, .5f);
        //    Gizmos.color = Color.white;
        //    lines[i].Draw();
        //}
        wind.OnDrawGizmos();
        if (editMode)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(mousePos, raycastRadius);
            DrawNewLine();
        }

        Gizmos.color = Color.white;
        foreach (Point point in points)
        {
            Gizmos.DrawSphere(point.position, .2f);
        }
        //Gizmos.color = Color.green;

        //if (quadTree != null)
        //{
        //    foreach (Point p in quadTree.Query(new Rect(points[0].position, Vector2.one * (points[0].r))))
        //    {
        //        Gizmos.DrawSphere(p.position, .2f);
        //    }
        //    Visualize(quadTree);
        //}
    }
    public void Visualize(QuadTree quadTree)
    {
        Gizmos.color = Color.red;

        if (quadTree.subdivided)
        {
            Visualize(quadTree.topLeft);
            Visualize(quadTree.topRight);
            Visualize(quadTree.bottomLeft);
            Visualize(quadTree.bottomRight);
        }
        Gizmos.DrawWireCube(quadTree.boundingBox.center, quadTree.boundingBox.halfRes * 2);

    }
    public struct VectorField
    {
        Vector3[,] field;

        public VectorField(int size)
        {
            field = new Vector3[size, size];
        }

    }
}

