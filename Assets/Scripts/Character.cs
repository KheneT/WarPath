using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{


    // Status 
    public double Health { get; set; }
    public float Radius { get; set; }

    // Actions
    public double Attack { get; set; }
    public float MovementX { get; set; }
    public float JumpForce { get; set; }
    public float MoveForce { get; set; }

    // Animations references
    public string RUN { get; set; } = "Running";

    // Components
    public Animator Anim { get; set; }
    public Rigidbody2D MyBody { get; set; }
    public GameObject AttackPoint { get; set; }
    public LayerMask Enemies { get; set; }
    public SpriteRenderer Sr { get; set; }

    // Tags
    public string GROUND_TAG { get; set; } = "Ground";

    // Check
    public bool IsGrounded { get; set; }



    public void characterProperties(double Health, double Attack, float JumpForce, float MoveForce)
    {
        this.Health = Health;
        this.Attack = Attack;
        this.JumpForce = JumpForce;
        this.MoveForce = MoveForce;
    }

    


    //Player movement x axis
    void PlayerMoveKeyboard(string axis)
    {
        this.MovementX = Input.GetAxisRaw(axis);
        transform.position += new Vector3(this.MovementX, 0f, 0f) * this.MoveForce * Time.deltaTime;
    }


    //Requirements: RUN variable being initializate
    void AnimatePlayer()
    {

        if (this.Anim != null)
        {

            if (this.MovementX > 0)
            {
                this.Anim.SetBool(RUN, true);
                this.Sr.flipX = false;
            }
            else if (this.MovementX < 0)
            {
                this.Anim.SetBool(RUN, true);
                this.Sr.flipX = true;
            }
            else
            {
                this.Anim.SetBool(RUN, false);
            }

        }
    }

    void PlayerJump(string buttonJump)
    {
        if (this.Anim != null)
        {
            if (Input.GetButtonDown(buttonJump) && IsGrounded)
            {
                IsGrounded = false;
                MyBody.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
            }
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GROUND_TAG))
        {
            IsGrounded = true;
        }
    }

    void PlayerAttackAnimation(string triggerName)
    {
        if (this.Anim != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Anim.SetTrigger(triggerName);
            }
        }
    }




    protected virtual void Awake()
    {
        MyBody = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        Sr = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    void Start()
    {

    }

    void Update()
    {

    }


}
