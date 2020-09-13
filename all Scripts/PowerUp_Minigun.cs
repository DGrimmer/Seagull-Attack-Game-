using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Minigun : PowerUp
{
    [SerializeField] private Color color;
    [SerializeField] private float duration = 15;

    protected override void Start()
    {
        base.Start();

        mat.color = color;
    }

    // Update is called once per frame
    void Update()
    {
    }

    protected override void FixedUpdate() 
    {
        base.FixedUpdate();
    }
    protected override void PickUp(){
        player.GetComponent<PlayerShoot>().StartMinigunPowerup(duration);

        Destroy(gameObject);
    }
}
