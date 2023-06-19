using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float moveForce = 10f;

    [SerializeField]
    private float jumpForce = 11f;

    private float movementX;

    [SerializeField]
    private Rigidbody2D myBody;

    private Animator anim;

    private SpriteRenderer sr;

    private string RUN = "Running";

    private bool IsGrounded;
    private string GROUND_TAG = "Ground";

    private string ATTACK = "Attacking";

    private bool Attacking = false;


    //Awake is used to get the components from unity, and is executed once the game is initiated
    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>(); // Instance the components added to the player to animate and move the body of the player.
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame updat
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        PlayerMoveKeyboard();
        AnimatePlayer();
        PlayerJump();
        PlayerAttack();


    }


    void PlayerMoveKeyboard()

    { // Get the position of the player, from -1 to 1.
        movementX = Input.GetAxisRaw("Horizontal_2");

        /* Change the position of the player multiplying the Vector3 which has 3 axis, 
            we only modify the first in order to move in the x axis, then we multiply with the moveForce
            and the deltaTime, which is a component that allow us to do a movement equals to the player frames quantity*/
        transform.position += new Vector3(movementX, 0f, 0f) * moveForce * Time.deltaTime;
    }

    /* In order to make the player move we set the animation to true, and if in case the player is moving to the left
    we flip the asset of the player*/

    void AnimatePlayer()
    {
        if (movementX > 0)
        {
            anim.SetBool(RUN, true);
            sr.flipX = false;
        }
        else if (movementX < 0)
        {
            anim.SetBool(RUN, true);
            sr.flipX = true;
        }
        else
        {
            anim.SetBool(RUN, false);
        }
    }

    //

    void PlayerJump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded)
        {
            IsGrounded = false;
            myBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

        }
    }

    // This function allow us to detect collisions between two objetcs

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If our object collision with our ground we set the value to true, to be able to use the JumpAnimation
        if (collision.gameObject.CompareTag(GROUND_TAG))
        {
            IsGrounded = true;
        }
    }


    void PlayerAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Active");
        }
    }

}