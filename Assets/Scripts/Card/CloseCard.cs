﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


//GameObject 

public class CloseCard : BattleCard, IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler{
	//インスタンス変数 : カード種類，近距離のどのパターンか，量，を引数
	//ペアレントの自分の座標が必要
	[SerializeField]  GameManager gameManager = null; 
	[SerializeField] GameObject turnchar = null;
	[SerializeField] GameObject[,] mapChips;
	public void InitialCard(int amount,string cardtype,int range_id)
	{
		this.amount = amount;
		this.range_id = range_id;
		setCardtype(cardtype);
		//this.range = proceesing (range_id);
	}

	void Start()
	{
		gameManager = GameManager.Instance;//GameManager取得
		InitialCard (1,"Close",1);
		mapChips = gameManager.mapChips;
	}

	//接近の処理を作成 引数：キャラの位置
	public override List<Vector2> RangeCreate(Vector2 localPostion)
	{
		//nowTurnCharacter
		//ローカル座標を保持しておけばよいこれに足せば，それは全体のマップ座標になる
		range_id = this.range_id;
		List<Vector2> rangetemp = new List<Vector2>();
		switch (range_id) 
		{
			//closeの攻撃の仕方を記述をidで記述
			case 1:
				Debug.Log ("Range:" + range_id);
					//(i - 1,j),(i + 1 , j), (i,j - 1),(i,j + 1 )
				int[] xloop = { 1, -1 };
				int[] yloop = { 1, -1 };
				foreach (int i in xloop) 
				{
					if(localPostion.x +i < mapChips.GetLength(0) && localPostion.x+i >=0)
					{
						rangetemp.Add (new Vector2 (localPostion.x + i, localPostion.y));
					}
				}
				foreach (int j in yloop) 
				{
				if (localPostion.y + j >= 0 && localPostion.y +j < mapChips.GetLength (1)) 
					{
					rangetemp.Add (new Vector2 (localPostion.x, localPostion.y + j));
					}
				}
				Debug.Log (rangetemp);

				break;
		}
		this.range = rangetemp;
		return rangetemp;
	}

	//光らす関数
	void StartEmphasiss(List<Vector2>range)
	{
		List<GameObject> emphasiss = new List<GameObject>();
		emphasiss.Clear ();

		foreach (Vector2 i in range) 
		{
				emphasiss.Add (mapChips[(int)i.x,(int)i.y]);
		
		}

		GameManager.Instance.emphasisSprite.EnableEmphasiss(emphasiss);

	}

	//消す関数
	void EndEmphasiss()
	{
		GameManager.Instance.emphasisSprite.DisenableEmphasiss();
	}

	/*------------UGUIメソッドここから------------*/
	public void OnPointerEnter(PointerEventData eventData) 
	{
		//Debug.Log("CardOnMouseEnter");
		turnchar = gameManager.nowTurnCharacter;

		//マップ上を光らす
		StartEmphasiss(RangeCreate (turnchar.transform.position));
	}


	// カーソルが対象オブジェクトから出た時
	public void OnPointerExit(PointerEventData eventData) 
	{
		//Debug.Log("CardOnMouseExit");
		EndEmphasiss ();
	}

	// マウスボタンが押された時にコールされる
	public void OnPointerDown(PointerEventData eventData)
	{
		print("MouseDown!");
		//TODO:どのマウスに攻撃するか　範囲のキャラにダメージを与える，this.rangeのなかでマウスを選ぶ

		foreach (Vector2 i in this.range) 
		{
			attack (mapChips[(int)i.x,(int)i.y]);//mapchipsのゲームオブジェクトを渡す．

		}
	}

	//マップチップの座標の子供にタグがキャラ判定の人がいるかを判定し，アタック
	public void attack(GameObject map)
	{
		Debug.Log ("データ："+map.transform);

		//子供に全員にアタックしてるのでコード変えなきゃいけない
		foreach(Transform child in map.transform)
		{
			print(child.name + ":" + child.localPosition);
		}


	}

}
