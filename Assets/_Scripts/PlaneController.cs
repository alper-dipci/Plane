using UnityEngine;

public class PlaneController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationLerpSpeed = 10f;
    public float acceleration = 10f;
    public float deceleration = 15f;
    
    [Header("References")]
    public Transform planeVisual; // Uçağın sprite'ının olduğu child object
    
    [Header("Debug")]
    public bool showDebugInfo = false;
    
    private InputReader inputReader;
    private Vector2 currentMovementInput;
    private Vector2 currentVelocity;
    private Vector2 smoothMovementInput;
    
    void Start()
    {
        // InputReader component'ini al
        inputReader = GetComponent<InputReader>();
        
        if (inputReader == null)
        {
            Debug.LogError("InputReader component not found!");
            return;
        }
        
        // Input event'lerini dinle
        inputReader.OnMoveInput += HandleMovementInput;
        
        // Eğer planeVisual atanmadıysa kendi transform'unu kullan
        if (planeVisual == null)
            planeVisual = transform;
    }
    
    void Update()
    {
        HandleMovement();
        HandleRotation();
    }
    
    void HandleMovementInput(Vector2 input)
    {
        currentMovementInput = input;
    }
    
    void HandleMovement()
    {
        // Smooth input geçişi için SmoothDamp kullan
        float smoothTime = currentMovementInput.magnitude > 0 ? 1f / acceleration : 1f / deceleration;
        
        smoothMovementInput = Vector2.SmoothDamp(
            smoothMovementInput, 
            currentMovementInput, 
            ref currentVelocity, 
            smoothTime
        );
        
        // Hareketi uygula
        Vector3 movement = new Vector3(smoothMovementInput.x, smoothMovementInput.y, 0) * moveSpeed * Time.deltaTime;
        transform.position += movement;
    }
    
    void HandleRotation()
    {
        // Sadece hareket varsa rotate et
        if (smoothMovementInput.magnitude > 0.1f)
        {
            // Hareket yönüne göre target açıyı hesapla
            float targetAngle = Mathf.Atan2(smoothMovementInput.y, smoothMovementInput.x) * Mathf.Rad2Deg - 90f;
            
            // Mevcut açıyı al
            float currentAngle = planeVisual.eulerAngles.z;
            
            // Smooth rotation uygula
            float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle);
            float newAngle = currentAngle + angleDifference * rotationLerpSpeed * Time.deltaTime;
            
            planeVisual.rotation = Quaternion.AngleAxis(newAngle, Vector3.forward);
        }
    }
    
    void OnDestroy()
    {
        // Event'leri temizle
        if (inputReader != null)
        {
            inputReader.OnMoveInput -= HandleMovementInput;
        }
    }
    
    // Debug için
    void OnDrawGizmos()
    {
        if (showDebugInfo && Application.isPlaying)
        {
            // Hareket yönünü göster
            Gizmos.color = Color.blue;
            Vector3 movementDirection = new Vector3(smoothMovementInput.x, smoothMovementInput.y, 0);
            Gizmos.DrawLine(transform.position, transform.position + movementDirection * 2f);
            
            // Input direction
            Gizmos.color = Color.red;
            Vector3 inputDirection = new Vector3(currentMovementInput.x, currentMovementInput.y, 0);
            Gizmos.DrawLine(transform.position, transform.position + inputDirection * 1.5f);
        }
    }
    
    void OnGUI()
    {
        if (showDebugInfo && Application.isPlaying)
        {
            GUILayout.BeginArea(new Rect(10, 100, 250, 120));
            GUILayout.Label($"Current Input: {currentMovementInput}");
            GUILayout.Label($"Smooth Input: {smoothMovementInput}");
            GUILayout.Label($"Velocity: {currentVelocity}");
            GUILayout.Label($"Speed: {smoothMovementInput.magnitude:F2}");
            GUILayout.EndArea();
        }
    }
}