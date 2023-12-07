using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 3f;
    public float attackRange = 10f;
    public float attackDamage = 20f;
    public float attackCooldown = 2f;
    private float nextAttackTime = 0f;

    private Transform player;

    private HealthSystem healthSystem;
    private Animator dragonAnimator;
    private bool isAttacking = false;
    private bool angry = false;
    public bool roaring = true;

    private bool canMove = false;
    private bool onGround = true;

    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private GameObject attackPoint;

    [SerializeField] GameObject fireParticle;
    public float downwardAngleOffset = 30f;
    [SerializeField] GameObject fireDetect;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Assuming the player has a tag "Player"
        dragonAnimator = GetComponent<Animator>();
        healthSystem = GetComponent<HealthSystem>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            healthSystem.Damage(55);
        }

        if (!healthSystem.IsFullHealth())
        {
            dragonAnimator.SetBool("Angry", true);
            angry = true;
        }

        if(healthSystem.GetCurrentHealthAmount() < 350)
        {
            dragonAnimator.SetBool("Fly", true);
            onGround = false;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, transform.rotation * Quaternion.Euler(downwardAngleOffset, 0f, 0f), rotationSpeed * Time.deltaTime);
        }

        if (!isAttacking && angry && !roaring && canMove)
        {

            // Rotate towards the player
            Vector3 direction = player.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

            // Move towards the player if within attack range
            if (Vector3.Distance(transform.position, player.position) <= attackRange)
            {
                dragonAnimator.SetBool("Inrange", true);
                // Attack logic
                if (Time.time >= nextAttackTime)
                {
                    
                    if (onGround)
                    {
                        Attack();
                    }
                    else
                    {
                        FlyAttack();
                        fireDetect.SetActive(false);
                    }
                    nextAttackTime = Time.time + attackCooldown;
                }
            }
            else
            {
                // Move towards the player
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                dragonAnimator.SetBool("Inrange", false);
                // You can add flying animations or movement here
            }
        }
    }

    void Attack()
    {
        isAttacking = true;
        int attack = Random.Range(0, 4);
        switch (attack)
        {
            case 1:
                dragonAnimator.SetTrigger("Attack1");
                break;
            case 2:
                dragonAnimator.SetTrigger("Attack2");
                break;
            case 3:
                dragonAnimator.SetTrigger("Attack3");
                break;

        }

        // Deal damage to the player
        // Implement your own logic to damage the player here

        // Set canMove to false during attack animation
        canMove = false;

        Invoke("ResetAttack", 1.5f); // Adjust this delay to match the attack animation length
    }

    void FlyAttack()
    {
        isAttacking = true;
        // Deal damage to the player
        // Implement your own logic to damage the player here
        dragonAnimator.SetTrigger("Attack4");
        // Set canMove to false during attack animation
        canMove = false;

        Invoke("ResetAttack", 5f); // Adjust this delay to match the attack animation length
    }

    void ResetAttack()
    {
        isAttacking = false;
        // Set canMove back to true after attack animation ends
        canMove = true;
        
    }

    public void RoaringFalse()
    {
        roaring = false;
        canMove = true; // Allow movement after roar animation
    }

    public void DamagePlayer()
    {
        if (Physics.CheckSphere(attackPoint.transform.position, 5f, playerLayer))
        {
            player.GetComponent<HealthSystem>().Damage(50);
        }
    }

    public void FireAttack()
    {
        GameObject fireattack = Instantiate(fireParticle, attackPoint.transform.position, transform.rotation);
        fireattack.transform.localScale = new Vector3(2f, 2f, 2f);
        fireattack.transform.SetParent(attackPoint.transform);
        fireDetect.SetActive(true);
        Invoke("ResetFireDetect", 2f);
    }

    void ResetFireDetect()
    {
        fireDetect.SetActive(false);
    }
}
