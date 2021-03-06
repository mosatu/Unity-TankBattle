using GameFramework;
using UnityGameFramework.Runtime;

namespace TankBattle {
    public class MenuForm : UIFormLogic {
        private ProcedureMenu m_ProcedureMenu;
        protected MenuForm() { }

        protected override void OnOpen(object userData) {
            base.OnOpen(userData);

            // 打开UI的时候我们把ProcedureMenu作为参数传递了进去，在这里OnOpen事件会把它传递过来
            m_ProcedureMenu = (ProcedureMenu)userData;
            if (m_ProcedureMenu == null) {
                return;
            }
        }

        public void OnStarButtonClick() {
            Log.Debug("Calling: OnStarButtonClick");
            m_ProcedureMenu.StartGame();
        }
    }
}