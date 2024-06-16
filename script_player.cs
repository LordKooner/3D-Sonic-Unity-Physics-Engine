using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_player : MonoBehaviour
{
    public float dir_x, dir_y, dir_z, angle_x, angle_y, angle_z, acc, speed, posYchange_var;
    private float speed_x, speed_y, speed_z, acc_v = 0.1f, speed_max = 0.08f, jumpStrenght = 10, rayLength = 1, midAir_Timer;
    private bool ground = false, isJumping = false;
    public KeyCode keyForward = KeyCode.W, keyBackward = KeyCode.S, keyRight = KeyCode.D, keyLeft = KeyCode.A, keyJump = KeyCode.Space;

    private Rigidbody rigidbody_;
    private Vector3 dir_downWard, dir_upWard, collision_forWard, collision_backWard;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody_ = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        movement();
        angles360();
        collisionFunction();
        RotatePlayerToGround();

    }

    private void movement()
    {
        if (Input.GetKey(keyForward)) //FORWARD
        {

            if (acc < 1)
            {
                acc += acc_v * Time.deltaTime;
            }

            speed_x = dir_x *  speed;
            speed_y = dir_y *  speed;
            speed_z = dir_z * -speed;
            
        }
        else
        {
            if (acc > 0)
            {
                acc -= acc_v * Time.deltaTime;
            }

            speed_x = dir_x *  speed;
            speed_y = dir_y *  speed;
            speed_z = dir_z * -speed;

            rigidbody_.velocity = new Vector3(0, rigidbody_.velocity.y, 0);
        }

        if (Input.GetKey(keyBackward)) //BACKWARD
        {

            if (acc > 0 && ground == true) //BREAKS
            {
                acc -= acc_v * 4 * Time.deltaTime;
            }
            else
            if (acc > -0.25)
            {
                acc -= acc_v * Time.deltaTime;
            }

            speed_x = dir_x *  speed;
            speed_y = dir_y *  speed;
            speed_z = dir_z * -speed;
            
        }
        else
        {
            if (acc < 0)
            {
                acc += acc_v * Time.deltaTime;
            }

            speed_x = dir_x *  speed;
            speed_y = dir_y *  speed;
            speed_z = dir_z * -speed;

            rigidbody_.velocity = new Vector3(0, rigidbody_.velocity.y, 0);
        }

        if (Input.GetKey(keyRight)) //TURNING
        {
            angle_y += 0.1f + speed;
        }

        if (Input.GetKey(keyLeft))
        {
            angle_y -= 0.1f + speed;
        }

        //STICKING TO THE SURFACE
        
        if (Mathf.Abs(acc) > 0.26f ) //CAN'T DO LOOPS IF NOT FAST ENOUGH
        {
            Physics.gravity = dir_downWard * 9.81f;
        }
        else
        {
            Physics.gravity = new Vector3(0, -9.81f, 0);
        }
        
        
        //JUMPING
        if (Input.GetKey(keyJump) && ground == true)
        {
            rigidbody_.velocity = dir_upWard * jumpStrenght;
            isJumping = true;
        }

        if (isJumping)
        {
            if (!Input.GetKey(keyJump) && rigidbody_.velocity.y > 0)
            {
                rigidbody_.velocity = new Vector3(rigidbody_.velocity.x, rigidbody_.velocity.y - 0.1f, rigidbody_.velocity.z);
            }

        }

        //TRANSFORMATION
        speed = acc * speed_max;
        angle_x = 0;

        dir_x = Mathf.Cos(angle_y * Mathf.Deg2Rad) * Mathf.Cos(angle_z * Mathf.Deg2Rad);
        dir_y = Mathf.Sin(angle_z * Mathf.Deg2Rad);
        dir_z = Mathf.Sin(angle_y * Mathf.Deg2Rad) * Mathf.Cos(angle_z * Mathf.Deg2Rad);

        dir_downWard = new Vector3(-(Mathf.Cos((angle_y) * Mathf.Deg2Rad) * Mathf.Cos((angle_z + 90) * Mathf.Deg2Rad)),
                                   -(Mathf.Sin((angle_z + 90) * Mathf.Deg2Rad)),
                                    (Mathf.Sin((angle_y) * Mathf.Deg2Rad) * Mathf.Cos((angle_z + 90) * Mathf.Deg2Rad)));

        dir_upWard = new Vector3((Mathf.Cos((angle_y) * Mathf.Deg2Rad) * Mathf.Cos((angle_z + 90) * Mathf.Deg2Rad)),
                                 (Mathf.Sin((angle_z + 90) * Mathf.Deg2Rad)),
                                -(Mathf.Sin((angle_y) * Mathf.Deg2Rad) * Mathf.Cos((angle_z + 90) * Mathf.Deg2Rad)));

        transform.position = new Vector3(transform.position.x + speed_x, transform.position.y + speed_y, transform.position.z + speed_z);
        transform.rotation = Quaternion.Euler(angle_x, angle_y, angle_z);

        //Physics.gravity = dir_downWard * 9.81f;
    }

    bool isObjectHere(Vector3 position) // I STOLE THIS FUNCTION >:3
    {
        Collider[] intersecting = Physics.OverlapSphere(position, 0.01f);
        if (intersecting.Length == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void collisionFunction()
    {

        collision_backWard = new Vector3(-(Mathf.Cos((angle_y) * Mathf.Deg2Rad) * Mathf.Cos((angle_z - 30) * Mathf.Deg2Rad)),
                                         -(Mathf.Sin((angle_z - 30) * Mathf.Deg2Rad)),
                                          (Mathf.Sin((angle_y) * Mathf.Deg2Rad) * Mathf.Cos((angle_z - 30) * Mathf.Deg2Rad)));

        collision_forWard = new Vector3((Mathf.Cos((angle_y) * Mathf.Deg2Rad) * Mathf.Cos((angle_z + 30) * Mathf.Deg2Rad)),
                                        (Mathf.Sin((angle_z + 30) * Mathf.Deg2Rad)),
                                       -(Mathf.Sin((angle_y) * Mathf.Deg2Rad) * Mathf.Cos((angle_z + 30) * Mathf.Deg2Rad)));

        if (Physics.CheckSphere(transform.position + collision_forWard/1.75f, 0.0625f))
        {
            if (acc > 0)
            {
                acc = 0;
                Debug.Log("Forward collision");
            }
        }

        if (Physics.CheckSphere(transform.position + collision_backWard/1.75f, 0.0625f))
        {
            if (acc < 0)
            {
                acc = 0;
                Debug.Log("Backward collision");
            }
        }

        //RESET ANGLE WHEN MID AIR
        if (!isObjectHere(transform.position + dir_downWard / 1.75f))
        {
            ground = false;
        }
        else
        {
            ground = true;
            isJumping = false;
        }
    }

    void RotatePlayerToGround()
    {
        RaycastHit hit;       

        int layerMask = 1 << 2;
        layerMask = ~layerMask;

        // Cast a ray downwards from the player's position
        if (Physics.Raycast(transform.position, dir_downWard, out hit, rayLength, layerMask)) 
            //||
            //(Physics.Raycast(transform.position, transform.TransformDirection(dir_x, dir_y, -dir_z), out hit, rayLength, layerMask) 
            //&& (angle_x <= 0.1 || angle_x >= -0.1)
            //&& (angle_z <= 0.1 || angle_z >= -0.1)))
        {
            // Get the surface normal
            Vector3 surfaceNormal = hit.normal;

            // Calculate the new rotation to align with the slope
            Quaternion slopeRotation = Quaternion.FromToRotation(dir_upWard, surfaceNormal) * transform.rotation;

            // Apply the rotation
            //transform.rotation = slopeRotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, slopeRotation, Time.deltaTime * 12f);

            // Extract the individual x, y, and z rotations
            Vector3 eulerAngles = transform.rotation.eulerAngles;
            //angle_x = eulerAngles.x;
            angle_z = eulerAngles.z;
         
        }
        else
        {
            /*
            if (angle_x > 0.1)
            { angle_x -= -0.1f; }
            else
                if (angle_x < -0.1)
            { angle_x += 0.1f; }
            */

            if (angle_z > 0.1)
            { angle_z -= 0.1f; }
            else
            if (angle_z < -0.1)
            { angle_z += 0.1f; }
        }
    }

    void angles360()
    {
        if (angle_x > 180)
        {
            angle_x -= 360;
        }
        else
        if (angle_x <= -180)
        {
            angle_x += 360;
        }

        if (angle_y > 180)
        {
            angle_y -= 360;
        }
        else
        if (angle_y <= -180)
        {
            angle_y += 360;
        }

        if (angle_z > 180)
        {
            angle_z -= 360;
        }
        else
        if (angle_z <= -180)
        {
            angle_z += 360;
        }
    }
}
