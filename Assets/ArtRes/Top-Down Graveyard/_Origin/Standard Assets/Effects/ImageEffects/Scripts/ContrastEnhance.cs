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
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Color Adjustments/Contrast Enhance (Unsharp Mask)")]
    class ContrastEnhance : PostEffectsBase
	{
        public float intensity = 0.5f;
        public float threshold = 0.0f;

        private Material separableBlurMaterial;
        private Material contrastCompositeMaterial;

        public float blurSpread = 1.0f;

        public Shader separableBlurShader = null;
        public Shader contrastCompositeShader = null;


        public override bool CheckResources ()
		{
            CheckSupport (false);

            contrastCompositeMaterial = CheckShaderAndCreateMaterial (contrastCompositeShader, contrastCompositeMaterial);
            separableBlurMaterial = CheckShaderAndCreateMaterial (separableBlurShader, separableBlurMaterial);

            if (!isSupported)
                ReportAutoDisable ();
            return isSupported;
        }

        void OnRenderImage (RenderTexture source, RenderTexture destination)
		{
            if (CheckResources()==false)
			{
                Graphics.Blit (source, destination);
                return;
            }

            int rtW = source.width;
            int rtH = source.height;

            RenderTexture color2 = RenderTexture.GetTemporary (rtW/2, rtH/2, 0);

            // downsample

            Graphics.Blit (source, color2);
            RenderTexture color4a = RenderTexture.GetTemporary (rtW/4, rtH/4, 0);
            Graphics.Blit (color2, color4a);
            RenderTexture.ReleaseTemporary (color2);

            // blur

            separableBlurMaterial.SetVector ("offsets", new Vector4 (0.0f, (blurSpread * 1.0f) / color4a.height, 0.0f, 0.0f));
            RenderTexture color4b = RenderTexture.GetTemporary (rtW/4, rtH/4, 0);
            Graphics.Blit (color4a, color4b, separableBlurMaterial);
            RenderTexture.ReleaseTemporary (color4a);

            separableBlurMaterial.SetVector ("offsets", new Vector4 ((blurSpread * 1.0f) / color4a.width, 0.0f, 0.0f, 0.0f));
            color4a = RenderTexture.GetTemporary (rtW/4, rtH/4, 0);
            Graphics.Blit (color4b, color4a, separableBlurMaterial);
            RenderTexture.ReleaseTemporary (color4b);

            // composite

            contrastCompositeMaterial.SetTexture ("_MainTexBlurred", color4a);
            contrastCompositeMaterial.SetFloat ("intensity", intensity);
            contrastCompositeMaterial.SetFloat ("threshhold", threshold);
            Graphics.Blit (source, destination, contrastCompositeMaterial);

            RenderTexture.ReleaseTemporary (color4a);
        }
    }
}
