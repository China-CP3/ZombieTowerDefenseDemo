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
    /// A Utility class for performing various image based rendering tasks.
    [AddComponentMenu("")]
    public class ImageEffects
    {
        public static void RenderDistortion(Material material, RenderTexture source, RenderTexture destination, float angle, Vector2 center, Vector2 radius)
        {
            bool invertY = source.texelSize.y < 0.0f;
            if (invertY)
            {
                center.y = 1.0f - center.y;
                angle = -angle;
            }

            Matrix4x4 rotationMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, angle), Vector3.one);

            material.SetMatrix("_RotationMatrix", rotationMatrix);
            material.SetVector("_CenterRadius", new Vector4(center.x, center.y, radius.x, radius.y));
            material.SetFloat("_Angle", angle*Mathf.Deg2Rad);

            Graphics.Blit(source, destination, material);
        }


        [Obsolete("Use Graphics.Blit(source,dest) instead")]
        public static void Blit(RenderTexture source, RenderTexture dest)
        {
            Graphics.Blit(source, dest);
        }


        [Obsolete("Use Graphics.Blit(source, destination, material) instead")]
        public static void BlitWithMaterial(Material material, RenderTexture source, RenderTexture dest)
        {
            Graphics.Blit(source, dest, material);
        }
    }
}
