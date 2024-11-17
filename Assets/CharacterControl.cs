using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//always adjust canvas scale first
public class CharacterControl : MonoBehaviour
{
    public GameControl gc;
    public bool isRunning;
    public bool isRunningBackwards;
    Rigidbody rb;       //public rigidbody
    public float jumpPower;
    Animator anim;

    public int life; 
    // pathIdx untuk menentukkan posisi index jalur
    int pathIdx;
    // jarak spacing antar jalur
    public float pathSpacing = 0.9f;
    // target tujuan untuk posisi karakter
    float targetX;
    // kecepatan pindah jalur
    public float turnSpeed = 10;

    //audio
    public AudioSource turnSFX;
    public AudioSource jumpSFX;
    public AudioSource impactSFX;
    public AudioSource coinSFX;
    public AudioSource stepSFX;


    void Start()
    {
        //isRunning = true;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    public void StartRunning()
    {
        isRunning = true;
        anim.SetTrigger("lari");
    }

    // Update is called once per frame
    void Update()
    {
        
        if (transform.position.x < targetX)
        {
            // Karakter gerak ke kanan
            transform.position += Vector3.right * turnSpeed * Time.deltaTime;
            if (transform.position.x > targetX)
            {
                transform.position = new Vector3(targetX, transform.position.y, 0);
            }
        }
        else if (transform.position.x > targetX)
        {
            // karakter gerak ke kiri
            transform.position += Vector3.left * turnSpeed * Time.deltaTime;
            if (transform.position.x < targetX)
            {
                transform.position = new Vector3(targetX, transform.position.y, 0);
            }
        }
    }

    public void jump()
    {
        if (isRunning)
        {
            if (transform.position.y <= 0.5)
            {
                rb.AddForce(Vector3.up * jumpPower);
                anim.SetTrigger("lompat");
                jumpSFX.Play();
            }
        }
        // transform.position.y


    }

    public void turnLeft()
    {
        if (isRunning)
        {
            pathIdx = pathIdx - 1;
            if (pathIdx < -1)
            {
                pathIdx = -1;
            }
            targetX = pathIdx * pathSpacing;
            turnSFX.Play();
        }
    }

    public void turnRight()
    {
        if (isRunning)
        {
            pathIdx = pathIdx + 1;
            if (pathIdx > 1)
            {
                pathIdx = 1;
            }
            targetX = pathIdx * pathSpacing;
            turnSFX.Play();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "obstacle" )
        {
            if(transform.position.y < 0.5)
            {
                impactSFX.Play();
                Debug.Log("kena mobil");
                anim.SetTrigger("jatuh");
                isRunning = false;
                life--;
                gc.lifeUI.GetChild(life).gameObject.SetActive(false);   
                if(life <= 0)
                {
                    anim.SetBool("gameover", true);
                    gc.GameOver();
                }

            }
            else
            {
                anim.SetTrigger("lari");
                impactSFX.Play();
            }

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "coin")
        {
            coinSFX.Play();
            other.gameObject.SetActive(false);
            gc.AddScore(); // tambah score saat menyentuh koin
        }
    }

    void StartRunningBackwards()
    {
        isRunningBackwards = true;
        //Invoke("StopRunningBackwards", 2); // sama dengan coroutine
        // function IEnumerator harus dijalankan dengan startCoroutine
        StartCoroutine(StopRunningBackwardsCoroutine(2));
    }

    void StopRunningBackwards()
    {
        isRunningBackwards = false;
        anim.SetTrigger("idle");
        gc.StartCountdown();    
    }

    IEnumerator StopRunningBackwardsCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        StopRunningBackwards();
    }

    void PlaystepSFX()
    {
        stepSFX.Play();
    }
}
