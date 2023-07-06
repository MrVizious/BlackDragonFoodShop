using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityMethods;
using ExtensionMethods;
using Sirenix.OdinInspector;

public class ConeVisionShaper : MonoBehaviour
{
    public int index = 0;
    public float radius = 1f, totalAngle = 45f;
    // Number of points between the tow main delimiters
    public int resolution = 0;
    public LayerMask collideWith;
    private MeshFilter filter;
    private Mesh _mesh;
    private void Awake()
    {
        filter = GetComponent<MeshFilter>();
        _mesh = new Mesh();
        _mesh.MarkDynamic();
    }

    void Update()
    {
        SetPoints();
    }

    private void SetPoints()
    {


        filter.mesh = GenerateCollisionPoints(GenerateIdealPoints());
        //GenerateIdealPoints();
        //filter.mesh = _mesh;
    }

    private Vector3[] GenerateIdealPoints()
    {
        List<Vector3> idealPoints = new List<Vector3>();
        // Add p0
        idealPoints.Add(Vector3.zero);
        float initialAngle = -90f - totalAngle / 2f;
        // Add p1
        idealPoints.Add(Math.PolarToCartesianClockwise(radius, initialAngle));

        float angleBetweenInnerPoints = totalAngle / (1 + resolution);
        for (int i = 0; i < resolution; i++)
        {
            idealPoints.Add((Vector3)Math.PolarToCartesianClockwise(radius, initialAngle + (i + 1) * angleBetweenInnerPoints));
        }
        // Add pN
        idealPoints.Add((Vector3)Math.PolarToCartesianClockwise(radius, initialAngle + totalAngle));
        _mesh.vertices = idealPoints.ToArray();

        List<int> triangles = new List<int>();
        for (int i = 1; i < idealPoints.Count - 1; i++)
        {
            triangles.Add(0);
            triangles.Add(i);
            triangles.Add(i + 1);
        }

        _mesh.triangles = triangles.ToArray();
        return idealPoints.ToArray();
    }

    private Mesh GenerateCollisionPoints(Vector3[] idealPoints)
    {
        List<Vector3> collisionPoints = new List<Vector3>();
        collisionPoints.Add(idealPoints[0]);
        Vector3 worldInitialPoint = transform.TransformPoint(idealPoints[0]);

        for (int i = 1; i < idealPoints.Length; i++)
        {
            Vector3 point = idealPoints[i];
            Vector3 worldPoint = transform.TransformPoint(point);
            //float worldRadius = transform.TransformVector(transform.right * radius).magnitude;

            RaycastHit2D hit = Physics2D.Raycast(worldInitialPoint, worldPoint - worldInitialPoint, radius, collideWith);

            // If it hits something...
            if (hit.collider != null)
            {
                collisionPoints.Add(transform.InverseTransformPoint(hit.point));
            }
            else
            {
                collisionPoints.Add(point);
            }
        }
        _mesh.vertices = collisionPoints.ToArray();


        return _mesh;
    }
}
