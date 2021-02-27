using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public Transform playerTransform;
    float accel = 0.8f;
    void Movement()
    {
        if(isLocalPlayer)
        {   
            if (Input.GetKey("up"))
            {
                if (playerTransform.position.y < 7.2f) //Upper limit
                    playerTransform.position += Vector3.up * accel;
            }
            if (Input.GetKey("down"))
            {
                if (playerTransform.position.y > -4.85f) //Lower limit
                    playerTransform.position += Vector3.down * accel;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }
}
