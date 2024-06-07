using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputSystem : MonoBehaviour
{
    public LayerMask layerMask;
    public bool raycast2d;
    public bool raycast3d;

    [Header("RUNTIME")]
    public Vector3? clickDownWPos;

    public void Init()
    {
    }

    private Vector3 GetInputWorld(Vector2 input)
    {
        Ray ray = Camera.main.ScreenPointToRay(input);
        float distance;
        if (new Plane(Vector3.forward, Vector3.zero).Raycast(ray, out distance))
        {
            Vector3 point = ray.GetPoint(distance);
            point.z = 0f;
            return point;
        }
        return Vector3.zero;
    }

    public void Tick(float dt)
    {
        clickDownWPos = null;

        // if (isBlockByUI)
        // {
        //     return;
        // }

        if (raycast2d)
            TickRaycast2D(dt);
        if (raycast3d)
            TickRaycast3D(dt);
    }

    private void TickRaycast2D(float dt)
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            var wp = GetInputWorld(Input.mousePosition);
            clickDownWPos = wp;
            // Debug.Log("xx-- set click down wpos");

            Debug.DrawLine(Camera.main.transform.position, wp, Color.blue, 10);
            // Debug.Log("xx-- screen pos > " + Input.mousePosition + " - " + Screen.width + " - " + Screen.height);
            RaycastHit2D hit = Physics2D.Raycast(wp, Vector2.zero, 1000f);
            if (hit.collider != null)
            {
                // Debug.Log("xx-- hit dist > " + hit.distance);
                // Debug.DrawLine(Camera.main.transform.position, hit.point, Color.blue, 10);
                var handler = hit.collider.GetComponent<InputHandler>();
                if (handler != null)
                {
                    var k = Input.GetMouseButtonDown(0) ? InputClickKey.LEFT : InputClickKey.RIGHT;
                    handler.ClickDown(k, wp);
                }
            }
        }

        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            var wp = GetInputWorld(Input.mousePosition);
            // Debug.Log("xx-- screen pos > " + Input.mousePosition + " - " + Screen.width + " - " + Screen.height);
            RaycastHit2D hit = Physics2D.Raycast(wp, Vector2.zero, 1000f);
            if (hit.collider != null)
            {
                // Debug.Log("xx-- hit dist > " + hit.distance);
                // Debug.DrawLine(Camera.main.transform.position, hit.point, Color.blue, 10);
                var handler = hit.collider.GetComponent<InputHandler>();
                if (handler != null)
                {
                    var k = Input.GetMouseButtonUp(0) ? InputClickKey.LEFT : InputClickKey.RIGHT;
                    handler.ClickUp(k, wp);
                }
            }
        }
    }

    private void TickRaycast3D(float dt)
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Debug.Log("xx-- screen pos > " + Input.mousePosition + " - " + Screen.width + " - " + Screen.height);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, layerMask.value))
            {
                // Debug.Log("xx-- hit dist > " + hit.distance);
                // Debug.DrawLine(Camera.main.transform.position, hit.point, Color.blue, 10);
                var handler = hit.collider.GetComponent<InputHandler>();
                if (handler != null)
                {
                    var k = Input.GetMouseButtonDown(0) ? InputClickKey.LEFT : InputClickKey.RIGHT;
                    handler.ClickDown(k, hit.point);
                }
            }
        }

        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Debug.Log("xx-- screen pos > " + Input.mousePosition + " - " + Screen.width + " - " + Screen.height);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, layerMask.value))
            {
                // Debug.Log("xx-- hit dist > " + hit.distance);
                // Debug.DrawLine(Camera.main.transform.position, hit.point, Color.blue, 10);
                var handler = hit.collider.GetComponent<InputHandler>();
                if (handler != null)
                {
                    var k = Input.GetMouseButtonUp(0) ? InputClickKey.LEFT : InputClickKey.RIGHT;
                    handler.ClickUp(k, hit.point);
                }
            }
        }
    }

    // private InputHandler lastCursorHandler = null;
    // private void TickCursorType(float dt)
    // {
    //     var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //     // Debug.Log("xx-- screen pos > " + Input.mousePosition + " - " + Screen.width + " - " + Screen.height);
    //     RaycastHit hit;
    //     if (Physics.Raycast(ray, out hit, 1000f, cursorLayerMask.value))
    //     {
    //         // Debug.Log("xx-- hit dist > " + hit.distance);
    //         Debug.DrawLine(Camera.main.transform.position, hit.point, Color.blue, 10);
    //         var handler = hit.collider.GetComponent<InputHandler>();
    //         if (lastCursorHandler == null && handler != null)
    //         {
    //             lastCursorHandler = handler;
    //             handler.MouseEnter();
    //         }
    //     }
    //     else
    //     {
    //         if (lastCursorHandler != null)
    //         {
    //             lastCursorHandler.MouseExit();
    //             lastCursorHandler = null;
    //         }
    //     }
    // }
}
