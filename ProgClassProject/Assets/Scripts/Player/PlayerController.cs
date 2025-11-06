using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Prefabs")]
    public Camera playerCamera;
    private Rigidbody playerRigidbody;
    public GameObject playerprefab;

    [Header("Settings")]
    public float moveSpeed = 5f;

    [Header("References")]
    private CharacterController cc;
    private GameMaster gm;
    public Room room;

    [Header("Rigidbody")]
    Rigidbody rb;

    public (int pprow, int ppcol) playerPos;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        gm = FindFirstObjectByType<GameMaster>();
        Cursor.lockState = CursorLockMode.Locked;
        room = FindFirstObjectByType<Room>();
        transform.position = new Vector3(room.transform.position.x, transform.position.y, room.transform.position.z);
        room.EnterRoom();
    }

    //let player move forward and backward with , w and s and turnd left and right with a and d
    //player moves on a grid and turns in 90 degree increments
    private void Update()
    {
        //movement input
        if (Input.GetKeyDown(KeyCode.W))
        {
                MoveToRoom(room?.north);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            MoveToRoom(room?.south);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            //ditto
            transform.Rotate(0, -90, 0);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            //ditto
            transform.Rotate(0, 90, 0);
        }

        //add search button
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (room != null) 
            {
                Debug.Log("Searching current room.");
                room.RoomSearch();
            }
            else
            {
                Debug.Log("PlayerController.cs room is null");
            }

        }
    }
    public void MoveToRoom(Room targetRoom)
    {
        if (targetRoom == null)
        {
            Debug.LogWarning("Cannot move: target room is null!");
            return;
        }

        room = targetRoom;
        transform.position = new Vector3(targetRoom.transform.position.x, transform.position.y, targetRoom.transform.position.z);
        targetRoom.EnterRoom();
    }

    private void OnTriggerEnter(Collider other)
    {
        Room enteredRoom = other.GetComponent<Room>();
        if (enteredRoom != null)
        {
            room = enteredRoom;
            Debug.Log($"Entered room: {room.name}, Type {room.rType}");
            room.EnterRoom();
        }
    }

    //instantiate player prefab at start of game
    public void SpawnPlayer()
    {
        playerPos = (1, 1);
        GameObject newPlayer = Instantiate(playerprefab,
            new Vector3(playerPos.ppcol * 2, 1, playerPos.pprow * 2),
            Quaternion.identity);
    }




}
