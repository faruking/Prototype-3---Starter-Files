using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public float floatForce;
    private float gravityModifier = 1.5f;
    private Rigidbody playerRb;
    public bool isOnGround = false;
    private bool isLowEnough = true;
    private float upperBound = 16f;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;


    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;


    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        playerRb = GetComponent<Rigidbody>();
        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y > upperBound){
            transform.position = new Vector3(transform.position.x,upperBound - 1, transform.position.z);
            isLowEnough = false;
        }
        else if(transform.position.y < upperBound){
            isLowEnough = true;
        }
        // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && isLowEnough)
        {
            playerRb.AddForce(Vector3.up * floatForce, ForceMode.Impulse);
            isOnGround = false;
        }
   
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

        }
        else if(other.gameObject.CompareTag("Ground") && !gameOver){
            isOnGround = true;
            playerRb.AddForce(Vector3.up * 25, ForceMode.Impulse);
        }

    }

}
