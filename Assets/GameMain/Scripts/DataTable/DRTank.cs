//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2019-03-03 22:00:41.769
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
    /// 坦克表。
    /// </summary>
    public class DRTank : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取坦克编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取坦克速度。
        /// </summary>
        public float Speed
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取坦克转速。
        /// </summary>
        public float TurnSpeed
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取发动机噪音的间距可以变化的量。
        /// </summary>
        public float PitchRange
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取导弹发射的最小蓄力。
        /// </summary>
        public float MinLaunchForce
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取导弹发射的最大蓄力。
        /// </summary>
        public float MaxLaunchForce
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取蓄力的最长时间。
        /// </summary>
        public float MaxChargeTime
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
            Speed = float.Parse(columnTexts[index++]);
            TurnSpeed = float.Parse(columnTexts[index++]);
            PitchRange = float.Parse(columnTexts[index++]);
            MinLaunchForce = float.Parse(columnTexts[index++]);
            MaxLaunchForce = float.Parse(columnTexts[index++]);
            MaxChargeTime = float.Parse(columnTexts[index++]);

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
                    Speed = binaryReader.ReadSingle();
                    TurnSpeed = binaryReader.ReadSingle();
                    PitchRange = binaryReader.ReadSingle();
                    MinLaunchForce = binaryReader.ReadSingle();
                    MaxLaunchForce = binaryReader.ReadSingle();
                    MaxChargeTime = binaryReader.ReadSingle();
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
