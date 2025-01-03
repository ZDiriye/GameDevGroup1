using UnityEngine;

public class FixRotation : MonoBehaviour
{
    void Start()
    {
        transform.eulerAngles = new Vector3(-90, 0, 0);
    }
}
