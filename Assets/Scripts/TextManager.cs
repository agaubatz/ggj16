using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class TextManager : MonoBehaviour {
	public static TextManager instance;
	public Text scoreText;
	public bool gameOver = false;

	public bool GameOver {
		get { return gameOver; }
	}

	void Awake() {
		instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameOver) {
			scoreText.text = "Witchy Points: " + ScoreManager.instance.score;
		}

		if (gameOver && Input.GetMouseButtonDown (0)) {
			SceneManager.LoadScene ("MainScene");
		}
	}

	public void EndGame() {
		//TODO
		gameOver = true;
		scoreText.fontSize = 28;
		scoreText.text = "Final Score: " + ScoreManager.instance.score + "\n\nClick anywhere to restart";
		scoreText.alignment = TextAnchor.MiddleCenter;
	}
}
