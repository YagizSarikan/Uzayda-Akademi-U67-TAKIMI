using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] Transform _logo;
    [SerializeField] GameObject _pressStartToPlay;

    IUserInput _userInput;
    bool _startPressed;

    void Awake()
    {
        _userInput = UserInput.Instance;
    }

    void OnEnable()
    {
        _pressStartToPlay.SetActive(false);
        _canvasGroup.alpha = 0f;
        _canvasGroup.DOFade(1f, 1f);
        _logo.localScale = Vector3.zero;
        _userInput.OnStartPressed += HandleOnStart;
        _logo.DOScale(Vector3.one, 3f)
            .OnComplete(EnablePressStartToPlay);
    }

    void OnDisable()
    {
        _userInput.OnStartPressed -= HandleOnStart;
    }

    void Update()
    {
        if (_pressStartToPlay.activeSelf && _startPressed)
        {
            SceneManager.LoadScene("Main");
        }
    }

    void HandleOnStart()
    {
        _startPressed = true;
    }

    void EnablePressStartToPlay()
    {
        _startPressed = false;
        _pressStartToPlay.SetActive(true);
    }
}
