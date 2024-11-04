using UnityEngine;

public class Star : MonoBehaviour
{
    SpriteRenderer sprite;
    Vector2 dir;
    public float speed = 0.1f;
    private float minSize = 0.1f;
    private float maxSize = 0.5f;
    private float Sizespeed = 1.0f;
    public Color[] colors;
    public float colorSpeed = 5f;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        dir = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        float size = Random.Range(minSize, maxSize);
        transform.localScale = new Vector2(size, size);
        sprite.color= colors[Random.Range(0, colors.Length)];
    }

    void Update()
    {
        transform.Translate(dir * speed);
        transform.localScale = Vector2.Lerp(transform.localScale, Vector2.zero, Time.deltaTime* Sizespeed);
        Color color = sprite.color;
        color.a = Mathf.Clamp(sprite.color.a, 0, Time.deltaTime * colorSpeed);
        sprite.color = color;
        if(sprite.color.a <= 0.01f)
        {
            Destroy(gameObject);
        }
    }
}
