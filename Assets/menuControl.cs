using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class menuControl : MonoBehaviour
{
    public TextMeshProUGUI highscoreText;
    // Start is called before the first frame update
    void Start()
    {

        highscoreText.text = PlayerPrefs.GetInt("highscore").ToString();
        Debug.Log(PlayerPrefs.GetInt("highscore"));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        SceneManager.LoadScene("game");
    }
}
