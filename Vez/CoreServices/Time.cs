﻿using System.Runtime.CompilerServices;

namespace Vez.CoreServices
{
    /// <summary>
    /// provides frame timing information
    /// </summary>
    public class Time : IUpdateService
    {
        /// <summary>
        /// total time the game has been running
        /// </summary>
        public float TotalTime;

        /// <summary>
        /// delta time from the previous frame to the current, scaled by timeScale
        /// </summary>
        public float DeltaTime;

        /// <summary>
        /// unscaled version of deltaTime. Not affected by timeScale
        /// </summary>
        public float UnscaledDeltaTime;

        /// <summary>
        /// secondary deltaTime for use when you need to scale two different deltas simultaneously
        /// </summary>
        public float AltDeltaTime;

        /// <summary>
        /// total time since the Scene was loaded
        /// </summary>
        public float TimeSinceSceneLoad;

        /// <summary>
        /// time scale of deltaTime
        /// </summary>
        public float TimeScale = 1f;

        /// <summary>
        /// time scale of altDeltaTime
        /// </summary>
        public float AltTimeScale = 1f;

        /// <summary>
        /// total number of frames that have passed
        /// </summary>
        public uint FrameCount;

        /// <summary>
        /// Maximum value that DeltaTime can be. This can be useful to prevent physics from breaking when dragging
        /// the game window or if your game hitches.
        /// </summary>
        public float MaxDeltaTime = float.MaxValue;

        void IUpdateService.Update(float dt)
        {
            if (dt > MaxDeltaTime)
                dt = MaxDeltaTime;
            TotalTime += dt;
            DeltaTime = dt * TimeScale;
            AltDeltaTime = dt * AltTimeScale;
            UnscaledDeltaTime = dt;
            TimeSinceSceneLoad += dt;
            FrameCount++;
        }


        internal void SceneChanged()
        {
            TimeSinceSceneLoad = 0f;
        }


        /// <summary>
        /// Allows to check in intervals. Should only be used with interval values above deltaTime,
        /// otherwise it will always return true.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CheckEvery(float interval)
        {
            // we subtract deltaTime since timeSinceSceneLoad already includes this update ticks deltaTime
            return (int)(TimeSinceSceneLoad / interval) > (int)((TimeSinceSceneLoad - DeltaTime) / interval);
        }
    }
}
