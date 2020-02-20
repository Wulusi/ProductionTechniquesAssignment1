using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Text Health;

    [SerializeField]
    private float CurrentHealth, MaxHealth;

    [SerializeField]
    private GameObject GameOver;

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CheckGameState()
    {
        if(CurrentHealth < 0)
        {
            //Lose State Here;
        }
    }
}
