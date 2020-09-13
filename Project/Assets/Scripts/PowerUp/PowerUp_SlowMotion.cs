using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_SlowMotion : PowerUp
{
    [SerializeField] private Color color;
    [SerializeField] private float duration = 5;
    [SerializeField] private float slowMotionFactor = 0.05f;
    
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
        StartCoroutine(SlowMotion());
    }

    private IEnumerator SlowMotion(){
        GetComponent<Collider>().enabled = false;
        box.GetComponent<MeshRenderer>().enabled = false;
        Time.timeScale = slowMotionFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        
        PlayerController controller = player.GetComponent<PlayerController>();
        float playerOrginalSpeed = controller.Speed;
        // Set speed of player to compensate for slowmotion
        // controller.Speed = playerOrginalSpeed / slowMotionFactor;

        // change pitch of all sounds
        AudioSource[] sources = FindObjectsOfType<AudioSource>() as AudioSource[];
        foreach(var s in sources)
            s.pitch = 0.3f;

        yield return new WaitForSeconds(duration);

        // controller.Speed = playerOrginalSpeed;
        foreach(var s in sources){
            // Some birds could have been destroyed 
            if(s != null)
                s.pitch = 1;
        }

        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
        Destroy(gameObject);
    }
}
