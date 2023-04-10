using TMPro;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] TMP_Text _scoreText;
    [SerializeField] GameObject _waveCompletePanel;
    
    GameManager _gameManager;

    void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        _gameManager.ScoreChanged += (score) => _scoreText.text = score.ToString();
        _gameManager.WaveComplete += () => _waveCompletePanel.SetActive(true);
    }
}
