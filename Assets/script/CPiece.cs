using UnityEngine;
using System.Collections;

public class CPiece : MonoBehaviour {

	private Sprite mSprite;
	private int mOriginPosX;
	private int mOriginPosY;
	private int mPosX;
	private int mPosY;
	private Vector3 mTargetPos;
	private Vector3 mMoveDist;
	private float mMoveTimer = 0.0f;

	public int posX
	{
		get { return mPosX; }
		set { mPosX = value; }
	}
	public int posY
	{
		get { return mPosY; }
		set { mPosY = value; }
	}

	private const float move_time = 0.04f;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update ()
	{
		updatePos();
	}

	private void updatePos()
	{
		if(mMoveTimer != 0.0f)
		{
			mMoveTimer -= Time.deltaTime;
			if(mMoveTimer <= 0.0f)
			{
				transform.position = mTargetPos;
				mMoveTimer = 0.0f;
			}
			else
			{
				float rate = mMoveTimer / move_time;
				transform.position = mTargetPos + (mMoveDist * rate * rate);
			}
		}
	}

	public bool isMove()
	{
		return (mMoveTimer != 0.0f);
	}

	public void setup(int pic_no, int pos_no, int pos_x, int pos_y)
	{
		string picture_path	= System.String.Format("sprite/picture_{0:00}", pic_no);
		string parts_name	= System.String.Format("picture_{0:00}_{1:00}", pic_no, pos_no);

		mSprite = CAppUtil.GetSprite(picture_path, parts_name);
		GetComponent<SpriteRenderer>().sprite = mSprite;

		mPosX = mOriginPosX = pos_x;
		mPosY = mOriginPosY = pos_y;

		setupPos(true);
	}

	public void setupPos(bool is_immediate)
	{
		Rect rc = mSprite.rect;

		float offset_x = 4.0f;	// 中央揃え。画面幅200,ピース幅48 (200-(48*4))/2=4
		float offset_y = 32.0f;	// 適当

		mTargetPos = CAppUtil.GetSpritePos(rc.width * mPosX + offset_x, rc.height * mPosY + offset_y);

		if(is_immediate)
		{
			transform.position = mTargetPos;
			mMoveTimer = 0.0f;
		}
		else
		{
			mMoveDist = transform.position - mTargetPos;
			mMoveTimer = move_time;
		}
	}

	public void setPos(int pos_x, int pos_y)
	{
		Debug.Log(string.Format(mSprite+"{0}:{1}",pos_x,pos_y));
		mPosX = pos_x;
		mPosY = pos_y;
		setupPos(false);
	}

	public bool isComplete()
	{
		return (mPosX == mOriginPosX && mPosY == mOriginPosY);
	}
}
