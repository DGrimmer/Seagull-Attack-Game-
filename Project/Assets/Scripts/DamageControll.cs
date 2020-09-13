using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class DamageControll : MonoBehaviour
{
    [SerializeField] private UI_UpdateText hpText = null;
    [SerializeField] private GameObject gameOverScreen = null;
    private bool hasEnded = false;

    private int hp{
        get{return RAW_hp;}
        set{
            RAW_hp = value;
            if(hpText != null)
                hpText.UpdateUITextWithValue(RAW_hp);
        }
    }
    private int RAW_hp;
    private int startHp = 50;
    private void Start() 
    {
        hp = startHp;
        if(hpText != null)
            hpText.UpdateUITextWithValue(hp);
    }

    public void TakeDamage(int amount)
    {
        if(hasEnded)
            return;

        hp -= amount;

        if(hp <= startHp/4)
            hpText.SetColor(Color.red);

        if(hp <=0){
            hasEnded = true;
            StartCoroutine( GameOver() );
        }
    }

    private IEnumerator GameOver()
    {
        print("GameOver");
        
        // enable the different parts of the gameover screen and update text
        gameOverScreen.GetComponent<Image>().enabled = true;
        gameOverScreen.GetComponentInChildren<Text>().enabled = true;
        gameOverScreen.GetComponentInChildren<UI_UpdateText>().UpdateUITextWithValue(GetComponent<ScoreControll>().score);


        // disable player movement
        FindObjectOfType<PlayerController>().enabled = false;

        // Reset timescale in case of slowmotion being applied (3 secounds in slow motion can take a long time)
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;

        yield return new WaitForSeconds(3);
        // Reset timescale in case of slowmotion being applied
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
