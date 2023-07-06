using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityMethods;
using Sirenix.OdinInspector;

[RequireComponent(typeof(PolygonCollider2D))]
public class ConeVisionShaper : MonoBehaviour
{
    [OnValueChanged("SetPoints")]
    public float radius = 1f, totalAngle = 45f;
    // Number of points between the tow main delimiters
    [OnValueChanged("SetPoints")]
    public int resolution = 0;
    private PolygonCollider2D col;
    private void Awake()
    {
        col = GetComponent<PolygonCollider2D>();
    }

    void Start()
    {
        SetPoints();
    }

    private void SetPoints()
    {
        col.points = GeneratePoints();
    }

    private Vector2[] GeneratePoints()
    {
        List<Vector2> pointsToReturn = new List<Vector2>();
        // Add p0
        pointsToReturn.Add(Vector2.zero);
        float initialAngle = -90f - totalAngle / 2f;
        // Add p1
        pointsToReturn.Add(Math.PolarToCartesianClockwise(radius, initialAngle));

        float angleBetweenInnerPoints = totalAngle / (1 + resolution);
        for (int i = 0; i < resolution; i++)
        {
            pointsToReturn.Add(Math.PolarToCartesianClockwise(radius, initialAngle + (i + 1) * angleBetweenInnerPoints));
        }
        pointsToReturn.Add(Math.PolarToCartesianClockwise(radius, totalAngle / 2f - 90f));
        return pointsToReturn.ToArray();
    }
}
