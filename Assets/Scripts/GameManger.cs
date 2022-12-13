using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameManger : MonoBehaviour
{
    // Event action triggered when the game is started
    public static Action startGame;
    public static bool isgameRunning;
    [SerializeField] private int gameTimeInSeconds;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private GameObject instructionPanel;

    

    // Start is called before the first frame update
    void Start()
    {
        instructionPanel.SetActive(true);
        isgameRunning = false;
    }
    public void StartGame()
    {
        instructionPanel.SetActive(false);
        endPanel.gameObject.SetActive(false);
        isgameRunning = true;
        StartCoroutine(StartGameTime(gameTimeInSeconds));
        startGame?.Invoke();
    }
    IEnumerator StartGameTime(int gameTime)
    {
        int currentTime =0;
        
        while(currentTime<gameTime)
        {
            
            currentTime++;
            int timer = gameTime - currentTime;
            float minutes = Mathf.Floor(timer / 60);
            float seconds = Mathf.RoundToInt(timer%60);
            string minute = minutes.ToString();
            string second = seconds.ToString();

            if(minutes < 10) {
                minute = "0" + minutes.ToString();
            }
            if(seconds < 10) {
                second = "0" + Mathf.RoundToInt(seconds).ToString();
            }
            timerText.text = (minute+": "+second).ToString();
            yield return new WaitForSeconds(1);
        }
        OnLoseGame();
    }
    private void OnLoseGame()
    {
        endPanel.SetActive(true);
        isgameRunning = false;
    }
    
}
