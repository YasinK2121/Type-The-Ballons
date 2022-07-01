using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private const int TIMERSECOND = 3;
    private const string TYPE = "TYPE!";

    [Header("GameLevelEditor")]
    public GameLevelEditor gameLevel;

    [Header("Spawn")]
    public GameObject spawn;
    public GameObject ballonPrefab;
    public GameObject ballon;
    public List<GameObject> gameInBallons;
    public List<GameObject> allBallonsObject;
    public List<Sprite> ballonSprite;
    public int randomNumb;
    public float randomPos;
    public int randomSprite;

    [Header("Level")]
    public int whichLevel;
    public int newLevel;
    public int textLevel;
    public int wordsCount;
    public int whichDifficulty;
    public int randomWord;
    public int stepCount;
    public int aaa;
    public float howSeconds;
    public float levelBallonSpeed;

    [Header("WordCheck")]
    public TouchScreenKeyboard keyboard;
    public InputField text;
    public bool checkNextLevel;
    public bool checkAgainLevel;
    public int checkLevelWordCounts;

    [Header("GamePanel")]
    public GameObject startPanel;
    public GameObject gameOverPanel;
    public GameObject nextLevelPanel;
    public GameObject ballonCountBar;
    public GameObject spine;
    public GameObject tile;

    [Header("GamePanelButton")]
    public Button keyboardButton;
    public Button startButton;
    public Button againButton;
    public Button nextLevelButton;

    [Header("GamePanelText")]
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI nextLevelText;
    public TextMeshProUGUI countDownText;

    [Header("LevelCountBar")]
    public TextMeshProUGUI firstText;
    public Image imageBar;
    public float imageFielled;
    public float imageFielledCalcu;
    public bool barCheck;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip gameOver;
    public AudioClip ballonBurst;

    private void Awake()
    {
        DefaultValue();
        PlayerPrefs.SetInt("level", whichLevel);
        PlayerPrefs.SetInt("textLevel", textLevel);
        whichLevel = PlayerPrefs.GetInt("level");
        textLevel = PlayerPrefs.GetInt("textLevel");
        textLevel = whichLevel;
    }

    private void Start()
    {
        startPanel.SetActive(true);
        startButton.onClick.AddListener(() => StartButton());
        againButton.onClick.AddListener(() => AgainButton());
        nextLevelButton.onClick.AddListener(() => NextLevelButton());
        keyboardButton.onClick.AddListener(() => KeyboardOpenButton());
        imageBar.fillAmount = 0;
        RandomLevel();
    }

    private void Update()
    {
        CheckNextLevel();
        if (barCheck)
        {
            imageBar.fillAmount += Time.deltaTime / 2;
            if (imageBar.fillAmount >= imageFielledCalcu)
            {
                barCheck = false;
                imageFielledCalcu += imageFielled;
            }
        }
    }

    private void StartLevel()
    {
        KeyboardOpenButton();
        countDownText.gameObject.SetActive(false);
        checkLevelWordCounts = 0;
        wordsCount = 0;

        for (int a = 0; a < gameLevel.levelData.Count; a++)
        {
            if (a == whichLevel)
            {
                whichDifficulty = (int)gameLevel.levelData[whichLevel].levelWords[0].item;
                wordsCount = gameLevel.levelData[whichLevel].levelWords[0].howManyWords;
                stepCount = 0;
            }
        }

        for (int b = 0; b < gameLevel.levelData[whichLevel].levelWords.Count; b++)
        {
            checkLevelWordCounts += gameLevel.levelData[whichLevel].levelWords[b].howManyWords;
        }

        for (int i = 0; i < checkLevelWordCounts; i++)
        {
            randomPos = Random.Range(-2f, 2f);
            randomSprite = Random.Range(0, ballonSprite.Count);
            randomWord = Random.Range(0, gameLevel.difficulty[whichDifficulty].Words.Count);
            levelBallonSpeed = gameLevel.difficulty[whichDifficulty].speed;
            ballon = Instantiate(ballonPrefab, new Vector3(randomPos, spawn.transform.position.y, spawn.transform.position.z), Quaternion.identity);
            gameInBallons.Add(ballon);
            allBallonsObject.Add(ballon);
            ballon.GetComponent<SpriteRenderer>().sprite = ballonSprite[randomSprite];
            ballon.GetComponent<BallonManager>().ballonSpeed = levelBallonSpeed;
            ballon.GetComponent<BallonManager>().ballonWord = gameLevel.difficulty[whichDifficulty].Words[randomWord].word;
            ballon.GetComponent<BallonManager>().movement = false;
            wordsCount--;

            if (wordsCount == 0)
            {
                stepCount++;
            }

            if (gameLevel.levelData[whichLevel].levelWords.Count > stepCount && wordsCount == 0)
            {
                whichDifficulty = (int)gameLevel.levelData[whichLevel].levelWords[stepCount].item;
                wordsCount = gameLevel.levelData[whichLevel].levelWords[stepCount].howManyWords;
            }
        }

        keyboardButton.gameObject.SetActive(true);
        imageFielled = 1f / checkLevelWordCounts;
        imageFielledCalcu = imageFielled;
        ballonCountBar.SetActive(true);
        firstText.text = (textLevel + 1).ToString();
        //StartCoroutine(SpawnBallon());
        RandomBallon();
    }

    IEnumerator TimerBallon(int second)
    {
        while (second >= -1)
        {
            if (second == 0)
            {
                countDownText.text = TYPE;
            }
            else if (second == -1)
            {
                StartLevel();
            }
            else
            {
                countDownText.text = second.ToString();
            }
            second--;
            yield return new WaitForSeconds(1f);
        }
    }

    //IEnumerator SpawnBallon()
    //{
    //    while (checkLevelWordCounts > 0)
    //    {
    //        yield return new WaitForSeconds(howSeconds);
    //        randomNumb = Random.Range(0, gameInBallons.Count);
    //        gameInBallons[randomNumb].GetComponent<BallonManager>().movement = true;
    //        gameInBallons.Remove(gameInBallons[randomNumb]);
    //    }
    //}

    private int dd = 0;

    public void SpawnBallons()
    {
        for (int i = 0; i < gameInBallons.Count; i++)
        {
            dd++;
            if (dd == gameInBallons.Count)
            {
                RandomBallon(); 
            }
        }
        dd = 0;
    }

    public void RandomBallon()
    {
        randomNumb = Random.Range(0, gameInBallons.Count);
        gameInBallons[randomNumb].GetComponent<BallonManager>().movement = true;
        gameInBallons.Remove(gameInBallons[randomNumb]);
    }

    public void DestroyBallons()
    {
        for (int i = 0; i < gameInBallons.Count; i++)
        {
            Destroy(gameInBallons[i].gameObject);
        }
        for (int k = 0; k < allBallonsObject.Count; k++)
        {
            Destroy(allBallonsObject[k].gameObject);
        }
        allBallonsObject.Clear();
        gameInBallons.Clear();
    }

    public void CheckNextLevel()
    {
        if (checkLevelWordCounts == 0 && checkNextLevel)
        {
            KeyboardClosedButton();
            keyboardButton.gameObject.SetActive(false);
            nextLevelPanel.SetActive(true);
            DestroyBallons();
            nextLevelText.text = "LEVEL " + (textLevel + 1) + " COMPLETED";
            checkNextLevel = false;
        }

        if (checkAgainLevel)
        {
            countDownText.gameObject.SetActive(true);
            StartCoroutine(TimerBallon(TIMERSECOND));
            checkAgainLevel = false;
        }
    }

    public void GameOver()
    {
        barCheck = false;
        imageBar.fillAmount = 0;
        keyboardButton.gameObject.SetActive(false);
        KeyboardClosedButton();
        gameOverPanel.SetActive(true);
        audioSource.PlayOneShot(gameOver, 1);
        DestroyBallons();
        gameOverText.text = "LEVEL " + (textLevel + 1) + " FAILED";
    }

    private void StartButton()
    {
        countDownText.gameObject.SetActive(true);
        StartCoroutine(TimerBallon(TIMERSECOND));
        startPanel.SetActive(false);
        checkNextLevel = false;
        spine.SetActive(true);
        tile.SetActive(true);
    }

    private void AgainButton()
    {
        imageBar.fillAmount = 0;
        gameOverPanel.SetActive(false);
        checkNextLevel = false;
        checkAgainLevel = true;
        CheckNextLevel();
    }

    private void KeyboardOpenButton()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        keyboard.active = true;
    }

    private void KeyboardClosedButton()
    {
        keyboard.active = false;
    }

    private void RandomLevel()
    {
        if (whichLevel < gameLevel.levelData.Count - 1)
        {
            whichLevel++;
            textLevel++;
            if (aaa == 0)
            {
                whichLevel = PlayerPrefs.GetInt("level");
                textLevel = PlayerPrefs.GetInt("textLevel");
            }
            aaa++;
        }
        else
        {
            newLevel = Random.Range(0, gameLevel.levelData.Count);
            whichLevel = newLevel;
            textLevel++;
        }
    }

    private void NextLevelButton()
    {
        RandomLevel();
        PlayerPrefs.SetInt("level", whichLevel);
        PlayerPrefs.SetInt("textLevel", textLevel);
        nextLevelPanel.SetActive(false);
        countDownText.gameObject.SetActive(true);
        barCheck = false;
        imageBar.fillAmount = 0;
        StartCoroutine(TimerBallon(TIMERSECOND));
        checkNextLevel = false;
        firstText.text = (textLevel + 1).ToString();
    }

    private void DefaultValue()
    {
        startPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        nextLevelPanel.SetActive(false);
        ballonCountBar.SetActive(false);
        countDownText.gameObject.SetActive(false);
        spine.SetActive(false);
        tile.SetActive(false);
        keyboardButton.gameObject.SetActive(false);
        barCheck = false;
    }
}
