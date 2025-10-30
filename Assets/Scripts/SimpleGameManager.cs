using UnityEngine;
using TMPro;
using System.Collections;

public class SimpleGameManager : MonoBehaviour
{
    public static SimpleGameManager instance;

    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private TMP_Text endGameTitle;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text levelCompleteCoins;

    private int coinCount = 0;
    private int totalCoins = 0;

    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        SimpleUIManager.instance.fadeFromBlack = true;
        UpdateGUI();
        FindTotalCoins();
    }

    public void IncrementCoinCount()
    {
        coinCount++;
        UpdateGUI();
    }

    private void UpdateGUI()
    {
        if (coinText != null)
        {
            coinText.text = coinCount.ToString();
        }
    }

    private void FindTotalCoins()
    {
        // Find all objects with Coin tag in the scene
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
        totalCoins = coins.Length;
    }

    public void LevelComplete()
    {
        blackScreen.SetActive(true);
        StartCoroutine(LevelCompleteCoroutine());
    }
   
    private IEnumerator LevelCompleteCoroutine()
    {
        yield return new WaitForSeconds(2f);
        endGamePanel.SetActive(true);
        endGameTitle.text = "LEVEL COMPLETE";
        levelCompleteCoins.text = "COINS COLLECTED: " + coinCount.ToString() + " / " + totalCoins.ToString();
    }
}