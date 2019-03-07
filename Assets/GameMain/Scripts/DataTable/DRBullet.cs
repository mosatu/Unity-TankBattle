//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2019-03-06 16:04:57.516
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace TankBattle
{
    /// <summary>
    /// 炮弹属性表。
    /// </summary>
    public class DRBullet : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取属性编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取炮弹打中坦克时的最大伤害。
        /// </summary>
        public float MaxDamage
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取爆炸的力量。
        /// </summary>
        public float ExplosionForce
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取炮弹的伤害有效时间。
        /// </summary>
        public float MaxLifeTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取炮弹伤害范围圈半径。
        /// </summary>
        public float ExplosionRadius
        {
            get;
            private set;
        }

        public override bool ParseDataRow(GameFrameworkSegment<string> dataRowSegment)
        {
            // Star Force 示例代码，正式项目使用时请调整此处的生成代码，以处理 GCAlloc 问题！
            string[] columnTexts = dataRowSegment.Source.Substring(dataRowSegment.Offset, dataRowSegment.Length).Split(DataTableExtension.DataSplitSeparators);
            for (int i = 0; i < columnTexts.Length; i++)
            {
                columnTexts[i] = columnTexts[i].Trim(DataTableExtension.DataTrimSeparators);
            }

            int index = 0;
            index++;
            m_Id = int.Parse(columnTexts[index++]);
            index++;
            MaxDamage = float.Parse(columnTexts[index++]);
            ExplosionForce = float.Parse(columnTexts[index++]);
            MaxLifeTime = float.Parse(columnTexts[index++]);
            ExplosionRadius = float.Parse(columnTexts[index++]);

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(GameFrameworkSegment<byte[]> dataRowSegment)
        {
            // Star Force 示例代码，正式项目使用时请调整此处的生成代码，以处理 GCAlloc 问题！
            using (MemoryStream memoryStream = new MemoryStream(dataRowSegment.Source, dataRowSegment.Offset, dataRowSegment.Length, false))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    m_Id = binaryReader.ReadInt32();
                    MaxDamage = binaryReader.ReadSingle();
                    ExplosionForce = binaryReader.ReadSingle();
                    MaxLifeTime = binaryReader.ReadSingle();
                    ExplosionRadius = binaryReader.ReadSingle();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(GameFrameworkSegment<Stream> dataRowSegment)
        {
            Log.Warning("Not implemented ParseDataRow(GameFrameworkSegment<Stream>)");
            return false;
        }

        private void GeneratePropertyArray()
        {

        }
    }
}
