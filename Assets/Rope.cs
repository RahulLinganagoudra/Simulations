using UnityEngine;
public class Rope
{
    Point[] points;
    Line[] lines;
    int[] order;

    public void Initialize(int vertexCount)
    {
        points = new Point[vertexCount];
        lines = new Line[points.Length - 1];
        
        for (int i = 0; i < points.Length; i++)
        {
            Point p = new Point(Vector3.zero, Vector3.zero);
            points[i] = p;
        }
        for (int i = 0; i < points.Length - 1; i++)
        {
            Line line = new Line(points[i], points[i + 1]);
            lines[i] = line;
        }
        
        CreateOrderArray();
    }
    public void DistributePoints(Vector3 start, Vector3 end)
    {
        for (int i = 0; i < points.Length; i++)
        {
            points[i].position=Vector3.Lerp(start,end,(float)i/points.Length);
        }
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].UpdateDistance();
        }
    }
    public void UpdateRope()
    {
        for (int i = 0; i < points.Length; i++)
        {
            points[i].position.y -= 10f * Time.deltaTime * Time.deltaTime;
            points[i].UpdatePoint();
        }
        for (int i = 0; i < lines.Length; i++)
        {
            lines[order[i]].UpdateLine();
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
        order = new int[lines.Length];
        for (int i = 0; i < order.Length; i++)
        {
            order[i] = i;
        }
        ShuffleArray(order, new System.Random());
    }
}

