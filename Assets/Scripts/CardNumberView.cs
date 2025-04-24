namespace Games.Bingo
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using DG.Tweening;
    public class CardNumberView : MonoBehaviour
    {
        [SerializeField] Sprite[] Daub_Sprites;
        public string Card_Letter;
        public int Card_No;
        [SerializeField] Text Card_text;
        [SerializeField] Button _button;
        [SerializeField] Image Card_img;
        public bool Is_marked = false;
        public bool isAlready_BingoRow, isAlready_BingoColom;
        bool Wrong_tween = true;
        Vector3 ScaleValue = new Vector3(1.14f, 1.14f, 1.14f);
        Vector3 Rotate_value = new Vector3(0f, 0f, 15f);
        [SerializeField] ParticleSystem Star_particle, Dust_particle;
        Balltubeview balltubeview;
        Bingocardview bingocardview;
        ScoreSummary scoreSummary;
        DOTweenAnimation tween;
        [SerializeField] DOTweenAnimation WrongTween;
        [SerializeField] Color Wrong_color;
        private float timeSinceLastClick = 0f;
        private bool isTimerActive = false;
        private void OnEnable()
        {
            bingocardview = Bingocardview.instance;
                transform.localScale = Vector3.zero;
            if (!Is_marked)
            {
            }
            else
            {
                GetComponent<CardNumberView>().enabled = false;
                transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack);
            }
        }
        private void Awake()
        {
            _button = GetComponent<Button>();
            Card_img = GetComponent<Image>();
        }
        private void Start()
        {
            balltubeview = Balltubeview.instance;
            scoreSummary = ScoreSummary.instance;
            _button.onClick.AddListener(Marked);
            tween = GetComponent<DOTweenAnimation>();
        }
        public void Set_No(int No, string Letter)
        {
            Card_Letter = Letter;
            Card_No = No;
            Card_text.text = Card_No.ToString();
        }
        public void Play_Wrong_tween()
        {
            if (Wrong_tween)
            {
                WrongTween.gameObject.SetActive(true);
                Wrong_tween = false;
                GetComponent<Image>().color = Wrong_color;
                SoundManager.instance.Wrng_tile_Fx();
               Invoke("_boolreset", 0.3f);
            }
        }
        public void _Initialize()
        {
            SoundManager.instance.Daub_Active_Fx();
            transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutBack);
        }
        void _boolreset()
        {
            Wrong_tween = true;
            GetComponent<Image>().color = Color.white;
        }
        public void Marked()
        {
            if (Input.touchCount > 0)
            {
                BingoInactivityManager.instance.ResetTimer();
                if (Bingocardview.instance.IsInstant3)
                {
                    for (int i = 0; i < Bingocardview.instance._instant_card_Nos.Count; i++)
                    {
                        if (Bingocardview.instance._instant_card_Nos[i] == Card_No)
                        {
                            Daub_Spchange(0);
                            CardParent.instance.Remove_Obj(GetComponent<CardNumberView>());
                            GetComponent<CardNumberView>().enabled = false;
                            return;
                        }
                        else
                        {
                            tween.DORestart();
                        }
                    }
                }
                else if (Bingocardview.instance.IsFreedaub)
                {
                    Daub_Spchange(0);
                    CardParent.instance.Remove_Obj(GetComponent<CardNumberView>());
                    GetComponent<CardNumberView>().enabled = false;
                    StartCoroutine(Bingocardview.instance.ExitPowerUP(0f));
                }
                else if (Bingocardview.instance.IsGold_Instant)
                {
                    if (Bingocardview.instance.Power_Set_Gold_number == Card_No)
                    {
                        Balltubeview.instance.Golden_Bingo_effect();
                        Daub_Spchange(1);
                        CardParent.instance.Remove_Obj(GetComponent<CardNumberView>());
                        GetComponent<CardNumberView>().enabled = false;
                        Balltubeview.instance.PowerUp_Replace_ANimation(true);
                        StartCoroutine(Bingocardview.instance.ExitPowerUP(0.55f));
                    }
                    else
                    {
                        Play_Wrong_tween();
                        tween.DORestart();
                    }
                }
                else
                {
                    for (int i = 0; i < balltubeview.bingoball.Count; i++)
                    {
                        if (balltubeview.Cur_bingoball != null && balltubeview.Cur_bingoball.Current_No == Card_No)
                        {
                            if (balltubeview.Is_click)
                            {
                                Bingocardview.instance.GamePlay_Scr(balltubeview.Cur_Filler.fillAmount, transform);
                                balltubeview.Faster_Move(false);
                                Daub_Spchange(0);
                                CardParent.instance.Remove_Obj(GetComponent<CardNumberView>());
                                GetComponent<CardNumberView>().enabled = false;
                            }
                            return;
                        }
                        else if (balltubeview.bingoball[i].Current_No == Card_No && balltubeview.bingoball[i].Crnt_ball_pos != 0)
                        {
                            if (Card_No == balltubeview.Cur_bingoball.Current_No)
                            {
                                Bingocardview.instance.GamePlay_Scr(balltubeview.Cur_Filler.fillAmount, transform);
                                balltubeview.Faster_Move(false);
                            }
                            Daub_Spchange(0);
                            CardParent.instance.Remove_Obj(GetComponent<CardNumberView>());
                            GetComponent<CardNumberView>().enabled = false;
                            return;
                        }
                        else if (i == balltubeview.bingoball.Count - 1)
                        {
                            Play_Wrong_tween();
                            scoreSummary.Bad_Daub(true, 100);
                            tween.DORestart();
                            SoundManager.instance.Play_Vibration(40);
                        }
                    }
                }
            }
        }
        private void OnMouseDown()
        {
        }
        bool istap = true;
        public void _Tap()
        {
            if (istap)
            {
                istap = false;
                SoundManager.instance.Daub_Active_Fx();
                transform.DOScale(new Vector3(1.05f, 1.05f, 1.05f), 0.2f).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
                {
                    istap = true;
                });
            }
        }
        public void Daub_Spchange(int changer_type)
        {
            if (changer_type == 0 || changer_type == 1)
            {
                CardParent.instance.Marked_Numbers.Add(Card_No);
                UIManager.instance.hasShownHintBefore = true;
                Star_particle.Play();
                scoreSummary.Set_Daub_Score();
                SoundManager.instance.Daub_Success_Fx();
            }
            GetComponent<Button>().enabled = false;
            Is_marked = true;
            Card_img.sprite = Daub_Sprites[changer_type];
            Card_text.color = Color.white;
            Card_text.DOFade(0.5f, 0.1f);
            if (changer_type >= 1)
            {
                Card_text.gameObject.SetActive(false);
            }
        }
        public void Time_out()
        {
            if (!Is_marked)
            {
                GetComponent<Image>().color = Color.grey;
                transform.GetChild(1).GetComponent<ParticleSystem>().Play();
                tween.DORestart();
            }
        }
        public void _bingo()
        {
            Daub_Spchange(2);
            transform.DOScale(ScaleValue, 0.3f).SetEase(Ease.InOutSine).SetLoops(2, LoopType.Yoyo);
            transform.DORotate(Rotate_value, 0.3f).SetEase(Ease.InOutSine).SetLoops(2, LoopType.Yoyo);
            transform.GetChild(0).gameObject.SetActive(false);
        }
        public void Highlight()
        {
        }
        public void ResetHighlight()
        {
        }
    }
}