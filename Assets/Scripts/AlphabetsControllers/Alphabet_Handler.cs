using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alphabet_Handler : MonoBehaviour

{    //This script handles logic for toggling between prev and next alphabets.

    //stores the list of alphabet.
    public List<GameObject> alphabets;
    int currentIndex=0;
    // Start is called before the first frame update
    void Start()
    {
        //Intially enable only "A" alphabet.
        foreach(GameObject alphabet in alphabets)
        {
            alphabet.SetActive(false);
        }
        alphabets[0].SetActive(true);
    }

    //This Method gets Next Alphabet
    public void GetNextAlphabet()
    {
        alphabets[currentIndex].SetActive(false);
        if (currentIndex >= alphabets.Count-1)   //if current index is greater than array elements set to 0
        {
            currentIndex = 0;
        }
        currentIndex++;
        alphabets[currentIndex].SetActive(true);
        
    }

    //This method gets previous alphabet
    public void GetPreviousAlphabet()
    {
        alphabets[currentIndex].SetActive(false);
        if (currentIndex == 0)    //if current element is zero and previous accessed set it to last alphabet.
        {
            currentIndex = alphabets.Count-1;
        }
        currentIndex--;
        alphabets[currentIndex].SetActive(true);
    }
}
