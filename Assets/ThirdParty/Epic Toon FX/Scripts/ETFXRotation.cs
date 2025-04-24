using UnityEngine;
using System.Collections;
using Games.Bingo;
namespace EpicToonFX
{
    public class ETFXRotation : MonoBehaviour
    {
        
        [Header("Rotate axises by degrees per second")]
        public Vector3 rotateVector = Vector3.zero;

        public enum spaceEnum { Local, World };
        public spaceEnum rotateSpace;
        ParticleSystem particle;
        Bingocardview bingocardview;

      
 
        // Use this for initialization
        void Start()
        {
            particle = GetComponent<ParticleSystem>();
            bingocardview = Bingocardview.instance;
        }
 
        // Update is called once per frame
        void Update()
        {
            if (particle.isPlaying && bingocardview.IsGold_Instant)
            {
                particle.gameObject.SetActive(false);
            
            }


            if (rotateSpace == spaceEnum.Local)
                transform.Rotate(rotateVector * Time.deltaTime);
            if (rotateSpace == spaceEnum.World)
                transform.Rotate(rotateVector * Time.deltaTime, Space.World);
        }
    }
}