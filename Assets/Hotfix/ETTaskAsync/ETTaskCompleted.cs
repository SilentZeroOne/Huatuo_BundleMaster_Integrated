using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
#pragma warning disable CS0436
namespace ET
{
    [AsyncMethodBuilder(typeof (AsyncETTaskCompletedMethodBuilder))]
    public struct ETTaskCompleted: ICriticalNotifyCompletion
    {
        [DebuggerHidden]
        public ETTaskCompleted GetAwaiter()
        {
            return this;
        }

        [DebuggerHidden]
        public bool IsCompleted => true;

        [DebuggerHidden]
        public void GetResult()
        {
        }

        [DebuggerHidden]
        public void OnCompleted(Action continuation)
        {
        }

        [DebuggerHidden]
        public void UnsafeOnCompleted(Action continuation)
        {
        }
    }
}