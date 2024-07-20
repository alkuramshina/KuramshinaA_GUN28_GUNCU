using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Desdinova
{
    public class ConsoleGUIController : MonoBehaviour
    {
        public enum ConsoleGUIAnchor
        {
            Top = 1,
            Bottom = 2
        }

        public enum ConsoleGUIHeight
        {
            Full = 1,
            Half = 2,
            Quarter = 4
        }

        public enum ConsoleGUIOrder
        {
            Normal = 1,
            Reverse = 2
        }

        private List<string> logValues = new List<string>();
        private string logText = string.Empty;
        private Vector2 scrollPosition;
        private Texture2D backgroundTexture;

        [Header("Show Properties")]
        public bool ShowConsole = true;
        public bool ShowStackTrace = true;
        public bool ShowTitle = true;
        public ConsoleGUIOrder ShowOrder = ConsoleGUIOrder.Normal;

        [Header("Key Properties")]
        public KeyCode KeyCode = KeyCode.Backslash;
        public string KeyString = "";

        [Header("GUI Properties")]
        public ConsoleGUIAnchor GUIAnchor = ConsoleGUIAnchor.Bottom;
        public ConsoleGUIHeight GUIHeight = ConsoleGUIHeight.Half;
        public int GUIFontSize = 15;
        public Color GUIColor = Color.black;

        [Header("Behaviours Properties")]
        public bool DoNotDestroyOnLoad = false;

        public GUIStyle DialogBoxStyle { get; private set; }

        void OnEnable() { UnityEngine.Application.logMessageReceived += Log; }
        void OnDisable() { UnityEngine.Application.logMessageReceived -= Log; }

        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = col;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        private void Start()
        {
            //Use in different scene
            if (this.DoNotDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }

            //Set static backgorund color (do not do it in the OnGUI method)
            backgroundTexture = MakeTex(2, 2, this.GUIColor);
        }

        void Update()
        {
            if (Input.GetKeyDown(this.KeyCode))
            {
                this.ShowConsole = !this.ShowConsole;
            }
            else if (KeyString.Length > 0)
            {
                if (Input.GetButtonDown(this.KeyString))
                {
                    this.ShowConsole = !this.ShowConsole;
                }
            }
        }

        public void Log(string logString, string stackTrace, LogType type)
        {
            string openTag = "";
            string closeTag = "";

            if (type == LogType.Error) { openTag = "<color=#FF534A>"; closeTag = "</color>"; }
            else if (type == LogType.Warning) { openTag = "<color=#FFC107>"; closeTag = "</color>"; }

            string stack = string.Empty;
            if (this.ShowStackTrace)
            {
                stack = "\n" + "<size=" + this.GUIFontSize / 1.25 + ">" + stackTrace + "</size>";
            }

            if (this.ShowOrder == ConsoleGUIOrder.Normal)
            {
                logValues.Add(openTag + "[" + DateTime.Now.ToLongTimeString() + "] " + logString + stack + closeTag);
            }
            else
            {
                logValues.Insert(0, openTag + "[" + DateTime.Now.ToLongTimeString() + "] " + logString + stack + closeTag);
            }

            logText = string.Empty;
            foreach (string s in logValues)
            {
                logText = logText + "\n" + s;
            }
        }


        void OnGUI()
        {
            if (this.ShowConsole)
            {
                string startTag = "<size=" + this.GUIFontSize + ">";
                string closeTag = "</size>";

                //Style
                GUIStyle newStyle = new GUIStyle(GUI.skin.box);
                newStyle.alignment = TextAnchor.UpperLeft;
                newStyle.richText = true;
                newStyle.normal.background = backgroundTexture;
                newStyle.wordWrap = true;

                //Size
                Rect newRect = new Rect();
                if (this.GUIAnchor == ConsoleGUIAnchor.Top)
                {
                    newRect = new Rect(10, 10, Screen.width - 20, (Screen.height / (int)this.GUIHeight) - 20);
                }
                else if (this.GUIAnchor == ConsoleGUIAnchor.Bottom)
                {
                    newRect = new Rect(10, Screen.height - (Screen.height / (int)this.GUIHeight), Screen.width - 20, (Screen.height / (int)this.GUIHeight) - 10);
                }

                //Contents
                string title = string.Empty;
                if (this.ShowTitle)
                {
                    title = UnityEngine.Application.productName + " " + UnityEngine.Application.version + " LOG CONSOLE: \n";
                }
                GUIContent content = new GUIContent(startTag + title + logText + closeTag);

                //Final height
                float dinamicHeight = Mathf.Max((Screen.height / (int)this.GUIHeight), newStyle.CalcHeight(content, Screen.width));

                //Begin scroll
                scrollPosition = GUI.BeginScrollView(newRect, scrollPosition, new Rect(0, 0, 0, dinamicHeight), false, true);

                //Draw box
                GUI.Box(new Rect(0, 0, Screen.width, dinamicHeight), content, newStyle);                

                //End scroll
                GUI.EndScrollView();
            }
        }

       
    }
}