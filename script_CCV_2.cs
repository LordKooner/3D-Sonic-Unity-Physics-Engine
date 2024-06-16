using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_CCV_2 : MonoBehaviour
{
    private GameObject player;
    private script_player player_script;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("obj_player");
        player_script = player.GetComponent<script_player>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector3(player.transform.position.x + (player_script.dir_x / 1.5f), player.transform.position.y + (player_script.dir_y / 1.5f), player.transform.position.z - (player_script.dir_z / 1.5f));

        float dir_x = Mathf.Cos((player_script.angle_y) * Mathf.Deg2Rad) * Mathf.Cos((player_script.angle_z - 30) * Mathf.Deg2Rad);
        float dir_y = Mathf.Sin((player_script.angle_z - 30) * Mathf.Deg2Rad);
        float dir_z = Mathf.Sin((player_script.angle_y) * Mathf.Deg2Rad) * Mathf.Cos((player_script.angle_z - 30) * Mathf.Deg2Rad);

        Vector3 offset = new Vector3(-dir_x, -dir_y, dir_z);
        transform.position = player.transform.position + offset/1.75f;
    }
}
