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
    private Vector3 characterDirection;//�˶�����



    public Animator CharacterAnimator;//����
    public float velocity;
    private bool isRun;

    public float WalkSpeed= 10f;//��·�ٶ�
    public float SprintingSpeed = 15f;//�ܲ��ٶ�
    public float CrouchWalkSpeed = 5f;//�����ƶ��ٶ�
    public float CrouchSprintingSpeed = 8f;//���¿����ƶ��ٶ�
    public float gravity=9.8f;//����
    public float jumpHight=2f;//��Ծ�߶�

    public bool isCrounch;  //�Ƿ����
    public float orignHeight;//վ���߶�
    public float crouchHeight=1f;  //���¸߶�

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
        if (characterController.isGrounded)//���ؼ��
        {
            float tmp_horizontal = Input.GetAxis("Horizontal");
            float tmp_vertical = Input.GetAxis("Vertical");
            inputDirection = new Vector3(tmp_horizontal, 0, tmp_vertical);
            characterDirection = characterTransform.TransformDirection(inputDirection);//���������굽��������任����

            if (Input.GetButtonDown("Jump"))
            {
                characterDirection.y = jumpHight;
            }


            if (isCrounch) {
                //���±��� or ��·
                tmp_moveSpeed = Input.GetKey(KeyCode.LeftShift) ? CrouchSprintingSpeed : CrouchWalkSpeed;
            }
            else {
                //�������� or ��·
                isRun = Input.GetKey(KeyCode.LeftShift);
                tmp_moveSpeed = isRun ? SprintingSpeed : WalkSpeed;
                
            }



                if (Input.GetKeyDown(KeyCode.C))
            {
                var tmp_crounchHeight = isCrounch ? orignHeight : crouchHeight;         //����  վ�� ����
                StartCoroutine(DoCrouch(tmp_crounchHeight));//Э��
               
                isCrounch = !isCrounch;


            }


            velocity = characterController.velocity.magnitude;
            CharacterAnimator.SetFloat("Velocity", velocity);
            CharacterAnimator.SetBool("Run", isRun);

            //ת��
            if (inputDirection.x != 0 || inputDirection.z != 0)
            {
                
                float lerpValue = 0.1f; // ��ֵ���������Ը�����Ҫ����
                Hoshino.transform.rotation = Quaternion.Lerp(Hoshino.transform.rotation, Quaternion.LookRotation(characterDirection), lerpValue);
                
            }




        }
        

        //�����˶������y��ֵ����ʵ���ܵ����µ�����
        characterDirection.y -= gravity * Time.deltaTime;

        //����characterController.Move�ӿ�ʵ���ƶ�  �˶�����(����)*�ٶ�*ʱ��
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
