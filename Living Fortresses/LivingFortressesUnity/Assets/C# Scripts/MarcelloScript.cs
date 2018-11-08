using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarcelloScript : MonoBehaviour {

    public Vector3 velocity;
    public float moveSpeed, jetSpeed, boostSpeed, xDirection, rotationSpeed, boostPrepTimer, boostPrepTimeMax, boostTimer, boostTimeMax
    ,moveAngle, shootAngle, attackSpeed, leftDirection, rightDirection, gunTimer, gunTimerMax, swordDamage, gunDamage, health, healthMax
    ,healthPercentage, hitTimer, hitTimerMax;
    public bool touchingGround, jetting, boostPrep, boosting, nextAttack, continuingAttack, moveAttack, facingOpposite;
    public Rigidbody2D rb;
    public BoxCollider2D boxy;
    public Animator anim;
    public GameObject rig;
    public Vector2 boostDirection, newAttackLocation;
    //public GameObject sword, gunArm, gunArmIK, gun, bullet;
    public GameObject sword, gunArm, gun, bullet;
    public Quaternion baseGunRotation;
    public Image healthBar;
    public Transform[] rigPieces;
    public Shader spriteDefault, whiteSprite;
    public int attackNumber;
    // Use this for initialization
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rig = GameObject.Find("Player_Rig");
        anim = rig.GetComponent<Animator>();
        xDirection = transform.localScale.x;
        rightDirection = transform.localScale.x;
        leftDirection = transform.localScale.x * -1;
        boostPrepTimer = boostPrepTimeMax;
        boostTimer = boostTimeMax;
        moveAngle = 0;
        sword.GetComponent<BoxCollider2D>().enabled = false;
        nextAttack = true;
        shootAngle = gunArm.transform.rotation.z;
        baseGunRotation = gunArm.transform.rotation;
        gunArm.GetComponent<Animator>().enabled = false;
        health = healthMax;
        rigPieces = GetComponentsInChildren<Transform>();
        whiteSprite = Shader.Find("GUI/Text Shader");
        spriteDefault = Shader.Find("Sprites/Default");
    }

    // Update is called once per frame
    void Update()
    {
        if (boosting == false && boostPrep == false)
        {
            velocity.x = Input.GetAxisRaw("Left Horizontal");
            velocity.y = Input.GetAxisRaw("Left Vertical");
            rb.MovePosition(transform.position + velocity * moveSpeed * Time.deltaTime);
            if (velocity.x > 0 && xDirection == rightDirection || velocity.x < 0 && xDirection == leftDirection)
            {
                facingOpposite = true;
            }
            else
            {
                facingOpposite = false;
            }
        }
        boostDirection.x = Input.GetAxis("Left Horizontal");
        boostDirection.y = Input.GetAxis("Left Vertical");
        if (Input.GetButton("Boost") && boostTimer >= boostTimeMax)
        {
            boostPrep = true;
            velocity.x = 0;
            //touchingGround = false;
            boostPrepTimer -= Time.deltaTime;
        }
        else
        {
            boostPrep = false;
        }
        if (Input.GetButtonUp("Boost") && boostPrepTimer > 0)
        {
            boostPrepTimer = boostPrepTimeMax;
            boostPrep = false;
        }
        transform.localScale = new Vector2(-xDirection, transform.localScale.y);
        anim.SetFloat("Velocity X", Mathf.Abs(velocity.x));
        anim.SetFloat("Velocity Y", velocity.y);
        anim.SetBool("TouchingGround", touchingGround);
        anim.SetBool("BoostPrep", boostPrep);
        anim.SetBool("Boosting", boosting);
        anim.SetBool("Facing Opposite", facingOpposite);
        anim.SetInteger("Attack Number", attackNumber);
        if (boostPrepTimer <= 0)
        {
            boosting = true;
            boostPrep = false;
            boostPrepTimer = boostPrepTimeMax;
        }
        if (boosting == true)
        {
            rb.MovePosition(transform.position + transform.up * (boostSpeed) * Time.deltaTime);
            if (Mathf.Abs(Input.GetAxis("Left Horizontal")) > .2f || Mathf.Abs(Input.GetAxis("Left Vertical")) > .2f)
            {
                moveAngle = Mathf.Atan2(-boostDirection.x, boostDirection.y) * Mathf.Rad2Deg;
            }
            boostTimer -= Time.deltaTime;
            sword.GetComponent<BoxCollider2D>().enabled = true;
            //touchingGround = false;
            if (boostTimer <= 0)
            {
                boosting = false;
                boostTimer = boostTimeMax;
            }
        }
        else
        {
            moveAngle = 0;
            sword.GetComponent<BoxCollider2D>().enabled = false;
        }
        anim.SetBool("Continuing Attack", continuingAttack);
        if (Input.GetButtonDown("Attack"))
        {
            anim.SetTrigger("Attack");
            continuingAttack = true;
        }
        if (Input.GetButtonUp("Attack"))
        {
            anim.ResetTrigger("Attack");
        }
        if (moveAttack == true)
        {
            transform.position = Vector2.Lerp(transform.position, newAttackLocation, .1f);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(moveAngle, Vector3.forward), Time.deltaTime * rotationSpeed);
        if (Mathf.Abs(Input.GetAxis("Right Horizontal")) > .3f || Mathf.Abs(Input.GetAxis("Right Vertical")) > .3f)
        {
            gunArm.GetComponent<Animator>().ResetTrigger("Return");
            shootAngle = Mathf.Atan2(Input.GetAxis("Right Horizontal"), Input.GetAxis("Right Vertical")) * Mathf.Rad2Deg;
            gunArm.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(shootAngle, Vector3.forward), .2f * Mathf.Rad2Deg);
            if (Input.GetAxis("Right Horizontal") < -.1f)
            {
                xDirection = rightDirection;
            }
            else if (Input.GetAxis("Right Horizontal") > .1f)
            {
                xDirection = leftDirection;
            }
            gunArm.GetComponent<Animator>().enabled = true;
            if (gunTimer <= 0)
            {
                GameObject thisBullet = Instantiate(bullet, gun.transform.position, Quaternion.identity);
                //thisBullet.transform.rotation = Quaternion.LookRotation(gun.transform.right, thisBullet.transform.up);
                thisBullet.transform.up = Vector3.Normalize(new Vector3(gun.transform.right.x * xDirection, gun.transform.right.y * xDirection, 0));
                gunArm.GetComponent<Animator>().SetTrigger("Fire");
                gunTimer = gunTimerMax;
            }
            gunTimer -= Time.deltaTime;
        }
        else
        {
            gunTimer = gunTimerMax;
            gunArm.GetComponent<Animator>().ResetTrigger("Fire");
            gunArm.transform.rotation = Quaternion.Slerp(transform.rotation, baseGunRotation, .01f);
            gunArm.GetComponent<Animator>().SetTrigger("Return");
            gunArm.GetComponent<Animator>().enabled = false;
            if (Input.GetAxis("Left Horizontal") <= -.5f)
            {
                xDirection = rightDirection;
            }
            if (Input.GetAxis("Left Horizontal") >= .5f)
            {
                xDirection = leftDirection;
            }
        }
        healthPercentage = health / healthMax;
        healthBar.fillAmount = healthPercentage;
        if(hitTimer >= hitTimerMax - .1f){
            foreach(Transform piece in rigPieces){
                if(piece.GetComponent<SpriteRenderer>() != null){
                    piece.GetComponent<SpriteRenderer>().material.shader = whiteSprite;
                }
            }
        }else{
            foreach (Transform piece in rigPieces)
            {
                if (piece.GetComponent<SpriteRenderer>() != null)
                {
                    piece.GetComponent<SpriteRenderer>().material.shader = spriteDefault;
                }
            }
        }
        if (hitTimer > 0)
        {
            hitTimer -= Time.deltaTime;
        }
        if(attackNumber > 4){
            attackNumber = 0;
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            touchingGround = true;
        }
    }
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            touchingGround = true;
        }
        else
        {
            touchingGround = false;
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            touchingGround = false;
        }
    }
}
