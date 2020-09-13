using UnityEngine;

public class PowerUp_Bullets : PowerUp
{
    [SerializeField] private Color color;
    [SerializeField] private int amountOfBullets = 10;

    protected override void Start()
    {
        base.Start();

        mat.color = color;
    }

    void Update()
    {
    }

    protected override void FixedUpdate() 
    {
        base.FixedUpdate();
    }
    protected override void PickUp(){
        player.GetComponent<PlayerShoot>().AddBullets(amountOfBullets);

        Destroy(gameObject);
    }
}
