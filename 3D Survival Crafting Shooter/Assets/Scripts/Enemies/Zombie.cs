using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class Zombie : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform playerTransform;
    [SerializeField] LayerMask playerLayer;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(playerTransform);
        bool inAttackRange = Physics.CheckSphere(transform.position, 2f, playerLayer);
        if (inAttackRange)
        {
            animator.SetTrigger("Attack");
        }
    }

    public void Attack()
    {
        if (Physics.CheckSphere(transform.position, 2f, playerLayer))
        {
            playerTransform.GetComponent<HealthSystem>().Damage(10);
            //Player ta damage
        }
    }
}
