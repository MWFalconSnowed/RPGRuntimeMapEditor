using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Texture2D idleSheet;
    public Texture2D walkSheet;
    public float frameRate = 10f;

    private SpriteRenderer spriteRenderer;
    private Sprite[] idleFrames;
    private Sprite[] walkFrames;

    private float timer;
    private int currentFrame;
    private bool walking;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        idleFrames = SliceSpriteSheet(idleSheet, 5);  // 5 frames d'idle
        walkFrames = SliceSpriteSheet(walkSheet, 8);  // 8 frames de marche
    }

    void Update()
    {
        // Détection simple du mouvement
        walking = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;

        timer += Time.deltaTime;
        if (timer >= 1f / frameRate)
        {
            timer = 0f;
            currentFrame = (currentFrame + 1) % (walking ? walkFrames.Length : idleFrames.Length);
            spriteRenderer.sprite = walking ? walkFrames[currentFrame] : idleFrames[currentFrame];
        }

        // Flip horizontal si on va à gauche
        if (Input.GetAxisRaw("Horizontal") < 0)
            spriteRenderer.flipX = true;
        else if (Input.GetAxisRaw("Horizontal") > 0)
            spriteRenderer.flipX = false;
    }

    Sprite[] SliceSpriteSheet(Texture2D sheet, int frameCount)
    {
        Sprite[] frames = new Sprite[frameCount];
        int width = sheet.width / frameCount;
        int height = sheet.height;

        for (int i = 0; i < frameCount; i++)
        {
            Rect rect = new Rect(i * width, 0, width, height);
            frames[i] = Sprite.Create(sheet, rect, new Vector2(0.5f, 0f), 100f);
        }

        return frames;
    }
}
