using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

// This script handles the tracing logic for the alphabet tracing game.
// It works by checking if the user's mouse position is close to the waypoints
// defined in the WaypointSystem component, in the correct order.

[RequireComponent(typeof(WaypointSystem))]
[RequireComponent(typeof(LineRenderer))]
public class TracingSystem : MonoBehaviour
{
    // A reference to the WaypointSystem component on the same GameObject.
    // This allows us to access the defined waypoints.
    private WaypointSystem waypointSystem;

    // A reference to the LineRenderer component on the same GameObject.
    // This is used to visualize the path the user is drawing.
    private LineRenderer lineRenderer;

    // The index of the current waypoint the user needs to reach.
    private int currentWaypointIndex = 0;

    // This list will store the positions of the path the user has drawn.
    private List<Vector3> drawnPath = new List<Vector3>();

    // A flag to check if the user is currently tracing.
    private bool isTracing = false;
    // a flag to check if current trace is compelete.
    [HideInInspector]
    public bool traceComplete = false;

    [Header("Public Properties")]
    // A tolerance value to check if the mouse position is "close enough" to a waypoint.
    // This can be adjusted in the Inspector to change the difficulty.
    public float tracingTolerance = 0.2f;
    public float brush_widht = 0.6f;
    public TMP_Text instructionText;

    //Audio Manager Refrence.
    public Tracing_Audio audioManager;

    // This method is called when the script instance is being loaded.
    // We use it to get a reference to the other required components on this GameObject.
    void Awake()
    {
        // Get a reference to the WaypointSystem component.
        waypointSystem = GetComponent<WaypointSystem>();
        if (waypointSystem == null)
        {
            Debug.LogError("WaypointSystem component not found on this GameObject.", this);
        }

        // Get a reference to the LineRenderer component.
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer component not found on this GameObject.", this);
        }
        lineRenderer.startWidth = lineRenderer.endWidth = brush_widht;
        SetLineRendererSortingOrder();
    }

    private void Start()
    {
        audioManager = FindObjectOfType<Tracing_Audio>();
    }
    // Update is called once per frame. We use it to handle continuous input.
    void Update()
    {
        HandleTracingInput();
    }

    // Handles the mouse input for tracing.
    private void HandleTracingInput()
    {
        // Get the current mouse position in world coordinates.
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0; // Ensure we are working in 2D space.

        // Check if the left mouse button has been clicked.
        if (Input.GetMouseButtonDown(0))
        {
            // Reset the tracing path and state.
            ResetTracing();
            isTracing = true;
            audioManager.TracingSound();
        }

        // Check if the left mouse button is held down.
        if (isTracing && Input.GetMouseButton(0))
        {
            // Check if the user's position is still on the valid path segment.
            if (!IsOnValidPath(mouseWorldPosition))
            {
                // If they've gone off the path, reset the trace.
                ResetTracing();
                instructionText.text = "Wrong Trace!! Off the path!";
                audioManager.WrongTraceAudio();
                Debug.Log("You went off the path. Tracing reset.");
                return; // Exit the function to prevent further updates in this frame.
            }

            // Add the current mouse position to the drawn path.
            drawnPath.Add(mouseWorldPosition);
            // Update the LineRenderer with the new path.
            lineRenderer.positionCount = drawnPath.Count;
            lineRenderer.SetPositions(drawnPath.ToArray());

            // Check if the user's position is close to the current target waypoint.
            CheckWaypointProximity(mouseWorldPosition);

        }

        // Check if the left mouse button has been released.
        if (Input.GetMouseButtonUp(0))
        {
            isTracing = false;
        }
    }

    
    private bool IsOnValidPath(Vector3 currentPosition)
    {
        Vector2 selfPosition = new Vector2(transform.position.x, transform.position.y);
        // We only check for deviation if we are not at the very first waypoint.
        if (currentWaypointIndex == 0)
        {
            // Check if we are starting within the tolerance of the first waypoint.
            Vector2 startWaypoint = waypointSystem.waypoints[0];
            Vector2 startWaypointWorld = selfPosition + new Vector2(startWaypoint.x, startWaypoint.y);
            return Vector3.Distance(currentPosition, startWaypointWorld) <= tracingTolerance;
        }
        else
        {
            // Get the previous and current waypoint positions.
            Vector2 prevWaypoint = waypointSystem.waypoints[currentWaypointIndex - 1];
            Vector2 prevWaypointWorld = selfPosition + new Vector2(prevWaypoint.x, prevWaypoint.y);

            Vector2 nextWaypoint = waypointSystem.waypoints[currentWaypointIndex];
            Vector2 nextWaypointWorld = selfPosition + new Vector2(nextWaypoint.x, nextWaypoint.y);

            // Calculate the distance from the current mouse position to the line segment.
            float distanceToPath = DistanceToLineSegment(currentPosition, prevWaypointWorld, nextWaypointWorld);

            // Check if the distance is within the tolerance.
            return distanceToPath <= tracingTolerance;
        }
    }

    
    private float DistanceToLineSegment(Vector3 p, Vector3 a, Vector3 b)
    {
        Vector3 ab = b - a;
        Vector3 ap = p - a;

        float lengthSquared = ab.sqrMagnitude;
        float dotProduct = Vector3.Dot(ap, ab);
        float t = dotProduct / lengthSquared;

        if (t < 0.0f)
        {
            // Closest point is 'a'.
            return Vector3.Distance(p, a);
        }
        else if (t > 1.0f)
        {
            // Closest point is 'b'.
            return Vector3.Distance(p, b);
        }

        // Closest point is on the segment.
        Vector3 projection = a + t * ab;
        return Vector3.Distance(p, projection);
    }

    // Checks if the user's position is close to the current target waypoint.
    private void CheckWaypointProximity(Vector3 currentPosition)
    {
        Vector2 selfPosition = new Vector2(transform.position.x, transform.position.y);
        // Check if there are any waypoints to trace.
        if (currentWaypointIndex >= waypointSystem.waypoints.Count)
        {
            // Tracing is complete.
            return;
        }

        // Get the position of the next waypoint to check.
        Vector2 targetWaypoint = waypointSystem.waypoints[currentWaypointIndex];
        Vector2 targetWaypointWorld = selfPosition + new Vector2(targetWaypoint.x, targetWaypoint.y);

        // Calculate the distance between the current mouse position and the target waypoint.
        float distance = Vector3.Distance(currentPosition, targetWaypointWorld);

        // Check if the distance is within our defined tolerance.
        if (distance <= tracingTolerance)
        {
            Debug.Log($"Waypoint {currentWaypointIndex + 1} reached!");
            currentWaypointIndex++;

            // If all waypoints have been reached, the tracing is successful.
            if (currentWaypointIndex >= waypointSystem.waypoints.Count)
            {
                OnTracingComplete();
            }
        }
    }

    // This method is called when the tracing is successfully completed.
    private void OnTracingComplete()
    {
        isTracing = false;
        instructionText.text = "Great Job!";
        audioManager.StrokeDoneAudio();
        Debug.Log("Tracing complete! Great job!");
        traceComplete = true;
    }

    // Resets the tracing state for a new attempt.
    private void ResetTracing()
    {
        currentWaypointIndex = 0;
        drawnPath.Clear();
        lineRenderer.positionCount = 0;
        isTracing = false; // Ensure isTracing is set to false here as well.
        instructionText.text = "Keep Going!";
        Debug.Log("Tracing reset. Starting new attempt.");
    }
    private void SetLineRendererSortingOrder()
    {
        lineRenderer.sortingLayerName = "TracingLayer";
        lineRenderer.sortingOrder = 10000;
    }
}
