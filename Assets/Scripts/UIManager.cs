namespace Games.Bingo
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using DG.Tweening;

    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;
        public Color[] _Ball_textcolors, FillingColor;
        [SerializeField] GameObject MainMenue_Scr, IngameScr, Pause_Scr,Setting_Scr,Quit_Scr,Tut_Scr, Scr_SummaryScr;
        public GameObject Empty_Panel, Timer_Red_Img, Ten_Anim;
        public Transform[] Anim;
        [SerializeField] Transform Canvas, Rew_Text_Anim, Reward_Destination, Particle_Main;
        public Text Info_text;
        SoundManager soundManager;
        public bool IsGame_Finish = false;
        public bool isAnim = true;
        Coroutine _Cor;
        public static bool gotoUsedStatus;
        private void Awake()
        {
            instance = this;
        }


        private void Start()
        {
            soundManager = SoundManager.instance;
            Show_MainScr();
            AutoRandom.ResetSeed();
        }


        public void Show_MainScr()
        {

            En_Dis(MainMenue_Scr, true);
            En_Dis(Pause_Scr, false);
            En_Dis(Tut_Scr, false);

        }
        public void Show_scoreSummary()
        {
            En_Dis(Quit_Scr, false);
            Empty_Panel.SetActive(true);
            Pause_scr_off();
            Timer.Instance.isTime = false;
            StartCoroutine(Timer.Instance.TimeOut_Function());

        }

        public IEnumerator Show_Scrsummary()
        {
            yield return new WaitForSeconds(0f);
            soundManager.Show_Ticktick_audiosource(false);
            Timer.Instance.OnPause_Timer(false);
            IsGame_Finish = true;
            ScoreSummary.instance.Play_Anim();
            soundManager.CheersSound();
            Pause_scr_off();
            ScoreSummary.instance.Put_Final_Score();
            En_Dis(Quit_Scr, false);
            En_Dis(Scr_SummaryScr, true);
            Timer.Instance.isTime = false;

        }
        public void Show_PauseScr()
        {
           
            En_Dis(Setting_Scr, false);
            En_Dis(Quit_Scr, false);
            En_Dis(Tut_Scr, false);
            SoundManager.instance.Bg_source.Pause();
            Timer.Instance.OnPause_Timer(false);
            soundManager.Show_Ticktick_audiosource(false);
            En_Dis(Pause_Scr, true);

            if (IngameScr.activeInHierarchy)
            {
                Bingocardview.instance._Stop_Crntfiller_Tut(false);
            }

        }
        public void Pause_scr_off()
        {
            Time.timeScale = 1;
        
            Bingocardview.instance._Stop_Crntfiller_Tut(true);

            SoundManager.instance.Bg_source.Play();
            if (Timer.Instance.totalTime > 0 && Timer.Instance.totalTime < 15 && !IsGame_Finish)
            {
                soundManager.Show_Ticktick_audiosource(true);
                Timer.Instance.OnPause_Timer(true);
            }



            En_Dis(Pause_Scr, false);
        }
        public void Show_InGameScreen()
        {

            En_Dis(MainMenue_Scr, false);
            En_Dis(IngameScr, true);
        }
        public void Show_Tut_scr()
        {
            Particle_Main.gameObject.SetActive(false);
            En_Dis(Pause_Scr,  false);
            En_Dis(Tut_Scr,    true);
        }

        public void Show_Quit_Scr()
        {
            En_Dis(Pause_Scr, false);
            En_Dis(Quit_Scr, true);
        }
        public void Show_Setting_Scr()
        {
            En_Dis(Pause_Scr, false);
            En_Dis(Setting_Scr, true);
        }

        public void Taturial_Close_Dec()
        {
            if (!IngameScr.activeInHierarchy)
            {

                En_Dis(Tut_Scr, false);
            }
            else
            {
                En_Dis(Pause_Scr, true);
                En_Dis(Tut_Scr, false);

            }
        }

        public void En_Dis(GameObject scr, bool ison)
        {
            scr.SetActive(ison);
        }
        bool ismessage = true;
        public void ShowMessage(string Message)
        {
            SoundManager.instance.Play_Vibration(50);
            if (ismessage && isAnim)
            {
                isAnim = false;
                ismessage = false;
                Info_text.text = Message.ToString();
                Info_text.transform.DOScale(Vector3.one, 1.5f).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
                {
                    isAnim = true;
                    ismessage = true;
                });
            }
        }

        public void Ten_Sec_anim()
        {
            if (!Ten_Anim.activeInHierarchy)
            {
                Ten_Anim.SetActive(true);
            }
        }
        public IEnumerator Play_Anim(int indx)
        {
            if (isAnim)
            {

                isAnim = false;
                if (indx == 1)
                {
                    soundManager._Bad_sndfx();
                }

                Anim[indx].gameObject.SetActive(true);
                yield return new WaitForSeconds(1.5f);
                isAnim = true;
            }
        }




        public void Spawn_Text(float Value, Transform strt_pos)
        {
            SoundManager.instance.Play_Vibration(20);
            Transform Spwn = Instantiate(Rew_Text_Anim.transform);
            Text _text = Spwn.GetComponent<Text>();
            Spwn.SetParent(IngameScr.transform);
            Spwn.localScale = Vector3.one;

            Spwn.transform.DOScale(Spwn.transform.localScale + Vector3.one, 0.5f).SetLoops(2, LoopType.Yoyo);
            Vector3 pos = Reward_Destination.position;
            Spwn.position = strt_pos.position;
            _text.color = Color.cyan;
            _text.text = "+" + Value;
            Spwn.DOMove(pos, 0.8f).SetDelay(0.12f).OnStart(() => { soundManager.Spawn_text_Fx(); }).OnComplete(() =>
            {

                Destroy(Spwn.gameObject);
            });
        }


        public void Coloring(int indx, Text _text)
        {

            Color colorWithFullAlpha = new Color(_Ball_textcolors[indx].r, _Ball_textcolors[indx].g, _Ball_textcolors[indx].b, 1f);
            _text.color = colorWithFullAlpha;
        }


    }
}
