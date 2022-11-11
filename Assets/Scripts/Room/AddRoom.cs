using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    private DungeonGenerator dungeonGenerator;

    // Start is called before the first frame update
    void Start()
    {
        dungeonGenerator = GameObject.FindGameObjectWithTag("Dungeon").GetComponent<DungeonGenerator>();

        // if(dungeonGenerator.rooms.Count < dungeonGenerator.maxRooms){
        //     dungeonGenerator.rooms.Add(this.gameObject);
        // }else{
        //     Debug.Log("Destroy" + this.gameObject);
        //     Destroy(this.gameObject);
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
