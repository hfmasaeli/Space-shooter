using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _scoreText;
    [SerializeField]
    private TextMeshProUGUI _gameOverText;
    [SerializeField]
    private TextMeshProUGUI _restartText;
    [SerializeField]
    private TextMeshProUGUI _quitText;

    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _liveSprites = new Sprite[4];

    // Start is called before the first frame update
    void Start()
    {

        _scoreText.text = "Score: " + 0;
        _gameOverText.enabled = false;
        _restartText.enabled = false;
        _quitText.enabled = false;



    }

    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score.ToString();
    }
    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _liveSprites[currentLives];
    }

    public void IsGameOver()
    {
        _gameOverText.enabled = true;
        _restartText.enabled = true;
        _quitText.enabled = true;


        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.enabled = true;
            _restartText.enabled = true;
            _quitText.enabled = true;


            yield return new WaitForSeconds(0.5f);
            _gameOverText.enabled = false;
            _restartText.enabled = false;
            _quitText.enabled = false;


            yield return new WaitForSeconds(0.5f);

        }
    }


}
