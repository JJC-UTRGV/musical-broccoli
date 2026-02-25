using UnityEngine;

public class WallDestructionEffect : MonoBehaviour
{
    [SerializeField] private float lifetime = 0.6f;
    [SerializeField] private float scaleMax = 1.5f;
    [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    [SerializeField] private Color flashColor = new Color(1f, 0.8f, 0.3f, 1f);

    private SpriteRenderer spriteRenderer;
    private float elapsedTime = 0f;
    private Color originalColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
        else
        {
            // Create a simple sprite renderer with a white square
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            Texture2D texture = new Texture2D(32, 32, TextureFormat.RGBA32, false);
            for (int x = 0; x < 32; x++)
                for (int y = 0; y < 32; y++)
                    texture.SetPixel(x, y, flashColor);
            texture.Apply();
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 32, 32), Vector2.one * 0.5f, 32);
            spriteRenderer.sprite = sprite;
            originalColor = flashColor;
        }

        // Add particle system if available
        ParticleSystem particles = GetComponent<ParticleSystem>();
        if (particles != null)
            particles.Play();

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        float progress = elapsedTime / lifetime;

        // Scale animation
        float scale = Mathf.Lerp(scaleMax, 0.5f, scaleCurve.Evaluate(progress));
        transform.localScale = Vector3.one * scale;

        // Fade out
        if (spriteRenderer != null)
        {
            Color color = originalColor;
            color.a = Mathf.Lerp(1f, 0f, progress);
            spriteRenderer.color = color;
        }
    }
}

