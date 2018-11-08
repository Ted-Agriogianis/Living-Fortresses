using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryScript : MonoBehaviour {
    public bool active, searching;
    public MarcelloScript marcello;
    public GameObject projectile, turret, booster, triggerZone;
    public float speed, fireRate, fireRateMax, turretAngle, boosterAngle, xDirection, leftDirection, rightDirection, health, healthMax
    , hitTimer, hitTimerMax, activeTimer, activeTimerMax;
    public Vector3 startDirection, moveDirection, turretRotation, boosterRotation;
    public Rigidbody2D rb;
    public Vector2 triggerBaseSize, triggerDoubleSize;
    public Transform[] rigPieces;
    public Shader spriteDefault, whiteSprite;
    public Animator anim;
    // Use this for initialization
    void Start () {
        marcello = GameObject.Find("Marcello Object").GetComponent<MarcelloScript>();
        rb = GetComponent<Rigidbody2D>();
        searching = true;
        triggerBaseSize = triggerZone.transform.localScale;
        triggerDoubleSize = (triggerZone.transform.localScale) * 2;
        xDirection = transform.localScale.x;
        rightDirection = transform.localScale.x * -1;
        leftDirection = transform.localScale.x;
        rigPieces = GetComponentsInChildren<Transform>();
        whiteSprite = Shader.Find("GUI/Text Shader");
        spriteDefault = Shader.Find("Sprites/Default");
        health = healthMax;
        activeTimer = activeTimerMax;
    }
	
	// Update is called once per frame
	void Update () {
        if (activeTimer > 0)
        {
            activeTimer -= Time.deltaTime;
        }
        if(activeTimer <= 0){
            active = true;
        }
        Debug.Log(rb.velocity.x);
        if (moveDirection.x <= -.3f){
            xDirection = leftDirection;
        }else if(moveDirection.x >= .3f){
            xDirection = rightDirection;
        }
        transform.localScale = new Vector2(xDirection, transform.localScale.y);
        if (active == false){
            rb.MovePosition(transform.position + startDirection * speed * 2 * Time.deltaTime);
        }else if(active == true){
        }
        if(searching == true && active == true){
            fireRate = fireRateMax + .5f;
            triggerZone.transform.localScale = triggerBaseSize;
            moveDirection = (marcello.gameObject.transform.position - transform.position);
            rb.MovePosition(transform.position + moveDirection * speed * Time.deltaTime);
            anim.SetBool("Moving", true);
        }
        else if(searching == false && active == true){
            turretRotation = -(marcello.gameObject.transform.position - transform.position);
            turretAngle = (Mathf.Atan2(turretRotation.x * xDirection, -turretRotation.y * xDirection) * Mathf.Rad2Deg) - 90;
            turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, Quaternion.AngleAxis(turretAngle, Vector3.forward), .05f);
            triggerZone.transform.localScale = triggerDoubleSize;
            fireRate -= Time.deltaTime;
            if (fireRate <= 0)
            {
                GameObject thisBullet = Instantiate(projectile, turret.transform.position, Quaternion.identity);
                thisBullet.transform.up = Vector3.Normalize(new Vector3(turret.transform.right.x * xDirection, turret.transform.right.y * xDirection, 0));
                fireRate = fireRateMax;
            }
            anim.SetBool("Moving", false);
        }
        if(hitTimer >= hitTimerMax - .1f){
            foreach (Transform piece in rigPieces)
            {
                if (piece.GetComponent<SpriteRenderer>() != null)
                {
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
        if(hitTimer > 0){
            hitTimer -= Time.deltaTime;
        }
        if(health <= 0){
            Destroy(gameObject);
        }
	}
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Marcello"){
            searching = false;
        }
        if (other.gameObject.tag == "Player Sword" && hitTimer <= 0)
        {
            health -= marcello.swordDamage;
            hitTimer = hitTimerMax;
        }else if(other.gameObject.tag == "Player Projectile" && hitTimer <= 0)
        {
            health -= marcello.gunDamage;
            hitTimer = hitTimerMax;
        }
    }
    void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.tag == "Marcello"){
            searching = true;
        }
    }
}
