namespace Games.Bingo
{
    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Tutorial : MonoBehaviour
{
    public GameObject[] images;
    public int count = 0;
    private void OnEnable()
    {
        Atstart();
        count = 0;
        images[count].SetActive(true);
         images[count].transform.DOLocalMoveX(0f,0.5f).SetEase(Ease.Linear);
     }
    private void OnDisable()
    {
         images[count].SetActive(false);
     }
    void Atstart()
    {
        for(int i=1;i< images.Length; i++)
        {
            images[i].gameObject.SetActive(false);
            images[i].transform.DOLocalMoveX(1100f,0f);
        }
    }
    public void onNext()
    {
         int a = count;
        images[a].transform.DOLocalMoveX(-1100f, 0.5f).SetEase(Ease.Linear).OnComplete(()=> {
            images[a].transform.DOLocalMoveX(0f, 0f);
            images[a].SetActive(false);
        });
         count++;
        images[count].SetActive(true);
        images[count].transform.DOLocalMoveX(0f, 0.5f).SetEase(Ease.Linear);
     }
}
}