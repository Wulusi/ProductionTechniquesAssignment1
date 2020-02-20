using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Text Health;

    //[SerializeField]
    public float CurrentHealth, MaxHealth;

    [SerializeField]
    private GameObject GameOver;

    // Start is called before the first frame update
    void Start()
    {
        GameOver.SetActive(false);
        CurrentHealth = MaxHealth;
        StartCoroutine(CheckState());
    }

    IEnumerator CheckState()
    {
        while (true)
        {
            UpdateUI();
            CheckGameState();
            yield return null;
        }
    }

    private void CheckGameState()
    {
        if(CurrentHealth <= 0)
        {
            GameOver.SetActive(true);
        }
    }

    private void UpdateUI()
    {
        Health.text = CurrentHealth.ToString();
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
