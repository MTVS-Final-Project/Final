using UnityEngine;

public class ParentManager : MonoBehaviour
{
    public bool allDeactivated;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        allDeactivated = CheckChildren();
    }

    private bool CheckChildren()
    {
      foreach (Transform child in transform)
        {
            if (child.gameObject.activeInHierarchy)
            {
                return false;
            }
        }
        return true;
    }
}
