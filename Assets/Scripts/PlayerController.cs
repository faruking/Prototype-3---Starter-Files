using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb; 
    private Animator playerAnim;
    private AudioSource playerAudio;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public float jumpforce = 10;
    public float gravityModifier;
    public bool isOnGround = true;
    public bool gameOver = false;

    public AudioClip jumpSound;
    public AudioClip crashSound;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        Physics.gravity *= gravityModifier;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver)
        {
            playerRb.AddForce(Vector3.up * jumpforce,ForceMode.Impulse);  
            isOnGround = false;
            playerAudio.PlayOneShot(jumpSound,1.0f);
            playerAnim.SetTrigger("Jump_trig");
            dirtParticle.Stop();
        }
    }
    private void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("Ground") && !gameOver){
            isOnGround = true;
            dirtParticle.Play();
        }
        else if(collision.gameObject.CompareTag("Obstacles")){
            dirtParticle.Stop();
            explosionParticle.Play();
            playerAnim.SetBool("Death_b",true);
            playerAnim.SetInteger("DeathType_int",1);
            playerAudio.PlayOneShot(crashSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over");
        }
    } 
}
