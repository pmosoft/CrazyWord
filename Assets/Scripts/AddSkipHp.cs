using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;

public class AddSkipHp : MonoBehaviour {

	//--------------------------
	// sqlite
	//--------------------------
	public static SqliteDatabase sql;
	public static string dbPathName;
	public static DataTable _data;
	public static string query;
	public static DataRow dr;

	int addHpSecond;
	int addSkipSecond;
	int addFreeHp;
	int addFreeSkip;

	int minute1 = 60;

	void Awake(){
		SqliteDBConn();
	}

	void Start() {
		//StartCoroutine (addSkip());
        
        // if allpack is purchased
        if(g.partMove == "111111111" && g.noBannerYn == "Y" && g.userPicYn == "Y") {
            g.freeSkipMinuteTime = 32 / 2;
            g.freeHpMinuteTime = 12 / 2;
        }

        //g.freeSkipMinuteTime = 2;
        //g.freeHpMinuteTime = 1;

		StartCoroutine (addSkip());
		StartCoroutine (addHp());
	}
	
	IEnumerator addSkip(){
		
		yield return new WaitForSeconds(1f);
		//g.hpAddTime--;
		//Debug.Log("hpAddTime1="+g.hpAddTime);
		
		sql.Open(dbPathName);
		query  = "select strftime('%s',datetime('now','localtime')) - strftime('%s',startTime) as addSkipSecond from AddSkip"; 
		//Debug.Log(query);
		_data = sql.ExecuteQuery(query); dr = _data.Rows[0];
		addSkipSecond = int.Parse(dr["addSkipSecond"].ToString());

        addFreeSkip = addSkipSecond / (minute1 * g.freeSkipMinuteTime); // freeSkipMinuteTime:32

        if(addFreeSkip + g.allSkip() > g.maxBonusSkipCnt) addFreeSkip = g.maxBonusSkipCnt - g.allSkip();
      
		//Debug.Log("addSkipSecond="+addSkipSecond);
		//Debug.Log("addFreeSkip="+addFreeSkip);
		//Debug.Log("g.freeSkip="+g.freeSkip);

		//Debug.Log("g.allSkip()="+g.allSkip());
		//Debug.Log("g.maxFreeSkipCnt="+g.maxFreeSkipCnt);

        if (addFreeSkip > 0 && g.allSkip() < g.maxBonusSkipCnt) { // maxFreeSkipCnt:3
			//Debug.Log("g.addFreeSkip="+g.addFreeSkip);
			g.freeSkip += addFreeSkip;
			//Debug.Log("g.freeSkip="+g.freeSkip);
			if(g.freeSkip > g.maxBonusSkipCnt) g.freeSkip = g.maxBonusSkipCnt;
			//g.hpAddTime = 5 * 60;
			//SELECT strftime('%s','2014-08-08 23:29:12') - strftime('%s','2014-08-08 23:29:09');
			
			query  = "update UserInfo set ";
			query += "freeSkip       = "+g.freeSkip         +",";
			query += "updateDate     = strftime('%Y%m%d','now','localtime'),updateTime = strftime('%H%M%S','now','localtime')";
			//Debug.Log(query);
			sql.ExecuteQuery(query);
			
			query  = "update AddSkip set startTime = datetime('now','localtime')"; 
			//Debug.Log(query);
			sql.ExecuteQuery(query);

		} else {
			g.skipAddTime = (minute1 * g.freeSkipMinuteTime) - addSkipSecond;
			if(g.skipAddTime < 0) g.skipAddTime = 0;

			//Debug.Log("g.skipAddTime="+g.skipAddTime);
		}
		sql.Close();
		
		StartCoroutine (addSkip());
		
	}

	IEnumerator addHp(){

		yield return new WaitForSeconds(1f);
		g.hpAddTime--;
		//Debug.Log("hpAddTime1="+g.hpAddTime);

		sql.Open(dbPathName);
		query  = "select strftime('%s',datetime('now','localtime')) - strftime('%s',startTime) as addHpSecond from AddHp"; 
		//Debug.Log(query);
		_data = sql.ExecuteQuery(query); dr = _data.Rows[0];
		addHpSecond = int.Parse(dr["addHpSecond"].ToString());

        addFreeHp = addHpSecond / (minute1 * g.freeHpMinuteTime); // freeHpMinuteTime:12

		//Debug.Log("g.freeHp="+g.freeHp+" g.maxBonusHpCnt="+g.maxBonusHpCnt+" addFreeHp="+addFreeHp+" g.allHp()="+g.allHp()+"   "+"g.maxBonusHpCnt="+g.maxBonusHpCnt);
        if(addFreeHp + g.allHp() > g.maxBonusHpCnt) addFreeHp = g.maxBonusHpCnt - g.allHp();
		//Debug.Log("addFreeHp="+addFreeHp);

        if (addFreeHp > 0 && g.allHp() < g.maxBonusHpCnt) { // maxFreeHpCnt:10
			//Debug.Log("g.freeHp="+g.freeHp);
			g.freeHp += addFreeHp;
			//Debug.Log("g.freeHp="+g.freeHp);
			if(g.freeHp > g.maxBonusHpCnt) g.freeHp = g.maxBonusHpCnt;
			//g.hpAddTime = 5 * 60;
			//SELECT strftime('%s','2014-08-08 23:29:12') - strftime('%s','2014-08-08 23:29:09');

			query  = "update UserInfo set ";
			query += "freeHp         = "+g.freeHp         +",";
			query += "updateDate     = strftime('%Y%m%d','now','localtime'),updateTime = strftime('%H%M%S','now','localtime')";
			//Debug.Log(query);
			sql.ExecuteQuery(query);

			query  = "update AddHp set startTime = datetime('now','localtime')"; 
			//Debug.Log(query);
			sql.ExecuteQuery(query);
		} else {
            g.hpAddTime = (minute1 * g.freeHpMinuteTime) - addHpSecond;
			if(g.hpAddTime < 0) g.hpAddTime = 0;
			//Debug.Log("hpAddTime="+g.hpAddTime);
		}
		sql.Close();

		StartCoroutine (addHp());

	}

	
	public void SqliteDBConn(){
		
        string path = Application.persistentDataPath + "/fb/com/dmp/";
        DirectoryInfo di = new DirectoryInfo(path);
        if (di.Exists == false) di.Create();

        dbPathName = Application.persistentDataPath + "/fb/com/dmp/"+g.sqliteName;
        //print (dbPathName);
        
        if (!System.IO.File.Exists(dbPathName)) {
            //Debug.Log("Exists ******  " + dbPathName);
            TextAsset ta = (TextAsset)Resources.Load("Sqlite/"+g.sqliteName+".db");
            System.IO.File.WriteAllBytes(dbPathName, ta.bytes); // 64MB limit on File.WriteAllBytes.
            ta = null;
        } else {
            Debug.Log("Exists ******  " + dbPathName);
        }
        sql = new SqliteDatabase();
		
	} 
}