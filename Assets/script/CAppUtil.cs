using UnityEngine;
using System.IO;
using System.Collections;

public enum TouchInfo
{
	Began		= TouchPhase.Began,			// タッチ開始
	Moved		= TouchPhase.Moved,			// タッチ移動
	Stationary	= TouchPhase.Stationary,	// タッチ静止
	Ended		= TouchPhase.Ended,			// タッチ終了
	Canceled	= TouchPhase.Canceled,		// タッチキャンセル
	None		= 99,						// タッチ無し
}

public static class CAppUtil
{
	private static Vector3 TouchPosition = Vector3.zero;
	
	// タッチ情報を取得(エディタと実機を考慮)
	// タッチ情報。タッチされていない場合は null
	public static TouchInfo GetTouch()
	{
		if (Application.isEditor)
		{
			if (Input.GetMouseButtonDown(0)) { return TouchInfo.Began; }
			if (Input.GetMouseButton(0)) { return TouchInfo.Moved; }
			if (Input.GetMouseButtonUp(0)) { return TouchInfo.Ended; }
		}
		else
		{
			if (Input.touchCount > 0)
			{
				return (TouchInfo)((int)Input.GetTouch(0).phase);
			}
		}
		return TouchInfo.None;
	}
	
	// タッチポジションを取得(エディタと実機を考慮)
	// タッチポジション。タッチされていない場合は (0, 0, 0)
	public static Vector3 GetTouchPosition()
	{
		if (Application.isEditor)
		{
			if (CAppUtil.GetTouch() != TouchInfo.None) { return Input.mousePosition; }
		}
		else
		{
			if (Input.touchCount > 0)
			{
				Touch touch = Input.GetTouch(0);
				TouchPosition.x = touch.position.x;
				TouchPosition.y = touch.position.y;
				return TouchPosition;
			}
		}
		return Vector3.zero;
	}

	// バイナリ取得
	public static byte[] GetBinaryStream(string path)
	{
		FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
		BinaryReader bin = new BinaryReader(fileStream);
		byte[] values = bin.ReadBytes((int)bin.BaseStream.Length);

		bin.Close();

		return values;
	}

	// テクスチャ読み込み
	public static Texture ReadTexture(string path, int width, int height)
	{
		byte[] readBinary = GetBinaryStream(path);

		Texture2D texture = new Texture2D(width, height);
		texture.LoadImage(readBinary);

		return texture;
	}

	// スプライト取得
	public static Sprite GetSprite(string fileName, string spriteName)
	{
		Sprite[] sprites = Resources.LoadAll<Sprite>(fileName);
		Sprite sp = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals(spriteName));

		Debug.Log(	fileName
					+ " : " + spriteName
					+ " : " + sp
					+ " : " + sp.rect);
		return sp;
	}

	// 下がマイナスなのがなんか気持ち悪いので作りました
	public static Vector3 GetSpritePos(float x, float y)
	{
		Vector3 pos;
		pos.x = x;
		pos.y = -y;
		pos.z = 0.0f;
		return pos;
	}

}

