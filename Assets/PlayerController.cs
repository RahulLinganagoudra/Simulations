//using UnityEngine;

//public class PlayerController : MonoBehaviour
//{
//    Rigidbody2D rb;
//    LineRenderer lineRenderer;
//    Camera cam;

//    Vector2 hookPoint = Vector2.up * 5;
//    Vector3 mousePos;
//    float horizontalInput;
    
//    bool holding;
//    // Start is called before the first frame update
//    void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        lineRenderer = GetComponent<LineRenderer>();
//        cam = Camera.main;

       
//        lineRenderer.positionCount = points.Length;
//    }

//    // Update is called once per frame
//    void Update()
//    {

//        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
//        mousePos.z = 0;
//        horizontalInput = Input.GetAxis("Horizontal");

//        UpdateHookPoint();

//        if (holding)
//        {
//            transform.position += Vector3.right * horizontalInput * Time.deltaTime*.3f;
//            points[points.Length - 1].position = transform.position;


            
//            for (int i = 0; i < lineRenderer.positionCount; i++)
//            {
//                lineRenderer.SetPosition(i, points[i].position);
//            }


//            transform.position = points[points.Length - 1].position;
//        }
//        else
//        {
//            Vector3 velocity = points[points.Length - 1].position - points[points.Length - 1].previousPos;
//            velocity.y += -10 * Time.deltaTime * Time.deltaTime;
//            transform.position += velocity;
//        }
//    }

//    void UpdateHookPoint()
//    {
//        if (Input.GetMouseButtonDown(0))
//        {
//            holding = true;
//            if (!lineRenderer.enabled) lineRenderer.enabled = true;

//            hookPoint = mousePos;
//            for (int i = 0; i < points.Length; i++)
//            {
//                Vector3 pos = Vector3.Lerp(hookPoint, transform.position, i / 10f);

//                points[i].position = pos;
//                points[i].previousPos = pos;
//            }
//            for(int i = 0;i<lines.Length; i++)
//            {
//                lines[i].UpdateDistance();
//            }
//        }
//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//            if (holding)
//            {
//                lineRenderer.enabled = false;
//            }
//            holding = false;

//        }
//    }

//}
