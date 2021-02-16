using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public Transform[] startingPositions;
    public GameObject[] rooms;

    public GameObject[] roomsLR;
    public GameObject[] roomsLRB;
    public GameObject[] roomsLRT;
    public GameObject[] roomsLRTB;

    private int direction;
    public float moveAmountX;
    public float moveAmountY;

    private float timeBtwRoom;
    public float startTimeBtwRoom = .25f;

    public float minX, maxX;
    public float minY;

    public bool stopGen = false;

    public LayerMask room;

    private int downCounter;

    // Start is called before the first frame update
    private void Start()
    {
        int randStartingPos = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randStartingPos].position;
        Instantiate(rooms[0], transform.position, Quaternion.identity);

        direction = Random.Range(1, 6);

        int r = Random.Range(6, 24);
        Debug.Log(r * 5);
    }

    private void Update()
    {
        if (timeBtwRoom <= 0 && stopGen == false)
        {
            NextRoom();
            timeBtwRoom = startTimeBtwRoom;
        } else
        {
            timeBtwRoom -= Time.deltaTime;
        }
    }

    private void NextRoom() // Spawns next room
    {
        if (direction == 1 || direction ==2) // Generate room to the right
        {
            if(transform.position.x < maxX)
            {
                downCounter = 0;

                Vector2 newPos = new Vector2(transform.position.x + moveAmountX, transform.position.y);
                transform.position = newPos;

                int rand = Random.Range(0, rooms.Length);

                //SpawnRandomRoomTile(rand);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                direction = Random.Range(1, 6); // Choose next direction
                if(direction == 3)
                {
                    direction = 2;
                } else if (direction == 4)
                {
                    direction = 5; // Go down
                }
            } else {
                direction = 5;
            }
            
        } else if (direction == 3 || direction == 4) // Generate room to the left
        {
            if(transform.position.x > minX)
            {
                downCounter = 0;

                Vector2 newPos = new Vector2(transform.position.x - moveAmountX, transform.position.y);
                transform.position = newPos;

                int rand = Random.Range(0, rooms.Length);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                direction = Random.Range(3, 6); // Choose next direction
            } else {
                direction = 5;
            }
            
        } else if (direction == 5)
        {
            downCounter++;

            if (transform.position.y > minY)
            {
                Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, room);
                if(roomDetection.GetComponent<RoomType>().type != 1 && roomDetection.GetComponent<RoomType>().type != 3)
                {
                    if (downCounter >= 2)
                    {
                        roomDetection.GetComponent<RoomType>().DestroyRoom();
                        Instantiate(rooms[3], transform.position, Quaternion.identity);
                    }
                    else
                    {
                        roomDetection.GetComponent<RoomType>().DestroyRoom();

                        int randBottomRoom = Random.Range(1, 4);
                        if (randBottomRoom == 2)
                        {
                            randBottomRoom = 1;
                        }
                        Instantiate(rooms[randBottomRoom], transform.position, Quaternion.identity);
                    }
                }

                Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveAmountY);
                transform.position = newPos;

                int rand = Random.Range(2, 3);

                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                direction = Random.Range(1, 6);
            } else 
            {
                stopGen = true; // Stop generating rooms
            }
        }
    }

    private void SpawnRandomRoomTile(int roomTypeIndex)
    {
        switch (roomTypeIndex)
        {
            case 0: // Spawn a room with left and right entrances
                Instantiate(roomsLR[Random.Range(0, roomsLR.Length)], transform.position, Quaternion.identity);
                break;

            case 1: // Spawn a room with left, right and bottom entrances
                Instantiate(roomsLRB[Random.Range(0, roomsLRB.Length)], transform.position, Quaternion.identity);
                break;

            case 2: // Spawn a room with left, right and top entrances
                Instantiate(roomsLRT[Random.Range(0, roomsLRT.Length)], transform.position, Quaternion.identity);
                break;

            case 3: // Spawn a room with entrances in all directions
                Instantiate(roomsLRTB[Random.Range(0, roomsLRTB.Length)], transform.position, Quaternion.identity);
                break;

            default:
                Instantiate(rooms[roomTypeIndex], transform.position, Quaternion.identity);
                break;
        }
    }
}
