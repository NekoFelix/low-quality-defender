using UnityEngine;

public class CursorEnabler : MonoBehaviour
{
    [SerializeField] bool _showCursor = true;
    void Start()
    {
        Cursor.visible = _showCursor; 
    }
}
