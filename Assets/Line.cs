using UnityEngine;

public class Line
{
    public Point a, b;
    public float distance;
    public float stiffness = 1f;

    public float radius;

    public Line()
    {
        a=null; 
        b=null; 
        distance=0;
    }
    public Line(Point a,Point b)
    {
        this.a=a; 
        this.b = b;
        distance = (a.position - b.position).magnitude;
    }
    public void UpdateLine()
    {
        radius = distance / 2;
        Vector3 center = (a.position + b.position) / 2f;
        Vector3 direction = a.position - b.position;
        float L = direction.magnitude;
        direction = direction.normalized;

        //if (L > distance)
        //{
        if (!a.locked)
        {
            a.position = center + (direction * radius);
        }
        if (!b.locked)
        {
            b.position = center - (direction * radius);
        }
        //}
    }
    public void BoundCheck()
    {
        a.BoundCheck();
        b.BoundCheck();
    }
    public void UpdateDistance()
    {
        distance = (a.position - b.position).magnitude;
    }
    public void Draw()
    {
        a.DrawPoint();
        b.DrawPoint();
        Gizmos.DrawLine(a.position, b.position);
    }
}

