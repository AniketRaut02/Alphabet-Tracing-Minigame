using UnityEngine;
using System.Collections.Generic;

// This script defines a waypoint system component for Unity 2D.
// It allows for the creation of a path using a series of waypoints,
// which can be visualized in the Unity editor using gizmos.

public class WaypointSystem : MonoBehaviour
{
    // A list to hold the waypoints.
    public List<Vector2> waypoints = new List<Vector2>();

    // This variable controls the radius of the gizmo circles for each waypoint.
    public float gizmoRadius = 0.2f;

    // A flag to determine if the gizmos should be drawn.
    public bool drawGizmos = true;

    // The color of the gizmo circles and lines.
    // This can be customized in the Inspector to fit your project's theme.
    public Color gizmoColor = Color.yellow;

    void Awake()
    {
        Debug.Log("WaypointSystem component initialized.");
    }

    private void OnDrawGizmos()
    {
        // Check if gizmos should be drawn to allow for toggling.
        if (!drawGizmos)
        {
            return;
        }

        // Set the color for all gizmos drawn in this method call.
        Gizmos.color = gizmoColor;

        // Draw the waypoints as spheres and lines.
        for (int i = 0; i < waypoints.Count; i++)
        {
            // Get the current waypoint position.
            Vector3 currentWaypoint = transform.position + new Vector3(waypoints[i].x, waypoints[i].y, 0);

            // Draw a wire sphere gizmo at the waypoint position to represent the waypoint.
            Gizmos.DrawWireSphere(currentWaypoint, gizmoRadius);

            // If there's a next waypoint in the list, draw a line to it.
            if (i < waypoints.Count - 1)
            {
                // Get the position of the next waypoint.
                Vector3 nextWaypoint = transform.position + new Vector3(waypoints[i + 1].x, waypoints[i + 1].y, 0);

                // Draw a line gizmo between the current and the next waypoint to show the path.
                Gizmos.DrawLine(currentWaypoint, nextWaypoint);
            }
        }
    }

    public Vector2 GetNextWaypoint(int currentIndex)
    {
        // Check if the provided index is valid.
        if (currentIndex < 0 || currentIndex >= waypoints.Count)
        {
            Debug.LogError("Invalid waypoint index.");
            return Vector2.zero;
        }

        // Return the next waypoint in the list.
        // The modulo operator ensures that we loop back to the beginning after the last waypoint.
        int nextIndex = (currentIndex + 1) % waypoints.Count;
        return waypoints[nextIndex];
    }
}
