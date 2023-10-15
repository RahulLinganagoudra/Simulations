using UnityEngine;

public class Wind 
{
    public Vector3 wind => GetWind();

    Vector3 endPoint;
    [SerializeField]private float directionChangeSpeed=.3f;
    public float windSpeed=1f;

    // Update is called once per frame
    Vector3 GetWind()
    {
        float theta = Time.time * directionChangeSpeed * Time.deltaTime;
        //endPoint = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta));
        endPoint = Vector3.right;
        return endPoint.normalized * Mathf.Clamp01((Mathf.Sin(Time.time*directionChangeSpeed) * windSpeed));
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(Vector3.zero, endPoint);
    }
}
