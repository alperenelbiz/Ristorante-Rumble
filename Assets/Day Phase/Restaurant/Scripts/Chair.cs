using UnityEngine;

public class Chair : MonoBehaviour
{
    public bool IsOccupied = false;

    private void OnDrawGizmos()
    {
        Gizmos.color = IsOccupied ? Color.red : Color.green;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}
