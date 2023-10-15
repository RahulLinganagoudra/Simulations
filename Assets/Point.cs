﻿using UnityEngine;
public class Point
{
    public Vector3 position, previousPos;
    public bool locked;
    const float GRAVITY = -9;
    public float r=.2f;
    public Point(Vector3 currentPos, Vector3 previousPos, bool locked = false)
    {
        position = currentPos;
        this.previousPos = previousPos;
        this.locked = locked;
    }
    public void UpdatePoint()
    {
        if (locked) return;

        Vector3 v = position - previousPos;
        previousPos = position;
        position += v;
        position.y += GRAVITY * Time.deltaTime * Time.deltaTime;
        BoundCheck();
    }
    public void BoundCheck()
    {
        Vector3 v = position - previousPos;
        float bounceLoss = .9f;
        if (position.x+r > 10)
        {
            position.x = 10-r;
            previousPos.x = position.x + v.x*bounceLoss;
        }
        else if (position.x-r < -10)
        {
            position.x = -10+r;
            previousPos.x = position.x + v.x*bounceLoss;
        }
        if (position.y+r > 10)
        {
            position.y = 10-r;
            previousPos.y = position.y + v.y*bounceLoss;
        }
        else if (position.y-r < -10)
        {
            position.y = -10+r;
            previousPos.y = position.y + v.y * bounceLoss;
        }
    }
    public void DrawPoint()
    {
        Gizmos.color = locked ? Color.red : Color.white;
        Gizmos.DrawSphere(position, .2f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(previousPos, .1f);
    }

    public void Fluid(Point b)
    {
            var direction=position - b.position;
        float L = direction.magnitude;
        float R = r + b.r;
        float d=R-L;
        float d2 = d/2f;

        if(d>0f)
        {
            //previousPos = position;
            position += (direction/L) * d2;
           // b.previousPos = b.position;
            b.position -= (direction/L) * d2;
        }
    }
}



