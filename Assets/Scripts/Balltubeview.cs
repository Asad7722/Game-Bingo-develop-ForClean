namespace Games.Bingo
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using DG.Tweening;
    public class Balltubeview : MonoBehaviour
    {
        public static Balltubeview instance;

        public Sprite[] Ball_Sprite;
        public List<Bingoball> bingoball, LastExisting, Save_Active_ball;
        [SerializeField] GameObject Bingo_Ball;
        public Transform _bingoSpwn_point, _bingoparent;
        public Bingoball Cur_bingoball;
        public Button Faster_Btn, Bingo_Btn;
        public Image Cur_Filler;
        public int check_Match_No = 0;
        public List<Bingoball> Shifting_balls;

        public bool Is_click = false;
        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            onstart();
            Create_Balls();
            Invoke("_Gen_MoveBall", 2.7f);
        }

        public void Create_Balls()
        {

            for (int i = 0; i < 6; i++)
            {

                Transform _ball = Instantiate(Bingo_Ball).transform;
                _ball.name = "BingoBallView " + i;
                _ball.SetParent(transform);
                _ball.localPosition = _bingoSpwn_point.localPosition; 
                _ball.localScale = Vector3.one;
                _ball.gameObject.SetActive(false);
                bingoball.Add(_ball.GetComponent<Bingoball>());
                _ball.SetParent(_bingoparent);
            } 
        }

        public IEnumerator _Click_on()
        {
            yield return new WaitForSeconds(0.9f);
            Is_click = true;
        }
        public void _Gen_MoveBall()
        { 
            if (!UIManager.instance.IsGame_Finish && Faster_Btn.isActiveAndEnabled)
            { 
                Bingo_Btn.interactable = true;
                Faster_Btn.interactable = false;
                if (LastExisting.Count <= 0)
                {

                    if (Cur_bingoball == null)
                    {
                        Is_click = false;
                        bingoball[0].gameObject.SetActive(true);

                    }
                    else
                    {
                        int index = bingoball.IndexOf(Cur_bingoball);
                        for (int i = 0; i <= index; i++)
                        {
                            bingoball[i].Move_Anim();
                        } 
                        Is_click = false;

                        index++;
                        bingoball[index].gameObject.SetActive(true);

                    }
                }
                else
                {
                      int LastEx_index = bingoball.IndexOf(LastExisting[0]);
                    for (int i = 0; i < bingoball.Count; i++)
                    {
                        if (i != LastEx_index)
                        {
                            bingoball[i].Move_Anim();

                        }
                    }
                    Is_click = false;
                    LastExisting[0].gameObject.SetActive(true);
                    LastExisting.RemoveAt(0);

                }
            }
        }

        public void Faster_Move(bool isShowMessage)
        {
            if (Timer.Instance.totalTime > 0)
            {
                StartCoroutine(Faster_btn_Interactable(false, 0f));
                if (Cur_Filler != null && Cur_Filler.fillAmount > 0.05f)
                {
                    CardParent.instance.Checkif_Miss(isShowMessage);
                    _Gen_MoveBall();
                }
            }
        }
        public IEnumerator Faster_btn_Interactable(bool Ison, float time)
        {
            yield return new WaitForSeconds(time);
            Faster_Btn.interactable = Ison;
        }
        public void onstart()
        {
            Faster_Btn.interactable = false;
            Bingo_Btn.interactable = false;
        }

        public void Replacing_Balls()
        {
            for (int i = 0; i < Shifting_balls.Count; i++)
            {

                int fetching_ball_crntPos = Shifting_balls[i].Crnt_ball_pos;
                if (fetching_ball_crntPos == 5 || fetching_ball_crntPos == 4 ||
                   fetching_ball_crntPos == 3 || fetching_ball_crntPos == 2)
                {
                    int Number = Shifting_balls[i + 1].Current_No;
                    
                    Shifting_balls[i].Set_Bg(Number, false);
                    Shifting_balls[i]._ShiftingElement(false);
                }else if(fetching_ball_crntPos == 1)
                {
                    Shifting_balls[i]._ShiftingElement(false);
                 }
            }
        }
        public void Replacing_Instant()
        {
            for (int i = 0; i < Shifting_balls.Count; i++)
            {

                int fetching_ball_crntPos = Shifting_balls[i].Crnt_ball_pos;
                if (fetching_ball_crntPos == 5)
                {
                    int Number = Shifting_balls[i + 3].Current_No;
                  
                    Shifting_balls[i].Set_Bg(Number, false);
                    Shifting_balls[i]._ShiftingElement(false);

                }
                else if (fetching_ball_crntPos == 4)
                {
                    int Number = Shifting_balls[i + 3].Current_No;
                   
                    Shifting_balls[i].Set_Bg(Number, false);
                    Shifting_balls[i]._ShiftingElement(false);
                 

                }
            if(fetching_ball_crntPos==1 || fetching_ball_crntPos == 2 || fetching_ball_crntPos == 3)
                {
                    Shifting_balls[i]._ShiftingElement(false);
                }   

            }
            

        }


        public void PowerUp_Replace_ANimation(bool IsGolden)
        {
            if (IsGolden)
            {
                for (int i = 0; i < Shifting_balls.Count; i++)
                {

                    int fetching_ball_crntPos = Shifting_balls[i].Crnt_ball_pos;
                    if (fetching_ball_crntPos == 5 || fetching_ball_crntPos == 4 ||
                       fetching_ball_crntPos == 3 || fetching_ball_crntPos == 2)
                    {
                       
                        Shifting_balls[i]._ShiftingElement(true);
                    }
                    if (fetching_ball_crntPos == 1)
                    {
                       
                        Shifting_balls[i].Add_No_Anim();
                    }


                }
            }
            else
            {
               StartCoroutine( InStant_PowerUP_Replacement());
                for (int i = 0; i < Shifting_balls.Count; i++)
                {

                    int fetching_ball_crntPos = Shifting_balls[i].Crnt_ball_pos;
                    if (fetching_ball_crntPos == 5)
                    {

                        Shifting_balls[i]._ShiftingElement(true);
                    }
                    else if (fetching_ball_crntPos == 4)
                    {
                        Shifting_balls[i]._ShiftingElement(true);
                

                    }

                   
                }

            }

        }

        public IEnumerator InStant_PowerUP_Replacement()
        {
            for (int i = 0; i < Shifting_balls.Count; i++)
            {

                int fetching_ball_crntPos = Shifting_balls[i].Crnt_ball_pos;
                
                if (fetching_ball_crntPos == 1 || fetching_ball_crntPos == 2 || fetching_ball_crntPos == 3)
                {
                    Shifting_balls[i].Add_No_Anim();
                }
            yield return new WaitForSeconds(0.1f);
            }

        } 

        public void Golden_Bingo_effect()
        {
            for (int i = 0; i < bingoball.Count; i++)
            {
                bingoball[i].Golden_Ball_effect();
            }
        }
        public void Instant_Powerup_effect()
        {

        }
        public void Stop_Ball_Working()
        {
            for (int i = 0; i < bingoball.Count; i++)
            {
                bingoball[i].Stop_Moving();
            }

        }
    }
}
