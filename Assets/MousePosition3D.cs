using UnityEngine;

public class MousePosition3D : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask layerMask;
    
    private void Update()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var rayCastHit, float.MaxValue, layerMask))
        {
            transform.position = rayCastHit.point;
        }
    }
}
