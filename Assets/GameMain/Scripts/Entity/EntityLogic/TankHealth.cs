using System;
using System.Collections;
using GameFramework.DataTable;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace TankBattle
{
    /// <summary>
    /// 装甲类
    /// </summary>
    public class TankHealth : MonoBehaviour
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
        
        private void Awake ()
        {
            // 初始化坦克的机动性相关属性
            IDataTable<DRTank> dtEntity = GameEntry.DataTable.GetDataTable<DRTank>();
            DRTank drEntity = dtEntity.GetDataRow(1);
            m_StartingHealth = drEntity.StartingHealth;
            
            // Instantiate the explosion prefab and get a reference to the particle system on it.
            m_ExplosionParticles = Instantiate (m_ExplosionPrefab).GetComponent<ParticleSystem> ();

            // Get a reference to the audio source on the instantiated prefab.
            m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource> ();

            // Disable the prefab so it can be activated when it's required.
            m_ExplosionParticles.gameObject.SetActive (false);
        }

        // 坦克被重置时，会触发该方法
        private void OnEnable()
        {
            IsDefended = false;
            // When the tank is enabled, reset the tank's health and whether or not it's dead.
            m_CurrentHealth = m_StartingHealth;
            m_Dead = false;
            
            StartCoroutine(HasDefended());
        }

        
        private IEnumerator HasDefended()
        {
            // 三秒无敌时间，再开始加载血条
            yield return new WaitForSeconds(6.0f);
            
            IsDefended = true;
            // Update the health slider's value and color.
            SetHealthUI();
        }

        public void TakeDamage (float amount)
        {
//            Log.Debug("isDefended: " + IsDefended);
            if (IsDefended)
            {
                // Reduce current health by the amount of damage done.
                m_CurrentHealth -= amount;
    
                // Change the UI elements appropriately.
                SetHealthUI();
    
                // If the current health is at or below zero and it has not yet been registered, call OnDeath.
                if (m_CurrentHealth <= 0f && !m_Dead)
                {
                    OnDeath();
                }
            }
        }


        private void SetHealthUI ()
        {
            if (IsDefended)
            {
                // Set the slider's value appropriately.
                m_Slider.value = m_CurrentHealth;

                // Interpolate the color of the bar between the choosen colours based on the current percentage of the starting health.
                m_FillImage.color =
                    Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
                
//                Log.Debug("FillImage: " + m_CurrentHealth / m_StartingHealth);
            }
        }


        private void OnDeath ()
        {
            // Set the flag so that this function is only called once.
            m_Dead = true;

            // Move the instantiated explosion prefab to the tank's position and turn it on.
            m_ExplosionParticles.transform.position = transform.position;
            m_ExplosionParticles.gameObject.SetActive (true);

            // Play the particle system of the tank exploding.
            m_ExplosionParticles.Play ();

            // Play the tank explosion sound effect.
            m_ExplosionAudio.Play();

            // Turn the tank off.
            gameObject.SetActive (false);
        }
    }
}