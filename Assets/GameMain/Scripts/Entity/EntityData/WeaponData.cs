//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.DataTable;
using System;
using UnityEngine;

namespace TankBattle
{
    /// <summary>
    /// 武器数据类
    /// </summary>
    [Serializable]
    public class WeaponData : AccessoryObjectData
    {
        [SerializeField]
        private int m_Attack = 0;                   //武器攻击力

        [SerializeField]
        private float m_AttackInterval = 0f;        //w武器的攻击间隔时间

        [SerializeField]
        private int m_BulletId = 0;                 // 武器发出的子弹Id编号

        [SerializeField]
        private float m_BulletSpeed = 0f;           // 子弹的射速

        [SerializeField]
        private int m_BulletSoundId = 0;            // 子弹播放的音效编号

        /// <summary>
        /// 武器数据的构造方法
        /// </summary>
        /// <param name="entityId">实体编号</param>
        /// <param name="typeId">实体类型编号</param>
        /// <param name="ownerId">武器拥有者编号</param>
        /// <param name="ownerCamp">武器拥有者阵营</param>
        public WeaponData(int entityId, int typeId, int ownerId, CampType ownerCamp)
            : base(entityId, typeId, ownerId, ownerCamp)
        {
            IDataTable<DRWeapon> dtWeapon = GameEntry.DataTable.GetDataTable<DRWeapon>();
            DRWeapon drWeapon = dtWeapon.GetDataRow(TypeId);
            if (drWeapon == null)
            {
                return;
            }

            m_Attack = drWeapon.Attack;
            m_AttackInterval = drWeapon.AttackInterval;
            m_BulletId = drWeapon.BulletId;
            m_BulletSpeed = drWeapon.BulletSpeed;
            m_BulletSoundId = drWeapon.BulletSoundId;
        }

        /// <summary>
        /// 攻击力。
        /// </summary>
        public int Attack
        {
            get
            {
                return m_Attack;
            }
        }

        /// <summary>
        /// 攻击间隔。
        /// </summary>
        public float AttackInterval
        {
            get
            {
                return m_AttackInterval;
            }
        }

        /// <summary>
        /// 子弹编号。
        /// </summary>
        public int BulletId
        {
            get
            {
                return m_BulletId;
            }
        }

        /// <summary>
        /// 子弹速度。
        /// </summary>
        public float BulletSpeed
        {
            get
            {
                return m_BulletSpeed;
            }
        }

        /// <summary>
        /// 子弹声音编号。
        /// </summary>
        public int BulletSoundId
        {
            get
            {
                return m_BulletSoundId;
            }
        }
    }
}
