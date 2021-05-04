using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //我是全域變數
    Transform playerTra;
    Rigidbody playerRig;
    Animator playerAni;

    public GameObject groundedObject;
    public bool isGrounded = false;
    public Camera playerCam;
    public Vector3 offset;

    public float horizontalDirection;
    public float speedX;
    public float speedY;
    public float forceX = 0.01f;
    public float forceY = 500;

    void Start()
    {
        //設定初始參數
        playerTra = GetComponent<Transform>();
        playerRig = GetComponent<Rigidbody>();
        playerAni = GetComponent<Animator>();
        offset = playerTra.position;
    }

    void Update()
    {
        //持續監聽並執行以下方法
        CameraMain();
        MovementX();
        MovementY();

        //speedX = playerRig.velocity.x;
    }

    //相機追人
    void CameraMain()
    {
        //只動X軸
        playerCam.transform.position += new Vector3(playerTra.position.x, 0, 0) - new Vector3(offset.x, 0, 0);
        offset = playerTra.position;
    }

    //水平移動
    void MovementX()
    {
        //horizontalDirection = Input.GetAxis("Horizontal");
        //playerRig.AddForce(new Vector3(forceX * horizontalDirection, 0));

        //左右同時按我就不動
        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)))
        {
            playerAni.ResetTrigger("isMoving");
        }
        //右
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (isGrounded)
                playerAni.SetTrigger("isMoving");
            else
                playerAni.ResetTrigger("isMoving");

            playerTra.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            playerRig.transform.Translate(Vector3.right * forceX);
        }
        //左
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (isGrounded)
                playerAni.SetTrigger("isMoving");
            else
                playerAni.ResetTrigger("isMoving");

            playerTra.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            playerRig.transform.Translate(Vector3.right * forceX);
        }
        //其它
        else
        {
            playerAni.ResetTrigger("isMoving");
        }
    }

    //跳躍
    void MovementY()
    {
        //記得隱藏不要的按鍵
        
        //空白鍵跳躍
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerAni.ResetTrigger("isMoving");
            playerRig.AddForce(Vector3.up * forceY);
        }

        //上或W跳躍
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && isGrounded)
        {
            playerAni.ResetTrigger("isMoving");
            playerRig.AddForce(Vector3.up * forceY);
        }

    }

    //碰撞中
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            foreach (ContactPoint element in collision.contacts)
            {
                if (element.normal.y > 0.25f)
                {
                    isGrounded = true;
                    groundedObject = collision.gameObject;
                    break;
                }
            }
        }
    }

    //離開碰撞
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == groundedObject)
        {
            groundedObject = null;
            isGrounded = false;
        }
    }
}
