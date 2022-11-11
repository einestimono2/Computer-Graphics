using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [Range(5, 20)]
    public int maxRooms;

    public GameObject room;
    public List<GameObject> rooms;

    // Start is called before the first frame update
    void Start()
    {
        // Tạo cửa ngẫu ở 4 góc
        bool down = Random.Range(0, 4) == 1;
        bool left = Random.Range(0, 4) == 1;
        bool right = Random.Range(0, 4) == 1;
        bool up = Random.Range(0, 4) == 1;
        bool[] status = new bool[] {down, left, right, up};

        // Trường hợp 4 hướng đều = false (Không có cửa) => Random 1 cửa trong 4 vị trí
        if(!down && !left && !right && !up) status[Random.Range(0, 4)] = true;

        // Folder gốc
        Transform dungeonTransform = GameObject.FindWithTag("Dungeon").transform;

        Instantiate(room, transform.position, room.transform.rotation, transform).GetComponent<RoomBehaviour>().UpdateRoom(status, -1);

        transform.SetParent(dungeonTransform, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
