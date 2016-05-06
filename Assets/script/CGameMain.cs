using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public enum eMainRno
{
	init = 0,
	top_init,
	top,
	shuffle,
	puzzle,
}

public class CGameMain : MonoBehaviour {

	public GameObject piece_origin;
	public GameObject canvas;
	public GameObject shuffle_button_origin;

	private eMainRno mRno = eMainRno.init;
	private GameObject[,] mPieces = new GameObject[piece_num_in_line, piece_num_in_line];
	private GameObject mShuffleButton;

	private int mEmptyPosX;
	private int mEmptyPosY;

	private int mShuffleCounter;

	private const int piece_num = 15;
	public const int piece_num_in_line = 4;

	// Use this for initialization
	void Start ()
	{
		Application.targetFrameRate = 60;
	}

	// Update is called once per frame
	void Update ()
	{
		switch (mRno)
		{
			case eMainRno.init:
				{
					for(int i = 0; i < piece_num; ++i)
					{
						GameObject obj = Instantiate(piece_origin);
						CPiece script = obj.GetComponent<CPiece>();

						int pos_x = i % piece_num_in_line;
						int pos_y = i / piece_num_in_line;
						script.setup(0, i, pos_x, pos_y);

						mPieces[pos_x, pos_y] = obj;
					}

					mEmptyPosX = 3;
					mEmptyPosY = 3;

					mRno = eMainRno.top_init;
				}
				break;

			case eMainRno.top_init:
				{
					mShuffleButton = Instantiate(shuffle_button_origin);
					mShuffleButton.transform.SetParent(canvas.transform, false);
					mRno = eMainRno.top;
				}
				break;

			case eMainRno.top:
				{
					CShuffleButton script = mShuffleButton.GetComponent<CShuffleButton>();
					if( script != null && script.isClicked )
					{
						Destroy(mShuffleButton);
						mShuffleCounter = 0;
						mRno = eMainRno.shuffle;
					}
				}
				break;

			case eMainRno.shuffle:
				{
					for (int i = 0; i < 3; ++i)
					{
						int rand = Random.Range(0, piece_num_in_line);
						if (mShuffleCounter % 2 == 0)
						{
							ClickPiece(rand, mEmptyPosY);
						}
						else
						{
							ClickPiece(mEmptyPosX, rand);
						}
						++mShuffleCounter;
					}

					if( mShuffleCounter >= 300 )
					{
						if( checkComplete() )
						{
							// シャッフルしたのに揃っちゃってたのでやり直し（そんな奇跡起きないと思うけど）
							mShuffleCounter = 0;
						}
						else
						{
							mRno = eMainRno.puzzle;
						}
					}
				}
				break;

			case eMainRno.puzzle:
				{
					if(CAppUtil.GetTouch() == TouchInfo.Began)
					{
						Vector2 tapPoint = Camera.main.ScreenToWorldPoint(CAppUtil.GetTouchPosition());
						Collider2D collider = Physics2D.OverlapPoint(tapPoint);
						if (collider != null)
						{
							CPiece script = collider.gameObject.GetComponent<CPiece>();
							if(script != null && script.isMove() == false)
							{
								if( ClickPiece(script.posX, script.posY) )
								{
									if( checkComplete() )
									{
										mRno = eMainRno.top_init;
									}
								}

							}
						}

					}
				}
				break;
		}
	}

	// ピースをクリックして移動
	private bool ClickPiece(int pos_x, int pos_y)
	{
		if(pos_x == mEmptyPosX)
		{
			int dir = (mEmptyPosY < pos_y) ? 1 : -1;
			for (int y = mEmptyPosY; y != pos_y; y+=dir)
			{
				mPieces[pos_x, y + dir].GetComponent<CPiece>().setPos(pos_x, y);
				mPieces[pos_x, y] = mPieces[pos_x, y + dir];
			}
			mEmptyPosY = pos_y;
			return true;
		}
		if(pos_y == mEmptyPosY)
		{
			int dir = (mEmptyPosX < pos_x) ? 1 : -1;
			for (int x = mEmptyPosX; x != pos_x; x+=dir)
			{
				mPieces[x + dir, pos_y].GetComponent<CPiece>().setPos(x, pos_y);
				mPieces[x, pos_y] = mPieces[x + dir, pos_y];
			}
			mEmptyPosX = pos_x;
			return true;
		}
		return false;
	}

	// 終了チェック
	private bool checkComplete()
	{
		if(mEmptyPosX+1 != piece_num_in_line || mEmptyPosY+1 != piece_num_in_line)
			return false;
		
		for( int x = 0; x < piece_num_in_line; ++x )
		{
			for( int y = 0; y < piece_num_in_line; ++y )
			{
				GameObject obj = mPieces[x, y];
				if( obj != null )
				{
					if( obj.GetComponent<CPiece>().isComplete() == false )
					{
						return false;
					}
				}
			}
		}
		return true;
	}
}
