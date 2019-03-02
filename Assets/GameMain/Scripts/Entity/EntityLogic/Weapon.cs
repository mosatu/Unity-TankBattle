//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace TankBattle
{
    /// <summary>
    /// 武器类。
    /// </summary>
    public class Weapon : Entity
    {
        private const string AttachPoint = "Weapon Point";

        [SerializeField]
        private WeaponData m_WeaponData = null;

        private float m_NextAttackTime = 0f;

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

            GameEntry.Entity.AttachEntity(Entity, m_WeaponData.OwnerId, AttachPoint);
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

        /// <summary>
        /// 使用武器攻击
        /// </summary>
        public void TryAttack()
        {
            // 此帧加载的时间小于0,不发动攻击
            if (Time.time < m_NextAttackTime)
            {
                return;
            }

            // 武器真正的攻击间隔时间是：此帧加载事件+武器设置的攻击间隔时间
            m_NextAttackTime = Time.time + m_WeaponData.AttackInterval;
            
            // 子弹实体被实例化出来，并展示子弹应有的状态表现
            GameEntry.Entity.ShowBullet(new BulletData(GameEntry.Entity.GenerateSerialId(), m_WeaponData.BulletId, m_WeaponData.OwnerId, m_WeaponData.OwnerCamp, m_WeaponData.Attack, m_WeaponData.BulletSpeed)
            {
                Position = CachedTransform.position,
            });
            
            // 全局Sound组件 播放子弹射击音效
            GameEntry.Sound.PlaySound(m_WeaponData.BulletSoundId);
        }
    }
}
