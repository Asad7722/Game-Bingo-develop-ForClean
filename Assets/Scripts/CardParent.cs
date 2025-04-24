namespace Games.Bingo
{
    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BrilliantBingo.Code.Infrastructure.Views;
using DG.Tweening;
    using TMPro;
    public class CardParent : MonoBehaviour
    {
        public static CardParent instance;
        private int _currentNo;
        public int CurrentNo
        {
            get => _currentNo;
            set
            {
                if (_currentNo != value)
                {
                    _currentNo = value;
                    UIManager.instance.tutorialScreen.SetActive(false);
                    if (!UIManager.instance.hasShownHintBefore || !UIManager.instance.hasShownSkipHintBefore)
                    {
                       Invoke( "ShowHint",2f);
                    }
                }
            }
        }
        public List<CardNumberView> Corner_Bingo, UpperSide_Bingo, DownSide_Bingo = new List<CardNumberView>();
        public List<CardNumberView> All_Btns = new List<CardNumberView>();
        public List<CardNumberView> All_Btns_cntRemoveable = new List<CardNumberView>();
        public List<int> Marked_Numbers;
        public Sprite Golden_Sp;
        public Transform Happy_Images;
        bool isCorner, is_upperdiag, is_downrdiag, isrow, iscoloum;
        Vector3 ScaleValue = new Vector3(1.14f, 1.14f, 1.14f);
        Vector3 Rotate_value = new Vector3(0f, 0f, 15f);
        public int How_much_Bingo = 0;
        ScoreSummary scoreSummary;
        UIManager uIManager;
        [SerializeField] private GameObject tutorialScreen;
        [SerializeField] private Button skipButton;
        [SerializeField] private GameObject handIndicator;
        [SerializeField] private TextMeshProUGUI tutorialText;
        public int MultiBingo_Timee;
        private void Awake()
        {
            instance = this;
        }
        private void Start()
        {
            uIManager = UIManager.instance;
            Corner_Bing_objects();
        }
        private void OnEnable()
        {
            scoreSummary = ScoreSummary.instance;
            Get_All_Btns();
            StartCoroutine(_Initialize());
            tutorialScreen.SetActive(false);
            BingoInactivityManager.OnTimeoutEvent += HandleTimeout;
            BingoInactivityManager.OnTimeResetEvent += HandleTimeReset;
        }
        private void OnDisable()
        {
            BingoInactivityManager.OnTimeoutEvent -= HandleTimeout;
            BingoInactivityManager.OnTimeResetEvent -= HandleTimeReset;
        }
        private void HandleTimeout()
        {
        }
        private void HandleTimeReset()
        {
            HideHand();
        }
        public IEnumerator _Initialize()
        {
            yield return new WaitForSeconds(0.5f);
            for(int i = 0; i < All_Btns.Count; i++)
            {
                All_Btns[i]._Initialize();
                yield return new WaitForSeconds(0.05f);
            }
        }
    public void Remove_Obj(CardNumberView Ob)
    {
        int Get_indx = All_Btns.IndexOf(Ob);
        if (Get_indx >= 0)
        {
        All_Btns.RemoveAt(Get_indx);
        }
        if(All_Btns.Count  <=3)
            {
            }
    }
    public void Happy_Particles(Transform ob)
    {
        Happy_Images.transform.position = ob.transform.position;
        Happy_Images.GetComponent<ParticleSystem>().Play();
    }
    void Corner_Bing_objects()
    {
        for (int i = 0; i < 2; i++)
        {
            if (i != 0)
            {
                i = transform.childCount - 1;
            }
            Corner_Bingo.Add(transform.GetChild(i).GetChild(0).GetComponent<CardNumberView>());
            Corner_Bingo.Add(transform.GetChild(i).GetChild(transform.childCount - 1).GetComponent<CardNumberView>());
        }
        for (int a = 0; a < transform.childCount; a++)
        {
            UpperSide_Bingo.Add(transform.GetChild(a).GetChild(a).GetComponent<CardNumberView>());
        }
        for (int a = 0; a < transform.childCount; a++)
        {
            DownSide_Bingo.Add(transform.GetChild(a).GetChild(transform.childCount - a - 1).GetComponent<CardNumberView>());
        }
    }
    public void Get_All_Btns()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            for (int a = 0; a < transform.GetChild(i).childCount; a++)
            {
                CardNumberView card = transform.GetChild(i).GetChild(a).GetComponent<CardNumberView>();
                if (card.enabled)
                {
                    All_Btns.Add(card);
                }
                    All_Btns_cntRemoveable.Add(card);
            }
        }
    }
    public void Checkif_Miss(bool ShowMessage)
    {
        for (int i = 0; i < All_Btns.Count; i++)
        {
            if (CurrentNo == All_Btns[i].Card_No && !All_Btns[i].Is_marked)
            {
                if (ShowMessage)
                {
                   Balltubeview.instance.Cur_bingoball.Miss_Ball();
                 StartCoroutine(uIManager.Play_Anim(0));
                scoreSummary.Bad_Daub(false,100);
                }
   return;
            }
        }
    }
  public void Check_Condition()
    {
          if (!isCorner)
        {
            int total = 0;
            for (int i = 0; i < Corner_Bingo.Count; i++)
            {
                if (Corner_Bingo[i].Is_marked == true)
                {
                    total++;
                }
                if (total == Corner_Bingo.Count)
                {
                    scoreSummary.Set_Bingo_Score();
                    MultiBingo_Timee++;
                    How_much_Bingo++;
                    for (int a = 0; a < Corner_Bingo.Count; a++)
                    {
                        isCorner = true;
                        Transform ob = Corner_Bingo[a].transform;
                        ob.GetComponent<CardNumberView>()._bingo();
                    }
                }
            }
        }
          if (!is_upperdiag)
        {
            int Upper_Digonal = 0;
            for (int i = 0; i < UpperSide_Bingo.Count; i++)
            {
                if (UpperSide_Bingo[i].Is_marked == true)
                {
                    Upper_Digonal++;
                }
                if (Upper_Digonal == UpperSide_Bingo.Count)
                {
                    scoreSummary.Set_Bingo_Score();
                    MultiBingo_Timee++;
                     How_much_Bingo++;
                    is_upperdiag = true;
                    for (int a = 0; a < UpperSide_Bingo.Count; a++)
                    {
                        Transform ob = UpperSide_Bingo[a].transform;
                        ob.GetComponent<CardNumberView>()._bingo();
                    }
                }
            }
        }
 if (!is_downrdiag)
        {
            int Down_Digonal = 0;
            for (int i = 0; i < DownSide_Bingo.Count; i++)
            {
                if (DownSide_Bingo[i].Is_marked)
                {
                    Down_Digonal++;
                }
                if (Down_Digonal == DownSide_Bingo.Count == true)
                {
                    scoreSummary.Set_Bingo_Score();
                    MultiBingo_Timee++;
                     How_much_Bingo++;
                    is_downrdiag = true;
                    for (int a = 0; a < DownSide_Bingo.Count; a++)
                    {
                        Transform ob = DownSide_Bingo[a].transform;
                        ob.GetComponent<CardNumberView>()._bingo();
                    }
                }
            }
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            int check = 0;
            Transform Check_Coloumn = transform.GetChild(i);
            for (int a = 0; a < Check_Coloumn.childCount; a++)
            {
                if (Check_Coloumn.GetChild(a).GetComponent<CardNumberView>().Is_marked == true && !Check_Coloumn.GetChild(a).GetComponent<CardNumberView>().isAlready_BingoColom)
                {
                    check++;
                }
                if (check == Check_Coloumn.childCount)
                {
                    scoreSummary.Set_Bingo_Score();
                    MultiBingo_Timee++;
                     How_much_Bingo++;
                    iscoloum = true;
                    for (int x = 0; x < Check_Coloumn.childCount; x++)
                    {
                        Transform ob = Check_Coloumn.GetChild(x);
                        ob.GetComponent<CardNumberView>()._bingo();
                        ob.GetComponent<CardNumberView>().isAlready_BingoColom = true;
                    }
                }
            }
        }
  for (int i = 0; i < transform.childCount; i++)
        {
            int check = 0;
            for (int a = 0; a < transform.childCount; a++)
            {
                if (transform.GetChild(a).GetChild(i).GetComponent<CardNumberView>().Is_marked == true && !transform.GetChild(a).GetChild(i).GetComponent<CardNumberView>().isAlready_BingoRow)
                {
                    check++;
                }
                if (check == transform.childCount)
                {
                    scoreSummary.Set_Bingo_Score();
                    MultiBingo_Timee++;
                    How_much_Bingo++;
                    isrow = true;
                    for (int x = 0; x < transform.childCount; x++)
                    {
                        Transform ob = transform.GetChild(x).GetChild(i);
                        ob.GetComponent<CardNumberView>()._bingo();
                        ob.GetComponent<CardNumberView>().isAlready_BingoRow = true;
                    }
                }
            }
        }
        if (MultiBingo_Timee > 1)
        {
            scoreSummary.Set_Multi_Bingo(MultiBingo_Timee);
             MultiBingo_Timee = 0;
              SoundManager.instance._Bingo_sndfx(1);
        }
        else
        {
                if (MultiBingo_Timee != 0 && MultiBingo_Timee <= 1)
                {
                    SoundManager.instance._Bingo_sndfx(0);
                }
                else
                {
                    scoreSummary.Bad_Daub(false, 100);
                }
                MultiBingo_Timee = 0;
        }
        if (MultiBingo_Timee == 0)
        {
                StartCoroutine( uIManager.Play_Anim(1));
            SoundManager.instance.Play_Vibration(70);
        }
        CHeck_More_Daub();
    }
    public IEnumerator Time_Out()
    {
        UIManager.instance.IsGame_Finish = true;
            Balltubeview.instance.Stop_Ball_Working();
            SoundManager.instance.Ticktick_source.enabled = false;
            yield return new WaitForSeconds(1f);
         if (All_Btns.Count > 1)
        {
            for (int i = 0; i < All_Btns.Count; i++)
            {
                All_Btns[i].GetComponent<CardNumberView>().Time_out();
                if (i == (All_Btns.Count - 1))
                {
                    StartCoroutine(uIManager.Show_Scrsummary());
                }
                yield return new WaitForSeconds(0.05f);
            }
        }
        else
        {
                    StartCoroutine(uIManager.Show_Scrsummary());
        }
    }
    public IEnumerator Check()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(Time_Out());
    }
    public void CHeck_More_Daub()
    {
        if (All_Btns.Count <= 0)
        {
            uIManager.Empty_Panel.SetActive(true);
             StartCoroutine(Check());
        }
    }
    public void Onbingo_BtnPressed()
    {
            if (Input.touchCount > 0)
            {
                Check_Condition();
            }
    }
        public void ShowHint()
        {
            UIManager.instance.isTutorial = true;

            foreach (var btn in All_Btns)
            {
                btn.ResetHighlight();
            }

            bool foundHint = false;
            CardNumberView hintButton = null;
            foreach (var btn in All_Btns)
            {
                if (btn.Card_No == CurrentNo && !btn.Is_marked)
                {
                    foundHint = true;
                    hintButton = btn;
                    break;
                }
            }

            UIManager.instance.isHintFound = foundHint;

            // Start the tutorial coroutine for both cases
            if (foundHint && !UIManager.instance.hasShownHintBefore)
            {
                UIManager.instance.hasShownHintBefore = true;
                StartCoroutine(ShowTutorialWithDelay("Tap the tiles to fill them in!", hintButton.transform, highlight: true));
            }
            else if (!foundHint && !UIManager.instance.hasShownSkipHintBefore)
            {
                UIManager.instance.hasShownSkipHintBefore = true;
                StartCoroutine(ShowTutorialWithDelay("Use the fast forward button to skip a number!", skipButton.transform, highlight: false));
            }
        }


        IEnumerator ShowTutorialWithDelay(string message, Transform handTarget, bool highlight)
        {
            tutorialScreen.SetActive(true);
            tutorialText.text = message;

            if (highlight && handTarget.TryGetComponent(out CardNumberView btn))
            {
                btn.Highlight();
            }

            AnimateHand(handTarget);

            yield return new WaitForSeconds(3f); // Same delay for both

            tutorialScreen.SetActive(false);
        }
        private void AnimateHand(Transform target)
        {
            handIndicator.transform.position = target.position;
        }
        public void HideHand()
        {
            tutorialScreen.SetActive(false);
            handIndicator.transform.DOKill();
        }
    }
}