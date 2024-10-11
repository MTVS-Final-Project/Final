using UnityEngine;
using UnityEngine.TextCore.Text;

public class DestroyLoad : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

}
