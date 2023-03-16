using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public string Room_Name;

    public List<Appliance> Interactables;

    public List<Node> LinkingNodes;

    public CTVCam Cam;

    private void Start()
    {
        Cam.currentCam = GetComponent<Camera>();
    }
}