/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/


@script ExecuteInEditMode

private var gui : GUIText;

private var updateInterval = 1.0;
private var lastInterval : double; // Last interval end time
private var frames = 0; // Frames over current interval

function Start()
{
    lastInterval = Time.realtimeSinceStartup;
    frames = 0;
}

function OnDisable ()
{
	if (gui)
		DestroyImmediate (gui.gameObject);
}

function Update()
{
#if !UNITY_FLASH
    ++frames;
    var timeNow = Time.realtimeSinceStartup;
    if (timeNow > lastInterval + updateInterval)
    {
		if (!gui)
		{
			var go : GameObject = new GameObject("FPS Display", GUIText);
			go.hideFlags = HideFlags.HideAndDontSave;
			go.transform.position = Vector3(0,0,0);
			gui = go.GetComponent.<GUIText>();
			gui.pixelOffset = Vector2(5,55);
		}
        var fps : float = frames / (timeNow - lastInterval);
		var ms : float = 1000.0f / Mathf.Max (fps, 0.00001);
		gui.text = ms.ToString("f1") + "ms " + fps.ToString("f2") + "FPS";
        frames = 0;
        lastInterval = timeNow;
    }
#endif
}
