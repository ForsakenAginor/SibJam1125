using UnityEngine;

public class SpiderPoint : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 2f);
    }
}


