using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver = false;

    public float floatForce = 50.0f;
    private float gravityModifier = 0.01f; //1.5f;
    private Rigidbody playerRb;
    private bool spacePressed = false;
    
    private float highLevel = 14.0f;
    
    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip groundSound;

    
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();

        // Apply a small upward force at the start of the game
        playerRb.AddForce(0.0f * Vector3.up, ForceMode.Impulse);
        playerRb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        // While space is pressed and player is low enough, float up
        if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) && spacePressed) && !gameOver)
        {
            if (Input.GetKey(KeyCode.Space))  spacePressed = true;
            playerRb.useGravity = true;
            playerRb.AddForce(floatForce * Vector3.up, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.DownArrow) && spacePressed && !gameOver)
        {
            playerRb.useGravity = true;
            playerRb.AddForce(floatForce * Vector3.down, ForceMode.Impulse);
        }
        if (highLevel < transform.position.y)
        {
            playerRb.useGravity = false;
            playerRb.velocity = Vector3.zero;
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
        // if player collides with ground
        else if (other.gameObject.CompareTag("Ground"))
        {
         if (spacePressed){
            playerAudio.PlayOneShot(groundSound, 1.0f);
            playerRb.velocity = Vector3.zero;
            playerRb.AddForce(3.0f * floatForce * Vector3.up, ForceMode.Impulse);
         }
        }
    }

}
