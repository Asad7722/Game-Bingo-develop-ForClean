namespace Games.Bingo
{
    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DownImage : MonoBehaviour
{
    RectTransform rectTransform;
    [SerializeField] RectTransform Bingo, Filler,Faster_Btn;
    void Start()
    {
         rectTransform = this.transform.GetComponent<RectTransform>();
        StartCoroutine(SetReact());
    }
    void Update()
    {
    }
    public IEnumerator SetReact()
    {
        yield return new WaitForSeconds(0.1f);
        ANN();
        Invoke("ANN", 0.5f);
    }
    public void ANN()
    {
        Faster_Btn.anchoredPosition = new Vector2(-98, 0);
        Bingo.anchoredPosition = new Vector2(-287, 0);
        Filler.anchoredPosition = new Vector2(236, 0);
    }
}
}