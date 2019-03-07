using System;
using UnityEngine;
using UnityEngine.UI;

namespace TankBattle
{
    public class TankBlood : MonoBehaviour
    {
        
        public Slider m_Slider;                             // The slider to represent how much health the tank currently has.
        public Image m_FillImage;                           // The image component of the slider.
        public Color m_FullHealthColor = Color.green;       // The color the health bar will be when on full health.
        public Color m_ZeroHealthColor = Color.red;         // The color the health bar will be when on no health.
        public GameObject m_ExplosionPrefab;                // A prefab that will be instantiated in Awake, then used whenever the tank dies.
        
        private float m_StartingHealth;               // The amount of health each tank starts with.
        private AudioSource m_ExplosionAudio;               // The audio source to play when the tank explodes.
        private ParticleSystem m_ExplosionParticles;        // The particle system the will play when the tank is destroyed.
        private float m_CurrentHealth;                      // How much health the tank currently has.
        private bool m_Dead;                                // Has the tank been reduced beyond zero health yet?

        private Boolean IsDefended = false;
        
        
        
        // 第一步被调用，Initialization模块
        private void Awake()
        {
            
            
        }
    }
}