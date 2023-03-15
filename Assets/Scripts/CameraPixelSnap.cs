using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways,RequireComponent(typeof(Camera))]
public class CameraPixelSnap : MonoBehaviour
{
    public PixelizeFeature feat;
    private PixelizePass pass;
    public Vector3 position;

    private Camera cam;

    // Start is called before the first frame update
    private void OnValidate()
    {
        cam = GetComponent<Camera>();
    }

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    int i = 0;
    // Update is called once per frame
    public void CalcCamera()
    {
        pass = feat.customPass;
        var pixelScreenHeight = pass.settings.screenHeight;
        var pixelScreenWidth = (int)(pixelScreenHeight * cam.aspect + 0.5f);

        Vector3 topCenter = cam.ViewportToWorldPoint(new Vector2(0, 1));
        Vector3 center = cam.ViewportToWorldPoint(new Vector2(0, 0));
        Vector3 rightCenter = cam.ViewportToWorldPoint(new Vector2(1, 0));

        float cameraWorldHeight = Vector3.Magnitude(topCenter - center) * 2.0f;
        float cameraWorldWidth = Vector3.Magnitude(rightCenter - center) * 2.0f;

        Vector2 pixelWorldSize = new Vector2(cameraWorldWidth / pixelScreenWidth / 2, cameraWorldHeight / pixelScreenHeight / 2);
        pass.settings.pixelOffset = new Vector2(
            position.x % pixelWorldSize.x / pixelWorldSize.x,
            position.y % pixelWorldSize.y / pixelWorldSize.y
            );

        Vector3 localx = Vector3.Project(position, transform.right);
        Vector3 localy = Vector3.Project(position, transform.up);
        Vector3 localz = Vector3.Project(position, transform.forward);

        float localx_f = Vector3.Magnitude(localx) * Vector3.Dot(Vector3.Normalize(localx), transform.right);
        float localy_f = Vector3.Magnitude(localy) * Vector3.Dot(Vector3.Normalize(localy), transform.up);

        transform.position = transform.right * (localx_f - localx_f % pixelWorldSize.x) +
            transform.up * (localy_f - localy_f % pixelWorldSize.y) +
            localz;
    }
}
