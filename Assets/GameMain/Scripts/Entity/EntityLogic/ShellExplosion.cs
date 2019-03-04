using GameFramework.DataTable;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace TankBattle
{
    /// <summary>
    /// 子弹类
    ///     赋予shell prefab的刚体属性，在刚体属性被触发时，能够对附近的所有目标造成范围伤害，伤害的大小由该子弹刚体与目标之间的距离决定。
    ///
    /// 用法
    ///     附着在shell prefab上
    /// </summary>
    public class ShellExplosion : Entity
    {
        public LayerMask m_TankMask;                        // Used to filter what the explosion affects, this should be set to "Players".
        public ParticleSystem m_ExplosionParticles;         // Reference to the particles that will play on explosion.
        public AudioSource m_ExplosionAudio;                // Reference to the audio that will play on explosion.
        
        private float m_MaxDamage;                    // The amount of damage done if the explosion is centred on a tank.
        private float m_ExplosionForce;              // The amount of force added to a tank at the centre of the explosion.
        private float m_MaxLifeTime;                    // The time in seconds before the shell is removed.
        private float m_ExplosionRadius;                // The maximum distance away from the explosion tanks can be and are still affected.

        // 在该子弹实体被创建的时候，已经设置了该实体的失效销毁时间，在指定时间结束后会被销毁。
        private void Start ()
        {
            
            // 初始化子弹类属性
            IDataTable<DRBullet> dtEntity = GameEntry.DataTable.GetDataTable<DRBullet>();
            DRBullet drEntity = dtEntity.GetDataRow(1000);
            m_MaxDamage = drEntity.MaxDamage;
            m_ExplosionForce = drEntity.ExplosionForce;
            m_MaxLifeTime = drEntity.ExplosionRadius;
            m_ExplosionRadius = drEntity.ExplosionRadius;
            
            // If it isn't destroyed by then, destroy the shell after it's lifetime.
            Destroy (gameObject, m_MaxLifeTime);
        }


        // 设置该子弹实体的刚体触发属性。
        private void OnTriggerEnter (Collider other)
        {
			// Collect all the colliders in a sphere from the shell's current position to a radius of the explosion radius.
            Collider[] colliders = Physics.OverlapSphere (transform.position, m_ExplosionRadius, m_TankMask);

            // Go through all the colliders...
            for (int i = 0; i < colliders.Length; i++)
            {
                // ... and find their rigidbody.
                Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody> ();

                // If they don't have a rigidbody, go on to the next collider.
                if (!targetRigidbody)
                    continue;

                // Add an explosion force.
                targetRigidbody.AddExplosionForce (m_ExplosionForce, transform.position, m_ExplosionRadius);

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

        // 根据 ： 目标点的坐标与子弹实体的距离 / 爆炸伤害圈的最长距离 = 子弹对目标点的伤害值
        private float CalculateDamage (Vector3 targetPosition)
        {
            // Create a vector from the shell to the target.
            Vector3 explosionToTarget = targetPosition - transform.position;

            // Calculate the distance from the shell to the target.
            float explosionDistance = explosionToTarget.magnitude;

            // Calculate the proportion of the maximum distance (the explosionRadius) the target is away.
            float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;

            // Calculate damage as this proportion of the maximum possible damage.
            float damage = relativeDistance * m_MaxDamage;

            // Make sure that the minimum damage is always 0.
            damage = Mathf.Max (0f, damage);

            return damage;
        }
    }
}