using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMovement : MonoBehaviour
{
    public float Speed;//�ƶ��ٶ�
    public float Gravity;//����
    public float JumpHeight;//��Ծ�߶�


    private Transform characterTransform;
    private Rigidbody characterRigdbody;

    private bool isGrounded;//���ؼ�����

    private bool isJump;//��Ծ������

    // Start is called before the first frame update
    void Start()
    {
        characterTransform = transform;
        characterRigdbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (isGrounded)//ֻ�д��ز����ƶ�
        {
            var tmp_Horizontal = Input.GetAxis("Horizontal");
            var tmp_Vertical = Input.GetAxis("Vertical"); //��ȡ��������

            var tmp_CurrentDirection = new Vector3(tmp_Horizontal, 0, tmp_Vertical);
            tmp_CurrentDirection = characterTransform.TransformDirection(tmp_CurrentDirection);//��ɫ����ת��Ϊ��������
            tmp_CurrentDirection *= Speed;

            var tmp_CurrentVelocity = characterRigdbody.velocity;
            var tmp_VelocityChange = tmp_CurrentDirection - tmp_CurrentVelocity;
            tmp_VelocityChange.y = 0;

            characterRigdbody.AddForce(tmp_VelocityChange, ForceMode.VelocityChange);

            if (isJump)//��Ծ
            {
                characterRigdbody.velocity = new Vector3(tmp_CurrentVelocity.x, CalculateJumpHeightSpeed(), tmp_CurrentVelocity.z);
                isJump = false;
            }

        }

        characterRigdbody.AddForce(new Vector3(0, -Gravity * characterRigdbody.mass, 0));//��ɫ�Ϳ�ʱ���ܵ����µ�����

    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            isJump = true;
        }
    }


    private float CalculateJumpHeightSpeed()
    {
        return Mathf.Sqrt(2 * Gravity * JumpHeight); //�������ٸ���Ծ�߶Ⱥ������й�
    }


    private void OnCollisionStay(Collision _other)
    {
        isGrounded = true;
    }


    private void OnCollisionExit(Collision _other)
    {
        isGrounded = false;
    }


}
