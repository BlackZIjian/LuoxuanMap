using UnityGameFramework.Runtime;
using GameFramework.Procedure;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using GameFramework.Fsm;
using GameFramework.Event;

namespace Luoxuan {
    public class ProcedureTest : ProcedureBase
    {
        private EntityComponent Entity;
        private SceneComponent Scene;
        private EventComponent Event;
        private string mGameSceneAssetName;
        private bool mIsInitLocalPlayer;
        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
            Entity = GameEntry.GetComponent<EntityComponent>();
            Entity.AddEntityGroup(EntityGroupInfo.Player, 0, 1, 0, 100);
            Scene = GameEntry.GetComponent<SceneComponent>();
            Event = GameEntry.GetComponent<EventComponent>();
            mGameSceneAssetName = SceneInfo.VoxelandScene;
            mIsInitLocalPlayer = false;
        }
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Event.Subscribe(EventId.LoadSceneSuccess, new System.EventHandler<GameEventArgs>(OnLoadGameSceneSuccess));
            Scene.LoadScene(mGameSceneAssetName);
        }
        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            Event.Unsubscribe(EventId.LoadSceneSuccess, new System.EventHandler<GameEventArgs>(OnLoadGameSceneSuccess));
        }
        private void OnLoadGameSceneSuccess(object o, GameEventArgs e)
        {
            LoadSceneSuccessEventArgs loadSceneArgs = e as LoadSceneSuccessEventArgs;
            if(!mIsInitLocalPlayer && loadSceneArgs != null && loadSceneArgs.SceneAssetName == mGameSceneAssetName)
            {
                Entity.ShowEntity<BaseCharacter>(EntityIdManager.Instance.EntityId, EntityInfo.LocalPlayer, EntityGroupInfo.Player);
                mIsInitLocalPlayer = true;
            }
        }
    }
}
