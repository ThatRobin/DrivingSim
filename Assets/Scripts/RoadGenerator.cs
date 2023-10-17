using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour {

    public int seed;

    public GameObject node;
    public Dictionary<int, List<GameObject>> nodes = new();

    public int length;


    public float duration = 2f;
    public int resolution = 10;
    public float height = 2f;
    void Start() {
        Random.InitState(seed);

        for (int i = 0; i <= length; i++) {
            List<GameObject> tempNodes = new();

            int rand = (int)(Random.Range(1f, 6f)/3f);

            if(nodes.Count == 0) {
                rand = 0;
            }
            float offset = 0f;
            if(rand != 0) {
                offset = (5f / (float)rand);
            }

            for (int x = 0; x <= rand; x++) {
                GameObject tempNode = Instantiate(node, new Vector3(10 * i, Mathf.Sin(i / 200f) * 100f, (5 * x) - offset/2f), Quaternion.identity, transform);

                tempNodes.Add(tempNode);
            }

            nodes.Add(i, tempNodes);
        }
    }

    void OnDrawGizmosSelected() {
        for(int i = 0; i <= nodes.Count; i++) {
            if (nodes.ContainsKey(i+1)) {
                if (nodes[i].Count == nodes[i+1].Count) {
                    for(int j = 0; j < nodes[i].Count; j++) {
                        DrawLerpedLine(nodes[i][j].transform.position, nodes[i + 1][j].transform.position);
                    }
                } else if(nodes[i].Count > nodes[i + 1].Count) {
                    for (int j = 0; j < nodes[i].Count; j++) {
                        DrawLerpedLine(nodes[i][j].transform.position, nodes[i + 1][0].transform.position);
                    }
                } else if (nodes[i].Count < nodes[i + 1].Count) {
                    for (int j = 0; j < nodes[i+1].Count; j++) {
                        DrawLerpedLine(nodes[i][0].transform.position, nodes[i+1][j].transform.position);
                    }
                }
            }
        }
    }

    private void DrawLerpedLine (Vector3 startPoint, Vector3 endPoint)
    {
        Vector3[] points = new Vector3[resolution + 1];

        for (int i = 0; i <= resolution; i++)
        {
            float t = i / (float)resolution;
            Vector3 currentPoint = Vector3.SlerpUnclamped(startPoint, endPoint, t);
            if (i > 0)
            {
                Vector3 previousPoint = Vector3.SlerpUnclamped(startPoint, endPoint, (i - 1) / (float)resolution);
                Debug.DrawLine(previousPoint, currentPoint, Color.yellow);
            }
        }
    }

    private Vector3 CalculatePointOnParabolicPath(Vector3 start, Vector3 end, float t)
    {
        Vector3 midPoint = (start + end) / 2f;

        Vector3 controlPoint = midPoint + Vector3.up * height;

        float u = 1 - t;
        Vector3 p = u * u * start + 2 * u * t * controlPoint + t * t * end;

        return p;
    }

}
