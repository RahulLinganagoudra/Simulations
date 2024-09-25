using UnityEngine;
using Random = UnityEngine.Random;


namespace Assets
{
    public class SpatialTest : MonoBehaviour
    {
        SpatialHash hash;
        Vector2[] points;
        public int size = 10;
        public uint cellSize;
        private void Start()
        {
            hash = new SpatialHash(cellSize);
            points = new Vector2[size];

            for (int i = 0; i < size; i++)
            {
                points[i] = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
                hash.Insert(points[i], i);
            }

        }

        private void OnDrawGizmos()
        {

            if (points != null&&points.Length>0)
            {
                for (int i = 0; i < size; i++)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawSphere(points[i], .2f);
                }
                Gizmos.color = Color.red;

                hash.DrawGrid();

                Gizmos.color = Color.red;
                Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                
                hash.GetGridXY(mousepos,out int mouseGridX,out int mouseGridY);


                foreach (var item in hash.GetCellElements(mouseGridX, mouseGridY, 5))
                {
                    Gizmos.DrawSphere(points[item], .2f);
                }

            }
        }

    }
}
