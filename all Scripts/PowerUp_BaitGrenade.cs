using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_BaitGrenade : PowerUp
{
    [SerializeField] private Color color;
    [SerializeField] private float duration = 5;
    [SerializeField] private float slowMotionFactor = 0.05f;
    
 protected override void Start()
    {
        base.Start();

        mat.color = color;
    }

    protected override void FixedUpdate() 
    {
        base.FixedUpdate();
    }
    protected override void PickUp(){
       player.GetComponent<PlayerShoot>().AddBaitGrenades(1);
       Destroy(gameObject);
    }
}
