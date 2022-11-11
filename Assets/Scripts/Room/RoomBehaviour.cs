using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    // 0 --> Down, 1 --> Left, 2 --> Right, 3 --> Up
    public GameObject[] walls;
    public GameObject[] doors;
    public GameObject[] spawnPoints;

    private DungeonGenerator dungeonGenerator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateRoom(bool[] status, int opposite){
        // Opposite: hướng của phòng trước đó ==> Để 2 phòng luôn thông vs nhau
        for (int i = 0; i < status.Length; i++){
            if(i == opposite){
                doors[opposite].SetActive(true);
                spawnPoints[opposite].SetActive(false);
                walls[opposite].SetActive(false);
            }else{
                doors[i].SetActive(status[i]);
                spawnPoints[i].SetActive(status[i]);
                walls[i].SetActive(!status[i]);
            }
        }

        dungeonGenerator = GameObject.FindGameObjectWithTag("Dungeon").GetComponent<DungeonGenerator>();
        if(dungeonGenerator.rooms.Count < dungeonGenerator.maxRooms){
            dungeonGenerator.rooms.Add(this.gameObject);
        }else{
            Destroy(this.gameObject);
        }
    }

    public void replaceDoor(int position){
        doors[position].SetActive(false);
        walls[position].SetActive(true);
    }
}
