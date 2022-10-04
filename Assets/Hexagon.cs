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
        // Notice that depending on engine requirements all this stuff is essentially static / single setup.
        // Setup plane equations from 3 of the hexagon points.
        // This needs done only once for all hexagons as we work in local space.
        var euler = transform.rotation.eulerAngles;
        
        // If you want flat top hexagons and don't want to rotate the hexagon/grid object, add 30 to this.
        var angleOffsetDegrees = euler.z;

        // One of the corner points
        var p = new Vector3(0, radius, 0); // texture is pointy top hexagon
        // Rotate into position for the 3 points we want to generate the 2 planes.
        var p0 = p.Rotated(0, 0, angleOffsetDegrees);
        var p1 = p.Rotated(0, 0, angleOffsetDegrees - 60);
        var p2 = p.Rotated(0, 0, angleOffsetDegrees - 120);
        // Find normals for the two planes
        var n0 = ((p0 + p1) * .5f).normalized;
        var n1 = ((p1 + p2) * .5f).normalized;

        // Generate the planes.
        plane0 = new Plane(n0, p0);
        plane1 = new Plane(n1, p1);

        cursorRenderer = cursor.GetComponent<Renderer>();
    }

    private void Update()
    {
        var cursorPosLocal = transform.InverseTransformPoint(cursor.transform.position);
        cursorRenderer.material = IsInside(cursorPosLocal) ? insideMaterial : outsideMaterial;
    }

    // This is the run time meat of the problem.
    // Absolute the local point into the planes quadrant.
    // Two dot products and additions for the plane checks.
    // Very fast. No trigonometry required.
    private bool IsInside(Vector3 pointLocal)
    {
        // Get the absolute point within the quadrant our two planes are in.
        var p = new Vector3(Mathf.Abs(pointLocal.x), Mathf.Abs(pointLocal.y), Mathf.Abs(pointLocal.z));
        // Check the planes and early return if outside.
        if (plane0.GetSide(p)) return false;
        return !plane1.GetSide(p);
    }
}