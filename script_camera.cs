using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_camera : MonoBehaviour
{
    private GameObject player;
    private Camera cam;
    private script_player player_script;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("obj_player");
        cam = GetComponent<Camera>();
        player_script = player.GetComponent<script_player>();
        
    }

    // Update is called once per frame
    void Update()
    {

        //FOV
        if (player_script.acc < 0.75)
        { cam.fieldOfView = 60; }
        else
        { cam.fieldOfView = 80 * player_script.acc; }

        //FOLLOWING THE PLAYER

        float dir_x = Mathf.Cos((player_script.angle_y) * Mathf.Deg2Rad) * Mathf.Cos((player_script.angle_z - 30) * Mathf.Deg2Rad);
        float dir_y = Mathf.Sin((player_script.angle_z - 30) * Mathf.Deg2Rad);
        float dir_z = Mathf.Sin((player_script.angle_y) * Mathf.Deg2Rad) * Mathf.Cos((player_script.angle_z - 30) * Mathf.Deg2Rad);

        Vector3 offset = new Vector3(-dir_x, -dir_y, dir_z);
        transform.position = player.transform.position + offset * 6;
        transform.LookAt(new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z));

        //transform.position = new Vector3(player.transform.position.x - player_script.dir_x * 6, player.transform.position.y + 2, player.transform.position.z + player_script.dir_z * 6);
        //transform.rotation = Quaternion.Euler(transform.rotation.x + 10, player_script.angle_y + 90, transform.rotation.z);
    }


}
