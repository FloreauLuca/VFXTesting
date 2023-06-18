using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform fromAnchor;
    [SerializeField] private Transform toAnchor;
    private void OnTriggerEnter(Collider collision)
    {
        UnityEngine.Debug.Log(collision);
        if (collision.GetComponent<Camera>())
        {
            collision.transform.position = (collision.transform.position - fromAnchor.position) + toAnchor.position;
        }
    }
}
