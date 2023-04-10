using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PlayerShip PlayerShip { get; private set; }
    public bool PlayerAlive { get; private set; }
    public int MapWidth => _mapWidth;
    public int Wave { get; private set; }
    public int Score { get; private set; }
    public int Lives { get; private set; }
    public int SmartBombs { get; private set; }

    public event Action PlayerShipSpawned = delegate { };
    public event Action<int> ScoreChanged;
    public event Action<int> PlayerLivesChanged;
    public event Action<int> SmartBombsChanged;
    public event Action<GameObject> EntityDestroyed;
    public event Action WaveComplete = delegate { };

    [SerializeField] GameObject _playerShipPrefab;
    [SerializeField] int _mapWidth = 50;

    IUserInput _userInput;
    float _smartBombDelay;
    MobManager _mobManager;
    Scene _currentScene;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        _userInput = UserInput.Instance;
        _userInput.OnSmartBombPressed += HandleSmartBombPressed;
    }

    void OnEnable()
    {
        StartGame();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode != LoadSceneMode.Additive) return;
        _currentScene = scene;
        _mobManager = FindObjectOfType<MobManager>();
        SpawnPlayerShip();
    }

    void OnSceneUnloaded(Scene scene)
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        StartWave(Wave + 1);
    }

    public void StartGame()
    {
        SoundManager.Instance.PlayAudioClip(SoundManager.Instance.StartSound);
        Score = 0;
        Lives = 3;
        SmartBombs = 3;
        StartWave(1);
    }

    void StartWave(int wave)
    {
        Wave = wave;
        SceneManager.LoadScene($"Level{Wave}", LoadSceneMode.Additive);
    }

    void SpawnPlayerShip()
    {
        if (PlayerAlive) return;
        PlayerShip = Instantiate(_playerShipPrefab).GetComponent<PlayerShip>();
        PlayerShip.name = "Player Ship";
        PlayerAlive = true;
        PlayerShip.GetComponent<CatchHuman>()?.Init(_mobManager.HumansContainer);
        PlayerShipSpawned();
    }

    public void AddPoints(int points)
    {
        Score += points;
        ScoreChanged?.Invoke(Score);
    }

    public void ComponentDestroyed(GameObject component)
    {
        if (component.TryGetComponent<PlayerShip>(out var player))
        {
            PlayerAlive = false;
            PlayerLivesChanged?.Invoke(--Lives);
            if (Lives > 0)
            {
                Invoke(nameof(SpawnPlayerShip), 3f);
            }
        }

        component.DropHumanPassenger(_mobManager.HumansContainer);
        Destroy(component);
        EntityDestroyed?.Invoke(component);

        if (Lives < 1)
        {
            GameOver();
            return;
        }

        if (_mobManager.RemainingEnemies < 1)
        {
            WaveComplete();
        }
    }

    public void NextWave()
    {
        WaveCleanUp();
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.UnloadSceneAsync(_currentScene);
    }

    void WaveCleanUp()
    {
        if (PlayerShip)
        {
            var human = PlayerShip.transform.GetComponentInChildren<Human>();
            if (human)
            {
                Destroy(human.gameObject);
            }
        }

        var patrolDestinations = FindObjectsOfType<PatrolDestination>();
        foreach (var destination in patrolDestinations)
        {
            Destroy(destination.gameObject);
        }

    }

    void GameOver()
    {
        Debug.Log("Game Over!");
        SceneManager.LoadScene("Kapanis");
    }

    void HandleSmartBombPressed()
    {
        if (SmartBombs < 1 || Time.time < _smartBombDelay) return;
        _smartBombDelay = Time.time + 0.25f;
        SmartBombsChanged?.Invoke(--SmartBombs);
    }
}
