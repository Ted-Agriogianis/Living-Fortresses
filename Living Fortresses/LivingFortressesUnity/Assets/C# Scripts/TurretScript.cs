using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour {
    public MarcelloScript marcello;
    public Vector3 turretRotation;
    public GameObject projectile, bulletSpawn1, bulletSpawn2;
    public GameObject sensor;
    public Vector2 sensorBaseSize, sensorDoubleSize;
    public bool active;
    public float turretAngle, hitTimer, hitTimerMax, fireRate, fireRateMax, health, healthMax, bulletCharge, bulletChargeMax;
    public Transform[] rigPieces;
    public Shader spriteDefault, whiteSprite;
    public int ammo, ammoMax;
    // Use this for initialization
    void Start () {
        marcello = GameObject.Find("Marcello Object").GetComponent<MarcelloScript>();
        sensorBaseSize = sensor.transform.localScale;
        sensorDoubleSize = sensorBaseSize * 2;
        health = healthMax;
        rigPieces = GetComponentsInChildren<Transform>();
        whiteSprite = Shader.Find("GUI/Text Shader");
        spriteDefault = Shader.Find("Sprites/Default");
        ammo = ammoMax;
    }
	
	// Update is called once per frame
	void Update () {
        if(active == true){
            sensor.transform.localScale = sensorDoubleSize;
            turretRotation = -(marcello.gameObject.transform.position - transform.position);
            turretAngle = (Mathf.Atan2(turretRotation.x, -turretRotation.y) * Mathf.Rad2Deg);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(turretAngle, Vector3.forward), .05f);
            fireRate -= Time.deltaTime;
            if (fireRate <= 0 && bulletCharge <= 0)
            {
                GameObject thisBullet = Instantiate(projectile, bulletSpawn1.transform.position, Quaternion.identity);
                thisBullet.transform.right = Vector3.Normalize(new Vector3(-transform.right.x, -transform.right.y, 0));
                GameObject thatBullet = Instantiate(projectile, bulletSpawn2.transform.position, Quaternion.identity);
                thatBullet.transform.right = Vector3.Normalize(new Vector3(-transform.right.x, -transform.right.y, 0));
                ammo -= 1;
                fireRate = fireRateMax;
            }
        }
        else{
            fireRate = fireRateMax + .5f;
            sensor.transform.localScale = sensorBaseSize;
        }
        if(ammo <= 0 && bulletCharge <= 0){
            bulletCharge = bulletChargeMax;
        }
        if(bulletCharge > 0){
            bulletCharge -= Time.deltaTime;
        }
        if(bulletCharge <= 0 && ammo <= 0){
            ammo = ammoMax;
        }
        if (hitTimer >= hitTimerMax - .1f)
        {
            foreach (Transform piece in rigPieces)
            {
                if (piece.GetComponent<SpriteRenderer>() != null)
                {
                    piece.GetComponent<SpriteRenderer>().material.shader = whiteSprite;
                }
            }
        }
        else
        {
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
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Marcello")
        {
            active = true;
        }
        if (other.gameObject.tag == "Player Sword" && hitTimer <= 0)
        {
            health -= marcello.swordDamage;
            hitTimer = hitTimerMax;
        }
        else if (other.gameObject.tag == "Player Projectile" && hitTimer <= 0)
        {
            health -= marcello.gunDamage;
            hitTimer = hitTimerMax;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Marcello")
        {
            active = false;
        }
    }
}
