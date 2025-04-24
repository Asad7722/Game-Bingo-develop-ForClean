namespace Games.Bingo
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using DG.Tweening;
    public class Bingoball : MonoBehaviour
    {

        [SerializeField] Text Letter_Text, Number_Text;
        [SerializeField] Image Filler, Ball_Image;
        public int Current_No = 0;
        public Tween Filler_tween;
        public int Crnt_ball_pos = 0;
        public Animator _Anim;
        [SerializeField] bool Deb;
        Bingoball bingball;
        public bool isGen = true;
        Balltubeview ballTubeView;
        CardParent cardParent;
        [SerializeField] Image Red_Zone, FilledImage;

        int Rndm;
        Coroutine Faster_Cor, Ball_tube;
        [SerializeField] Color[] Filling_color,Filling_Color;
        private void OnEnable()
        {
            Filler.fillAmount = 1;
            isGen = true;
            ballTubeView = Balltubeview.instance;
            cardParent = CardParent.instance;
            OnStart(0.5f);
            ballTubeView.Cur_bingoball = GetComponent<Bingoball>();
            ballTubeView.Save_Active_ball.Add(ballTubeView.Cur_bingoball);
            Faster_Cor = StartCoroutine(ballTubeView.Faster_btn_Interactable(true, 1f));
            StartCoroutine(ballTubeView._Click_on());
   
            ballTubeView.Shifting_balls.Add(GetComponent<Bingoball>());
           
        }

        private void OnDisable()
        {
            ballTubeView.Shifting_balls.Remove(GetComponent<Bingoball>());
        }

        public void Golden_Ball_effect()
        {
            if (Crnt_ball_pos == 1 && gameObject.activeInHierarchy)
            {

           
                FilledImage.fillAmount = 1f;
                FilledImage.DOFillAmount(0, 1.5f).SetDelay(0.05f);
                Set_Bg(Bingocardview.instance.Power_Set_Gold_number,false);
            }

        }

        public void Instant_BallEffect(int Replace_No)
        {
           
            FilledImage.fillAmount = 1f;
            FilledImage.DOFillAmount(0, 1.5f);
            Set_Bg(Replace_No,false);
        }
        public void Miss_Ball()
        {
            SoundManager.instance.Wrong_Buzzer_Fx();
            Red_Zone.DOFade(0.7f, 0);
            Red_Zone.DOFade(0, 0.8f).SetEase(Ease.Linear);

        }

        public void On_Off_Fillertween(bool is_play)
        {
            if (Filler_tween != null)
            {
                if (is_play)
                    Filler_tween.Play();
                else
                {
                    Filler_tween.Pause();

                }
            }

        }


        private void Start()
        {
            bingball = GetComponent<Bingoball>();
        }

        public void Set_Bg(int Check_No,bool IsActive_No)
        {
            if (IsActive_No) 
            { 
            for (int i = 0; i < cardParent.All_Btns.Count; i++)
            {
                if (Check_No == cardParent.All_Btns[i].Card_No)
                {
                    ballTubeView.check_Match_No = 0;
                    goto LetsGo;
                }
                if (i == cardParent.All_Btns.Count - 1)
                {

                    ballTubeView.check_Match_No++;
                }
            }
        LetsGo:
            if (ballTubeView.check_Match_No > 3 && cardParent.All_Btns.Count > 0)
            {
                Check_No = cardParent.All_Btns[0].Card_No;

                ballTubeView.check_Match_No = 0;
            }
            }


            if (Check_No >= 0 && Check_No < 16)
            {
                Coloring(0);
                Ball_Image.sprite = ballTubeView.Ball_Sprite[0];
                Letter_Text.text = "B".ToString();
                goto Continue;
            }
            else if (Check_No >= 16 && Check_No < 31)
            {
                Coloring(1);
                Ball_Image.sprite = ballTubeView.Ball_Sprite[1];
                Letter_Text.text = "I".ToString();
                goto Continue;
            }
            else if (Check_No >= 31 && Check_No < 46)
            {
                Coloring(2);
                Ball_Image.sprite = ballTubeView.Ball_Sprite[2];
                Letter_Text.text = "N".ToString();
                goto Continue;
            }
            else if (Check_No >= 46 && Check_No < 61)
            {
                Coloring(3);
                Ball_Image.sprite = ballTubeView.Ball_Sprite[3];
                Letter_Text.text = "G".ToString();
                goto Continue;
            }
            else
            {
                Coloring(4);
                Ball_Image.sprite = ballTubeView.Ball_Sprite[4];
                Letter_Text.text = "O".ToString();
                goto Continue;
            }
        Continue:
            Number_Text.text = Check_No.ToString();
            Current_No = Check_No;
            if (IsActive_No)
            {
            CardParent.instance.CurrentNo = Current_No;
            }
            
            Bingocardview bingocardview = Bingocardview.instance;
            if (!bingocardview.IsFreedaub && !bingocardview.IsGold_Instant && !bingocardview.IsInstant3)
            {
                string Letter_voice = Letter_Text.text;
                string NO_Voice = Number_Text.text;
 }
            if (IsActive_No)
            {
             SoundManager.instance._OnBorad_snd(Current_No-1);
            }

            return;

        }
        public IEnumerator Latter_Sound(string Letter_voice,string NO_Voice)
        {
            yield return new WaitForSeconds(0.2f);
            SoundManager.instance.Speech_Sound(Letter_voice + NO_Voice, true);
        }

        public void Coloring(int indx)
        {
            Color colorWithFullAlpha = new Color(Filling_color[indx].r, Filling_color[indx].g, Filling_color[indx].b, 0.8f);
            FilledImage.color = colorWithFullAlpha;
            Color Text_Color = new Color(Filling_color[indx].r, Filling_color[indx].g, Filling_color[indx].b, 1f);
            Letter_Text.color = Text_Color;
            Number_Text.color = Text_Color;
           
            Color Filler_Color = new Color(Filling_Color[indx].r, Filling_Color[indx].g, Filling_Color[indx].b, 1f);
            Filler.color = Filler_Color;
        }

        public void OnStart(float wait)
        {
            Filler.enabled = true;
            ballTubeView.Cur_Filler = Filler;
        Repeating:

            Rndm = AutoRandom.Range(1, 76);
   

            for (int i = 0; i < ballTubeView.bingoball.Count; i++)
            {
                if (ballTubeView.bingoball[i].Current_No == Rndm)
                {
         

                    goto Repeating;
                }

            }

            for (int a = 0; a < cardParent.All_Btns_cntRemoveable.Count; a++)
            {
                if (cardParent.All_Btns_cntRemoveable[a].Card_No == Rndm && cardParent.All_Btns_cntRemoveable[a].Is_marked)
                {

                    goto Repeating;
                }

            }
            Set_Bg(Rndm,true);
            transform.localPosition = Vector3.zero;

            Filler.fillAmount = 1;
            Filler_tween = Filler.DOFillAmount(0, 5f).SetDelay(wait).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (isGen)
                {
                    ballTubeView._Gen_MoveBall();
                }
            });


        }

        public void Stop_Moving()
        {
           
            if (Filler_tween != null)
            {
                Filler_tween.Pause();
            }
        }


        public void Move_Anim()
        {
            if (gameObject.activeInHierarchy)
            {
                if (Filler_tween != null)
                {
                    if (Faster_Cor != null)
                    {


                        StopCoroutine(Faster_Cor);
                        Faster_Cor = null;
                    }

                    DOTween.Kill(Filler_tween);
                    isGen = false;
                    Filler.fillAmount = 0;
                    Filler.enabled = false;
                }

                Crnt_ball_pos++;
                _Anim.SetInteger("CurrentBallPosition", Crnt_ball_pos);
                if (Crnt_ball_pos == 5)
                {
                    transform.SetAsFirstSibling(); 
                    ballTubeView.LastExisting.Add(GetComponent<Bingoball>());
                    Crnt_ball_pos = 0;
                    Invoke("Resetp", 0.4f);

                }
            }
        }
        public void Resetp()
        {
            gameObject.SetActive(false);
            transform.position = ballTubeView._bingoSpwn_point.position;

        }

    


        public void _ShiftingElement(bool isOn)
        {
            if (!isOn)
            {
                _Anim.enabled = false;
                transform.DOScale(Vector3.zero, 0f);
            }
            else
            {
              
                transform.DOScale(new Vector3(0.6f, 0.6f, 0.6f), 0.5f).SetDelay(Random.Range(0.1f,0.25f)).SetEase(Ease.OutBack).OnComplete(()=> {

                    _Anim.enabled = true;
                    _Anim.SetInteger("CurrentBallPosition", Crnt_ball_pos);
                } );
            }
        }
        
       


        public void Add_No_Anim()
        {
            transform.localScale = new Vector3(1.2f,1.2f,1.2f);
            transform.DOScale(new Vector3(0.6f, 0.6f, 0.6f), 0.4f).OnComplete(() => {

                _Anim.enabled = true;
                _Anim.SetInteger("CurrentBallPosition", Crnt_ball_pos);

            });
          

        }
    }
}
