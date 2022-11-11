using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    // 0 --> Down, 1 --> Left, 2 --> Right, 3 --> Up
    public int doorDirection;

    public GameObject room;

    private DungeonGenerator dungeonGenerator;
    private bool spawned = false;
    private Transform parent;

    // Start is called before the first frame update
    void Start()
    {
        parent = GameObject.FindGameObjectWithTag("Dungeon").transform;
        dungeonGenerator = GameObject.FindGameObjectWithTag("Dungeon").GetComponent<DungeonGenerator>();
        
        if(dungeonGenerator.rooms.Count < dungeonGenerator.maxRooms){
            // Chạy hàm tên Spawn sau 1s
            Invoke("Spawn", 0.5f);
        }
        else{
            // Khi vượt quá số phòng tối đa, chặn tất cả các cửa dư thừa (chuyển cửa thành tường) (tránh trường hợp không có phòng khi mở cửa ra)
            Debug.LogError(this.gameObject.transform.parent.gameObject.transform.parent);
            Debug.Log(this.gameObject);

            this.gameObject.transform.parent.gameObject.transform.parent.GetComponent<RoomBehaviour>().replaceDoor(doorDirection);

            // Tắt tạo phòng tại điểm đó.
            this.gameObject.SetActive(false);
            
        }
    }

    // Update is called once per frame
    void Spawn()
    {
        if(spawned == false){
            // Lấy vị trí ngược lại của doorDirection ( 0 <=> 3, 1 <=> 2)
            int opposite = -1;
            if(doorDirection == 0) opposite = 3; 
            else if(doorDirection == 1) opposite = 2; 
            else if(doorDirection == 2) opposite = 1; 
            else if(doorDirection == 3) opposite = 0; 

            // Tạo phòng với cửa ngẫu nhiên ở trái or phải or trên or dưới
            Instantiate(room, transform.position, room.transform.rotation, parent).GetComponent<RoomBehaviour>().UpdateRoom(randomDirection(opposite), opposite);          

            spawned = true;
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.CompareTag("RoomSpawnPoint")){
            if(other.GetComponent<RoomGenerator>().spawned == false && spawned == false){
                // Trường hợp tạo phòng bị đè lên nhau
                // Thay thay thế phòng ở ngoài cùng thành phòng không có cửa
                Debug.LogError("Trigger: " + gameObject.transform.parent.gameObject.transform.parent);
				
                gameObject.transform.parent.gameObject.transform.parent.GetComponent<RoomBehaviour>().replaceDoor(doorDirection);

				// Destroy(gameObject);
                gameObject.SetActive(false);
			} 
			spawned = true;
        }
    }

    bool[] randomDirection(int opposite){
        if(opposite == 0){
            switch (Random.Range(0, 6))
            {
                case 0:
                    return new bool[]{true, true, false, false}; //Down Left
                case 1:
                    return new bool[]{true, false, true, false}; //Down Right
                case 2:
                    return new bool[]{true, false, false, true}; //Down Up
                case 3:
                    return new bool[]{true, true, true, false}; //Down Left Right
                case 4:
                    return new bool[]{true, true, false, true}; //Down Left Up
                case 5:
                    return new bool[]{true, false, true, true}; //Down Right Up
                default: 
                    return new bool[]{false, false, false, false}; //
            }

        }else if(opposite == 1){
            switch (Random.Range(0, 6))
            {
                case 0:
                    return new bool[]{true, true, false, false}; //Down Left
                case 1:
                    return new bool[]{false, true, true, false}; //Left Right
                case 2:
                    return new bool[]{false, true, false, true}; //Left Up
                case 3:
                    return new bool[]{true, true, true, false}; //Down Left Right
                case 4:
                    return new bool[]{true, true, false, true}; //Down Left Up
                case 5:
                    return new bool[]{false, true, true, true}; //Left Right Up
                default:
                    return new bool[]{false, false, false, false}; //
            }
            
        }else if(opposite == 2){
            switch (Random.Range(0, 6))
            {
                case 0:
                    return new bool[]{false, true, true, false}; //Right Left
                case 1:
                    return new bool[]{true, false, true, false}; //Down Right
                case 2:
                    return new bool[]{false, false, true, true}; //Right Up
                case 3:
                    return new bool[]{true, true, true, false}; //Down Left Right
                case 4:
                    return new bool[]{false, true, true, true}; //Right Left Up
                case 5:
                    return new bool[]{true, false, true, true}; //Down Right Up
                default:
                    return new bool[]{false, false, false, false}; //
            }
            
        }else{
            switch (Random.Range(0, 6))
            {
                case 0:
                    return new bool[]{false, true, false, true};//Up Left
                case 1:
                    return new bool[]{false, false, true, true};//Up Right
                case 2:
                    return new bool[] {true, false, false, true};//Down Up
                case 3:
                    return new bool[]{false, true, true, true};//Up Left Right
                case 4:
                    return new bool[]{true, true, false, true};//Down Left Up
                case 5:
                    return new bool[]{true, false, true, true};//Down Right Up
                default:
                    return new bool[]{false, false, false, false}; //
            }
        }
    }
}
