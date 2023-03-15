using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways,RequireComponent(typeof(Camera))]
public class RotateCamera : MonoBehaviour
{
    public Transform lookAt;
    public Vector3 offset;
    public float rotationSpeed = 5.0f;
    private CameraPixelSnap snap;
    private Vector3 origPos;
    private float angle;

    // Start is called before the first frame update
    void Start()
    {
        snap = GetComponent<CameraPixelSnap>();
        origPos = snap.position;
    }

    private void Awake()
    {
    }

    int i = 0;
    // Update is called once per frame
    void Update()
    {
        snap.position = Vector3.Lerp(snap.position, lookAt.position + lookAt.forward * offset.z + lookAt.right * offset.x + lookAt.up * offset.y, Time.deltaTime * rotationSpeed);
        //snap.position = lookAt.position + lookAt.forward * offset.z + lookAt.right * offset.x + lookAt.up * offset.y;
        snap.CalcCamera();
        Vector3 forward = lookAt.position - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(forward, Vector3.Cross(Vector3.Cross(forward, Vector3.up), forward));
        if (Quaternion.Angle(transform.rotation, lookRot) > 1)
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRot, Time.deltaTime * rotationSpeed);
            //transform.rotation = lookRot;
    }
}
