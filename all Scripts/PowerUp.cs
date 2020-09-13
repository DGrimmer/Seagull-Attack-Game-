using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    protected GameObject player;
    [SerializeField] private float rotationSpeed = 2;
    [SerializeField] protected Transform box;
    protected Material mat;


    protected virtual void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        mat = box.GetComponent<Renderer>().material;
    }

    protected virtual void FixedUpdate() {
        box.Rotate(new Vector3(0,rotationSpeed,0));
    }

    protected abstract void PickUp();

    private void OnTriggerEnter(Collider other) {
        if(other.transform.tag == "Player")
            PickUp();
    }
}
