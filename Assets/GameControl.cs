using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

// Harus drag dari game object ke object lain, tidak bisa langsung drag script
public class GameControl : MonoBehaviour
{
    public CharacterControl myChar;
    public int score;
    public TextMeshProUGUI scoreTxt;
    public int countdownStart;
    int currCountdown;
    public TextMeshProUGUI countdownTxt;
    public GameObject gameoverMenu;
    public GameObject pauseMenu;
    public Transform lifeUI;
    public AudioSource gameBGM;
    public AudioClip gameoverClip;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i < myChar.life; i++)
        {
            Transform heart = Instantiate(lifeUI.GetChild(0), lifeUI);
            heart.localPosition = new Vector2(i * 100, 0);
        }
        StartCountdown();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Tap()
    {
        if (!isDragging)
        {
            Debug.Log("tap");
            myChar.jump();
        }

    
    }
    Vector2 startDragPosition;
    bool isDragging = false;
    public void BeginDrag(BaseEventData bed)
    {

        PointerEventData ped = bed as PointerEventData; //type casting
        startDragPosition = ped.position;
        isDragging = true;

    }

    public void EndDrag(BaseEventData bed)
    {
        PointerEventData ped = bed as PointerEventData;
        if (ped.position.x < startDragPosition.x)
        {
            Debug.Log("drag kiri");
            myChar.turnLeft();
        }
        else if ( ped.position.x > startDragPosition.x)
        {
            Debug.Log("drag kanan");
            myChar.turnRight();
        }
        isDragging = false;
    }

    public void AddScore()
    {
        score++;
        scoreTxt.text = score.ToString();
    }

    public void StartCountdown()
    {
        currCountdown = countdownStart;
        countdownTxt.text = currCountdown.ToString();
        scoreTxt.text = score.ToString();
        countdownTxt.gameObject.SetActive(true);
        StartCoroutine(DoCountdown());
    }

    IEnumerator DoCountdown()
    {
        yield return new WaitForSeconds(1);
        if (currCountdown > 1)
        {
            currCountdown--;
            countdownTxt.text = currCountdown.ToString();
            StartCoroutine(DoCountdown()); 
        }
        else
        {
            countdownTxt.gameObject.SetActive(false);
            myChar.StartRunning(); 
        }
    }

    public void GameOver()
    {
        if(score > PlayerPrefs.GetInt("highscore"))
        {
            PlayerPrefs.SetInt("highscore", score);
        }

        Invoke("ShowGameOver", 1);
    }
    void ShowGameOver()
    {
        GetComponent<AdsControl>().ShowAd();

        gameoverMenu.SetActive(true);
        gameBGM.clip = gameoverClip;
        gameBGM.loop = false;
        gameBGM.Play();
    }
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void ContinueGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void RetryGame()
    {
        SceneManager.LoadScene("game");
        Time.timeScale = 1;
    }
    public void ExitGame()
    {
        SceneManager.LoadScene("menu");
        Time.timeScale = 1;
    }
}
