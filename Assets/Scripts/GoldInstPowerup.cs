namespace Games.Bingo
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using DG.Tweening;
    public class GoldInstPowerup : MonoBehaviour
    {
        public Image[] Gold_btns;
        [SerializeField] List<CardNumberView> all_cards;
        [SerializeField] Sprite[] Ball_sprites;
        public Image Instant_Bar, Golden_Image;
        Bingocardview bingocardview;
        [SerializeField] List<int> Get_rndm_no, All_No;
        public  List<Bingoball> _Forinst_Ball;
        bool auto_randm = false;
        Image gld_btn;
       public Balltubeview balltubeview;
        bool instant3 = false;
        private void OnEnable()
        {
            bingocardview = Bingocardview.instance;
            balltubeview = Balltubeview.instance;
            if (Get_rndm_no.Count > 0)
            {
                Get_rndm_no.Clear();
            }
            Instant_BallinQueue();
            if (Instant_Bar != null)
            {
                Instant_Bar.fillAmount = 1;
                Instant_Bar.DOFillAmount(0, 6f).SetEase(Ease.Linear);
            }
            if (Golden_Image != null)
            {
                auto_randm = false;
                Golden_Image.fillAmount = 1;
                Golden_Image.DOFillAmount(0, 5f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    if (!auto_randm)
                    {
                        string text = Gold_btns[0].transform.GetChild(1).GetComponent<Text>().text;
                        int number = int.Parse(text);
                        Set_Golden_Instant(number, 0);
                    }
                });
            }
            for (int i = 0; i < CardParent.instance.All_Btns.Count; i++)
            {
                All_No.Add(CardParent.instance.All_Btns[i].Card_No);
                all_cards.Add(CardParent.instance.All_Btns[i]);
            }
            int instant_randm = AutoRandom.Range(0, 2);
            int rndm = 0;
            int goldenBallLimitCounter = 0;
            if (CardParent.instance.All_Btns.Count > 2) {
                goldenBallLimitCounter = 3;
            }
            else
            {
                goldenBallLimitCounter = CardParent.instance.All_Btns.Count;
            }
            for (int i = 0; i < goldenBallLimitCounter; i++)
            {
                if (bingocardview.IsInstant3 && i > instant_randm)
                {
                Continue:
                    rndm = AutoRandom.Range(1, 76);
                    if (Get_rndm_no.Contains(rndm) || All_No.Contains(rndm) || CardParent.instance.Marked_Numbers.Contains(rndm))
                    {
                        goto Continue;
                    }
                    else
                    {
                        Get_rndm_no.Add(rndm);
                        Set_values(rndm, i);
                    }
                }
                else
                {
                Continue:
                    rndm = AutoRandom.Range(0, All_No.Count);
                    if (Get_rndm_no.Contains(rndm))
                    {
                        goto Continue;
                    }
                    else
                    {  Get_rndm_no.Add(All_No[rndm]);
                        Set_values(All_No[rndm], i);
                        All_No.RemoveAt(rndm);
                    }
                }
            }
        }
        public void Replace_Instant()
        {
            if (instant3)
            {
                for (int inst = 0; inst < 3; inst++)
                {
                    _Forinst_Ball[inst].Instant_BallEffect(Get_rndm_no[inst]);
                }
                instant3 = false;
            }
        }
        public void Instant_BallinQueue()
        {
            instant3 = bingocardview.IsInstant3;
            if (instant3)
            {
                if (_Forinst_Ball.Count > 0)
                {
                    _Forinst_Ball.Clear();
                }
                for (int i = 0; i < balltubeview.bingoball.Count; i++)
                {
                    int Crnt_pos = balltubeview.bingoball[i].Crnt_ball_pos;
                    if (Crnt_pos == 1 || Crnt_pos == 2 || Crnt_pos == 3)
                    {
                        _Forinst_Ball.Add(balltubeview.bingoball[i]);
                    }
                }
            }
        }
        private void OnDisable()
        {
            Replace_Instant();
            bingocardview._instant_card_Nos.Clear();
            all_cards.Clear();
            All_No.Clear();
            balltubeview.check_Match_No = 0;
            foreach(Image img in Gold_btns)
            {
                img.gameObject.SetActive(false);
            }
        }
        public void Set_values(int Check_No, int btnindx)
        {
            int coloring_indx;
            gld_btn = Gold_btns[btnindx];
            gld_btn.gameObject.SetActive(true);
            string Card_Letter;
            if (!Bingocardview.instance.IsInstant3)
            {
                gld_btn.GetComponent<Button>().onClick.AddListener(() => Set_Golden_Instant(Check_No, btnindx));
              }
            else
            {
                bingocardview._instant_card_Nos.Add(Check_No);
            }
            if (Check_No >= 0 && Check_No < 16)
            {
                coloring_indx = 0;
                Card_Letter = "B";
                gld_btn.sprite = Ball_sprites[0];
                goto Continue;
            }
            else if (Check_No >= 16 && Check_No < 31)
            {
                coloring_indx = 1;
                Card_Letter = "I";
                gld_btn.sprite = Ball_sprites[1];
                goto Continue;
            }
            else if (Check_No >= 31 && Check_No < 46)
            {
                coloring_indx = 2;
                Card_Letter = "N";
                gld_btn.sprite = Ball_sprites[2];
                goto Continue;
            }
            else if (Check_No >= 46 && Check_No < 61)
            {
                coloring_indx = 3;
                Card_Letter = "G";
                gld_btn.sprite = Ball_sprites[3];
                goto Continue;
            }
            else
            {
                coloring_indx = 4;
                Card_Letter = "O";
                gld_btn.sprite = Ball_sprites[4];
                goto Continue;
            }
        Continue:
            Text text1, text2;
            text1 = gld_btn.transform.GetChild(0).GetComponent<Text>();
            text2 = gld_btn.transform.GetChild(1).GetComponent<Text>();
            text1.text = Card_Letter.ToString();
            text2.text = Check_No.ToString();
            UIManager.instance.Coloring(coloring_indx, text1);
            UIManager.instance.Coloring(coloring_indx, text2);
        }
  public void Set_Golden_Instant(int number, int _btnindx)
        {
            SoundManager.instance.Btn_clickSound();
            Button genreted_btn = Gold_btns[_btnindx].GetComponent<Button>();
            auto_randm = true;
            Bingocardview bingocardview = Bingocardview.instance;
            bingocardview.totalTime = 8;
            bingocardview.Power_ups_panles(0);
            bingocardview.IsGold_Instant = true;
            bingocardview.Power_Set_Gold_number = number;
 Image image = genreted_btn.GetComponent<Image>();
            bingocardview.GoldenDemo.sprite = image.sprite;
            Text txt0child, txt1child;
            txt0child = genreted_btn.transform.GetChild(0).GetComponent<Text>();
            txt1child = genreted_btn.transform.GetChild(1).GetComponent<Text>();
            Color colorWithFullAlpha = new Color(txt0child.color.r, txt0child.color.g, txt0child.color.b, 1f);
            string Letter = txt0child.text;
            string Number = txt1child.text;
            Text Goldtxt0child, Goldtxt1child;
            Goldtxt0child = bingocardview.GoldenDemo.transform.GetChild(0).GetComponent<Text>();
            Goldtxt1child = bingocardview.GoldenDemo.transform.GetChild(1).GetComponent<Text>();
            Goldtxt0child.text = Letter;
            Goldtxt1child.text = Number;
            Goldtxt0child.color = colorWithFullAlpha;
            Goldtxt1child.color = colorWithFullAlpha;
        }
    }
}