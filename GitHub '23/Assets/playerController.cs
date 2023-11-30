using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class playerController : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb2d;
    BoxCollider2D box;
    Vector2 movementVector;
    public float jumpHeight = 2.0f;
    public float moveSpeed = 1.0f;
    public float airMoveSpeed = 1.0f;
    float gravityScale;
    public bool onGround;
    public GameObject currentCam;
    public bool canMove = true;
    int playerLayer = 1 << 11;
    int wallLayer = 1 << 12;
    int cameraLayer = 1 << 13;
    int goalLayer = 1 << 14;
    int deathBoxLayer = 1 << 15;

    public bool CanMove
    {
        get {return canMove;}
        set 
        {
            if(!FindObjectOfType<cameraManager>().blending && FindObjectOfType<cameraManager>().zoomedIn)
            {
                canMove = true;
            }
            else
            {
                canMove = value;
            }
        }
    }

    private void Start() 
    {
        rb2d = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        gravityScale = rb2d.gravityScale;
    }

    void Update()
    {        
        transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z + 1);

        FixBox();

        checkDeathBox();

        checkGoal();
        
        findCam();

        RaycastHit2D floor = 
          Physics2D.Raycast(new Vector2(transform.position.x - box.size.x / 2 * transform.localScale.x,
                                        transform.position.y - box.size.y / 2 * transform.localScale.y - .1f),
                            new Vector2(1,0),
                            box.size.x * transform.localScale.x,
                            wallLayer);

        onGround = floor;

        if(canMove)
        {
            move();

            jump();
        }

    }

    private void move()
    {
        if(!onGround && Input.GetAxisRaw("Horizontal") != 0)
        {
            RaycastHit2D wall = 
              Physics2D.Raycast(new Vector2(Input.GetAxisRaw("Horizontal") * 0.1f + transform.position.x + box.size.x  * Input.GetAxisRaw("Horizontal") * transform.localScale.x / 2,
                                            transform.position.y - box.size.y / 2 * transform.localScale.y),
                                new Vector2(0,1),
                                box.size.x * transform.localScale.x,
                                wallLayer);

            if(wall)
            {
                movementVector.x = 0;
            }
            else
            {
                movementVector.x = Input.GetAxisRaw("Horizontal") * airMoveSpeed;
            }
        }
        else
        {
            movementVector.x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        }  
    }

    private void jump()
    {
        if((Input.GetKeyDown("space") || Input.GetKeyDown("up") || Input.GetKeyDown("w")) && onGround)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpHeight);
        }

        if((Input.GetKey("space") || Input.GetKey("up") || Input.GetKey("w")) && rb2d.velocity.y > 0)
        {
            rb2d.gravityScale = gravityScale/2;
        }
        else
        {
            rb2d.gravityScale = gravityScale/2;
        }
    }

    private void FixedUpdate() 
    {
        if(!FindObjectOfType<cameraManager>().zoomedIn)
        {
            movementVector = new Vector2(0, movementVector.y);
        }
        
        rb2d.velocity = new Vector2(movementVector.x, rb2d.velocity.y);    
    }

    private void findCam()
    {
        RaycastHit2D camera = Physics2D.Raycast(transform.position, transform.position, 0.1f, cameraLayer);

        if(camera)
        {
            currentCam = camera.transform.gameObject;       
        }
    }

    void checkGoal()
    {
        RaycastHit2D goal = 
          Physics2D.Raycast(new Vector2(transform.position.x - box.size.x / 2 * transform.localScale.x,
                                        transform.position.y - box.size.y / 2 * transform.localScale.y),
                            new Vector2(1,0),
                            box.size.x * transform.localScale.x,
                            goalLayer);

        if(goal)
        {
            goal.transform.GetComponent<goal>().Win();
        }

        goal = 
          Physics2D.Raycast(new Vector2(transform.position.x - box.size.x / 2 * transform.localScale.x,
                                        transform.position.y - box.size.y / 2 * transform.localScale.y),
                            new Vector2(0,1),
                            box.size.x * transform.localScale.x,
                            goalLayer);

        if(goal)
        {
            goal.transform.GetComponent<goal>().Win();
        }

        goal = 
          Physics2D.Raycast(new Vector2(transform.position.x + box.size.x / 2 * transform.localScale.x,
                                        transform.position.y + box.size.y / 2 * transform.localScale.y),
                            new Vector2(0,-1),
                            box.size.x * transform.localScale.x,
                            goalLayer);

        if(goal)
        {           
            goal.transform.GetComponent<goal>().Win();
        }

        goal = 
          Physics2D.Raycast(new Vector2(transform.position.x + box.size.x / 2 * transform.localScale.x,
                                        transform.position.y + box.size.y / 2 * transform.localScale.y),
                            new Vector2(-1,0),
                            box.size.x * transform.localScale.x,
                            goalLayer);

        if(goal)
        {            
            goal.transform.GetComponent<goal>().Win();
        }
    }

    void checkDeathBox()
    {
        RaycastHit2D deathbox = Physics2D.Raycast(transform.position, Vector2.down, 1f, deathBoxLayer);

        if(deathbox)
        {
            respawn(deathbox.transform.GetComponent<deathbox>().target.position);
        }
    }

    public void respawn(Vector2 vector2)
    {
        rb2d.position = vector2;
        
        StartCoroutine(respawnMoveDelay());
    }

    IEnumerator respawnMoveDelay()
    {
        while(!onGround)
        {
            movementVector.x = 0;

            yield return new WaitForEndOfFrame();
        }
    }

    void FixBox()
    {
        RaycastHit2D box = Physics2D.Raycast(transform.position, Vector2.up, 1f, wallLayer);

        if(box)
        {
            if(box.transform.CompareTag("box"))
            {
                transform.Translate(new Vector2(0,2));
            }
        }
    }
}
