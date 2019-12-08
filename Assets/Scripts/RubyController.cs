using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour {
    public int maxHealth = 5;
    public float speed = 3.0f;
    public float timeInvincible = 2.0f;
    int currentHealth;
    bool isInvincible;
    float invincibleTimer;
    public int health { get { return currentHealth; }}
    Rigidbody2D rb2d;
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);
    AudioSource audio;
    public AudioClip throwAudio;
    public AudioClip hitAudio;
    public GameObject projectilePrefab;
    // Start is called before the first frame update
    void Start() {
        audio = GetComponent<AudioSource>();
        rb2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
                
        Vector2 move = new Vector2(horizontal, vertical);
        
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        
        Vector2 position = rb2d.position;
        
        position = position + move * speed * Time.deltaTime;
        
        rb2d.MovePosition(position);


        if(isInvincible){
            invincibleTimer -= Time.deltaTime;
            if(invincibleTimer < 0){
                animator.SetTrigger("Hit");
                isInvincible = false;
            }
        }
        if(Input.GetButtonDown("Fire1")){
            Launch();
        }
        if(Input.GetKeyDown(KeyCode.Q)){
            RaycastHit2D hit = Physics2D.Raycast(rb2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if(hit.collider != null){
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null){
                    character.DisplayDialog();
                }
            }
        }

    }
    public void changeHealth(int amount){
        if (amount < 0){
            if (isInvincible){
                return;
            }
            isInvincible = true;
            invincibleTimer = timeInvincible;
        }
        audio.PlayOneShot(hitAudio);
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }
    public void Launch(){
        GameObject projectileObject = Instantiate(projectilePrefab, rb2d.position + Vector2.up * 0.05f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);
        animator.SetTrigger("Launch");
        audio.PlayOneShot(throwAudio);
    }
    public void PlaySound(AudioClip clip){
        audio.PlayOneShot(clip);
    }
}
