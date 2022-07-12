using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraMove : MonoBehaviour
{
    public float FastSpeed = 1f;
    public float SlowSpeed = 0.5f;

    public float UpDownSpeed = 0.5f;

    public Transform cam;
    private void FixedUpdate()
    {
        var speed = SlowSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
            speed = FastSpeed;

        transform.Translate(cam.forward * Input.GetAxisRaw("Vertical") * speed, Space.World);

        transform.Translate(cam.right * Input.GetAxisRaw("Horizontal") * speed, Space.World);

        if(Input.GetKey(KeyCode.Space))
            transform.Translate(Vector3.up * UpDownSpeed, Space.World);
        else if(Input.GetKey(KeyCode.LeftControl))
            transform.Translate(Vector3.up * -UpDownSpeed, Space.World);
    }
}
