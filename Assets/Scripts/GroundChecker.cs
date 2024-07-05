using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] private Transform point;
    [SerializeField, Range(0, 1)] private float distance;
    RaycastHit hit;
    public bool isOpen;

    private void Update()
    {
        if(Physics.Raycast(point.position, Vector3.down, out hit, distance, layerMask))
        {
            if(hit.transform.CompareTag("Kitchen"))
            {
                isOpen = true;
            }
            else if(hit.transform.CompareTag("Office"))
            {
                isOpen = false;
            }
        }
    }
}
