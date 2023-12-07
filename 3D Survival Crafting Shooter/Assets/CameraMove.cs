using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform CameraPosition;
    // Update is called once per frame
    void Update()
    {
        transform.position = CameraPosition.position;
    }
}
