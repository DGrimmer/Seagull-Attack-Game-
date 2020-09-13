using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seagull : MonoBehaviour, iDamageable
{
    public static int static_numberOfSeagulls = 0;
    [SerializeField] private int hpMax=1;
    private int hp;
    [SerializeField] float speed = 1;
    [SerializeField] float attackSpeed = 2;
    private float attackTimer = 0;
    [SerializeField] private int attackDmg = 1;
    [SerializeField] Transform target;

    [SerializeField] private Rigidbody rb;
    private AudioSource audioSource;
    [SerializeField] private AudioClip die;
    [SerializeField] private AudioClip call;
    [SerializeField] private ParticleSystem onDeath;
    [SerializeField] private float callFrequency = 5;
    [SerializeField] private float callFrequencyVariation = 2;

    private float callTimer = 0;
    private bool isDead = false;

    void Awake()
    {
        hp = hpMax;
        static_numberOfSeagulls += 1;
        // Set a small random delay so multiple spawned birds are not in sync
        callTimer = Random.Range(0, 1.5f);
        //rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead)
            return;

        callTimer -= Time.deltaTime;
        attackTimer -= Time.deltaTime;

        

        if(callTimer < 0){
            audioSource.PlayOneShot(call);
            callTimer = callFrequency + Random.Range(0, 2);
        }
    }
    private void FixedUpdate() 
    {
        if(isDead)
            return;

        transform.LookAt(target);
        rb.AddForce(transform.forward * speed);
    }
    
    public void TakeDamage(int damage, Vector3 fromDirection)
    {
        hp -= damage;
        if(hp <= 0 && !isDead)
        {
            isDead = true;
            // layer 2 is default for ignore raycast
            gameObject.layer = 2;
            StartCoroutine( Die(fromDirection.normalized) );
        }
    }

    public void SetTarget(Transform newtarget){
        target = newtarget;
    }

    private IEnumerator Die(Vector3 fromDirection)
    {
        static_numberOfSeagulls -= 1;
        audioSource.PlayOneShot(die, 1);
        Instantiate(onDeath,transform.position,Quaternion.identity).Play();
        

        // Remove main collider and rigidbody
        Destroy(GetComponent<BoxCollider>());
        Destroy(rb);
        GetComponentInChildren<Animator>().enabled = false;

        // Activate ragdoll
        Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody body in rbs)
        {
            body.isKinematic = false;
            body.useGravity = true;
        }
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider c in colliders)
            c.enabled = true;

        rbs[1].AddForce((fromDirection + new Vector3(0,1,0)).normalized * 1000 );
        
        yield return new WaitForSeconds(5);
        // Sink through ground before disappearing
        foreach(Collider c in colliders)
            if(c != null)
                c.enabled = false;

        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
    
    private void OnTriggerStay(Collider other) 
    {
        if(other.transform.tag == "Burger" && attackTimer < 0)
        {
            attackTimer = attackSpeed;
            other.GetComponent<DamageControll>().TakeDamage(attackDmg);
        }
    }
    
}
