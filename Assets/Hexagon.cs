using MysticCoderExtenstions;
using UnityEngine;
using UnityEngine.Serialization;

public class Hexagon : MonoBehaviour
{
    [SerializeField] private GameObject cursor;
    [SerializeField] private float radius = 0.5f;
    [SerializeField] private Material insideMaterial;
    [SerializeField] private Material outsideMaterial;
    private Renderer cursorRenderer;
    private Plane plane0, plane1;

    private void Awake()
    {
        // Setup plane equations from 3 of the hexagon points.
        // This needs done only once for all hexagons as we work in local space.
        var euler = transform.rotation.eulerAngles;
        
        // If you want flat top hexagons and don't want to rotate the hexagon/grid object, add 30 to this.
        var angleOffsetDegrees = euler.z;

        // One of the corner points
        var p = new Vector3(0, radius, 0); // texture is pointy top hexagon
        var p0 = p.Rotated(0, 0, angleOffsetDegrees);
        var p1 = p.Rotated(0, 0, angleOffsetDegrees - 60);
        var p2 = p.Rotated(0, 0, angleOffsetDegrees - 120);
        var n0 = ((p0 + p1) * .5f).normalized;
        var n1 = ((p1 + p2) * .5f).normalized;

        plane0 = new Plane(n0, p0);
        plane1 = new Plane(n1, p1);

        cursorRenderer = cursor.GetComponent<Renderer>();
    }

    private void Update()
    {
        var cursorPosLocal = transform.InverseTransformPoint(cursor.transform.position);
        cursorRenderer.material = IsInside(cursorPosLocal) ? insideMaterial : outsideMaterial;
    }

    private bool IsInside(Vector3 pointLocal)
    {
        var p = new Vector3(Mathf.Abs(pointLocal.x), Mathf.Abs(pointLocal.y), Mathf.Abs(pointLocal.z));

        // Debug.Log($"plane0: {plane0.ToString()}\nplane1: {plane1.ToString()}");
        // Debug.Log($"ina:{ina} inb:{inb}");
        
        var out0 = plane0.GetSide(p);
        if (out0) return false; // early out if outside this plane
        var out1 = plane1.GetSide(p);
        return !out1;
        // return !out0 && !out1;
    }
}