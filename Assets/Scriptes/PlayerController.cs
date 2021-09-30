using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour 
{
    private Rigidbody2D rb;
    private Animator anim;
    private enum State { idle, run, jump, falling, hurt};
    private State state = State.idle;
    private Collider2D coll;
    private AudioSource footstep;
    [SerializeField] private LayerMask Ground; 
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private int cherries = 0;
    [SerializeField] private Text cherryText;
    [SerializeField] private float hurtForce = 10f;
    [SerializeField] private int health;
    [SerializeField] private Text healthAmount;
   

    

    
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        healthAmount.text = health.ToString();
        footstep = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        if(state != State.hurt){
              Movement();

        }
      
    
        VelocityState();
        anim.SetInteger("State", (int)state);



    }


    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Collectable"){
            Destroy(collision.gameObject);
            cherries += 1;
            cherryText.text = cherries.ToString();
        }
        if(collision.tag == "Powerup"){
            Debug.Log("Powerup");


        }
    }
    private void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag == "Enemy"){



         if(state == State.falling){   
            Destroy(other.gameObject);
         }else{
             state = State.hurt;
             PlayerHurt();
             
             if(other.gameObject.transform.position.x > transform.position.x){
                 // enemy is to right
                 rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
             }
             else{
                 // enemy is to left
                 rb.velocity = new Vector2(hurtForce, rb.velocity.y);
             }




         }

    }

    }

    // Call this function when player is hurt by touching with
    // the enemy. This decreases player health by 1. If health
    // reaches 0 game ends.
    private void PlayerHurt(){
        health -= 1;
        healthAmount.text = health.ToString();

        if (health <= 0){
            SceneManager.LoadScene(0);
        }
    }


    private void VelocityState()
    {
         if (state == State.jump)
         {
            if(rb.velocity.y < .1f){
                state = State.falling;
            } 
         }   
            else if(state == State.falling){
                if(coll.IsTouchingLayers(Ground)){
                    state = State.idle;
                }
            }
         
        else if(Mathf.Abs(rb.velocity.x) > 2f){
            state = State.run;
        }
        else{
            state = State.idle;
             }
    }
   private void Movement(){
       float hDirection = Input.GetAxis("Horizontal");

        // if player falls restart the game
        if(rb.transform.position.y < -12){
            SceneManager.LoadScene(0);
        }
        

        if (hDirection > 0) {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1); 
         
        }

       else if (hDirection < 0){
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1); 
            
        } else{

        }

        // Player can only jump when he is on the ground.
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(Ground)){
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            state = State.jump;
        }
    } 

    private void FootStep()
    {
        footstep.Play();
    }
        

}   
