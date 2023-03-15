using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5.0f;
    public float rotationSpeed = 5.0f;
    private float angle = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
            transform.position -= transform.forward * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
            transform.position += transform.forward * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.W))
            transform.position += transform.right * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            transform.position -= transform.right * speed * Time.deltaTime;


        if (Input.GetKey(KeyCode.Q))
            transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y - rotationSpeed * Time.deltaTime, 0));
        if (Input.GetKey(KeyCode.E))
            transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y + rotationSpeed * Time.deltaTime, 0));
    }
}
