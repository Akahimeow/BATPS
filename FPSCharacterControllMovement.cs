using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCharacterControllMovement : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Hoshino;


    private CharacterController characterController;
    private Transform characterTransform;
    private Vector3 inputDirection;
    private Vector3 characterDirection;//运动方向



    public Animator CharacterAnimator;//动画
    public float velocity;
    private bool isRun;

    public float WalkSpeed= 10f;//走路速度
    public float SprintingSpeed = 15f;//跑步速度
    public float CrouchWalkSpeed = 5f;//蹲下移动速度
    public float CrouchSprintingSpeed = 8f;//蹲下快速移动速度
    public float gravity=9.8f;//重力
    public float jumpHight=2f;//跳跃高度

    public bool isCrounch;  //是否蹲下
    public float orignHeight;//站立高度
    public float crouchHeight=1f;  //蹲下高度

    private Vector3 currentDirection;



    void Start()
    {
        characterController = GetComponent<CharacterController>();
        characterTransform = transform;
        orignHeight = characterController.height;
    }

    // Update is called once per frame
    void Update()
    {
        float tmp_moveSpeed = WalkSpeed;
        if (characterController.isGrounded)//触地检测
        {
            float tmp_horizontal = Input.GetAxis("Horizontal");
            float tmp_vertical = Input.GetAxis("Vertical");
            inputDirection = new Vector3(tmp_horizontal, 0, tmp_vertical);
            characterDirection = characterTransform.TransformDirection(inputDirection);//从自身坐标到世界坐标变换方向。

            if (Input.GetButtonDown("Jump"))
            {
                characterDirection.y = jumpHight;
            }


            if (isCrounch) {
                //蹲下奔跑 or 走路
                tmp_moveSpeed = Input.GetKey(KeyCode.LeftShift) ? CrouchSprintingSpeed : CrouchWalkSpeed;
            }
            else {
                //正常奔跑 or 走路
                isRun = Input.GetKey(KeyCode.LeftShift);
                tmp_moveSpeed = isRun ? SprintingSpeed : WalkSpeed;
                
            }



                if (Input.GetKeyDown(KeyCode.C))
            {
                var tmp_crounchHeight = isCrounch ? orignHeight : crouchHeight;         //蹲下  站立 交替
                StartCoroutine(DoCrouch(tmp_crounchHeight));//协程
               
                isCrounch = !isCrounch;


            }


            velocity = characterController.velocity.magnitude;
            CharacterAnimator.SetFloat("Velocity", velocity);
            CharacterAnimator.SetBool("Run", isRun);

            //转向
            if (inputDirection.x != 0 || inputDirection.z != 0)
            {
                
                float lerpValue = 0.1f; // 插值比例，可以根据需要调整
                Hoshino.transform.rotation = Quaternion.Lerp(Hoshino.transform.rotation, Quaternion.LookRotation(characterDirection), lerpValue);
                
            }




        }
        

        //更改运动方向的y轴值，以实现受到向下的重力
        characterDirection.y -= gravity * Time.deltaTime;

        //利用characterController.Move接口实现移动  运动方向(向量)*速度*时间
        characterController.Move(characterDirection.normalized * tmp_moveSpeed * Time.deltaTime);

        

        

        
    }



    private IEnumerator DoCrouch(float _target)
    {
        float tmp_currentHeight = 0f;
        while (Mathf.Abs(characterController.height-_target)>0.1f) {

            yield return null;
            characterController.height =
                Mathf.SmoothDamp(characterController.height, _target,
                ref tmp_currentHeight, Time.deltaTime * 5);

        }
        

    }






}
