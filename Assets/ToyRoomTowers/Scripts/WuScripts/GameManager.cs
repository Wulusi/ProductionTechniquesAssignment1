using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Current temporary game manager
    public float CurrentHealth, MaxHealth;

    [SerializeField]
    private Text Health;

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
