namespace Games.Bingo
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using DG.Tweening;

    public class Bingocardview : MonoBehaviour
    {
        public static Bingocardview instance;
        [SerializeField] Sprite[] Dubs_Sprites;
        [SerializeField] Image[] Dubs_Filler;
        public List<Button> power_dubs;
        public GameObject[] Powerups_Panel;
        public List<int> _instant_card_Nos;
        public Image Current_Filler;

        [SerializeField] GameObject power_ups_parent, GoldenSlection_scr;
        [SerializeField] Transform Upper_Senario;
        [SerializeField] Button Dub_SpwnBtn;

        public bool IsGold_Instant, IsFreedaub, IsInstant3;
        public int Power_Set_Gold_number;
        public float Filling_Bar_power;
        public Image GoldenDemo;
        public Text timeLetf;
        public float totalTime;
        public bool checkforPowerup = false;
        bool Not_Check = false;
        ScoreSummary scoreSummary;
        SoundManager soundManager;
        UIManager uIManager;

        Timer timer;
        private void Awake()
        {
            instance = this;

        }
        private void OnEnable()
        {
            Current_Filler = Dubs_Filler[0];
        }
        private void Start()
        {
            scoreSummary = ScoreSummary.instance;
            soundManager = SoundManager.instance;
            uIManager = UIManager.instance;
            timer = Timer.Instance;
        }
    
        public IEnumerator Update_Filler(bool IsIncrease)
        {
            yield return new WaitForSeconds(0.2f);

            int indx = Array.IndexOf(Dubs_Filler, Current_Filler);
            if (IsIncrease)
            {

                if (indx < Dubs_Filler.Length - 1)
                {
                    indx++;
                }
                Current_Filler = Dubs_Filler[indx];
            }
            else
            {


                Not_Check = true;
                for (int i = 0; i < Dubs_Filler.Length - 1; i++)
                {
                   
                    if (Dubs_Filler[i].transform.childCount == 0)
                    {

                        if (Not_Check && Dubs_Filler[i].fillAmount == 0 && Dubs_Filler[i].transform.childCount == 0
                        && Dubs_Filler[i + 1].fillAmount == 0 && Dubs_Filler[i + 1].transform.childCount == 0)
                        {
                            Not_Check = false;
                            Current_Filler = Dubs_Filler[i];
                            goto Gooo;
                        }
                        else
                    if (Dubs_Filler[i + 1].transform.childCount != 0)
                        {
                            Not_Check = false;

                            Transform ob = Dubs_Filler[i + 1].transform.GetChild(0);
                            ob.SetParent(Dubs_Filler[i].transform);
                            ob.DOLocalMove(Vector3.zero, 0f);
                            Current_Filler = Dubs_Filler[i + 1];

                        }
                        else if (Dubs_Filler[i + 1].fillAmount != 0)
                        {
                            Not_Check = false;
                            Dubs_Filler[i].fillAmount = Dubs_Filler[i + 1].fillAmount;
                            Dubs_Filler[i + 1].fillAmount = 0;
                            Current_Filler = Dubs_Filler[i];
                        }
                        else
                        {


                        }
                        if (Current_Filler.fillAmount >= 1)
                        {
                            Current_Filler.fillAmount = 0;
                        }

                    }

                }
            Gooo:
                UIManager.gotoUsedStatus = true;

            }
        }

        public void GamePlay_Scr(float Scr, Transform Dest)
        {
            Filling_Bar_power = 0f;


            if (Scr >= 0.65f)
            {
                Filling_Bar_power = 0.75f;

            }
            else if (Scr >= 0.35f)
            {
                Filling_Bar_power = 0.5f;

            }
            else if (Scr < 0.35f && Scr > 0.02f)
            {
                Filling_Bar_power = 0.25f;

            }
            else
            {
                Filling_Bar_power = 0f;

            }

            Current_Filler.DOFillAmount(Current_Filler.fillAmount + Filling_Bar_power, 0.1f).OnComplete(() =>
            {
                if (Current_Filler.fillAmount >= 1f)
                {


                    Dubs_Imp();
                }


            });



            scoreSummary.GamePlay_Timer_Scrfun(Scr, Dest);


        }




        public void Dubs_Imp()
        {
            if (timer.totalTime > 0)
            {
                if (power_dubs.Count < 3)
                {
                    StartCoroutine(UIManager.instance.Play_Anim(2));
                    int randm = AutoRandom.Range(0, Dubs_Sprites.Length);
                    Button SpawnBtn = Instantiate(Dub_SpwnBtn);
                    SpawnBtn.transform.SetParent(Current_Filler.transform);
                    power_dubs.Add(SpawnBtn);
                    SpawnBtn.transform.localPosition = Vector3.zero;

                    SpawnBtn.transform.localScale = new Vector3(-1.3f, 1.3f, 1.3f);
                    SpawnBtn.transform.DOScale(new Vector3(-1f, 1f, 1f), 0.3f).SetEase(Ease.OutBack);
                    SpawnBtn.GetComponent<Image>().sprite = Dubs_Sprites[randm];

                    if (randm == 0) { SpawnBtn.onClick.AddListener(() => GoldenBall(SpawnBtn.gameObject)); }
                    else if (randm == 1) { SpawnBtn.onClick.AddListener(() => freeDaub(SpawnBtn.gameObject)); }
                    else if (randm == 2) { SpawnBtn.onClick.AddListener(() => Get_3Balls_PowerUPs(SpawnBtn.gameObject)); }
                    else { SpawnBtn.onClick.AddListener(Time_Inc_powerup); }


                    SpawnBtn.onClick.AddListener(() => Destroy_DubObj(SpawnBtn.gameObject));
                    StartCoroutine(Update_Filler(true));

                }
            }
        }
    
        public void Destroy_DubObj(GameObject ob)
        {
            if (timer.totalTime > 0)
            {
                soundManager.Btn_clickSound();
                int indx = power_dubs.IndexOf(ob.GetComponent<Button>());
                Dubs_Filler[indx].fillAmount = 0;
                power_dubs.Remove(power_dubs[indx]);
                Destroy(ob);
            }
        }
        public void _Stop_Crntfiller_Tut(bool isOn)
        {
            
            _Current_filler_update(isOn);
            Timer.Instance.isTime = isOn;
        }

        public void _Current_filler_update(bool IsRun)
        {


            Bingoball _current_ball = Balltubeview.instance.Cur_Filler.transform.parent.GetComponent<Bingoball>();
            _current_ball.On_Off_Fillertween(IsRun);
        }
   public void Time_Inc_powerup()
        {

            if (timer.totalTime > 0)
            {

                if (timer.totalTime > 0)
                {
                    uIManager.Ten_Sec_anim();
                    timer.totalTime += 10f;
                    StartCoroutine(Update_Filler(false));
                    if (timer.totalTime > 15)
                    {

                        timer.Timer_Powerup();
                    }
                }
                else
                {
                    StartCoroutine(Update_Filler(false));
                }
            }

        }

      
        public void GoldenBall(GameObject ob)
        {
            if (timer.totalTime > 0)
            {
                timer.Timer_Powerup();

                if (CardParent.instance.All_Btns.Count > 3)
                {

                    
                    Balltubeview.instance.Replacing_Balls();
                    SoundManager.instance._PowerUp(1);
                    totalTime = 8;
                    _Current_filler_update(false);
                    IsGold_Instant = true;
                      timeLetf.transform.parent.localScale = new Vector3(0.4f, 0.4f, 0.4f);

                    GoldenSlection_scr.gameObject.SetActive(true);

                    StartCoroutine(Update_Filler(false));

                }
                else
                {
                    uIManager.ShowMessage("Can't Use");

                    StartCoroutine(Update_Filler(false));

                }
            }



        }
   public void freeDaub(GameObject ob)
        {


            timer.Timer_Powerup();

            if (timer.totalTime > 0)
            {
                if (CardParent.instance.All_Btns.Count >= 1)
                {
                    SoundManager.instance._PowerUp(0);
                    totalTime = 8;
                    _Current_filler_update(false);
                    timeLetf.transform.parent.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    IsFreedaub = true;

                    Power_ups_panles(1);
                   
                    StartCoroutine(Update_Filler(false));
                 
                }
                else
                {
                    uIManager.ShowMessage("Can't Use");
                    StartCoroutine(Update_Filler(false));

                }
            }

        }
         public void Get_3Balls_PowerUPs(GameObject ob)
        {

            timer.Timer_Powerup();
            if (timer.totalTime > 0)
            {
                if (CardParent.instance.All_Btns.Count > 3)
                {
                    Balltubeview.instance.Replacing_Instant();
                 
                    SoundManager.instance._PowerUp(2);
                    totalTime = 6;
                    _Current_filler_update(false);
  timeLetf.transform.parent.localScale = Vector3.zero;
                    IsInstant3 = true;
                    Power_ups_panles(2);
                    StartCoroutine(Update_Filler(false));
                }
                else
                {
                    uIManager.ShowMessage("Can't Use");
                    StartCoroutine(Update_Filler(false));
                }
            }

        }


        public void Reset_PowerUPs()
        {
            IsGold_Instant = false;
            Power_Set_Gold_number = 0;
        }


        private void Update()
        {
            if (IsGold_Instant || IsFreedaub || IsInstant3)
            {
                if (totalTime > 0)
                {
                    totalTime -= Time.deltaTime;
                    timeLetf.text = totalTime.ToString();
                    UpdateTImer(totalTime);

                }
                else
                {
                    if (IsGold_Instant)
                    {
                        Balltubeview.instance.Golden_Bingo_effect();
                    }
                    else if (IsInstant3)
                    {
                        Balltubeview.instance.PowerUp_Replace_ANimation(false);
                    }
                   StartCoroutine(ExitPowerUP(0.95f));

                }
            }

        }
        public IEnumerator ExitPowerUP(float Waitingtime)
        {
            uIManager.Empty_Panel.SetActive(true);
            IsGold_Instant = false;
            IsFreedaub = false;
            IsInstant3 = false;
            power_ups_parent.SetActive(false);
            Power_ups_panles(-1);
            totalTime = 8;
        
            yield return new WaitForSeconds(Waitingtime);
            uIManager.Empty_Panel.SetActive(false);
            _Current_filler_update(true);

        }

        public void Power_ups_panles(int indx)
        {
            for (int i = 0; i < Powerups_Panel.Length; i++)
            {
                Powerups_Panel[i].SetActive(false);
            }
            if (indx == 0)
            {
                GoldenSlection_scr.SetActive(false);
            }

            if (indx >= 0)
            {
                power_ups_parent.SetActive(true);
                Powerups_Panel[indx].SetActive(true);
            }
            else
            {
                power_ups_parent.SetActive(false);

            }
        }
 void UpdateTImer(float currentTime)
        {
            currentTime += 1;
            float minutes = Mathf.FloorToInt(currentTime / 60);
            float seconds = Mathf.FloorToInt(currentTime % 60);
            timeLetf.text = string.Format("{01:00}", minutes, seconds);
        }


    }
}
