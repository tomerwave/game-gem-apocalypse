using UnityEngine;

public class LaserEmitter : MonoBehaviour
{
    [Header("Laser Settings")]
    public float maxDistance = 100f;
    public LayerMask collisionMask = ~0; // Collides with everything by default
    
    [Header("Visual Settings")]
    public Color laserColor = Color.red;
    public float laserWidth = 0.05f;
    public Material laserMaterial;
    
    private LineRenderer lineRenderer;
    private Vector3 hitPoint;
    private bool isHitting;
    
    void Start()
    {
        SetupLineRenderer();
    }
    
    void SetupLineRenderer()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;
        
        if (laserMaterial != null)
        {
            lineRenderer.material = laserMaterial;
        }
        else
        {
            // Create a simple unlit material if none assigned
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        }
        
        lineRenderer.startColor = laserColor;
        lineRenderer.endColor = laserColor;
    }
    
    void Update()
    {
        UpdateLaser();
    }
    
    void UpdateLaser()
    {
        Vector3 startPoint = transform.position;
        Vector3 direction = transform.forward;
        
        // Raycast to find collision point
        if (Physics.Raycast(startPoint, direction, out RaycastHit hit, maxDistance, collisionMask))
        {
            hitPoint = hit.point;
            isHitting = true;
        }
        else
        {
            // No collision - laser extends to max distance
            hitPoint = startPoint + direction * maxDistance;
            isHitting = false;
        }
        
        // Update line renderer positions
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, hitPoint);
    }
    
    public Vector3 GetHitPoint()
    {
        return hitPoint;
    }
    
    public bool IsHitting()
    {
        return isHitting;
    }
    
    public RaycastHit? GetHitInfo()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, maxDistance, collisionMask))
        {
            return hit;
        }
        return null;
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = laserColor;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2f);
    }
}
