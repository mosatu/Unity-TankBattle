//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;
using UnityEngine;

namespace TankBattle
{
    [Serializable]
    public class BulletData : EntityData
    {
        [SerializeField]
        private int m_OwnerId = 0;

        [SerializeField]
        private CampType m_OwnerCamp = CampType.Unknown;

        [SerializeField]
        private int m_MaxDamage = 0;
        
        [SerializeField]
        private float m_ExplosionForce = 0;
        
        [SerializeField]
        private float m_MaxLifeTime = 0;
        
        [SerializeField]
        private float m_ExplosionRadius = 0;


        /// <summary>
        /// 一个炮弹对象拥有的属性
        /// </summary>
        /// <param name="entityId">实体编号</param>
        /// <param name="typeId">实体类型编号</param>
        /// <param name="ownerId">实体拥有者的编号</param>
        /// <param name="ownerCamp">实体拥有者的阵营类型</param>
        /// <param name="attack">炮弹的伤害</param>
        /// <param name="speed">炮弹的速度</param>
        public BulletData(int entityId, int typeId, int ownerId, CampType ownerCamp, int maxDamager, float explosionForce, float maxLifeTime, float explosionRadius)
            : base(entityId, typeId)
        {
            m_OwnerId = ownerId;
            m_OwnerCamp = ownerCamp;
            m_MaxDamage = maxDamager;
            m_ExplosionForce = explosionForce;
            m_MaxLifeTime = maxLifeTime;
            m_ExplosionRadius = explosionRadius;
        }

        public int OwnerId
        {
            get
            {
                return m_OwnerId;
            }
        }

        public CampType OwnerCamp
        {
            get
            {
                return m_OwnerCamp;
            }
        }

        public int MaxDamage
        {
            get => m_MaxDamage;
        }

        public float ExplosionForce
        {
            get => m_ExplosionForce;
        }

        public float MaxLifeTime
        {
            get => m_MaxLifeTime;
        }

        public float ExplosionRadius
        {
            get => m_ExplosionRadius;
        }
    }
}
