using UnityEngine;

public class CursorChange : MonoBehaviour
{
    [SerializeField] Texture2D cursorImg;
    void Start()
    {
        
    }
    void Update()
    {
        Cursor.SetCursor(cursorImg, Vector2.zero, CursorMode.ForceSoftware);
    }
}
