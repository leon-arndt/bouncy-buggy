using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public PlayerController target;
    private Camera cam;
    public Vector3 offset; //0, 1.32, -1.41

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //position
        Vector3 desiredPos = target.transform.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime);

        //rotation
        transform.LookAt(target.transform);
        
        //update camera fov according to speed
        float desiredFov = Mathf.Lerp(cam.fieldOfView, Mathf.Min(110, 70f + target.speed), 4f * Time.deltaTime); //+ rb.velocity.magnitude
        cam.fieldOfView = desiredFov;


        //slowly reset the camera to normal
        //transform.GetChild(0).GetComponent<Camera>().transform.rotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z));
        //this.gameObject.transform.GetChild(0).GetComponent<Camera>().transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, transform.eulerAngles.y, 0)), 0.001f * Time.time);
    }
}
