using UnityEngine;
using UnityEngine.UI;

public class UIScrollTexture : MonoBehaviour
{
    [SerializeField] private Image image; // UI Image
    [SerializeField] private Vector2 scrollSpeed = new Vector2(0.1f, 0f); // X ve Y hız

    private Material mat;

    private void Awake()
    {
        // Image material’ının kopyasını alıyoruz, yoksa tüm UI Image’lar etkilenir
        mat = Instantiate(image.material);
        image.material = mat;
    }

    private void Update()
    {
        Vector2 offset = mat.mainTextureOffset;
        offset += scrollSpeed * Time.deltaTime;
        mat.mainTextureOffset = offset;
    }
}