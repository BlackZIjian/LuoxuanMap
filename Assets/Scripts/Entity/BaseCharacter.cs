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
        private Vector3 mMoveVelocity;

        private class YieldCutOffVelocityStruct
        {
            public Vector3 lastFramePos;
            public ControllerColliderHit hit;
        }

        private List<YieldCutOffVelocityStruct> mYieldCutOffList;
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

            mMoveVelocity = Vector3.zero;
            mYieldCutOffList = new List<YieldCutOffVelocityStruct>();
        }
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if(mMoveVelocity != Vector3.zero)
            {
                YieldCutOffVelocityStruct yieldCutOff = new YieldCutOffVelocityStruct();
                yieldCutOff.hit = hit;
                yieldCutOff.lastFramePos = hit.transform.position;
                mYieldCutOffList.Add(yieldCutOff);
            }
        }
        
        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            //强行改变transform值时调用Move函数重新计算一次更新ChracterController
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
                    if(mUnityCharacterController != null)
                    {
                        mMoveVelocity.y -= mGravityValue * elapseSeconds;
                    }
            }

            if(mYieldCutOffList.Count > 0)
            {
                for(int i=0;i<mYieldCutOffList.Count;i++)
                {
                    Vector3 delta = mYieldCutOffList[i].hit.transform.position - mYieldCutOffList[i].lastFramePos;
                    Vector3 normal = mYieldCutOffList[i].hit.normal.normalized;
                    Vector3 normalSubVelocity = Vector3.Dot(mMoveVelocity, normal) * normal;
                    Vector3 planeSubVelocity = mMoveVelocity - normalSubVelocity;
                    planeSubVelocity *= 0.8f;//模拟摩擦阻力
                    mMoveVelocity = planeSubVelocity + Vector3.Dot(delta / elapseSeconds, normal) * normal;
                }
                mYieldCutOffList.Clear();
            }
            if (mMoveVelocity.magnitude <= 0.05f)
                mMoveVelocity = Vector3.zero;
            if(mMoveVelocity != Vector3.zero)
            {
                mUnityCharacterController.Move(mMoveVelocity * elapseSeconds);
            }
        }
    }
}
