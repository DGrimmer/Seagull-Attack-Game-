using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baitGrenade : MonoBehaviour
{
    List<Seagull> affectedEnemies = new List<Seagull>();
    [SerializeField] private float duration = 5;
    void Start()
    {
        StartCoroutine( Attract() );
    }

    IEnumerator Attract(){

        yield return new WaitForSeconds(duration);
        // Set the target back for all birds affected
        Transform burger = GameObject.FindGameObjectWithTag("Burger").transform;
        foreach (var s in affectedEnemies)
            s.SetTarget(burger);
        Destroy(gameObject);
    } 
    private void OnTriggerEnter(Collider other) {
        if(other.transform.tag == "Enemy"){
            Seagull s = other.GetComponent<Seagull>();
            s.SetTarget(transform);
            affectedEnemies.Add(s);
        }
    }
}
