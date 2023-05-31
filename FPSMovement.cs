using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMovement : MonoBehaviour
{
    public float Speed;//移动速度
    public float Gravity;//重力
    public float JumpHeight;//跳跃高度


    private Transform characterTransform;
    private Rigidbody characterRigdbody;

    private bool isGrounded;//触地检测参数

    private bool isJump;//跳跃检测参数

    // Start is called before the first frame update
    void Start()
    {
        characterTransform = transform;
        characterRigdbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (isGrounded)//只有触地才能移动
        {
            var tmp_Horizontal = Input.GetAxis("Horizontal");
            var tmp_Vertical = Input.GetAxis("Vertical"); //获取方向输入

            var tmp_CurrentDirection = new Vector3(tmp_Horizontal, 0, tmp_Vertical);
            tmp_CurrentDirection = characterTransform.TransformDirection(tmp_CurrentDirection);//角色坐标转化为世界坐标
            tmp_CurrentDirection *= Speed;

            var tmp_CurrentVelocity = characterRigdbody.velocity;
            var tmp_VelocityChange = tmp_CurrentDirection - tmp_CurrentVelocity;
            tmp_VelocityChange.y = 0;

            characterRigdbody.AddForce(tmp_VelocityChange, ForceMode.VelocityChange);

            if (isJump)//跳跃
            {
                characterRigdbody.velocity = new Vector3(tmp_CurrentVelocity.x, CalculateJumpHeightSpeed(), tmp_CurrentVelocity.z);
                isJump = false;
            }

        }

        characterRigdbody.AddForce(new Vector3(0, -Gravity * characterRigdbody.mass, 0));//角色滞空时会受到向下的重力

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
        return Mathf.Sqrt(2 * Gravity * JumpHeight); //能跳多少跟跳跃高度和重力有关
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
