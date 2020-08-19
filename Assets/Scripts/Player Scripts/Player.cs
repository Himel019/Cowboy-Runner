using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private AudioClip jumpClip;
    [SerializeField]
    private AudioClip deathClip;

    [SerializeField]
    private float jumpForce = 12f;
    [SerializeField]
    private float forwardForce = 1f;
    [SerializeField]
    private float backwardForce = 2f;
    [SerializeField]
    private float xPosition = -5f;

    private bool canJump;

    private Rigidbody2D myRigidbody;
    private Animator myAnimator;
    private AudioSource audioSource;
    private Button jumpButton;


    public delegate void EndGame();
    public static event EndGame endGame;


    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D> ();
        myAnimator = GetComponent<Animator> ();
        audioSource = GetComponent<AudioSource> ();
        jumpButton = GameObject.Find("Jump Button").GetComponent<Button> ();

        jumpButton.onClick.AddListener(() => Jump());

        StartCoroutine(AnimationSpeedChange());
    }

    // Update is called once per frame
    void Update()
    {
        if( Mathf.Abs(myRigidbody.velocity.y) == 0 ) {

            if(!canJump) {
                canJump = true;
                myAnimator.SetBool("Jump", false);
            } else {
                if(transform.position.x > xPosition){
                   Vector3 temp = new Vector3(backwardForce, 0, 0);
                   transform.position -= temp * Time.deltaTime;
                }
            }
        }
    }

    public void Jump() {
        if(canJump) {
            canJump = false;

            audioSource.PlayOneShot(jumpClip, 0.6f);

            float force = forwardForce;
            if(transform.position.x >= 5f) {
                force = 0f;
            }

            myAnimator.SetBool("Jump", true);
            myRigidbody.velocity = new Vector2(force, jumpForce);
        }
    }

    private void PlayerDiedEndGame() {
        if(endGame != null){
            endGame();
        }

        if(audioSource.isPlaying){
            audioSource.Stop();
        }
        AudioSource.PlayClipAtPoint(deathClip, transform.position);
        
        Destroy(gameObject);
    }

    private IEnumerator AnimationSpeedChange() {
        yield return new WaitForSecondsRealtime(120f);

        myAnimator.speed = 1.5f;

        yield return new WaitForSecondsRealtime(120f);

        myAnimator.speed = 1.8f;
    }

    /// Sent when an incoming collider makes contact with this object's collider (2D physics only).
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Obstacle") {
            PlayerDiedEndGame();
        }
    }

    /// Sent when another object enters a trigger collider attached to this object (2D physics only).
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Collector") {
            PlayerDiedEndGame();
        }
    }
}
