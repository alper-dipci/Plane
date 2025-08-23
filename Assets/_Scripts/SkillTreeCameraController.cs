using UnityEngine;

public class SkillTreeCameraController : MonoBehaviour
{
    [Header("Pan")]
    public float panSpeed = 1f;

    [Header("Zoom")]
    public float zoomSpeed = 5f;
    public float minZoom = 2f;
    public float maxZoom = 15f;

    [Header("Bounds")]
    public RectTransform skillTreeArea; // Canvas içindeki skill tree alanı

    private Camera cam;
    private Vector3 lastMousePosition;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        HandlePan();
        HandleZoom();
        ClampCameraPosition();
    }

    private void HandlePan()
    {
        if (Input.GetMouseButtonDown(0))
            lastMousePosition = Input.mousePosition;

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;

            // Zoom ile ölçekli pan speed
            float zoomFactor = cam.orthographicSize / 5f; // 5f normal zoom referans değeri
            Vector3 move = new Vector3(-delta.x, -delta.y, 0) * panSpeed * zoomFactor * Time.deltaTime;

            cam.transform.Translate(move, Space.Self);

            lastMousePosition = Input.mousePosition;
        }
    }


    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            float newZoom = cam.orthographicSize - scroll * zoomSpeed;
            newZoom = Mathf.Clamp(newZoom, minZoom, maxZoom);

            // Kameranın yeni yarı boyutları
            float camHalfHeight = newZoom;
            float camHalfWidth = camHalfHeight * cam.aspect;

            // Canvas sınırları
            Vector2 areaSize = skillTreeArea.rect.size;
            float minX = -areaSize.x / 2 + camHalfWidth;
            float maxX =  areaSize.x / 2 - camHalfWidth;
            float minY = -areaSize.y / 2 + camHalfHeight;
            float maxY =  areaSize.y / 2 - camHalfHeight;

            // Eğer mevcut pozisyon ile canvas sınırlarını aşıyorsa zoom yapma
            if (cam.transform.position.x < minX || cam.transform.position.x > maxX ||
                cam.transform.position.y < minY || cam.transform.position.y > maxY)
            {
                return; // zoom iptal
            }

            cam.orthographicSize = newZoom;
        }
    }


    private void ClampCameraPosition()
    {
        if (skillTreeArea == null) return;

        Vector2 areaSize = skillTreeArea.rect.size;

        // Kamera yarı boyutları
        float camHalfHeight = cam.orthographicSize;
        float camHalfWidth = camHalfHeight * cam.aspect;

        // Clamp değerlerini zoom’a duyarlı yap
        float minX = -areaSize.x / 2 + camHalfWidth;
        float maxX =  areaSize.x / 2 - camHalfWidth;
        float minY = -areaSize.y / 2 + camHalfHeight;
        float maxY =  areaSize.y / 2 - camHalfHeight;

        Vector3 pos = cam.transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        cam.transform.position = pos;
    }
}
