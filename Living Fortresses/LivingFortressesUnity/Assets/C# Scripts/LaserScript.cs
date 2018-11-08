using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour {

    public MarcelloScript marcello;
    public Vector3 turretRotation;
    public GameObject projectile, bulletSpawn1, bulletSpawn2;
    public GameObject sensor;
    public Vector2 sensorBaseSize, sensorDoubleSize;
    public bool active;
    public float turretAngle, hitTimer, hitTimerMax, laserTimer, laserTimerMax, health, healthMax, laserCharge, laserChargeMax;
    public Transform[] rigPieces;
    public Shader spriteDefault, whiteSprite;
    // Use this for initialization
    void Start()
    {
        marcello = GameObject.Find("Marcello Object").GetComponent<MarcelloScript>();
        sensorBaseSize = sensor.transform.localScale;
        sensorDoubleSize = sensorBaseSize * 2;
        health = healthMax;
        rigPieces = GetComponentsInChildren<Transform>();
        whiteSprite = Shader.Find("GUI/Text Shader");
        spriteDefault = Shader.Find("Sprites/Default");
    }

    // Update is called once per frame
    void Update()
    {
        if (active == true)
        {
            sensor.transform.localScale = sensorDoubleSize;
            turretRotation = -(marcello.gameObject.transform.position - transform.position);
            turretAngle = (Mathf.Atan2(turretRotation.x, -turretRotation.y) * Mathf.Rad2Deg);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(turretAngle, Vector3.forward), .02f);
            laserTimer -= Time.deltaTime;
            if (laserTimer > 0)
            {
                GameObject thisBullet = Instantiate(projectile, transform.position, Quaternion.identity);
                thisBullet.transform.right = Vector3.Normalize(new Vector3(-transform.right.x, -transform.right.y, 0));
            }
        }
        else
        {
            laserTimer = laserTimerMax;
            sensor.transform.localScale = sensorBaseSize;
        }
        if (laserTimer <= 0 && laserCharge <= 0)
        {
            laserCharge = laserChargeMax;
        }
        if (laserCharge > 0)
        {
            laserCharge -= Time.deltaTime;
        }
        if (laserCharge <= 0 && laserTimer <= 0)
        {
            laserTimer = laserTimerMax;
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
