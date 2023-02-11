using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Rewired;

public class CameraSwapSystem : MonoBehaviour
{
    [SerializeField]
    internal int playerID = 0;
    internal Player rewiredInput;

    private Room currentRoom;
    public List<Room> RoomList = new List<Room>();
    int currentRoomIndex = 0;

    public Text CameraName;

    public Vector2 CamRotation;
    // Start is called before the first frame update

    private void Awake()
    {
        rewiredInput = ReInput.players.GetPlayer(playerID);
    }
    void Start()
    {
        currentRoom = RoomList[currentRoomIndex];
        CameraName.text = "CAM " + currentRoom.Name;
        currentRoom.currentRot = currentRoom.currentCam.transform.localRotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        CamRotation.x = rewiredInput.GetAxis("Horizontal");
        CamRotation.y = rewiredInput.GetAxis("Vertical");
        currentRoom.currentCam.transform.localRotation = Quaternion.Euler(currentRoom.currentRot+new Vector3(-CamRotation.y, CamRotation.x ));
        if (rewiredInput.GetButtonDown("CamPrevious"))
        {
            currentRoomIndex--;
        }

        if (rewiredInput.GetButtonDown("CamNext"))
        {
            currentRoomIndex++;
        }

        if (currentRoomIndex < 0)
            currentRoomIndex += RoomList.Count;
        else if (currentRoomIndex > RoomList.Count - 1)

            currentRoomIndex = 0;
        SwapCamera();

    }

    void SwapCamera()
    {
        currentRoom.currentCam.gameObject.SetActive(false);
        
        currentRoom = RoomList[currentRoomIndex];
        CameraName.text = "CAM " + currentRoom.Name;
        currentRoom.currentCam.gameObject.SetActive(true);

        currentRoom.currentRot = currentRoom.currentCam.transform.localRotation.eulerAngles;
    }
}
[System.Serializable]
public class Room
{
    public string Name;
    public Camera currentCam;
    public Vector3 currentRot;
}
