using UnityGameFramework.Runtime;
using GameFramework.Procedure;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using GameFramework.Fsm;

namespace Luoxuan
{
    public class ProcedureLaunch : ProcedureBase
    {
        private UIComponent UI;
        private int mLoginUIId;
    
        public void Login()
        {
        }
        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
            UI = GameEntry.GetComponent<UIComponent>();
            UI.AddUIGroup(UIGroupId.LoginGroup,0);
        }
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            mLoginUIId = UI.OpenUIForm(UIFormId.LoginForm, UIGroupId.LoginGroup,this);
        }
        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            UI.CloseUIForm(mLoginUIId);
        }
    }
}
