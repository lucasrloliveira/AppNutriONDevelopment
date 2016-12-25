using UnityEngine;
using System.Collections;

public class MenuAnimatorDetectTouchScript : MonoBehaviour {
	
	public float MinAcceptanceTouchDistance = 50;
	public bool IsTouchDetectionActive = true;

    private Vector2 startPoint;
	private Vector2 endPoint;
	private MenuAnimatorScript menuAnimatorScript;

	void Awake () {
		menuAnimatorScript = this.GetComponent<MenuAnimatorScript> ();
	}

    void Update()
    {
        if (IsTouchDetectionActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPoint = Input.mousePosition; //touch.position;
                endPoint = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                SetTransformPosition();
            }
            if (Input.GetMouseButtonUp(0))
            {
                endPoint = Input.mousePosition;
                Move();
            }
        }
    }

    protected virtual void SetTransformPosition()
    {
        Camera v_camera = Camera.allCameras.Length > 0? Camera.allCameras[0] : null;

        if (v_camera != null)
        {
            
            Vector3 v_startPosition = endPoint; //v_camera.ScreenToWorldPoint(endPoint);
            Vector3 v_endPosition = Input.mousePosition; //v_camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 v_delta = v_endPosition - v_startPosition;
            v_delta.y = 0;

            foreach (var v_item in menuAnimatorScript.Items)
            {
                v_item.transform.localPosition += v_delta;
            }
            endPoint = v_endPosition;
        }
    }

    void Move()
    {
        if (IsTouchDetectionActive)
        {
            var direction = startPoint.x - endPoint.x;
            var moveDistance = Mathf.Abs(direction);

            if (direction < 0 && moveDistance > MinAcceptanceTouchDistance)
            {
                menuAnimatorScript.MoveLeft();
            }
            if (direction > 0 && moveDistance > MinAcceptanceTouchDistance)
            {
                menuAnimatorScript.MoveRight();
            }
            if (moveDistance < MinAcceptanceTouchDistance)
            {
                menuAnimatorScript.MoveNeutral();
            }

            startPoint = Vector2.zero;
            endPoint = Vector2.zero;
        }
    }

}
