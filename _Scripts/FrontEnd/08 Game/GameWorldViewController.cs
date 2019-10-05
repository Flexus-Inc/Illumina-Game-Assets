using System.Collections;
using System.Collections.Generic;
using Illumina;
using UnityEngine;

public class GameWorldViewController : MonoBehaviour {
    public GameObject CameraObject;
    Vector3 CamPos;
    Vector3 StartTouch;
    bool isPanAllowed;
    public float MapBoundaryX1;
    public float MapBoundaryX2;
    public float MapBoundaryY1;
    public float MapBoundaryY2;
    Limit _MapBoundaryX;
    Limit _MapBoundaryY;

    void Start() {
        _MapBoundaryX = new Limit(MapBoundaryX1, MapBoundaryX2);
        _MapBoundaryY = new Limit(MapBoundaryY1, MapBoundaryY2);

        CamPos = CameraObject.transform.position;
    }

    // Update is called once per frame
    void Update() {
        //var scroll_pos = Input.GetAxis("Mouse ScrollWheel"); // latest scroll position
        //if (scroll_pos != ScrollPos)
        //{
        //ZoomView(scroll_pos);
        // if (ActualGameManager.IsGameAccesible()) {
        //     
        // }
        PanInMap();
        //}

    }
    void PanInMap() {
        //Vector2 cam_pos;
        if (Input.GetMouseButtonDown(0)) {
            StartTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0)) {
            var cam_pos = Camera.main.transform.position;
            var mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var displacement = StartTouch - mouse_pos;
            var result_cam_pos = cam_pos + displacement;

            if (!_MapBoundaryX.CompareTo(result_cam_pos.x) || !_MapBoundaryY.CompareTo(result_cam_pos.y)) {
                Camera.main.transform.position += displacement;
                var reqx = Mathf.Clamp(cam_pos.x, _MapBoundaryX.x, _MapBoundaryX.y);
                var reqy = Mathf.Clamp(cam_pos.y, _MapBoundaryY.x, _MapBoundaryY.y);
                var req = new Vector3(reqx, reqy, cam_pos.z);
                StartCoroutine(GoBack(req));
            } else {

                Camera.main.transform.position += displacement;
            }
        }
    }

    public IEnumerator GoBack(Vector3 req, float lerpTime = 0.5f) {
        Vector3 end = req;
        Vector3 start = CameraObject.transform.position;
        float z = CameraObject.transform.position.z;
        float timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;
        while (true) {
            timeSinceStarted = Time.time - timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;
            var currentX = Mathf.Lerp(start.x, end.x, percentageComplete);
            var currentY = Mathf.Lerp(start.y, end.y, percentageComplete);
            transform.position = new Vector3(currentX, currentY, z);
            if (percentageComplete >= 1) break;
            yield return new WaitForEndOfFrame();
        }
    }
}