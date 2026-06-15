using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [Header("Drag next waypoints")]
    public List<Waypoint> nextWaypoints = new List<Waypoint>();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.5f);

        if (nextWaypoints == null) return;

        Gizmos.color = Color.yellow;
        foreach (Waypoint neighbour in nextWaypoints)
        {
            if (neighbour != null)
            {
                Gizmos.DrawLine(transform.position, neighbour.transform.position);
            }
        }
    }
}
