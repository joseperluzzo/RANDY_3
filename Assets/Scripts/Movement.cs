using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public float movementSpeed = 20; // Default = 25 (TBC)
    public float jumpHeight = 1.25f; // Default = 1.25
    public float gravity = -2.45f; // Default = -2.45
    public float Dash = 3.5f; //Default = 10
    public float dashFriction = -10; //Default = -20
    public float dashCooldown = 2; //Default = 2
    public float vRaycastOffset = 0.1f; //Default = 0.1
    public float hRaycastOffset = 0.35f; //Default = 0.35
    public GameObject squarePrefab;
    GameObject newSquare1;
    GameObject newSquare2;
    GameObject newSquare3;


    //Jump values

    float verticalPull = 0;
    float jumpForce = 0;

    //Dash values

    float dashForce = 0;
    float dashTimer = float.MaxValue;
    bool isGrounded = false;
    bool canDash = true;

    //Collision values

    float vScale;
    float hScale;


    float minY = -float.MaxValue;
    float maxY = float.MaxValue;
    float minX = -float.MaxValue;
    float maxX = float.MaxValue;

    //Collision functions

    RaycastHit2D collisionRaycast(Vector3 pos, Vector2 direction, Vector3 offset)
    {

        RaycastHit2D hit1 = Physics2D.Raycast(pos + offset * 1, direction);
        RaycastHit2D hit2 = Physics2D.Raycast(pos + offset * 0, direction);
        RaycastHit2D hit3 = Physics2D.Raycast(pos + offset * -1, direction);

        RaycastHit2D targetRay = hit2;

        if (hit1.distance <= targetRay.distance)
        {
            targetRay = hit1;
        }
        if (hit3.distance <= targetRay.distance)
        {
            targetRay = hit3;
        }

        return targetRay;

    }




    void Start()
    {
        vScale = transform.localScale.y / 2;
        hScale = transform.localScale.x / 2;

        GameObject newSquare1 = Instantiate(squarePrefab, Vector3.zero, Quaternion.identity);
        GameObject newSquare2 = Instantiate(squarePrefab, Vector3.zero, Quaternion.identity);
        GameObject newSquare3 = Instantiate(squarePrefab, Vector3.zero, Quaternion.identity);

        print(newSquare1);
        print(newSquare2);

    }

    //Objects

    void Update()
    {

        float horizontalInput = Input.GetAxis("Horizontal");

        //Jump & gravity
        verticalPull = (transform.position.y <= minY) ? 0 : Mathf.Clamp(verticalPull + gravity * Time.deltaTime, gravity, 0);
        jumpForce = Mathf.Clamp(jumpForce + gravity * Time.deltaTime, 0, jumpHeight);
        isGrounded = (verticalPull == 0) ? true : false;

        //Dash
        dashTimer += Time.deltaTime;
        dashForce = Mathf.Clamp(dashForce + dashFriction * Time.deltaTime, 1, Dash);
        canDash = (dashTimer >= dashCooldown) ? true : false;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpForce = jumpHeight;
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            dashForce = Dash;
            dashTimer = 0;
        }

        //Collisions

        //Floor collider

        RaycastHit2D floorHit = collisionRaycast(transform.position + new Vector3(0, -vScale + vRaycastOffset, 0), Vector2.down, new Vector3(hScale - vRaycastOffset, 0, 0));
        if (floorHit.collider != null)
        {
            minY = floorHit.point.y + vScale;
        }
        else
        {
            minY = -float.MaxValue;
        }

        //Roof collider

        RaycastHit2D roofHit = collisionRaycast(transform.position + new Vector3(0, vScale - vRaycastOffset, 0), Vector2.up, new Vector3(hScale - vRaycastOffset, 0, 0));
        if (roofHit.collider != null)
        {
            maxY = roofHit.point.y - vScale;
        }
        else
        {
            maxY = float.MaxValue;
        }

        //Right collider

        RaycastHit2D rightHit = collisionRaycast(transform.position + new Vector3(0, hScale - hRaycastOffset, 0), Vector2.right, new Vector3(0, vScale - hRaycastOffset, 0));
        if (rightHit.collider != null)
        {
            maxX = rightHit.point.x - hScale;
        }
        else
        {
            maxX = float.MaxValue;
        }

        //Left collider

        RaycastHit2D leftHit = collisionRaycast(transform.position + new Vector3(0, -hScale + hRaycastOffset, 0), Vector2.left, new Vector3(0, vScale - hRaycastOffset, 0));
        if (leftHit.collider != null)
        {
            minX = leftHit.point.x + hScale;
        }
        else
        {
            minX = -float.MaxValue;
        }


        Vector3 movement = new Vector3(horizontalInput * dashForce, verticalPull + jumpForce) * movementSpeed * Time.deltaTime;
        transform.Translate(movement);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), Mathf.Clamp(transform.position.y, minY, maxY), transform.position.z);

        if (transform.position.y == maxY)
        {
            jumpForce = jumpForce / 10;
        }

        /*
        print("MaxX : " + maxX);
        print("MinX : " + minX);
        print("MaxY : " + maxY);
        print("MinY : " + minY);
        print("--------------");
        */

    }
}
