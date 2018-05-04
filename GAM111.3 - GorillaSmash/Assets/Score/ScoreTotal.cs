using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreTotal : MonoBehaviour {
    PlayerMovement playerScore;
    int bananaScore;
    int superBananaScore;

    public Text menuBScore;
    public Text menuSBScore;
    public Text totalScore;
    int bananas;
    int superBananas;
    int totalScoreInt;

    private void Start() {
        playerScore = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    private void Update() {
        bananaScore = playerScore.bananasCollected;
        superBananaScore = playerScore.superBananasCollected;
        menuBScore.text = bananaScore.ToString();
        menuSBScore.text = superBananaScore.ToString();
        bananas = bananaScore * 10;
        superBananas = superBananaScore * 100;
        totalScoreInt = bananas + superBananas;
        totalScore.text = totalScoreInt.ToString();
    }
}
