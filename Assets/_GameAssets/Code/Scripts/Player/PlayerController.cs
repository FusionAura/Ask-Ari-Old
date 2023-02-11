using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    internal Player rewiredInput;

    [SerializeField] private Transform UICanvas;
    [SerializeField] private CameraSwapSystem CCTVSetup;

    [SerializeField] private Transform OSModeCollection;
    [SerializeField] private Transform HoloModeCollection;
    [SerializeField] private Transform HologramSpawnPoint;

    [SerializeField] private AiState currentplayerState = AiState.OS_Mode;
    [SerializeField] private AiState NewplayerState = AiState.OS_Mode;

    private Transform hologramActor;

    // Start is called before the first frame update

    private void Update()
    {
        if (currentplayerState == NewplayerState)
            return;
        SwapPerspective(NewplayerState);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void SwapPerspective(AiState currentState)
    {
        currentplayerState = currentState;
        switch (currentplayerState)
        {
            case AiState.OS_Mode: SetOSMode(); break;
            case AiState.Hologram_Mode: SetHologramMode(); break;
        }
    }

    private void SetOSMode()
    {
        OSModeCollection.gameObject.SetActive(true);
        Destroy(hologramActor.gameObject);
    }

    private void SetHologramMode()
    {
        hologramActor = Instantiate(HoloModeCollection, HologramSpawnPoint.position, HologramSpawnPoint.rotation);
        OSModeCollection.gameObject.SetActive(false);
    }

    public void ForceHologram()
    {
        NewplayerState = AiState.Hologram_Mode;
    }
    public void ForceOSMode()
    {
        NewplayerState = AiState.OS_Mode;
    }
}
public enum AiState
{
    OS_Mode, Hologram_Mode//, Frozen, Rebooting //Extra ideas. commented out for now.
}