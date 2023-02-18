//自定义类
//https://melonwiki.xyz/#/modders/il2cppdifferences?id=custom-components-il2cpp-type-inheritance
using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnhollowerRuntimeLib;

namespace GGD_Hack
{
    [RegisterTypeInIl2Cpp]
    public class UnityMainThreadDispatcher : MonoBehaviour
    {
        public UnityMainThreadDispatcher(IntPtr ptr) : base(ptr) { }

        // Optional, only used in case you want to instantiate this class in the mono-side
        // Don't use this on MonoBehaviours / Components!
        public UnityMainThreadDispatcher() : base(ClassInjector.DerivedConstructorPointer<UnityMainThreadDispatcher>()) => ClassInjector.DerivedConstructorBody(this);


        private static UnityMainThreadDispatcher instance = null;
        private static readonly Queue<System.Action> executionQueue = new Queue<System.Action>();

        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if(ML_Manager.GetComponent<UnityMainThreadDispatcher>() == null) {

                instance = ML_Manager.AddComponent<UnityMainThreadDispatcher>();
            }
        }

        public static UnityMainThreadDispatcher Instance()
        {
            if (instance == null)
            {
                Init();
            }

            return instance;
        }

        private void Update()
        {
            lock (executionQueue)
            {
                while (executionQueue.Count > 0)
                {
                    executionQueue.Dequeue().Invoke();
                }
            }
        }

        public void Enqueue(System.Action action)
        {
            lock (executionQueue)
            {
                executionQueue.Enqueue(action);
            }
        }
    }
}