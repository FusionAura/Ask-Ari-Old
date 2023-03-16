using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Rewired;
using TMPro;

public class CameraSwapSystem : MonoBehaviour
{
    [SerializeField]
    private Node CurrentNode;
    int currentNodeIndex = 0;
    private Camera CurrentCamera;

    [SerializeField]
    private List<Node> HouseNodes = new List<Node>();

    [SerializeField]
    internal int playerID = 0;
    internal Player rewiredInput;

    [SerializeField]
    private Transform TextPrefab;
    [SerializeField]
    private Transform buttonContainer;
    [SerializeField]
    private Transform AppliancesContainer;
    [SerializeField]
    private Transform ActionListContainer;


    public Text CameraName;

    public Vector2 CamRotation;

    private void Awake()
    {
        rewiredInput = ReInput.players.GetPlayer(playerID);
    }
    void Start()
    {
        CurrentNode = HouseNodes[currentNodeIndex];
        NodesButtonList();
        ApplianceList();
    }

    // Update is called once per frame
    void Update()
    {
        CamRotation.x = rewiredInput.GetAxis("Horizontal");
        CamRotation.y = rewiredInput.GetAxis("Vertical");
    }

    private void NodesButtonList()
    {
        for (int i = 0; i < CurrentNode.LinkingNodes.Count; i++)
        {
            GameObject listoption = Instantiate(TextPrefab.gameObject, buttonContainer);
            listoption.name = CurrentNode.LinkingNodes[i].Room_Name;

            TextMeshProUGUI textComp = listoption.GetComponentInChildren<TextMeshProUGUI>();
            textComp.text = CurrentNode.LinkingNodes[i].name;
            int tmp = i;
            listoption.GetComponent<Button>().onClick.AddListener(delegate { SwitchNode(tmp); });
        }
    }

    private void ApplianceList()
    {
        for (int i = 0; i < CurrentNode.Interactables.Count; i++)
        {
            GameObject listoption = Instantiate(TextPrefab.gameObject, AppliancesContainer);
            listoption.name = CurrentNode.Interactables[i].name;

            TextMeshProUGUI textComp = listoption.GetComponentInChildren<TextMeshProUGUI>();
            textComp.text = CurrentNode.Interactables[i].name;
            int tmp = i;
            GameObject interactableAppliance = CurrentNode.Interactables[i].gameObject;
            listoption.GetComponent<Button>().onClick.AddListener(delegate { InteractionList(interactableAppliance); });
        }
    }

    void InteractionList(GameObject interactedObj)
    {
        ClearApplianceActions();
        Debug.Log(interactedObj);
        List<string> possibleActions = interactedObj.GetComponent<IInteractables>().ReturnPossibleActions();
        for (int i = 0; i < possibleActions.Count; i++)
        {
            GameObject listoption = Instantiate(TextPrefab.gameObject, ActionListContainer);
            listoption.name = possibleActions[i];

            TextMeshProUGUI textComp = listoption.GetComponentInChildren<TextMeshProUGUI>();
            textComp.text = possibleActions[i];
            int tmp = i;
            listoption.GetComponent<Button>().onClick.AddListener(delegate { interactedObj.GetComponent<IInteractables>().Interact(tmp); });
        }
    }

    void SwitchNode(int tmp)
    {
        ClearLists();

        CurrentNode = CurrentNode.LinkingNodes[tmp];
        SwapCamera();
        ApplianceList();
        NodesButtonList();
    }

    void SwapCamera()
    {
        CurrentNode.Cam.currentCam.GetComponent<Camera>().enabled = false;
        CurrentCamera = CurrentNode.Cam.currentCam;
        CameraName.text = "CAM " + CurrentNode.Room_Name;
        CurrentNode.Cam.currentCam.GetComponent<Camera>().enabled = true;

        CurrentNode.Cam.currentRot = CurrentNode.Cam.currentCam.transform.localRotation.eulerAngles;
    }

    void ClearLists()
    {
        foreach (Transform a in buttonContainer)
            Destroy(a.gameObject);
        foreach (Transform a in AppliancesContainer)
            Destroy(a.gameObject);
        foreach (Transform a in ActionListContainer)
            Destroy(a.gameObject);
    }

    void ClearApplianceActions()
    {
        foreach (Transform a in ActionListContainer)
            Destroy(a.gameObject);
    }
}
[System.Serializable]
public class CTVCam
{
    public string Name;
    public Camera currentCam;
    public Vector3 currentRot;
}
