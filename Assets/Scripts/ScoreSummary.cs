namespace Games.Bingo
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using DG.Tweening;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;
    using CodeStage.AntiCheat.ObscuredTypes;
    using Zenject;

    public class ScoreSummary : MonoBehaviour
    {
        public static ScoreSummary instance;
        [SerializeField] int Time_SaveScr;
        public int daub_scr, Bingoscr, Bad_daub, Multi_bingo, Total_time_multibingo, Missdaub;
        float temp = 0;
        int Speedy_count = 0;
        int multi_bingotime = 0, Bingo_scr_count;
        [Header("All text used in game")]
        [SerializeField] Text GamePlay_fillerscr;
        [SerializeField] Text Daub_Score_text, Daub_Time__text;
        [SerializeField] Text Bingo_Score_text, Bingo_Time_text;
        [SerializeField] Text Multi_BingoScore_text, Muli_bingo_time_text, multi_bingo_timecount_text;
        [SerializeField] Text Bad_Daub_score_text, Bad_Daub_time_text;
        [SerializeField] Text timeSaver_txt_text, Speedy_time_text, MissDaub, Miss_counter, Time_BonusTxt;
        [SerializeField] Text Final_score_text, Penallety_txt;
        SoundManager soundManager;
        public List<GameObject> items = new List<GameObject>();
        public float fadeTime = 1f;
        [SerializeField] int[] _bingoscr;
        int final_scr, BonusTime_Value;
        [SerializeField] ObscuredInt bingoPlayerScore = 0;

#if GO4_CORE_APP	
        [Inject] private GO4CoreAppBridge _appBridge;
#endif

        public ObscuredInt BingoPlayerScore
        {
            get { return bingoPlayerScore; }
            set
            {
                bingoPlayerScore = Mathf.Max(0, value); // Prevent negative scores
            }
        }

        private void Awake()
        {
            instance = this;
        }
        private void Start()
        {
            soundManager = SoundManager.instance;
        }
        public void Play_Anim()
        {
            StartCoroutine(ItemsAnimation());
        }
        IEnumerator ItemsAnimation()
        {
            foreach (var item in items)
            {
                item.transform.localScale = Vector3.zero;
            }
            foreach (var item in items)
            {
                item.transform.DOScale(1f, fadeTime).SetEase(Ease.OutBounce);
                yield return new WaitForSeconds(0.25f);
            }
        }
        public void Set_Daub_Score()
        {
            daub_scr += 100;
            Daub_Score_text.text = daub_scr.ToString();
            Daub_Time__text.text = (daub_scr / 100).ToString();
            UpdateScoreForAntiCheating();
        }
        public void Set_Bingo_Score()
        {
            StartCoroutine(UIManager.instance.Play_Anim(4));
            Bingoscr += 1000;
            Bingo_scr_count++;
            Bingo_Score_text.text = Bingoscr.ToString();
            Bingo_Time_text.text = Bingo_scr_count.ToString();
        }
        public void Set_Multi_Bingo(int multi_time)
        {
            StartCoroutine(UIManager.instance.Play_Anim(4));
            multi_bingotime++;
            Total_time_multibingo += multi_time;
            Multi_bingo = _bingoscr[Total_time_multibingo - 1];
            Multi_BingoScore_text.text = Multi_bingo.ToString();
            multi_bingo_timecount_text.text = "X" + Total_time_multibingo.ToString();
            Muli_bingo_time_text.text = multi_bingotime.ToString();
        }
        int Bad = 0;
        public void Bad_Daub(bool Ismessage_show,int bad_score)
        {
            if (Ismessage_show)
            {
                StartCoroutine(UIManager.instance.Play_Anim(1));
            }
            Bad++;
            Bad_daub -= bad_score;
            Bad_Daub_score_text.text = Bad_daub.ToString();
            Bad_Daub_time_text.text = (Bad).ToString();
            if (Bad > 1)
            {
                Penallety_txt.text = "Penalties";
            }
        }
        public void Miss_Daub_score()
        {
            Missdaub -= 100;
            MissDaub.text = Missdaub.ToString();
            Miss_counter.text = (Missdaub / 100).ToString();
        }
        public void GamePlay_Timer_Scrfun(float Scr, Transform Dest)
        {
            if (Scr > 0)
            {
                int Score_percentage = (int)(Scr * 100);
                Time_SaveScr += Score_percentage;
                UIManager.instance.Spawn_Text(Score_percentage, Dest);
                DOTween.To(() => temp, x => temp = x, temp + Score_percentage, 1f).SetDelay(1f).SetEase(Ease.Linear).OnUpdate(() =>
                {
                    GamePlay_fillerscr.text = ((int)temp).ToString();
                });
                timeSaver_txt_text.text = Time_SaveScr.ToString();
                Speedy_count++;
                Speedy_time_text.text = Speedy_count.ToString();
            }
        }
        public void Put_Final_Score()
        {
            final_scr = daub_scr + Bingoscr  + Time_SaveScr;
            final_scr = final_scr + Bad_daub + Missdaub;
            Score_Bonus();
            final_scr += BonusTime_Value;
            if (final_scr < 0)
            {
                final_scr = 0;
            }
            BingoPlayerScore = final_scr;
            Final_score_text.text = final_scr.ToString();
        }
        public void Submit_Score()
        {
            SoundManager.instance.Btn_clickSound();
            
#if GO4_CORE_APP
            _appBridge.SubmitScoreAndReturnToCoreApp(BingoPlayerScore);
#else
            SceneManager.LoadScene("GameScene_Bingo");
#endif
        }
        public void Score_Bonus()
        {
            BonusTime_Value = Bingo_scr_count * (int)Timer.Instance.totalTime;
            Time_BonusTxt.text = BonusTime_Value.ToString();
        }
        public void UpdateScoreForAntiCheating()
        {
            BingoPlayerScore = daub_scr + Bingoscr + Time_SaveScr;
            BingoPlayerScore = BingoPlayerScore + Bad_daub + Missdaub;
        }
    }
}