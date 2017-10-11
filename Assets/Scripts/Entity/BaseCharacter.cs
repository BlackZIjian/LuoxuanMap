using GameFramework;
using UnityEngine;
using System.Collections.Generic;
using UnityGameFramework.Runtime;
using GameFramework.Fsm;

namespace Luoxuan
{
    public class CharacterGroundMoveState : FsmState<BaseCharacter>
    {
        protected override void OnUpdate(IFsm<BaseCharacter> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            fsm.Owner.SetForwardSpeed(Input.GetAxis("Vertical"));
            fsm.Owner.SetRightSpeed(Input.GetAxis("Horizontal"));
            if (!fsm.Owner.mIsGrounded)
            {
                ChangeState<CharacterInAirState>(fsm);
            }
        }
    }
    public class CharacterInAirState : FsmState<BaseCharacter>
    {
        protected override void OnUpdate(IFsm<BaseCharacter> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            if (fsm.Owner.mIsGrounded)
            {
                ChangeState<CharacterGroundMoveState>(fsm);
            }
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
            SlopeLimit = 75;
            StepOffset = 0.3f;
            SkinWidth = 0.08f;
            MinMoveDistance = 0.001f;
            CenterPos = new Vector3(0, 1.005f, 0);
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
        private Vector3 mMoveVelocity;
        private IFsm<BaseCharacter> mFsm;

        public float mForwardSpeed;

        public float mRightSpeed;

        protected internal override void OnInit(object userData)
        {
            base.OnInit(userData);
            //读取存档
            mCharacterControllerInfo = new CharacterControllerInfo();
            mPosition = new Vector3(0, 20, 0);
            mRotation = Quaternion.Euler(0, 0, 0);
            isGravity = true;
            mGravityValue = 9.8f;

            mForwardSpeed = 5;

            mRightSpeed = 5;
            
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
            CharacterGroundMoveState moveState = new CharacterGroundMoveState();
            
            CharacterInAirState airState = new CharacterInAirState();

            FsmState<BaseCharacter>[] states = new FsmState<BaseCharacter>[2]
            {
                moveState, airState
            };
            
            mFsm = GameEntry.GetComponent<FsmComponent>().CreateFsm(this, states);
            
            mFsm.Start<CharacterInAirState>();

            mMoveVelocity = Vector3.zero;
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            //这里处理和别的物体碰撞的逻辑
        }

        public void SetForwardSpeed(float speed)
        {
            mMoveVelocity.z = speed * mForwardSpeed;
        }

        public void SetRightSpeed(float speed)
        {
            mMoveVelocity.x = speed * mRightSpeed;
        }

        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            //强行改变transform值时调用Move函数重新计算一次更新ChracterController
            if (mPosition != transform.position)
            {
                mPosition = transform.position;
                //if (isGravity && mIsGrounded)
                //mUnityCharacterController.Move(Vector3.zero);
            }
            if (mRotation != transform.rotation)
            {
                mRotation = transform.rotation;
            }

            if (isGravity)
            {
                if (mUnityCharacterController != null)
                {
                    if (mIsGrounded)
                    {
                        mMoveVelocity.y = 0;
                    }
                    mMoveVelocity.y -= mGravityValue * elapseSeconds;
                }
            }
            if (mMoveVelocity.magnitude <= 0.05f)
                mMoveVelocity = Vector3.zero;
            if (mMoveVelocity != Vector3.zero)
            {
                mUnityCharacterController.Move(transform.TransformDirection(mMoveVelocity) * elapseSeconds);
            }
        }
    }
}
