using GameFramework;
using UnityEngine;
using System.Collections;
using UnityGameFramework.Runtime;
using UnityEngine.UI;

namespace Luoxuan
{
    public class LoginForm : UIFormLogic
    {
        private InputField m_AccountInput = null;
        private InputField m_PasswordInput = null;
        private Button m_LoginButton = null;
        private Text m_LogText = null;
        private float targetAlphaValue = 0;
        private float nowAlphaChangeSpeed = 0;
        private bool isChangingAlpha = false;

        private ProcedureLaunch m_ProcedureLaunch = null;

        public void OnLoginButtonClick()
        {
            string account = m_AccountInput.text;
            string password = m_PasswordInput.text;
            if (account == "heibe" && password == "heibe")
            {
                m_ProcedureLaunch.Login();
            }
            else
            {
                m_LogText.text = "登录失败";
                Occur(m_LogText);
            }
        }
        protected internal override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_AccountInput = transform.Find("account").GetComponent<InputField>();
            m_PasswordInput = transform.Find("password").GetComponent<InputField>();
            m_LoginButton = transform.Find("login").GetComponent<Button>();
            m_LogText = transform.Find("Log").GetComponent<Text>();
            m_LoginButton.onClick.AddListener(OnLoginButtonClick);
        }
        protected internal override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_ProcedureLaunch = (ProcedureLaunch)userData;
            if (m_ProcedureLaunch == null)
            {
                Log.Warning("ProcedureMenu is invalid when open MenuForm.");
                return;
            }
        }

        protected internal override void OnClose(object userData)
        {
            m_ProcedureLaunch = null;

            base.OnClose(userData);
        }
        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (isChangingAlpha)
            {
                Color color = m_LogText.color;
                color.a += nowAlphaChangeSpeed * elapseSeconds;
                if (nowAlphaChangeSpeed > 0)
                {
                    if (m_LogText.color.a >= targetAlphaValue)
                    {
                        color.a = targetAlphaValue;
                        isChangingAlpha = false;
                    }
                }
                else
                {
                    if (m_LogText.color.a <= targetAlphaValue)
                    {
                        color.a = targetAlphaValue;
                        isChangingAlpha = false;
                    }
                }
                m_LogText.color = color;
            }
        }
        private void Occur(Text text)
        {
            nowAlphaChangeSpeed = 3f;
            targetAlphaValue = 1;
            isChangingAlpha = true;
            StartCoroutine(DelayDisapear(0.8f));
        }

        private IEnumerator DelayDisapear(float yieldTime)
        {
            yield return new WaitForSeconds(yieldTime);
            nowAlphaChangeSpeed = -0.4f;
            targetAlphaValue = 0;
            isChangingAlpha = true;
        }
    }
}
