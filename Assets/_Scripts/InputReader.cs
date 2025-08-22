using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    [Header("Input Events")]
    public System.Action<Vector2> OnMoveInput;
    
    [Header("Input Values")]
    public Vector2 MovementInput { get; private set; }
    
    [Header("Debug")]
    public bool showDebugInfo = false;
    
    private InputSystem_Actions inputActions;
    
    void Awake()
    {
        // Input Actions'ı oluştur
        inputActions = new InputSystem_Actions();
    }
    
    void OnEnable()
    {
        // Input Actions'ları aktif et
        inputActions.Player.Enable();
        
        // Event'leri bağla
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
    }
    
    void OnDisable()
    {
        // Event'leri ayır
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        
        // Input Actions'ları deaktif et
        inputActions.Player.Disable();
    }
    
    void OnMove(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
        OnMoveInput?.Invoke(MovementInput);
    }
    
    void OnDestroy()
    {
        inputActions?.Dispose();
    }
    
    // Debug için
    void OnGUI()
    {
        if (showDebugInfo)
        {
            GUILayout.BeginArea(new Rect(10, 10, 250, 40));
            GUILayout.Label($"Movement: {MovementInput}");
            GUILayout.EndArea();
        }
    }
}