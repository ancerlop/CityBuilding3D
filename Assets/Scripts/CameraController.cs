using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float moveSpeed;

    public float minXRot;
    public float maxXRot;

    private float curXRot;

    public float minZoom;
    public float maxZoom;

    public float zoomSpeed;
    public float rotateSpeed;

    private float curZoom;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        curZoom = cam.transform.localPosition.y;
        curXRot = -50;
    }

    private void Update()
    {
        //getting the scroll wheel value
        curZoom += Input.GetAxis("Mouse ScrollWheel") * -zoomSpeed;

        //clamping it between the min/max values
        curZoom = Mathf.Clamp(curZoom, minZoom, maxZoom);

        //applying to the camera
        cam.transform.localPosition = Vector3.up * curZoom;

        //camera look
        //if right mouse button is pressed
        if (Input.GetMouseButton(1))
        {
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");

            curXRot += -y * rotateSpeed;
            curXRot = Mathf.Clamp(curXRot, minXRot, maxXRot);

            transform.eulerAngles = new Vector3(curXRot, transform.eulerAngles.y + (x * rotateSpeed), 0f);
        }

        //camera move
        Vector3 forward = cam.transform.forward;
        forward.y = 0f;       

        Vector3 right = cam.transform.right;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        //get a local direction
        Vector3 dir = forward * moveZ + right * moveX;
        dir.Normalize();

        dir *= moveSpeed * Time.deltaTime;

        transform.position += dir;

        


    }
}
