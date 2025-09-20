using UnityEngine;
using UnityEditor;

// This attribute tells Unity that this is a custom editor for the WaypointSystem class.
[CustomEditor(typeof(WaypointSystem))]
public class WaypointSystemEditor : Editor
{
    // A reference to the WaypointSystem component being edited.
    protected WaypointSystem waypointSystem;

    // This method is called when the Inspector is opened for the WaypointSystem component.
    private void OnEnable()
    {
        waypointSystem = (WaypointSystem)target;
    }

    // This method is called multiple times per second and is used to draw gizmos and handles in the Scene view.
    private void OnSceneGUI()
    {
        // Ensure the WaypointSystem component exists.
        if (waypointSystem == null || waypointSystem.waypoints == null)
        {
            return;
        }

        // We use BeginChangeCheck and EndChangeCheck to know if the user modified a waypoint.
        // This is important for saving changes correctly.
        EditorGUI.BeginChangeCheck();

        // Loop through each waypoint and create a position handle for it.
        for (int i = 0; i < waypointSystem.waypoints.Count; i++)
        {
            // Get the current waypoint's position in world space.
            Vector3 worldPosition = waypointSystem.transform.position + new Vector3(waypointSystem.waypoints[i].x, waypointSystem.waypoints[i].y, 0);

            // Create a position handle at the waypoint's position.
            // This is the key function that makes the waypoint draggable with the move tool.
            Vector3 newWorldPosition = Handles.PositionHandle(worldPosition, Quaternion.identity);

            // Update the waypoint position if the user moved the handle.
            waypointSystem.waypoints[i] = newWorldPosition - waypointSystem.transform.position;
        }

        // If the user made any changes, mark the object as dirty to save the changes.
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(waypointSystem);
        }
    }
}
