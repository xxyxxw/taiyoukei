using UnityEngine;


public class CameraController : MonoBehaviour
{
    public Transform target; // í«è]ëŒè€ÅiEarthÇ»Ç«Åj
    public float distance = 50f;
    public float orbitSpeed = 60f;
    public float zoomSpeed = 200f;


    float az = 0f, el = 20f;


    void Update()
    {
        if (target == null) return;
        if (Input.GetMouseButton(1))
        {
            az += Input.GetAxis("Mouse X") * orbitSpeed * Time.deltaTime;
            el -= Input.GetAxis("Mouse Y") * orbitSpeed * Time.deltaTime;
            el = Mathf.Clamp(el, -80f, 80f);
        }
        distance = Mathf.Max(1f, distance - Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime);


        Quaternion rot = Quaternion.Euler(el, az, 0);
        Vector3 pos = target.position + rot * (Vector3.back * distance);
        transform.position = pos;
        transform.rotation = rot * Quaternion.Euler(0, 180, 0);
    }
}