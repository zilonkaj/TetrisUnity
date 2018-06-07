using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Canvas startscreen;
    public Button startbutton;
    public GameObject panel;
    public Text ScoreEquals;
    public Text score;
    public Text title;
    public GridManager gridManager;


	public void StartGame()
    {
        gridManager.StartGame();
        gridManager.score = 0;
        startbutton.gameObject.SetActive(false);
        panel.SetActive(false);
        title.gameObject.SetActive(false);
        ScoreEquals.gameObject.SetActive(true);
        score.gameObject.SetActive(true);
    }

    public void EndGame()
    {
        startbutton.gameObject.SetActive(true);
        panel.SetActive(true);
        title.gameObject.SetActive(true);
    }


    // Use this for initialization
	void Start () {
        score.gameObject.SetActive(false);
        ScoreEquals.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        score.text = gridManager.score.ToString();

        if (gridManager.gameover)
        {
            EndGame();
            gridManager.gameover = false;
        }
	}
}
