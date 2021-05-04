using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //�ڬO�����ܼ�
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
        //�]�w��l�Ѽ�
        playerTra = GetComponent<Transform>();
        playerRig = GetComponent<Rigidbody>();
        playerAni = GetComponent<Animator>();
        offset = playerTra.position;
    }

    void Update()
    {
        //�����ť�ð���H�U��k
        CameraMain();
        MovementX();
        MovementY();

        //speedX = playerRig.velocity.x;
    }

    //�۾��l�H
    void CameraMain()
    {
        //�u��X�b
        playerCam.transform.position += new Vector3(playerTra.position.x, 0, 0) - new Vector3(offset.x, 0, 0);
        offset = playerTra.position;
    }

    //��������
    void MovementX()
    {
        //horizontalDirection = Input.GetAxis("Horizontal");
        //playerRig.AddForce(new Vector3(forceX * horizontalDirection, 0));

        //���k�P�ɫ��ڴN����
        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)))
        {
            playerAni.ResetTrigger("isMoving");
        }
        //�k
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (isGrounded)
                playerAni.SetTrigger("isMoving");
            else
                playerAni.ResetTrigger("isMoving");

            playerTra.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            playerRig.transform.Translate(Vector3.right * forceX);
        }
        //��
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (isGrounded)
                playerAni.SetTrigger("isMoving");
            else
                playerAni.ResetTrigger("isMoving");

            playerTra.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            playerRig.transform.Translate(Vector3.right * forceX);
        }
        //�䥦
        else
        {
            playerAni.ResetTrigger("isMoving");
        }
    }

    //���D
    void MovementY()
    {
        //�O�o���ä��n������
        
        //�ť�����D
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerAni.ResetTrigger("isMoving");
            playerRig.AddForce(Vector3.up * forceY);
        }

        //�W��W���D
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && isGrounded)
        {
            playerAni.ResetTrigger("isMoving");
            playerRig.AddForce(Vector3.up * forceY);
        }

    }

    //�I����
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

    //���}�I��
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == groundedObject)
        {
            groundedObject = null;
            isGrounded = false;
        }
    }
}
