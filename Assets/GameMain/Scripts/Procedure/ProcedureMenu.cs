//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Event;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace TankBattle {
    public class ProcedureMenu : ProcedureBase {
        private bool m_StartGame = false;
        private MenuForm m_MenuForm = null;

        
        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }
        
        public void StartGame() {
            m_StartGame = true;
        }

        /// <summary>
        /// 加载场景及资源
        /// </summary>
        protected override void OnEnter(ProcedureOwner procedureOwner) {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            m_StartGame = false;

            // 加载UI预制体
            GameEntry.UI.OpenUIForm("Assets/GameMain/UI/UIForms/MenuForm.prefab", "DefaultGroup", this);
        }

        /// <summary>
        /// 销毁资源
        /// </summary>
        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown) {
            base.OnLeave(procedureOwner, isShutdown);

            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            if (m_MenuForm != null) {
                GameEntry.UI.CloseUIForm(m_MenuForm.UIForm);
                m_MenuForm = null;
            }
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds) {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            // 每一帧都在检测是否开始游戏
            if (m_StartGame) {
                procedureOwner.SetData<VarInt>(Constant.ProcedureData.NextSceneId, GameEntry.Config.GetInt("Scene.Main"));
                procedureOwner.SetData<VarInt>(Constant.ProcedureData.GameMode, (int)GameMode.Survival);
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e) {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if (ne.UserData != this) {
                return;
            }
            m_MenuForm = (MenuForm)ne.UIForm.Logic;
        }
    }
}
