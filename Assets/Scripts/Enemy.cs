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

    private string RUN = "Run";

    private string GROUNDED = "Grounded";
    private bool IsGrounded;
    private string GROUND_TAG = "Ground";
    private string JUMP = "Jump";
    private string ATTACK = "Attack";

    private bool Attacking = false;

    private string PLAYER_TAG = "Player";

    public double Attack { get; set; } = 1;

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
        PlayerAttack();
        
    }

    void PlayerMoveKeyboard()
    {
        movementX = Input.GetAxisRaw("Horizontal_2");
        transform.position += new Vector3(movementX, 0f, 0f) * moveForce * Time.deltaTime;
    }

    void AnimatePlayer()
    {
        if (movementX > 0)
        {
            anim.SetBool(RUN, true);
            sr.flipX = true;
        }
        else if (movementX < 0)
        {
            anim.SetBool(RUN, true);
            sr.flipX = false;
        }
        else
        {
            anim.SetBool(RUN, false);
        }
    }

    void PlayerJump()
    {
        if (Input.GetButtonDown("Jump_2") && IsGrounded)
        {
            IsGrounded = false;
            anim.SetBool(GROUNDED, false);
            anim.SetTrigger(JUMP);
            myBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GROUND_TAG))
        {
            IsGrounded = true;
            anim.SetBool(GROUNDED, true);
        }

    }

    void PlayerAttack()
    {
        if (Input.GetButtonDown("Attack")) //Input manager axe
        {
            anim.SetTrigger("Attack");
        }
    }


    void AttackAction()
    {
        Collider2D[] Enemy = Physics2D.OverlapCircleAll(attackPoint.transform.position, Radius, Enemies);

        foreach (Collider2D enemyGameObject in Enemy)
        {
            Debug.Log("Hit enemy");
            Player player = enemyGameObject.GetComponent<Player>();
            if (player != null)
            {
                if (player.Health > this.Attack)
                {
                    player.Health -= this.Attack;
                }
                else
                {
                    Destroy(player.gameObject);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.transform.position, Radius);
    }
}
