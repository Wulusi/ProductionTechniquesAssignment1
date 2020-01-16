using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FunTimeTests : MonoBehaviour
{

    public const string SayStuff = " Wu Ate: ";
    public float CheeseBugerCount, MaxChzCount;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SayCheese(CheeseBugerCount));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SayCheese(float count)
    {
        while(count < MaxChzCount)
        {
            Debug.Log(SayStuff + count + " Cheeseburgers");

            yield return new WaitForSeconds(0.5f);

            Debug.Log(" Munch Munch " + " he says its yummy!");

            count++;
        }

        Debug.Log(" Man he ate too much cheeseburgers");
        yield return null; 
    }

    private void EatBurgers()
    {

        //Get some Burgers and eat it here :D
    }
}
