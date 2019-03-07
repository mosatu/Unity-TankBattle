//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using UnityEngine;
using UnityGameFramework.Runtime;

namespace TankBattle
{
    /// <summary>
    /// 炮弹类。能实现的主要功能：
    ///     1. 获取炮弹本身的伤害，通过OnShow注入Bullet实体中，
    ///     2. 赋予刚体触发属性，能够自动计算伤害，并获取到血条组件，进行扣血
    ///     （待完成）3. 炮弹对象实例：根据用户数据进行初始化 - 创建炮弹实例 - 每一帧更新自己的位置，相当于炮弹的飞行逻辑
    /// </summary>
    public class TankBullet : Entity
    {
        public LayerMask m_TankMask;                        // Used to filter what the explosion affects, this should be set to "Players".
        public ParticleSystem m_ExplosionParticles;         // Reference to the particles that will play on explosion.
        public AudioSource m_ExplosionAudio;                // Reference to the audio that will play on explosion.
        
        [SerializeField]
        private BulletData m_BulletData = null;
        
        /// 第一步被调用，在流程中的Initialization模块中被调用
        private void Start()
        {
            // 在流程调用的开端 指定实体在存活时间之后被销毁
            Destroy(gameObject, m_BulletData.MaxLifeTime);
            
        }


        /// 第二步被调用，在pyhsics流程中被执行。
        ///       赋予实体的刚体组件的触发逻辑脚本
        private void OnTriggerEnter(Collider other)
        {
            // Collect all the colliders in a sphere from the shell's current position to a radius of the explosion radius.
            Collider[] colliders = Physics.OverlapSphere (transform.position, m_BulletData.ExplosionRadius, m_TankMask);

            // Go through all the colliders...
            for (int i = 0; i < colliders.Length; i++)
            {
                // ... and find their rigidbody.
                Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody> ();

                // If they don't have a rigidbody, go on to the next collider.
                if (!targetRigidbody)
                    continue;

                // Add an explosion force.
                targetRigidbody.AddExplosionForce (m_BulletData.ExplosionForce, transform.position, m_BulletData.ExplosionRadius);

                // Find the TankHealth script associated with the rigidbody.
                TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth> ();

                // If there is no TankHealth script attached to the gameobject, go on to the next collider.
                if (!targetHealth)
                    continue;

                // Calculate the amount of damage the target should take based on it's distance from the shell.
                float damage = CalculateDamage (targetRigidbody.position);

                // Deal this damage to the tank.
                targetHealth.TakeDamage (damage);
            }

            // Unparent the particles from the shell.
            m_ExplosionParticles.transform.parent = null;

            // Play the particle system.
            m_ExplosionParticles.Play();

            // Play the explosion sound effect.
            m_ExplosionAudio.Play();

            // Once the particles have finished, destroy the gameobject they are on.
            ParticleSystem.MainModule mainModule = m_ExplosionParticles.main;
            Destroy (m_ExplosionParticles.gameObject, mainModule.duration);

            // Destroy the shell.
            Destroy (gameObject);
        }

        /// 获取撞击数据：
        ///     目标点的坐标与子弹实体的距离 / 爆炸伤害圈的最长距离 = 子弹对目标点的伤害值
        public float CalculateDamage(Vector3 targetPosition)
        {
            // Create a vector from the shell to the target.
            Vector3 explosionToTarget = targetPosition - transform.position;

            // Calculate the distance from the shell to the target.
            float explosionDistance = explosionToTarget.magnitude;

            // Calculate the proportion of the maximum distance (the explosionRadius) the target is away.
            float relativeDistance = (m_BulletData.ExplosionRadius - explosionDistance) / m_BulletData.ExplosionRadius;

            // Calculate damage as this proportion of the maximum possible damage.
            float damage = relativeDistance * m_BulletData.MaxDamage;

            // Make sure that the minimum damage is always 0.
            damage = Mathf.Max (0f, damage);

            return damage;
        }


        /// <summary>
        /// 炮弹实体显示。
        ///     首先将用户数据赋予到炮弹对象上
        /// </summary>
        /// <param name="userData"></param>
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_BulletData = userData as BulletData;
            if (m_BulletData == null)
            {
                Log.Error("Bullet data is invalid.");
                return;
            }
            
        }

        /// <summary>
        /// 每帧画面 在子弹转变的方向和位置上进行变换
        ///     主要实现子弹发射之后的表现
        ///
        /// shooting 开火的时候会调用  子弹的飞行效果
        /// </summary>
        /// <param name="elapseSeconds"></param>
        /// <param name="realElapseSeconds"></param>
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

//            CachedTransform.Translate(Vector3.forward * m_BulletData.Speed * elapseSeconds, Space.World);
        }
        
    }
    
}
