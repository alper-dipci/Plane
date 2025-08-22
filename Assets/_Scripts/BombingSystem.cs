using UnityEngine;
using System.Collections;

public class BombingSystem : MonoBehaviour
{
    [Header("Bombing Settings")]
    public float bombInterval = 1.5f;
    public float explosionRadius = 3f;
    public float explosionDamage = 25f;
    public LayerMask enemyLayerMask = -1;
    
    [Header("Visual Effects")]
    public GameObject explosionEffectPrefab;
    
    [Header("Animation")]
    public float explosionScaleAnimation = 1.2f;
    public float explosionAnimationDuration = 0.3f;
    public AnimationCurve explosionCurve;
    
    [Header("Upgrades")]
    [Range(0.5f, 10f)]
    public float radiusMultiplier = 1f;
    [Range(0.1f, 5f)]
    public float damageMultiplier = 1f;
    [Range(0.1f, 3f)]
    public float intervalMultiplier = 1f;
    
    [Header("Debug")]
    public bool showDebugInfo = false;
    
    private float lastBombTime;
    private Camera mainCamera;
    private int enemiesHitLastBomb = 0;
    
    void Start()
    {
        mainCamera = Camera.main;
        lastBombTime = Time.time;
    }
    
    void Update()
    {
        // Otomatik bomba atma
        if (Time.time - lastBombTime >= (bombInterval * intervalMultiplier))
        {
            DropBomb();
            lastBombTime = Time.time;
        }
    }
    
    public void DropBomb()
    {
        float currentRadius = explosionRadius * radiusMultiplier;
        float currentDamage = explosionDamage * damageMultiplier;
        
        // Çember içindeki düşmanları bul
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, currentRadius, enemyLayerMask);
        
        enemiesHitLastBomb = 0;
        
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                //give damage
            }
        }
        
        // Explosion effect'i spawn et
        if (explosionEffectPrefab != null)
        {
            GameObject explosion = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            
            // Explosion'ı radius'a göre scale et
            explosion.transform.localScale = Vector3.one * currentRadius;
            
            // Explosion animasyonu
            AnimateExplosion(explosion);
            
            // Explosion'ı otomatik yok et
            Destroy(explosion, explosionAnimationDuration + 0.5f);
        }
        
    }
    
    private void AnimateExplosion(GameObject explosion)
    {
        
    }
    
    // IEnumerator FlashRadiusIndicator()
    // {
    //     Color originalColor = radiusIndicator.color;
    //     Color flashColor = new Color(originalColor.r, originalColor.g, originalColor.b, originalColor.a * 2f);
    //     
    //     // Flash in
    //     radiusIndicator.color = flashColor;
    //     yield return new WaitForSeconds(0.1f);
    //     
    //     // Flash out
    //     radiusIndicator.color = originalColor;
    // }
    
    // Public methods for upgrades
    public void UpgradeRadius(float multiplier)
    {
        radiusMultiplier = multiplier;
    }
    
    public void UpgradeDamage(float multiplier)
    {
        damageMultiplier = multiplier;
    }
    
    public void UpgradeInterval(float multiplier)
    {
        intervalMultiplier = multiplier;
    }
    
    // Debug ve editor için
    void OnDrawGizmosSelected()
    {
        if (showDebugInfo)
        {
            // Explosion radius'u göster
            Gizmos.color = Color.red;
            
            // Enemy detection layer'ı göster
            Gizmos.color = Color.yellow;
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius * radiusMultiplier, enemyLayerMask);
            foreach (Collider2D enemy in enemies)
            {
                Gizmos.DrawWireSphere(enemy.transform.position, 0.2f);
            }
        }
    }
    
    void OnGUI()
    {
        if (showDebugInfo && Application.isPlaying)
        {
            GUILayout.BeginArea(new Rect(10, 150, 300, 120));
            GUILayout.Label($"Current Radius: {explosionRadius * radiusMultiplier:F1}");
            GUILayout.Label($"Current Damage: {explosionDamage * damageMultiplier:F1}");
            GUILayout.Label($"Current Interval: {bombInterval * intervalMultiplier:F1}s");
            GUILayout.Label($"Enemies Hit Last Bomb: {enemiesHitLastBomb}");
            GUILayout.EndArea();
        }
    }
}