using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;
using TMPro;

public class PlayerActor : MonoBehaviour
{
    [Header ("Object References Settings")]
    [SerializeField]
    Camera playerCam;
    [SerializeField]
    internal int playerID = 0;
    internal Player rewiredInput;

    [Header("Movement Settings")]
    [SerializeField]
    float maxMoveSpeed = 5f;    
    [SerializeField]
    float acceleration = 5f;

    [Header("Mini UI Settings")]
    [SerializeField]
    private Transform targetedObject;
    [SerializeField]
    private Transform HoloCanvas;
    [SerializeField]
    private Transform ActionListContainer;
    [SerializeField]
    private Transform TextPrefab;

    [SerializeField]
    LayerMask IgnoreUI = 1<<1| 1 << 7 | 1 << 8 | 1 << 9;
    Vector3 heading, forward, right;
    Vector3 rightMovement;
    Vector3 upMovement;

    float moveHorizontal, moveVertical;
    float SavedUp = 0, SavedRight = 0;

    float speed = 0;
    CharacterController cc;
    Ray cameraRay;                // The ray that is cast from the camera to the mouse position
    RaycastHit cameraRayHit;     // The object that the ray $$anonymous$$ts
    private readonly float interactDist = 5;

    // Start is called before the first frame update
    void Awake()
    {
        cc = GetComponent<CharacterController>();

        rewiredInput = ReInput.players.GetPlayer(playerID);

        forward = playerCam.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);

        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        ClearDistance();
        Interact();
    }

    void Movement()
    {
        moveHorizontal = rewiredInput.GetAxis("Horizontal");
        moveVertical = rewiredInput.GetAxis("Vertical");

        if (moveHorizontal != 0 || moveVertical != 0)
        {
            rightMovement = MovementDir(right, moveHorizontal);
            upMovement = MovementDir(forward, moveVertical);

            SavedRight = moveHorizontal;
            SavedUp = moveVertical;

            speed += acceleration * Time.deltaTime;
            if (speed >= maxMoveSpeed)
                speed = maxMoveSpeed;

            heading = Vector3.Normalize(rightMovement + upMovement);
        }
        else
        {
            rightMovement = MovementDir(right, SavedRight);
            upMovement = MovementDir(forward, SavedUp);

            //Decellerate Player speed to 0.
            if (speed > 0)
            {
                speed -= acceleration * Time.deltaTime;
                speed = Mathf.Clamp(speed, 0, maxMoveSpeed);
            }
        }
        cc.SimpleMove(heading * speed);
    }

    void ClearDistance()
    {
        if (targetedObject == null)
            return;
        if (Vector3.Distance(transform.position, targetedObject.transform.position) > interactDist)
        {
            targetedObject.GetComponent<IInteractables>()?.OutOfRangeInteraction();
            targetedObject = null;
            ClearInteractionList();
            HoloCanvas.gameObject.SetActive(false);
        } 
    }

    void Interact()
    {
        if (rewiredInput.GetButtonDown("Interact"))
        {
            cameraRay = playerCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(cameraRay, out cameraRayHit))
            {
                if (cameraRayHit.transform.CompareTag("Interactable")|| cameraRayHit.transform.CompareTag("Human"))
                {
                    ClearInteractionList();
                    HoloCanvas.gameObject.SetActive(true);
                    var interactable = cameraRayHit.transform.GetComponent<IInteractables>();
                    if (interactable == null) return;

                    interactable.SetInteractionTarget(transform);
                    targetedObject = cameraRayHit.transform;
                    InteractionList(targetedObject.gameObject);
                }
            }
        }
    }

    void InteractionList(GameObject interactedObj)
    {
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

    void ClearInteractionList()
    {
        if (ActionListContainer.childCount == 0)
            return;
        foreach (Transform a in ActionListContainer)
        {
            Destroy(a.gameObject);     
        }
    }

    private void LateUpdate()
    {
        cameraRay = playerCam.ScreenPointToRay(Input.mousePosition);

        // If the ray strikes an object...
        if (Physics.Raycast(cameraRay, out cameraRayHit,IgnoreUI))
        {
            // ...and if that object is the ground...
            //https://answers.unity.com/questions/1023116/check-if-colliding-with-a-layer.html
            //https://answers.unity.com/questions/805776/isometric-game-player-look-at-cursor.html
            if (cameraRayHit.transform.tag == "Environment" || cameraRayHit.transform.gameObject.layer == 7)
            {
                // ...make the cube rotate (only on the Y axis) to face the ray $$anonymous$$t's position 
                Vector3 targetPosition = new Vector3(cameraRayHit.point.x, transform.position.y, cameraRayHit.point.z);
                transform.LookAt(targetPosition);
            }
        }
    }

    Vector3 MovementDir(Vector3 dir, float playerInput)
    {
        return (speed * Time.deltaTime) * playerInput * dir;
    }
}
