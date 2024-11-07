using UnityEngine;

public class SpritePos : MonoBehaviour
{
    Transform child;
    void Start()
    {
         child = transform.GetChild(1);

    }

    void Update()
    {
        // Check if the y-rotation of this object is 180 degrees (with a small tolerance)
        if (Mathf.Abs(transform.eulerAngles.y - 180f) < 0.1f)
        {
            // Ensure the object has at least 2 children (index 1 exists)
            if (transform.childCount > 1)
            {
                // Get the child at index 1
                Transform child = transform.GetChild(1);

                // Change the position's y value to 0.01 while keeping x and z unchanged
                Vector3 newPosition = child.localPosition;
                newPosition.z = 0.01f;
                child.localPosition = newPosition;
            }
        }
        else
        {
            Vector3 newPosition = child.localPosition;
            newPosition.z = -0.01f;
            child.localPosition = newPosition;
        }
    }
}
