using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;

public class GameManager : MonoBehaviour
{
    private const int COIN_SCORE_AMOUNT = 5;
    public static GameManager Instance { set; get;}

    public bool IsDead { set; get; }

    private bool  isGameStarted = false;
    private PlayerMotor motor;

    //UI and UI fields
    public Animator gameCanvas,menuAnim,coinAnim;
    public Text scoreText, coinText, modifierText,hiscoreText;
    private float score, modifierScore;
    private int lastScore, coinScore;

    //death menu
    public Animator deathMenuAnim;
    public Text deadScoreText, deadCoinText;

    //google play services menu
    //public GameObject connectedMenu, diconnectedMenu;  //###bbbbbbb


    private void Awake()
    {
        Instance = this;
        modifierScore =  1;
        motor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
        
        modifierText.text = "x" + modifierScore.ToString("0.0");
        coinText.text = coinScore.ToString("0");
        scoreText.text = scoreText.text = score.ToString("0");

        hiscoreText.text = PlayerPrefs.GetInt("Hiscore").ToString();


        //Googleplay service
     //   PlayGamesPlatform.Activate();
        //OnConnectionResponse(PlayGamesPlatform.Instance.localUser.authenticated);

    }
    private void Update()
    {
        if(MobileInput.Instance.Tap && !isGameStarted)
        {
            isGameStarted = true;
            motor.StartRunning();
           // FindObjectOfType<MountainSpawner>().IsScrolling = true;
            FindObjectOfType<CameraMotor>().IsMoving = true;
            //FindObjectOfType<CristalSpawner>().IsScrolling = true;
            gameCanvas.SetTrigger("Show");
            menuAnim.SetTrigger("Hide");
           FindObjectOfType<TileManager>().IsScrolling = true; //$$$$$
          // FindObjectOfType<cristalspawner>().IsScrolling = true;
            
        }
        if(isGameStarted  && !IsDead )
        {
            //bump score Up
            lastScore = (int)score;
            score +=(Time.deltaTime * modifierScore);
            if(lastScore != (int)score)
            {
                scoreText.text = score.ToString("0");
            }
        }

    }

    public void GetCoin()
    {
        coinAnim.SetTrigger("Collect");
        coinScore++;

        // check if we unlocked an achivement #####bbbbbbbbbbbbbbbbbbb
        //switch(coinScore)
       // {
        //    case 50:
        //        UnlockAchivement(RPGPS.achivement_collect_50_coins);
        //        break;
        //    case 100:
        //        UnlockAchivement(RPGPS.achivement_collect_100_coins);
        //        break;
       //     case 150:
        //        UnlockAchivement(RPGPS.achivement_collect_150_coins);
       //         break;
        //    case 200:
        //        UnlockAchivement(RPGPS.achivement_collect_200_coins);
         //       break;
         //   default:
         //       break;
       // }

        coinText.text = coinScore.ToString("0");
        score += COIN_SCORE_AMOUNT;
        scoreText.text = scoreText.text = score.ToString("0");
    }

    public void UpdateModifier(float modifierAmount)
    {
        modifierScore = 1.0f + modifierAmount;
        modifierText.text = "x" + modifierScore.ToString("0.0");
    }

    public void OnPlayButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainLevel");
    }
    
    public void OnDeath()
    {
        IsDead = true;
    //FindObjectOfType<MountainSpawner>().IsScrolling = false; ///$$$$
        
        FindObjectOfType<TileManager>().IsScrolling = false; //stop scroll roadcristalspawner$$$$
       // FindObjectOfType<cristalspawner>().IsScrolling = false;

        deadScoreText.text = score.ToString("0");
        deadCoinText.text = coinScore.ToString("0");
        deathMenuAnim.SetTrigger("Dead");
        gameCanvas.SetTrigger("Hide");

      //  ReportScore((int)score);  //###bbbbbbbbbbbbbbb
        
        //check high score
        if(score > PlayerPrefs.GetInt("Hiscore"))
        {
            float s = score;
            if(s % 1 == 0)
                s += 1;
            PlayerPrefs.SetInt("Hiscore",(int)s);

        }
        

    }

    //Google play service
  //  public void OnConnectionClick()
  //  {
    //    Social.localUser.Authenticate((bool success) =>
//        {OnConnectionResponse(success);
    //    });
   // }

  //  private void OnConnectionResponse(bool authenticated)
  //  {

  //      if(authenticated)
   //     {
          //  UnlockAchivement(RPGPS.acivement_login);  //#####bbbbbbbbbbbbbbbbb
   //         connectedMenu.SetActive(true);
    //        diconnectedMenu.SetActive(false);
   //     }
     //   else
    //    {
    //        connectedMenu.SetActive(false);
   //         diconnectedMenu.SetActive(true);
   //     }
  //  }

    //Achivement
  //  public void OnAchivementClick()
  //  {
  //      if(Social.localUser.authenticated)
   //     {
  ///          Social.ShowAchivementsUI();
  //      }

 //   }

   // public void UnlockAchivement(string achivementID)
   // {
   //     Social.ReportProgress(achivementID,100.0f,(bool success) =>
    //    {
    //        Debug.Log("acivement unlocked "+ success.ToString());
     //   });
   // }

    //leadboard
    //public void OnLeadBoardClick()
   // {
   //     if(Social.localUser.authenticated)
    //    {
   //         Social.ShowLeaderboardUI();
    //    }

   // }

    //public void ReportScore(int score)
  //  {
    //    Social.ReportScore(score,RPGPS.leadboard_highscore,(bool success)=>
    //    {
   //         Debug.Log("Reported score to leaderboard "+ success.ToString());
   //     });
   // }


}
