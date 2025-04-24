namespace Games.Bingo
{
    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CardLine : MonoBehaviour
{
    [SerializeField]List<int> No;
    [SerializeField] CardNumberView[] cardNumberView;
    [SerializeField] string Line_Letter; 
    [SerializeField] int start_vale;


    private void Start()
    {
        int endvalue = start_vale + 15;
        for (int i = start_vale; i < endvalue; i++)
        {
            No.Add(i);
        }

        for(int i = 0; i < cardNumberView.Length; i++)
        {
           
            int Rndm_no = AutoRandom.Range(0, No.Count);
            cardNumberView[i].Set_No(No[Rndm_no], Line_Letter);
            No.RemoveAt(Rndm_no);
            if (i == cardNumberView.Length-1)
            {
                Destroy(GetComponent<CardLine>());
            }
        }
    }
}
}
