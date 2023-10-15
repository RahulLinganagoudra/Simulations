using UnityEngine;
using Random = UnityEngine.Random;


namespace Assets
{
    public class SpatialTest : MonoBehaviour
    {
        SpatialHash hash;
        Vector2[] points;
        public uint[] spaceHash;
        public int size = 10;
        public uint cellSize;
        private void Start()
        {
            hash = new SpatialHash();
            hash.cellSize = cellSize;
            points = new Vector2[size];
            spaceHash = new uint[size];

            for (int i = 0; i < size; i++)
            {
                points[i] = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
                spaceHash[i] = hash.GetSpatialHashIndex(points[i].x, points[i].y, cellSize);
                hash.Insert(points[i], i);
            }
            int start = 0;
            uint last = spaceHash[0];
            for (int i = 0; i < size; i++)
            {
                if (last != spaceHash[i])
                {
                    start = i;
                }
                last = spaceHash[i];
            }
        }

       
        int[] GetNeighbouringIndexes(Vector2 pos, int radius)
        {
            int length = radius / (int)cellSize;
            int res = (int)SpatialHash.res;
            int halfRes = (int)SpatialHash.halfRes;
            int count = 0;
            int start = 0;
            int[] indexes;

            if (length == 0)
            {
                indexes = new int[1];
                uint hashcode = hash.GetSpatialHashIndex(pos.x, pos.y, cellSize);
                for (int k = start; k < size; k++)
                {
                    if (hashcode == spaceHash[k])
                    {
                        indexes[count++] = startIndexes[k];
                        break;
                    }
                }
            }
            else
            {
                indexes = new int[4 * (length * length) + (2 * length)]; // (a+b)^2 = a ^2 + b^2 + 2ab

                for (int i = -length; i < length; i++)
                {
                    for (int j = -length; j < length; j++)
                    {
                        int xl = ((int)((halfRes + Mathf.FloorToInt(pos.x)) / cellSize)) + i;
                        int yl = ((int)((halfRes + Mathf.FloorToInt(pos.y)) / cellSize)) + j;

                        int hashcode = res * (xl) + yl;
                        for (int k = start; k < size; k++)
                        {
                            if (hashcode == spaceHash[k])
                            {
                                indexes[count++] = startIndexes[k];
                                start = k;
                                break;
                            }
                        }
                    }
                }
            }
            return indexes;
        }
        private void OnDrawGizmos()
        {

            if (points != null && spaceHash != null)
            {
                for (int i = 0; i < size; i++)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawSphere(points[i], .2f);
                }
                Gizmos.color = Color.red;
                
                Gizmos.color = Color.grey;

                for (int i = -10 + (int)SpatialHash.halfRes; i <= SpatialHash.halfRes; i += (int)cellSize)
                {
                    int s = -10, e = 10;
                    if (i % cellSize == 0)
                    {
                        Gizmos.DrawLine(new(i, s, 0), new(i, e, 0));
                        Gizmos.DrawLine(new(s, i, 0), new(e, i, 0));
                    }

                }

                Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Gizmos.color = Color.red;
                
                if (hash != null)
                    foreach (int item in hash.GetCellElements(mousepos,cellSize))
                    {
                        Gizmos.DrawSphere(points[item], .2f);
                    }

            }
        }

    }
}
