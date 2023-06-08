using UnityEngine;

public class CS_CameraFade : MonoBehaviour
{
    public Color fadeColor = Color.black;
    private Texture2D texture;

    [SerializeField][Range(0f, 1f)] float newAlpha = 0;
    [SerializeField] float speed = 1;

    int direction = 0; //-1 fade out || 1 fade in

    private void Start()
    {
        texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, newAlpha));
        texture.Apply();
    }

    private void Update()
    {
        if(direction != 0)
        {
            newAlpha = newAlpha + (Time.deltaTime * direction * Mathf.Abs(speed));
            if(newAlpha >= 1 || newAlpha <= 0)
            {
                newAlpha = Mathf.Clamp01(newAlpha);
                direction = 0;
            }
        }
    }

    public void OnGUI()
    {
        if (newAlpha > 0f) GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
        texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, newAlpha));
        texture.Apply();
    }
   
    public bool InFade() { return direction != 0; }

    public void FadeOut()
    {
        direction = -1;
    }

    public void FadeIn()
    {
        direction = 1;
    }
}