using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Plane))]
public class PickPlanePoint : MonoBehaviour {

    public Collider groundPlane;
    public Transform markerObject;
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (groundPlane.Raycast(ray, out hit, 100))
                markerObject.position = ray.GetPoint(hit.distance);
            
        }
    }
}
