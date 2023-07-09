using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveForce = 10f;

    [SerializeField]
    private float jumpForce = 11f;

    private float movementX;

    [SerializeField]
    private Rigidbody2D myBody;

    public Animator anim { get; set; }

    private SpriteRenderer sr;

    private string RUN = "Running";

    private bool IsGrounded;
    private string GROUND_TAG = "Ground";




    public double Attack { get; set; } = 0.5;

    public double Health { get; set; } = 3;

    [SerializeField]
    private GameObject attackPoint;

    [SerializeField]
    private float Radius;

    [SerializeField]
    private LayerMask Enemies;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {

    }

    void Update()
    {
        PlayerMoveKeyboard();
        AnimatePlayer();
        PlayerJump();
        PlayerAttackAnimation();

    }

    void PlayerMoveKeyboard()
    {
        movementX = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(movementX, 0f, 0f) * moveForce * Time.deltaTime;
    }

    void AnimatePlayer()
    {

        if (this.anim != null) { 

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
    }

    void PlayerJump()
    {
        if (this.anim != null)
        {
            if (Input.GetButtonDown("Jump") && IsGrounded)
            {
                IsGrounded = false;
                myBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GROUND_TAG))
        {
            IsGrounded = true;
        }
    }


    void PlayerAttackAnimation()
    {
        if (this.anim != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                anim.SetTrigger("Active");
            }
        }
    }

    void AttackAction()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.transform.position, Radius, Enemies);

        foreach (Collider2D enemyGameObject in hitEnemies)
        {
            Debug.Log("Hit enemy");
            Enemy otherEnemy = enemyGameObject.GetComponent<Enemy>();
            if (otherEnemy != null && otherEnemy.anim != null)
            {
                if (otherEnemy.Health > this.Attack)

                {

                    otherEnemy.Health -= this.Attack;
                    otherEnemy.anim.SetTrigger("Hurt");
                        
                }
                else
                {
                    otherEnemy.playerDeath();
                    stopMoving();
                    
                        
                }
            }
        }
    }


    public void stopMoving()
    {

        this.Attack = 0;
        this.moveForce = 0;
        this.jumpForce = 0;
        this.anim = null;


    }

    public void playerDeath()
    {

        this.anim.SetTrigger("Death");
        this.Attack = 0;
        this.moveForce = 0;
        this.jumpForce = 0;
        this.anim = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.transform.position, Radius);
    }
}
