using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreControll : MonoBehaviour
{
    [SerializeField] private UI_UpdateText scoreText = null;
    
    private float scoreTimer = 0;
    private float scoreEveryAfterTime = 2;

    public int score{
        get{return RAW_score;}
        set{
            RAW_score = value;
            if(scoreText != null)
                scoreText.UpdateUITextWithValue(RAW_score);
        }
    }
   [SerializeField] private int RAW_score = 0;
    
    void Start()
    {
        if(scoreText != null)
            scoreText.UpdateUITextWithValue(RAW_score);
    }

    private void Update() {
        scoreTimer -= Time.deltaTime;
        if(scoreTimer < 0){
            scoreTimer = scoreEveryAfterTime;
            score++;
        }
    }

}
