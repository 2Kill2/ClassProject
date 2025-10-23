using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Prefabs")]
    public Camera playerCamera;
    private Rigidbody playerRigidbody;
    private GameObject playerprefab;

    [Header("Settings")]
    public float moveSpeed = 5f;
    private float rotationX = 0f;
    private float rotationY = 0f;

    [Header("References")]
    private CharacterController cc;

    public (int pprow, int ppcol) playerPos;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    //let player move forward and backward with , w and s and turnd left and right with a and d
    //player moves on a grid and turns in 90 degree increments
    private void Update()
    {
        //movement input
        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.position += transform.forward * 2;
            playerPos.pprow += 1;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.position -= transform.forward * 2;
            playerPos.pprow -= 1;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.Rotate(0, -90, 0);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.Rotate(0, 90, 0);
        }

        //add search button
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //do this later
        }
    }

    //instantiate player prefab at start of game
    public void SpawnPlayer()
    {
        playerPos = (1, 1);
        playerprefab = Instantiate(playerprefab, new Vector3(playerPos.ppcol * 2, 1, playerPos.pprow * 2), Quaternion.identity);

    }



}
