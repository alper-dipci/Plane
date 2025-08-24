using UnityEngine;
using UnityEngine.UI;

public class UIScrollTexture : MonoBehaviour
{
    [SerializeField] private RawImage image; // UI Image
    [SerializeField] private Vector2 scrollSpeed = new Vector2(0.1f, 0f); // X ve Y hız

    private void Update()
    {
        image.uvRect = new Rect(
            image.uvRect.position + scrollSpeed * Time.deltaTime,
            image.uvRect.size);
    }
}