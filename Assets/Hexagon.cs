using UnityEngine;

// Our Hexagon is aligned in local 3D space around the [0,0,1] plane normal
public class Hexagon : MonoBehaviour
{
    [SerializeField] private float radius = 0.5f;
    [SerializeField] private GameObject cursor;
    [SerializeField] private Material insideMaterial;
    [SerializeField] private Material outsideMaterial;
    private Renderer cursorRenderer;
    private readonly Plane[] plane = new Plane[2];

    private void Awake()
    {
        // Notice that depending on engine requirements all this stuff is essentially static / single setup.
        // Setup plane equations from 3 of the hexagon points.
        // This needs done only once for all hexagons as we work in local space.

        // Rotate clockwise 60 degrees around Z to keep the points in the +ve quadrant for the absolute checks
        var pointRotation = Quaternion.Euler(Vector3.back * 60);

        // Top center corner point, assuming texture / hex grid is set up this way.
        // If it's a flat top hexagon rotate this first point by 30 degrees around the (local) origin. 
        var p0 = new Vector3(0, radius, 0); // texture is pointy top hexagon

        // Rotate into position for the 3 points we want to generate the 2 planes.
        var p1 = pointRotation * p0;
        var p2 = pointRotation * p1;

        // Find normals for the two planes.
        // From the origin this is the two middle of the end points normalized.
        var n0 = ((p0 + p1) * .5f).normalized;
        var n1 = ((p1 + p2) * .5f).normalized;

        // Generate the planes.
        plane[0] = new Plane(n0, p0);
        plane[1] = new Plane(n1, p1);

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
    // Very fast. No trigonometry required in 3D space.
    // Total cost - 3 Abs, 2 Dot Products, 2 Adds, 1 or 2 comparisons per hexagon.
    private bool IsInside(Vector3 pointLocal)
    {
        // Get the absolute point within the quadrant our two planes are in.
        pointLocal.x = Mathf.Abs(pointLocal.x);
        pointLocal.y = Mathf.Abs(pointLocal.y);
        pointLocal.z = Mathf.Abs(pointLocal.z);
        
        // Check the planes and return if on positive side of either.
        return !plane[0].GetSide(pointLocal) && !plane[1].GetSide(pointLocal);
    }
}