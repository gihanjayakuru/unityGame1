using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private const float LANE_DISTANCE = 3f ;
    private const float TURN_SPEED = 1.5f;


    //Func
    private bool isRunning = false;

    //Animation
    private Animator anim;

    //movement
    private CharacterController controller;
    private float jumpForce =9.0f;
    private float gravity = 25.0f;
    private float verticalVelocity ;
    
    private int desiredLane = 1;

    //Speed modifier
    private float originalSpeed = 7.0f;
    private float speed = 7.0f;
    private int n = 7;
    private float speedIcreaseLastTick;
    private float speedIncreaseTime = 3.5f;
    private float speedIncreaseAmount = 0.8f;

    private void Start()
    {
        speed = originalSpeed;
        controller =GetComponent<CharacterController>();
        anim= GetComponent<Animator>();
    }

    private void Update()
    {
        if(!isRunning)
            return;

        if(Time.time - speedIcreaseLastTick > speedIncreaseTime)
        {
            speedIcreaseLastTick = Time.time;
            speed += speedIncreaseAmount;
            
            GameManager.Instance.UpdateModifier(speed - originalSpeed);
        }

        //gether the Input.GetButton
        if(MobileInput.Instance.SwipeLeft)
            
             MoveLane(false);
        if(MobileInput.Instance.SwipeRight)
             MoveLane(true);
        //calculate future posi
        Vector3 targetPosition = transform.position.z * Vector3.forward;

        if(desiredLane == 0){
            targetPosition += Vector3.left * LANE_DISTANCE;
            }
        else if(desiredLane == 2){
            targetPosition += Vector3.right * LANE_DISTANCE;
        }
        //Let's 
        Vector3 moveVector = Vector3.zero;
        moveVector.x = (targetPosition - transform.position).normalized.x * n ;//*speed ####bbbbbbb

        

       bool isGrounded = IsGrounded();
        anim.SetBool("Grounded",isGrounded);
    //calculate Y
       if (isGrounded)// if grounded
       {
           
           verticalVelocity = 0.0f;////-1bbbbbbbbbbbbbbb###

        if (MobileInput.Instance.SwipeUp)
           
           {    //jump
                anim.SetTrigger("Jump");
               verticalVelocity = jumpForce;
           }
           else if(MobileInput.Instance.SwipeDown)
           {
               
               StartSliding();
               Invoke("StopSliding",1.0f);

           }
       }

       else
       {
           verticalVelocity -= (gravity * Time.deltaTime);

           //Fast falling mechanic
           if(MobileInput.Instance.SwipeDown)
           {
               verticalVelocity = -jumpForce;
           }
       }

        moveVector.y = verticalVelocity;
        moveVector.z = speed;

        //move the character
        controller.Move(moveVector * Time.deltaTime);
       // rotate the charater to going
        Vector3 dir=controller.velocity;

        if(dir != Vector3.zero)
        {
        dir.y = 0;
        transform.forward = Vector3.Lerp(transform.forward, dir,TURN_SPEED);

        }
        
    }
    private void StartSliding()
    {
        anim.SetBool("Sliding",true);
        controller.height /= 2;
        controller.center = new Vector3(controller.center.x,controller.center.y /2, controller.center.z); 
    }
    private void StopSliding()
    {
        anim.SetBool("Sliding",false);
        controller.height *= 2;
        controller.center = new Vector3(controller.center.x,controller.center.y * 2, controller.center.z);
    }

    private void MoveLane(bool goingRight)
    {
        desiredLane += (goingRight) ? 1 : -1;
        desiredLane = Mathf.Clamp(desiredLane, 0, 2);

    }

    private bool IsGrounded()
    {
        Ray groundRay = new Ray(new Vector3(controller.bounds.center.x,
        (controller.bounds.center.y - controller.bounds.extents.y) + 0.2f,controller.bounds.center.z),Vector3.down);

        Debug.DrawRay(groundRay.origin, groundRay.direction,Color.cyan,1.0f);
    
        return Physics.Raycast(groundRay, 0.2f + 0.1f);
        

    }


    public void StartRunning()
    {
        isRunning = true; 
        anim.SetTrigger("StartRuning");

    }

    private void Crash()
    {
        anim.SetTrigger("Death");
        isRunning = false;
        GameManager.Instance.OnDeath();
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch(hit.gameObject.tag)
        {
            case "Obstacle":
                Crash();
            break;
        }

    }

}