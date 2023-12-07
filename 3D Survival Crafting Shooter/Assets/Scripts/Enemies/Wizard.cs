using UnityEngine;

public class Wizard : MonoBehaviour
{
    [SerializeField] Transform player;  
    [SerializeField] float moveSpeed = 2.5f;  
    [SerializeField] float attackDistance = 3.0f;
    private Animator anim;
    [SerializeField] GameObject SnowAOE;
    [SerializeField] GameObject snowBall;
    [SerializeField] Transform shootPosition;
    private float timeBetweenAttack = 10f;
    private bool alreadyAttacked;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        transform.LookAt(player);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > attackDistance)
        {
            anim.SetBool("Walking", true);
            Vector3 direction = (player.position - transform.position).normalized;
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
        else
        {
            anim.SetBool("Walking", false);
            if (!alreadyAttacked)
            {
                anim.SetTrigger("Attack");
                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttack);
            }
            else
            {
                anim.SetTrigger("Attack2");
            }
        }
    }

    public void Attack()
    {
        GameObject snowAoe = Instantiate(SnowAOE, (player.transform.position), Quaternion.identity);
        Destroy(snowAoe, 10f);
        
    }

    public void Attack2()
    {
        GameObject bulletObj = Instantiate(snowBall, shootPosition.position, shootPosition.transform.rotation);
        Rigidbody bulletRig = bulletObj.GetComponent<Rigidbody>();
        bulletRig.AddForce(bulletRig.transform.forward * 32f, ForceMode.Impulse);
        Destroy(bulletObj, 5f);
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
