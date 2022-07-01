using UnityEngine;
using TMPro;

public class BallonManager : MonoBehaviour
{
    public float ballonSpeed;
    public string ballonWord;
    public TextMeshProUGUI ballonText;
    public GameManager gameManager;
    public GameObject particleEffect;
    public bool movement;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        ballonText.text = ballonWord;
        //movement = false;
    }
    private void Update()
    {
        if (movement)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - Time.deltaTime * ballonSpeed, transform.position.z);
        }
       
        //if (ballonWord.ToLower() == gameManager.keyboard.text.ToLower() && movement)
        if (ballonWord.ToLower() == gameManager.text.text.ToLower() && movement)
        {
            gameManager.checkLevelWordCounts--;
            gameManager.SpawnBallons();
            BallonDestroy();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("RedLine"))
        {
            BallonDestroy();
            gameManager.GameOver();
        }
    }

    private void BallonDestroy()
    {
        Instantiate(particleEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        Destroy(this.gameObject);
        gameManager.text.text = "";
        gameManager.keyboard.text = "";
        gameManager.checkNextLevel = true;
        gameManager.barCheck = true;
        gameManager.audioSource.PlayOneShot(gameManager.ballonBurst, 1);
    }
}
