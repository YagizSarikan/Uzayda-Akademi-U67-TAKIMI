using System.Collections;
using TMPro;
using UnityEngine;

public class WaveCompleteUI : MonoBehaviour
{
    [SerializeField] TMP_Text _attackWaveText, _bonusText;
    [SerializeField] Transform _survivingHumansContainer;
    [SerializeField] GameObject _survivingHumanPrefab;

    GameManager _gameManager;
    MobManager _mobManager;

    void OnEnable()
    {
        _gameManager = GameManager.Instance;
        _mobManager = FindObjectOfType<MobManager>();
        var bonusValue = _gameManager.Wave * 100;
        _attackWaveText.text = $"Attack Wave {_gameManager.Wave}";
        _bonusText.text = $"Bonus x {bonusValue}";
        while (_survivingHumansContainer.childCount < _mobManager.RemainingHumans)
        {
            Instantiate(_survivingHumanPrefab, _survivingHumansContainer);
        }

        for (int i = 0; i < _mobManager.RemainingHumans; ++i)
        {
            _survivingHumansContainer.GetChild(i).gameObject.SetActive(true);
            _gameManager.AddPoints(bonusValue);
        }

        StartCoroutine(NextWave());
    }

    IEnumerator NextWave()
    {
        yield return new WaitForSeconds(3f);
        for (int i = 0; i < _mobManager.RemainingHumans; ++i)
        {
            _survivingHumansContainer.GetChild(i).gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
        _gameManager.NextWave();
    }
}
