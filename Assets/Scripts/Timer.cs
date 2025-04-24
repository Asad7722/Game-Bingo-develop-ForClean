namespace Games.Bingo
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using DG.Tweening;
    using BrilliantBingo.Code.Infrastructure.Views;
    using CodeStage.AntiCheat.ObscuredTypes;

    public class Timer : MonoBehaviour
    {
        public static Timer Instance;
        public bool isTime;
        public ObscuredFloat totalTime;
        public Text timeLetf;
       public bool LessFive = true, Red_on = false;
        public GameObject Red_Image;
        UIManager uIManager;
        SoundManager soundManager;
        [SerializeField] DOTweenAnimation Clock_Anim;
        Bingocardview bingocardview;
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            uIManager = UIManager.instance;
            soundManager = SoundManager.instance;
            bingocardview = Bingocardview.instance;
        }
        void Update()
        {
            if (isTime)
            {
                if (totalTime > 0)
                {
                    totalTime -= Time.deltaTime;
                    timeLetf.text = totalTime.ToString();
                    UpdateTImer(totalTime);
                }
                else if (totalTime <= 0 && !Bingocardview.instance.IsGold_Instant && !Bingocardview.instance.IsFreedaub
                       && !Bingocardview.instance.IsInstant3)
                {
                    Balltubeview.instance.Stop_Ball_Working();
                    StartCoroutine(uIManager.Play_Anim(3));
                    soundManager.Show_Ticktick_audiosource(false);
                    StartCoroutine(Time_Out());
                    isTime = false;
                }
                if (totalTime <= 15f && !Bingocardview.instance.IsGold_Instant && !Bingocardview.instance.IsFreedaub
                       && !Bingocardview.instance.IsInstant3 && !Red_on)
                {
                    soundManager.Ticktick_source.enabled = true;
                    Red_on = true;
                    OnPause_Timer(true);
                }
            }
        }
        public IEnumerator Time_Out()
        {
            soundManager._TimesUp_sndfx();
            yield return new WaitForSeconds(2f);
            uIManager.Empty_Panel.SetActive(true);
 StartCoroutine(TimeOut_Function());
        }
        public void OnPause_Timer(bool isOn)
        {
            if (totalTime <= 15 || totalTime <= 0)
            {
                if (isOn)
                {
                    Clock_Anim.DORestart();
                    Red_Image.SetActive(true);
                }
                else
                {
                    Clock_Anim.DOPause();
                    Red_Image.SetActive(false);
                }
                soundManager.Last_chance_audiosource.enabled = isOn;
            }
        }
        public void Timer_Powerup()
        {
            Red_on = false;
            Clock_Anim.DOPause();
            Red_Image.SetActive(false);
            soundManager.Last_chance_audiosource.enabled = false;
            soundManager.Ticktick_source.enabled = false;
        }
        void UpdateTImer(float currentTime)
        {
            currentTime += 1;
            float minutes = Mathf.FloorToInt(currentTime / 60);
            float seconds = Mathf.FloorToInt(currentTime % 60);
            timeLetf.gameObject.GetComponent<Text>().text = string.Format("{00:00}:{01:00}", minutes, seconds);
        }
        public IEnumerator TimeOut_Function()
        {
            soundManager.Mul_Ring_Fx();
            yield return new WaitForSeconds(0);
            StartCoroutine(CardParent.instance.Time_Out());
            yield return new WaitForSeconds(2.5f);
        }
        public IEnumerator End_Game()
        {
            yield return new WaitForSeconds(0);
            isTime = false;
            StartCoroutine(CardParent.instance.Time_Out());
        }
        public IEnumerator Show_Score_Summary()
        {
            yield return new WaitForSeconds(1f);
            uIManager.Empty_Panel.gameObject.SetActive(false);
        }
    }
}