using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour
{
    static public GameSystem instance;

    [SerializeField] private GameObject player;
    [SerializeField] private float playerSafeSpace;
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private GameObject asteroidFolder;
    [SerializeField] private int nbrOfAsteroids = 10;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI subText;

    private bool finished = false;
    private float timer;

    void Start()
    {
        instance = this;
        text.text = "";
        subText.text = "";
        SpawnAsteroids();
    }

    private void Update()
    {
        if (!finished) timer += Time.deltaTime;
        else
        {
            if ((Input.GetKey(KeyCode.Space)))
            {
                timer = 0;
                SceneManager.LoadScene(0);
                Destroy(gameObject);
            }
        }
    }

    private void SpawnAsteroids()
    {
        for (int i = 0; i < nbrOfAsteroids; i++)
        {
            float spawnY = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
            float spawnX = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);

            if (Vector3.Distance(new Vector3(spawnX, spawnY), player.transform.position) > playerSafeSpace)
            {
                Instantiate(asteroidPrefab, new Vector3(spawnX, spawnY), Quaternion.identity, asteroidFolder.transform);
            }
            else { i--; }
        }
    }

    public void GameOver()
    {
        finished = true;
        Destroy(player);
        text.text = "GAMEOVER";
        subText.text = "Press Space to retry";
    }

    public void WonGame()
    {
        finished = true;
        text.text = "YOU WON";
        subText.text = "Your time was " + timer;
    }

    private void OnDrawGizmosSelected()
    {
        if (player != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(player.transform.position, playerSafeSpace);
        }
    }
}
