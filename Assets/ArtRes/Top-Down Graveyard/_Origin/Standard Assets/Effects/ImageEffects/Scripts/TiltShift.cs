/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
    [RequireComponent (typeof(Camera))]
    [AddComponentMenu ("Image Effects/Camera/Tilt Shift (Lens Blur)")]
    class TiltShift : PostEffectsBase {
        public enum TiltShiftMode
        {
            TiltShiftMode,
            IrisMode,
        }
        public enum TiltShiftQuality
        {
            Preview,
            Normal,
            High,
        }

        public TiltShiftMode mode = TiltShiftMode.TiltShiftMode;
        public TiltShiftQuality quality = TiltShiftQuality.Normal;

        [Range(0.0f, 15.0f)]
        public float blurArea = 1.0f;

        [Range(0.0f, 25.0f)]
        public float maxBlurSize = 5.0f;

        [Range(0, 1)]
        public int downsample = 0;

        public Shader tiltShiftShader = null;
        private Material tiltShiftMaterial = null;


        public override bool CheckResources () {
            CheckSupport (true);

            tiltShiftMaterial = CheckShaderAndCreateMaterial (tiltShiftShader, tiltShiftMaterial);

            if (!isSupported)
                ReportAutoDisable ();
            return isSupported;
        }

        void OnRenderImage (RenderTexture source, RenderTexture destination) {
            if (CheckResources() == false) {
                Graphics.Blit (source, destination);
                return;
            }

            tiltShiftMaterial.SetFloat("_BlurSize", maxBlurSize < 0.0f ? 0.0f : maxBlurSize);
            tiltShiftMaterial.SetFloat("_BlurArea", blurArea);
            source.filterMode = FilterMode.Bilinear;

            RenderTexture rt = destination;
            if (downsample > 0f) {
                rt = RenderTexture.GetTemporary (source.width>>downsample, source.height>>downsample, 0, source.format);
                rt.filterMode = FilterMode.Bilinear;
            }

            int basePassNr = (int) quality; basePassNr *= 2;
            Graphics.Blit (source, rt, tiltShiftMaterial, mode == TiltShiftMode.TiltShiftMode ? basePassNr : basePassNr + 1);

            if (downsample > 0) {
                tiltShiftMaterial.SetTexture ("_Blurred", rt);
                Graphics.Blit (source, destination, tiltShiftMaterial, 6);
            }

            if (rt != destination)
                RenderTexture.ReleaseTemporary (rt);
        }
    }
}
