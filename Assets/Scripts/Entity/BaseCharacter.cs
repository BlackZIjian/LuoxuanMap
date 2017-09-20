using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework.Fsm;

namespace Luoxuan
{
    public class CharacterGroundMoveState : FsmState<BaseCharacter>
    {
        protected override void OnUpdate(IFsm<BaseCharacter> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            Debug.Log("Move Useful");
        }
    }
    class CharacterControllerInfo
    {
        public float SlopeLimit;
        public float StepOffset;
        public float SkinWidth;
        public float MinMoveDistance;
        public Vector3 CenterPos;
        public float Radius;
        public float Height;
        public CharacterControllerInfo()
        {
            SlopeLimit = 45;
            StepOffset = 0.3f;
            SkinWidth = 0.08f;
            MinMoveDistance = 0.001f;
            CenterPos = new Vector3(0,1.005f,0);
            Radius = 0.5f;
            Height = 2;
        }
    }
    public class BaseCharacter : EntityLogic
    {
        public bool mIsGrounded
        {
            get
            {
                if (mUnityCharacterController != null)
                    return mUnityCharacterController.isGrounded;
                return false;
            }
        }

        private CharacterController mUnityCharacterController;
        private Vector3 mPosition;
        private Quaternion mRotation;
        private CharacterControllerInfo mCharacterControllerInfo;
        private bool isGravity;
        private float mGravityValue;
        protected internal override void OnInit(object userData)
        {
            base.OnInit(userData);
            //读取存档
            mCharacterControllerInfo = new CharacterControllerInfo();
            mPosition = new Vector3(0, 20, 0);
            mRotation = Quaternion.Euler(0, 0, 0);
            isGravity = true;
            mGravityValue = 9.8f;
            transform.position = mPosition;
            transform.rotation = mRotation;
            ///

            //character controller 赋值
            mUnityCharacterController = gameObject.AddComponent<CharacterController>();
            mUnityCharacterController.slopeLimit = mCharacterControllerInfo.SlopeLimit;
            mUnityCharacterController.stepOffset = mCharacterControllerInfo.StepOffset;
            mUnityCharacterController.skinWidth = mCharacterControllerInfo.SkinWidth;
            mUnityCharacterController.minMoveDistance = mCharacterControllerInfo.MinMoveDistance;
            mUnityCharacterController.center = mCharacterControllerInfo.CenterPos;
            mUnityCharacterController.radius = mCharacterControllerInfo.Radius;
            mUnityCharacterController.height = mCharacterControllerInfo.Height;

            //创建状态机
            GameEntry.GetComponent<FsmComponent>().CreateFsm(this, new CharacterGroundMoveState());
        }
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            
        }
        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (mPosition != transform.position)
            {
                mPosition = transform.position;
                if (isGravity && mIsGrounded)
                    mUnityCharacterController.Move(Vector3.zero);
            }
            if(mRotation != transform.rotation)
            {
                mRotation = transform.rotation;
            }
            if(isGravity)
            {
                if(!mIsGrounded)
                {
                    if(mUnityCharacterController != null)
                    {
                        float velocityY = mUnityCharacterController.velocity.y - mGravityValue * elapseSeconds;
                        Vector3 gravityMotion = new Vector3(0, velocityY * elapseSeconds, 0);
                        mUnityCharacterController.Move(gravityMotion);
                    }
                }
            }
        }
    }
}
