using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _homeUI, _inGameUI, _finishUI, _gameOverUI;
    [SerializeField] private GameObject _allButtons;

    private bool buttons;

    [Header("PreGame")]
    [SerializeField] private Button _soundButton;
    [SerializeField] private Sprite _soundOnS, _soundOffS;

    [Header("InGame")]
    [SerializeField] private Image _levelSlider;
    [SerializeField] private Image _currentLevelImg;
    [SerializeField] private Image _nextLevelImg;
    [SerializeField] private Text _currentLevelText, _nextLevelText;

    [Header("Finsh")]
    [SerializeField] private Text _finishLevelText;

    [Header("GameOver")]
    [SerializeField] private Text _gameOverScoreText;
    [SerializeField] private Text _gameOverBestText;

    private Material ballMaterial;
    private Ball ball;

    SecurePlayerPrefs securePlayerPrefs;

    #region UnityLifecycle
    void Awake()
    {
        ballMaterial = FindObjectOfType<Ball>().transform.GetChild(0).GetComponent<MeshRenderer>().material;
        ball = FindObjectOfType<Ball>();

        securePlayerPrefs = new SecurePlayerPrefs(PlayerPrefsTags.KEY, PlayerPrefsTags.HIGH_SCORE);

        _levelSlider.transform.parent.GetComponent<Image>().color = ballMaterial.color + Color.gray;
        _levelSlider.color = ballMaterial.color;
        _currentLevelImg.color = ballMaterial.color;
        _nextLevelImg.color = ballMaterial.color;

        _soundButton.onClick.AddListener(() => SoundManager.instance.SoundOnOff());
    }

    void Start()
    {
        _currentLevelText.text = FindObjectOfType<LevelSpawner>().level.ToString();
        _nextLevelText.text = FindAnyObjectByType<LevelSpawner>().level + 1 + "";
    }

    // Update is called once per frame
    void Update()
    {
        if (ball.ballState == Ball.BallState.Prepare)
        {
            if (SoundManager.instance.sound && _soundButton.GetComponent<Image>().sprite != _soundOnS)
                _soundButton.GetComponent<Image>().sprite = _soundOnS;
            else if (!SoundManager.instance.sound && _soundButton.GetComponent<Image>().sprite != _soundOffS)
                _soundButton.GetComponent<Image>().sprite = _soundOffS;
        }

        if (Input.GetMouseButtonDown(0) && !IgnoreUI() && ball.ballState == Ball.BallState.Prepare)
        {
            ball.ballState = Ball.BallState.Playing;
            _homeUI.SetActive(false);
            _inGameUI.SetActive(true);
            _finishUI.SetActive(false);
            _gameOverUI.SetActive(false);
        }

        if (ball.ballState == Ball.BallState.Finish)
        {
            _homeUI.SetActive(false);
            _inGameUI.SetActive(false);
            _finishUI.SetActive(true);
            _gameOverUI.SetActive(false);

            _finishLevelText.text = "Level " + FindObjectOfType<LevelSpawner>().level;
        }

        if (ball.ballState == Ball.BallState.Died)
        {
            _homeUI.SetActive(false);
            _inGameUI.SetActive(false);
            _finishUI.SetActive(false);
            _gameOverUI.SetActive(true);

            _gameOverScoreText.text = ScoreManager.instance.score.ToString();
            _gameOverBestText.text = PlayerPrefs.GetInt(PlayerPrefsTags.HIGH_SCORE).ToString();

            if (Input.GetMouseButtonDown(0))
            {
                if (ScoreManager.instance.score > PlayerPrefs.GetInt(PlayerPrefsTags.HIGH_SCORE))
                {
                    int tempValue = PlayerPrefs.GetInt(PlayerPrefsTags.HIGH_SCORE);
                    PlayerPrefs.SetInt(PlayerPrefsTags.HIGH_SCORE, ScoreManager.instance.score);
                    securePlayerPrefs.Save();
                    CheckForCheating(tempValue);
                }
                else
                {
                    ScoreManager.instance.ResetScore();
                    SceneManager.LoadScene(0);
                }
            }
        }
    }
    #endregion

    #region FunctionPrivate
    private bool IgnoreUI()
    {
        // PointerEventData: mouse clicks, touch events, and pen input.
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
        for (int i = 0; i < raycastResults.Count; i++)
        {
            if (raycastResults[i].gameObject.GetComponent<Ignore>() != null)
            {
                raycastResults.RemoveAt(i);
                i--;
            }
        }

        return raycastResults.Count > 0;
    }

    private void CheckForCheating(int _checkCheatValue)
    {
        if (securePlayerPrefs.HasBeenEdited())
        {
            PlayerPrefs.SetInt(PlayerPrefsTags.HIGH_SCORE, _checkCheatValue);
            SceneManager.LoadScene(0);
        }
        else
        {
            ScoreManager.instance.ResetScore();
            SceneManager.LoadScene(0);
        }
    }
    #endregion

    #region FunctionPublic
    public void LevelSliderFill(float _fillAmount)
    {
        _levelSlider.fillAmount = _fillAmount;
    }
    #endregion

    #region EventButton
    public void Setting()
    {
        buttons = !buttons;
        _allButtons.SetActive(buttons);
    }
    #endregion
}
