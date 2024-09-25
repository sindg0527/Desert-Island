using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
    public GameObject GetItem;
    public LayerMask toolMask;
    void Start()
    {
        
    }

    void Update()
    {
        Debug.Log("E Key");
        Debug.DrawLine(transform.position, transform.right, Color.red, 1.0f);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 1, toolMask);
        transform.rotation = Quaternion.Euler(0, 0, 0);

        //if(hit.collider)
        hit.collider.transform.SetParent(transform);
        hit.collider.transform.localPosition = Vector3.zero;

        GetItem = hit.collider.gameObject;
    }
}
