//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace TankBattle
{
    /// <summary>
    /// 武器类。具有射击动作
    ///     可以实现武器挂载在player身上
    ///     武器可以实例炮弹对象，并赋予炮弹的发射方向
    /// </summary>
    public class TankWeapon : Entity
    {
        public Slider m_AimSlider;                  // 坦克实体的子对象,用来显示蓄力进度条
        public Rigidbody m_Shell;                   // Prefab of the shell.

//        public Transform m_FireTransform; 
        private const string m_FireAttachPoint = "Weapon Point";    // 坦克实体的子对象,用来表示炮弹产生的地方
        
        /// <summary> 这三个音频资源暂时按照拖拽方式加载
        public AudioSource m_ShootingAudio;         // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.
        public AudioClip m_ChargingClip;            // Audio that plays when each shot is charging up.
        public AudioClip m_FireClip;                // Audio that plays when each shot is fired.
        /// <summary>
        
        [SerializeField] private WeaponData m_WeaponData = null;

        private float m_NextAttackTime = 0f;
        private string m_FireButton;                // The input axis that is used for launching shells.
        private float m_CurrentLaunchForce;         // The force that will be given to the shell when the fire button is released.
        private float m_ChargeSpeed;                // How fast the launch force increases, based on the max charge time.
        private bool m_Fired;                       // Whether or not the shell has been launched with this button press.
        private Transform fireTransform;

        // 第一步被调用，Initialization模块
        private void OnEnable()
        {
            // When the tank is turned on, reset the launch force and the UI
            m_CurrentLaunchForce = m_WeaponData.MinLaunchForce;
            m_AimSlider.value = m_WeaponData.MinLaunchForce;
        }

        // 第二步被调用, Initialization模块
        private void Start()
        {
                
            fireTransform = GetComponent<Transform>().Find("Weapon Point");

            // The fire axis is Fire button.
            m_FireButton = "Fire";

            // The rate that the launch force charges up is the range of possible forces by the max charge time.
            m_ChargeSpeed = (m_WeaponData.MaxLaunchForce - m_WeaponData.MinLaunchForce) / m_WeaponData.MaxChargeTime;
            
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        /// <summary>
        /// 附加子实体
        /// </summary>
        /// <param name="userData">WeaponData</param>
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_WeaponData = userData as WeaponData;
            if (m_WeaponData == null)
            {
                Log.Error("Weapon data is invalid.");
                return;
            }

            GameEntry.Entity.AttachEntity(Entity, m_WeaponData.OwnerId, m_FireAttachPoint);
        }

        /// <summary>
        /// 将武器实体附加到父实体上
        /// </summary>
        /// <param name="parentEntity">父实体</param>
        /// <param name="parentTransform">父实体位置</param>
        /// <param name="userData">用户数据</param>
        protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);

            Name = Utility.Text.Format("Weapon of {0}", parentEntity.Name);
            CachedTransform.localPosition = Vector3.zero;
        }

        
        private void Update ()
        {
            // The slider should have a default value of the minimum launch force.
            m_AimSlider.value = m_WeaponData.MinLaunchForce;

            // If the max force has been exceeded and the shell hasn't yet been launched...
            if (m_CurrentLaunchForce >= m_WeaponData.MaxLaunchForce && !m_Fired)
            {
                // ... use the max force and launch the shell.
                m_CurrentLaunchForce = m_WeaponData.MaxLaunchForce;
                Fire ();
            }
            // Otherwise, if the fire button has just started being pressed...
            else if (Input.GetButton (m_FireButton) || ETCInput.GetButtonDown (m_FireButton))
            {
                FireButtonDown();
            }
            // Otherwise, if the fire button is being held and the shell hasn't been launched yet...
            else if (Input.GetButton (m_FireButton) || ETCInput.GetButton (m_FireButton) && !m_Fired)
            {
                FireButton();
            }
            // Otherwise, if the fire button is released and the shell hasn't been launched yet...
            else if (Input.GetButton (m_FireButton) || ETCInput.GetButtonUp (m_FireButton) && !m_Fired)
            {
                // ... launch the shell.
                Fire ();
            }
        }
        
        
        /// 将按键开火分模块来区分 ///
        
        // 开火按键按下
        // if the fire button has just started being pressed...
        public void FireButtonDown()
        {
            // ... reset the fired flag and reset the launch force.
            m_Fired = false;
            m_CurrentLaunchForce = m_WeaponData.MinLaunchForce;

            // Change the clip to the charging clip and start it playing.
            m_ShootingAudio.clip = m_ChargingClip;
            m_ShootingAudio.Play ();
        }
        
        // 按下但还没松开，处于蓄力阶段
        // if the fire button is being held and the shell hasn't been launched yet...
        public void FireButton()
        {
            // Increment the launch force and update the slider.
            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;

            m_AimSlider.value = m_CurrentLaunchForce;
        }
        
        // 发动攻击
        public void Fire ()
        {
            // Set the fired flag so only Fire is only called once.
            m_Fired = true;

            // Create an instance of the shell and store a reference to it's rigidbody.
            Rigidbody shellInstance =
                Instantiate (m_Shell, fireTransform.position, fireTransform.rotation) as Rigidbody;

            // Set the shell's velocity to the launch force in the fire position's forward direction.
            shellInstance.velocity = m_CurrentLaunchForce * fireTransform.forward; 

            // Change the clip to the firing clip and play it.
            m_ShootingAudio.clip = m_FireClip;
            m_ShootingAudio.Play ();

            // Reset the launch force.  This is a precaution in case of missing button events.
            m_CurrentLaunchForce = m_WeaponData.MinLaunchForce;
        }
    }
}