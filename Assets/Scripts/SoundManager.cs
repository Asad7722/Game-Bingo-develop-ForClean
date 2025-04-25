namespace Games.Bingo
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;
        public Slider Music_slider, Sound_slider, Voiceover_slider;
        public AudioSource Ticktick_source, Bg_source, audioSource, voicer_AudioSource, faster_source, Last_chance_audiosource;
        public AudioClip cherrs_sound, One_Ring, Ring_bell, Active, _tile_success, Wrng_tile, Btn_click, Wrong_Buzzer, Spawn_text,
        Ready_steady,_miss,Bad,TimesUp;
        public AudioClip[] _Board_Sound;
        public AudioClip[] _powerup_snd,_Bingo_snd;
        private bool isVibrationEnabled;
        public Sprite VibrationOnImage, VibrationOffImage;
        public Image vibrationToggleImage;
        private void Awake()
        {
            instance = this;
        }
        private void Start()
        {
            At_startSnd();
            UpdateVibrationUI();
        }
        public void Play_Vibration(int _value)
        {
#if UNITY_ANDROID || UNITY_IOS
            if (isVibrationEnabled)
            {
                int _Value = _value;
                Vibration.Vibrate(_Value);
            }
#endif
        }
        public void ToggleVibration()
        {
            isVibrationEnabled = !isVibrationEnabled;
            JsonPrefs.SetInt("Vibration", isVibrationEnabled ? 1 : 0);
            UpdateVibrationUI();
        }
        private void UpdateVibrationUI()
        {
            vibrationToggleImage.sprite = isVibrationEnabled?VibrationOnImage:VibrationOffImage;
        }
        public void At_startSnd()
        {
            if (JsonPrefs.GetInt("FRST") == 0)
            {
                JsonPrefs.SetFloat("Sound", 1f);
                JsonPrefs.SetFloat("Music", 1f);
                JsonPrefs.SetFloat("Voicer", 1f);
                JsonPrefs.SetInt("Vibration", 1);
                JsonPrefs.SetInt("FRST", 1);
            }
            isVibrationEnabled = JsonPrefs.GetInt("Vibration") == 1;
            audioSource.volume = JsonPrefs.GetFloat("Sound");
            Bg_source.volume = JsonPrefs.GetFloat("Music");
            voicer_AudioSource.volume = JsonPrefs.GetFloat("Voicer");
            Last_chance_audiosource.volume = JsonPrefs.GetFloat("Sound");
            Music_slider.value = JsonPrefs.GetFloat("Music");
            Sound_slider.value = JsonPrefs.GetFloat("Sound");
            Voiceover_slider.value = JsonPrefs.GetFloat("Voicer");
        }
        public void _OnBorad_snd(int no)
        {
            voicer_AudioSource.PlayOneShot(_Board_Sound[no]);
        }
        public void _Readysteadyfx()
        {
            voicer_AudioSource.PlayOneShot(Ready_steady);
        }
        public void _PowerUp(int i)
        {
            voicer_AudioSource.PlayOneShot(_powerup_snd[i]);
        } public void _Bingo_sndfx(int i)
        {
            voicer_AudioSource.PlayOneShot(_Bingo_snd[i]);
        }public void _Miss_sndfx()
        {
            voicer_AudioSource.PlayOneShot(_miss);
        }public void _TimesUp_sndfx()
        {
            voicer_AudioSource.PlayOneShot(TimesUp);
        }public void _Bad_sndfx()
        {
            voicer_AudioSource.PlayOneShot(Bad);
        }
        private void Update()
        {
            Bg_source.volume = Music_slider.value;
            voicer_AudioSource.volume = Voiceover_slider.value;
            audioSource.volume = Sound_slider.value;
            Last_chance_audiosource.volume = Sound_slider.value;
        }
        public void Slider_ValuesUpdation(int No)
        {
            if (No == 0)
            {
                JsonPrefs.SetFloat("Sound", Sound_slider.value);
            }
            else if (No == 1)
            {
                JsonPrefs.SetFloat("Voicer", Voiceover_slider.value);
            }
            else
            {
                JsonPrefs.SetFloat("Music", Music_slider.value);
            }
        }
        public void Speech_Sound(string Sount_info, bool isLaststop)
        {
            if (voicer_AudioSource.volume > 0)
            {
                if (isLaststop)
                {
                    EasyTTSUtil.StopSpeech();
                }
                EasyTTSUtil.SpeechAdd(Sount_info, voicer_AudioSource.volume, 0.5f, voicer_AudioSource.pitch);
            }
        }
        public void CheersSound()
        {
            audioSource.PlayOneShot(cherrs_sound);
        }
        public void Btn_clickSound()
        {
            audioSource.PlayOneShot(Btn_click);
        }
        public void Mul_Ring_Fx()
        {
            audioSource.PlayOneShot(Ring_bell);
        }
        public void Wrong_Buzzer_Fx()
        {
            audioSource.PlayOneShot(Wrong_Buzzer);
            _Miss_sndfx();
        }
        public void Daub_Active_Fx()
        {
            audioSource.PlayOneShot(Active);
        }
        public void Daub_Success_Fx()
        {
            audioSource.PlayOneShot(_tile_success);
        } public void Spawn_text_Fx()
        {
            audioSource.PlayOneShot(Spawn_text);
        }
        public void OneRing_Fx()
        {
            audioSource.PlayOneShot(One_Ring);
        }
        public void Wrng_tile_Fx()
        {
            audioSource.PlayOneShot(Wrng_tile);
        }
        public void Show_Ticktick_audiosource(bool isenable)
        {
            Ticktick_source.enabled = isenable;
        }
    }
}