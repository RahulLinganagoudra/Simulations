using System.Collections.Generic;
using UnityEngine;
public partial class VerletIntegration : MonoBehaviour
{
    bool editMode = true;
    Camera cam;
    Vector3 mousePos;
    List<Point> points = new List<Point>();

    [SerializeField] GameObject prefab;
    Point selectedPoint;
    Line newLine;
    float raycastRadius=.5f;
    void EditMode()
    {
        if (Input.GetMouseButtonDown(0))
        {
            selectedPoint = null;
            for (int i = 0; i < points.Count; i++)
            {
                if (CollisionCheck(points[i]))
                {
                    selectedPoint = points[i];
                    break;
                }
            }
            if (selectedPoint == null)
            {
                Point p = new Point(mousePos, mousePos);
                points.Add(p);
                selectedPoint = p;
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i] == null) continue;
                if (CollisionCheck(points[i]))
                {

                    if (newLine == null)
                    {
                        newLine = new Line();
                        newLine.a = points[i];
                    }
                    else
                    {
                        if (newLine.a == points[i])
                        {
                            newLine = null;
                            break;
                        }
                        newLine.b = points[i];
                        newLine.distance = (newLine.a.position - newLine.b.position).magnitude;
                        lines.Add(newLine);
                        CreateOrderArray();
                        newLine = null;
                    }
                    break;

                }
            }
            //points.Add(new Point(mousePos, mousePos));
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            selectedPoint.locked = !selectedPoint.locked;
        }

    }
    bool CollisionCheck(Point point)
    {
        float collisionCheckRadius = raycastRadius * raycastRadius;
        return (point.position - mousePos).sqrMagnitude <= collisionCheckRadius;
    }
    void DrawNewLine()
    {
        
        if (newLine == null) return;

        newLine.a.DrawPoint();
        Gizmos.color = Color.white;
        Gizmos.DrawLine(newLine.a.position, mousePos);
    }
}
