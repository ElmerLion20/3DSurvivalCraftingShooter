using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giant : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform playerTransform;
    [SerializeField] LayerMask playerLayer;
    private Animator animator;
    [SerializeField] GameObject schockwave;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(playerTransform);
        bool inAttackRange = Physics.CheckSphere(transform.position, 5f, playerLayer);
        if (inAttackRange)
        {
            animator.SetTrigger("Stomp");

        }
        
    }

    public void Stomp()
    {
        Instantiate(schockwave, gameObject.transform);
        if (Physics.CheckSphere(transform.position, 5f, playerLayer))
            {
            playerTransform.GetComponent<HealthSystem>().Damage(10);
                //TA hp av spelare
            }
        
    }
}
