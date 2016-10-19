using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;

using SQLite4Unity3d;


public class DBO : MonoBehaviour
{

    public class UserInfo {                            
        [PrimaryKey] public string userId { get; set; }
        public string userName    { get; set; }
        public string gameCode    { get; set; }
        public string nickName    { get; set; }
        public string deviceId    { get; set; }         
        public int    curStage    { get; set; }         
        public int    bestScore   { get; set; }         
        public int    coin        { get; set; }         
        public int    freeHp      { get; set; }         
        public int    buyHp       { get; set; }         
        public int    giftHp      { get; set; }         
        public int    itemHp      { get; set; }         
        public int    freeSkip    { get; set; }         
        public int    buySkip     { get; set; }         
        public int    giftSkip    { get; set; }         
        public int    itemSkip    { get; set; }         
        public string partMove    { get; set; }         
        public string noBannerYn  { get; set; }         
        public string userPicYn   { get; set; }         
        public string fbShareDate { get; set; }         
        public string friendYn    { get; set; }         
        public string giftYn      { get; set; }         
        public string achievement { get; set; }         
        public int    userBgCol   { get; set; }         
        public int    userBgImg   { get; set; }         
        public string fbId        { get; set; }         
        public string createDate  { get; set; }
        public string createTime  { get; set; }
        public string updateDate  { get; set; }
        public string updateTime  { get; set; }
    }

    public class Uft8Update {
        public string v1 { get; set; }
        public string v2 { get; set; }
        public string v3 { get; set; }
        public string v4 { get; set; }
        public string v5 { get; set; }
        public string v6 { get; set; }
    }

    public class FbUserInfo {
        [PrimaryKey] public string userId { get; set; }
        [PrimaryKey] public string fbId   { get; set; }
        public string fbName     { get; set; }
        public string fbPicUrl   { get; set; }
        public string fbUrl      { get; set; }
        public string fbGender   { get; set; }
        public string fbLocale   { get; set; }
        public string createDate { get; set; }
        public string createTime { get; set; }
        public string updateDate { get; set; }
        public string updateTime { get; set; }
    }

    public class FbFriendInfo {
        [PrimaryKey] public string fbId       { get; set; }
        [PrimaryKey] public string fbFriendId { get; set; }
        public string fbFriendName   { get; set; }
        public string fbFriendPicUrl { get; set; }
        public int    score          { get; set; }
        public string createDate     { get; set; }
        public string createTime     { get; set; }
        public string updateDate     { get; set; }
        public string updateTime     { get; set; }
    }


    //--------------------------
    // sqlite
    //--------------------------
    SqliteDatabase sql;
    string dbPathName;
    DataTable _data, _data1, _data2;
    string query;
    DataRow dr, dr1, dr2;



    void Awake()
    {
        SqliteDBConn();
    }

    private ISQLiteConnection _connection;

    void Start()
    {
        //test05();
    }


    public void test05() {
        var factory = new Factory();_connection = factory.Create(dbPathName);

        //del
        query = "delete from Uft8Update";
        Debug.Log("query=" + query);
        _connection.Execute(query);

        //ins
        _connection.InsertAll (new[]{new Uft8Update{v1="피승현2",v2="",v3="",v4="",v5="",v6=""}});

        //upt
        query = "update UserInfo set createDate = (select v1 from Uft8Update)";
        Debug.Log("query=" + query);
        _connection.Execute(query);

        _connection.Close();

    }

    void test04()
    {
        Debug.Log("test04");

        var factory = new Factory();_connection = factory.Create(dbPathName);

        query = "delete from FbFriendInfo";
        Debug.Log("query=" + query);
        _connection.Execute(query);

        _connection.InsertAll (new[]{
            new FbFriendInfo{
                 fbId           = "1483906351878206" 
                ,fbFriendId     = "744649378961239"
                ,fbFriendName   = "피승현"     
                ,fbFriendPicUrl = ""
                ,score          = 0
                ,createDate     = " "
                ,createTime     = " "
                ,updateDate     = ""        
                ,updateTime     = ""        
        
            }
        });

        _connection.Close();

    }

    void test03()
    {
        Debug.Log("test02");

        var factory = new Factory();
        _connection = factory.Create(dbPathName);

        query = "delete from imsi02 where stageNum = 1";
        Debug.Log("query=" + query);
        _connection.Execute(query);

        query = "insert into imsi02 values (1,'bb')";
        Debug.Log("query=" + query);
        _connection.Execute(query);

        imsi02 im2 = _connection.Table<imsi02>().Where(x => x.stageNum == 1).FirstOrDefault();

        Debug.Log("im2.texName=" + im2.texName);

        _connection.Update(new imsi02 { stageNum = 1, texName = "あいうぇえおか" } );



        //query = "update imsi02 set Name='aaaa'";

        //string query = "update imsi02 set Name='aaaa'";

        //query = string.Format("update Person set Name=\"{0}\" ", val1);

        ////System.Text.Encoding.UTF8.GetByteCount(query);
        ////_connection.Execute(query);
        ////_connection.ExecuteScalar

        ////Person pp = _connection.Table<Person>().Where(x => x.Name == "あいうぇえおか").FirstOrDefault();


        //_connection.Update(new Person { Id = 1, Name = "탐", Surname = "あいうぇえおか", Age = 56 });

        ////_connection.Execute("update Person set Name='abc'");

        _connection.Close();

    }

    void test02()
    {
        Debug.Log("test02");

        var factory = new Factory();

        var dbPath = string.Format(@"Assets/StreamingAssets/{0}", "existing.db");
        _connection = factory.Create(dbPath);
        Debug.Log("Final PATH: " + dbPath);


        var people = _connection.Table<Person>();
        Debug.Log("people=" + people.Count());

        foreach (Person p1 in people)
        {
            Debug.Log("p1.Name=" + p1.Name);
        }

        //CreateDB2();

        string query = "update Person set Name='あいうぇえおか'";

        string val1 = "あいうぇえおか";
        //val1 = "aaa";

        query = string.Format("update Person set Name=\"{0}\" ", val1);

        //System.Text.Encoding.UTF8.GetByteCount(query);
        //_connection.Execute(query);
        //_connection.ExecuteScalar

        //Person pp = _connection.Table<Person>().Where(x => x.Name == "あいうぇえおか").FirstOrDefault();


        _connection.Update(new Person { Id = 1, Name = "탐", Surname = "あいうぇえおか", Age = 56 });

        //_connection.Execute("update Person set Name='abc'");




        _connection.Close();

    }

    void Sqlite_Zone________() { }


    public void test01()
    {
        sql.Open(dbPathName);

        query = "insert into imsi01 values ('aaa')";

        //string convertTxt = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(query));
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();

    }


    //-------------------------------------------------------------------------
    //                              TextureInfo
    //-------------------------------------------------------------------------
    public DataTable SelTextureInfo(int stageNum, string texName) {

        sql.Open(dbPathName);

        query =  "select * from TextureInfo \n";
        query += " where stageNum=" + stageNum;
        if (texName.Length > 0) query += " and   texName='" + texName + "'";
        //Debug.Log(query);
        _data = sql.ExecuteQuery(query);
        sql.Close();

        return _data;
    }


    public DataTable SelTextureInfoTurn(int stageNum, int turnTexNum) {

        sql.Open(dbPathName);

        query =  "select * from TextureInfo ";
        query += " where stageNum=" + stageNum;
        query += " and   turnTexNum=" + turnTexNum;
        Debug.Log(query);

        _data = sql.ExecuteQuery(query);

        sql.Close();

        return _data;
    }

    public DataTable SelTextureInfoCorrect(int stageNum) {

        sql.Open(dbPathName);

        query  = "select a.*,b.state,b.comboYn                                                    \n";
        query += "from  TextureInfo a,              \n";
        query += "      UserCorrectHis b            \n";
        query += " where a.largeCode = b.largeCode  \n";
        query += " and a.middleCode  = b.middleCode \n";
        query += " and replace(a.texName,'_',' ') = replace(b.texName,'_',' ') \n";
        query += " and a.stageNum     = " + stageNum +"                        \n";
        query += " order by a.stageNum, a.turnTexNum ";

        //Debug.Log(query);

        _data = sql.ExecuteQuery(query);

        sql.Close();

        return _data;
    }


    // download
    public void UptTextureInfo(int stageNum,
                               int turnTexNum,
                               int texNum,
                               string texUrlYn,
                               string downYn
                               )
    {

        sql.Open(dbPathName);

        query = "update TextureInfo ";
        query += "set texUrlYn='" + texUrlYn + "'";
        query += "   ,downYn='" + downYn + "'";
        query += "where stageNum=" + stageNum;
        query += "  and turnTexNum=" + turnTexNum;
        query += "  and texNum=" + texNum;
        //Debug.Log(query);

        _data = sql.ExecuteQuery(query);

        sql.Close();

    }

    // download
    public void UptTextureInfo2(int stageNum,
                               int turnTexNum,
                               int texNum,
                               string reYn
                               )
    {

        sql.Open(dbPathName);

        query = "update TextureInfo ";
        query += "set updateTime='" + reYn + "'";
        query += "where stageNum=" + stageNum;
        query += "  and turnTexNum=" + turnTexNum;
        query += "  and texNum=" + texNum;
        //Debug.Log(query);

        _data = sql.ExecuteQuery(query);

        sql.Close();

    }


    // Download
    public DataTable SelCopyTextureZone()
    {

        sql.Open(dbPathName);

        query =  " select * from TextureInfo             \n";
        query += " where stageNum > 0                    \n";
        query += " and stageNum = " + g.curStage +"      \n";
        query += " order by stageNum,turnTexNum          \n";

        //Debug.Log(query);

        _data = sql.ExecuteQuery(query);

        sql.Close();

        return _data;
    }


    // Download
    public DataTable  SelDownTextureZone(string kind)
    {

        sql.Open(dbPathName);


        if (kind == "real") {
            query =  " select * from PhotoInfo                            \n";
            query += " where stageNum > 0                                   \n";
            query += " and stageNum = " + g.curStage                           ;
            query += " order by stageNum,turnTexNum,texNum           \n";
        } else if (kind == "all") {
            query = "select * from PhotoInfo where texNum<=4 and texUrlYn = 'Y' and downYn='N' and stageNum between 46 and 50";

            //query = "select * from PhotoInfo where texName = 'CASUAL' and texNum = 3";

            //query = "select * from PhotoInfo where largeCode between 2 and 3 and photoSource = 'manual'";
            //query += " and texName ='FRACTION'";

        } else if (kind == "bulk") {
            //query =  " select * from PhotoInfo                            \n";
            //query += " where stageNum > 0                                   \n";
            //query += " and texUrlYn = 'Y' and downYn = 'N'                  \n";
            //query += " and texName in (";
            //query += " 'DECIMAL','CASUAL','UNIFORM','FOUNDATION','ACCOMPANIMENT','CANNED_FOOD','CHICKEN','PORK'              ";
            //query += ",'STARCH_SYRUP','SPICES','VINEGAR','FRY','RECIPE','STEREO','SCOOP','DUSTPAN','RIB','KIDNEY'              ";
            //query += " 'LIVER','LUNG','PANCREAS','WIND','PHYSICAL','ANEMIA','HANGOVER','HEART_ATTACK','OVERWORK'             ";
            //query += ",'DERMATOLOGY','HOSPITAL_GOWN','NEUROLOGY','PLASTIC_SURGERY','RADIOLOGY','PAINKILLER','PLASTER','DIVORCE','HUMAN_LIFE','MAN'   ";
            //query += ") ";
            //query += " and updatetime = 'Y'                                 \n";

            //query += " order by stageNum,turnTexNum,texNum           \n";

            query =  " select * from PhotoInfo  where length(photoLink) > 10  \n";
            //query += " where stageNum > 0                                   \n";
            //query += " and stageNum = " + g.curStage                      \n";
            //query += " and texName in ('ESCALATOR')                         \n";
            query += " order by stageNum,turnTexNum,texNum           \n";

        }

        Debug.Log(query);

        _data = sql.ExecuteQuery(query);

        sql.Close();

        return _data;
    }

    // ViewTex
    public DataTable  SelShowTextureAuto()
    {

        sql.Open(dbPathName);

        query =  " select distinct stageNum,turnTexNum,texName from PhotoInfo    \n";
        query += " where stageNum > 0                                   \n";
        //query += " and texUrlYn = 'Y' and downYn = 'N'                  \n";
        query += " order by volNum,stageNum,turnTexNum,texNum           \n";

        //Debug.Log(query);

        _data = sql.ExecuteQuery(query);

        sql.Close();

        return _data;
    }

    // ViewTex
    public DataTable  SelShowTexture(int stageNum,int turnTexNum)
    {

        sql.Open(dbPathName);

        query =  " select stageNum,turnTexNum,texName,updateTime from PhotoInfo  \n";
        query += " where stageNum = "+stageNum+"                                 \n";
        query += " and turnTexNum = "+turnTexNum+"                               \n";
        query += " order by stageNum,turnTexNum,texNum                    \n";

        //Debug.Log(query);

        _data = sql.ExecuteQuery(query);

        sql.Close();

        return _data;
    }

    // ViewTex
    public DataTable  SelShowTextureBack(int stageNum,int turnTexNum)
    {

        int stageNum1, turnTexNum1;
        stageNum1 = stageNum;
        turnTexNum1 = turnTexNum;

        sql.Open(dbPathName);

        if(turnTexNum1 == 1) { stageNum1--; turnTexNum1=5;}
        else if(turnTexNum1 == 2) { turnTexNum1=1;}
        else if(turnTexNum1 == 3) { turnTexNum1=2;}
        else if(turnTexNum1 == 4) { turnTexNum1=3;}
        else if(turnTexNum1 == 5) { turnTexNum1=4;}
        if(stageNum1 < 1) stageNum1 = 1;

        query  = " select * from PhotoInfo \n";
        query += " where stageNum = "+stageNum1;
        query += " and turnTexNum = "+turnTexNum1;

        //Debug.Log(query);

        _data = sql.ExecuteQuery(query);

        sql.Close();

        return _data;
    }

    // ViewTex
    public DataTable  SelShowTextureFowd(int stageNum,int turnTexNum)
    {
        int stageNum1, turnTexNum1;
        stageNum1 = stageNum;
        turnTexNum1 = turnTexNum;

        sql.Open(dbPathName);

        if(turnTexNum1 == 1) { turnTexNum1=2;}
        else if(turnTexNum1 == 2) { turnTexNum1=3;}
        else if(turnTexNum1 == 3) { turnTexNum1=4;}
        else if(turnTexNum1 == 4) { turnTexNum1=5;}
        else if(turnTexNum1 == 5) { stageNum1++; turnTexNum1=1;}
        if(stageNum1 > 200) stageNum1 = 200;

        query  = " select * from PhotoInfo \n";
        query += " where stageNum = "+stageNum1;
        query += " and turnTexNum = "+turnTexNum1;

        //Debug.Log(query);

        _data = sql.ExecuteQuery(query);

        sql.Close();

        return _data;
    }

    //-------------------------------------------------------------------------
    //                              PhotoInfo
    //-------------------------------------------------------------------------
    public DataTable SelPhotoInfo(int texNum)
    {
        sql.Open(dbPathName);

        query  = " select * from PhotoInfo ";
        query += " where stageNum = "+g.curStage;
        query += " and turnTexNum = "+g.stageTurn;
        query += " and texNum = "+texNum;

        //Debug.Log(query);

        _data = sql.ExecuteQuery(query);

        sql.Close();

        return _data;
    }

    public DataTable SelPhotoInfo2(int stageNum, string texName, int texNum)
    {
        sql.Open(dbPathName);

        query  = " select * from PhotoInfo ";
        query += " where stageNum = "+stageNum;
        query += " and texName = '"+texName+"'";
        query += " and texNum = "+texNum;

        //Debug.Log(query);

        _data = sql.ExecuteQuery(query);

        sql.Close();

        return _data;
    }

    public DataTable SelPhotoInfo3(int stageNum)
    {
        sql.Open(dbPathName);

        query  = " select * from PhotoInfo ";
        query += " where stageNum = "+stageNum;
        query += " order by stageNum,turnTexNum,texNum";

        //Debug.Log(query);

        _data = sql.ExecuteQuery(query);

        sql.Close();

        return _data;
    }


    //-------------------------------------------------------------------------
    //                              UserInfo
    //-------------------------------------------------------------------------

    //  public void SelUserInfo(){
    //      
    //      sql.Open(dbPathName);
    //      
    //      query = "select * from UserInfo";
    //      _data = sql.ExecuteQuery(query); dr=_data.Rows[0];
    //
    //      sql.Close();
    //
    //  }

    public DataTable SelUserInfo()
    {

        sql.Open(dbPathName);

        query = "select * from UserInfo";
        //Debug.Log(query);
        _data = sql.ExecuteQuery(query); dr = _data.Rows[0];

        DataTable _returnData = _data;

        g.userId = dr["userId"].ToString();
        g.deviceId = dr["deviceId"].ToString();
        g.nickName = dr["nickName"].ToString();
        g.curStage = int.Parse(dr["curStage"].ToString());
        if( g.bestScore < int.Parse(dr["bestScore"].ToString()) ) {
            g.bestScore = int.Parse(dr["bestScore"].ToString());
        }
        g.coin = int.Parse(dr["coin"].ToString());
        g.partMove = dr["partMove"].ToString();
        g.noBannerYn = dr["noBannerYn"].ToString();
        g.userPicYn = dr["userPicYn"].ToString();
        g.achievement = dr["achievement"].ToString();
        
        g.freeHp = int.Parse(dr["freeHp"].ToString());
        g.buyHp = int.Parse(dr["buyHp"].ToString());
        g.giftHp = int.Parse(dr["giftHp"].ToString());
        g.itemHp = int.Parse(dr["itemHp"].ToString());
        g.freeSkip = int.Parse(dr["freeSkip"].ToString());
        g.buySkip = int.Parse(dr["buySkip"].ToString());
        g.giftSkip = int.Parse(dr["giftSkip"].ToString());
        g.itemSkip = int.Parse(dr["itemSkip"].ToString());
        g.fbShareDate = dr["fbShareDate"].ToString();

        //g.friendYn = dr["friendYn"].ToString();
        //g.giftYn = dr["giftYn"].ToString();

        g.userBgCol = int.Parse(dr["userBgCol"].ToString());
        g.userBgImg = int.Parse(dr["userBgImg"].ToString());

        g.fbId = dr["fbId"].ToString();

        query = "select ifnull(max(stageNum),'0') maxStageNum from TextureInfo";
        //Debug.Log(query);
        _data = sql.ExecuteQuery(query); dr = _data.Rows[0];
        g.maxStage = int.Parse(dr["maxStageNum"].ToString());



        sql.Close();

        return _returnData;
    }

    public void DelUserInfo()
    {

        sql.Open(dbPathName);

        query = "delete from UserInfo";
        sql.ExecuteQuery(query);

        sql.Close();
    }

    public void CreateUser()
    {

        Debug.Log("CreateUser");

        string deviceId = GetUniqueId();

        var factory = new Factory();_connection = factory.Create(dbPathName);

        query = "delete from UserInfo";
        //Debug.Log("query=" + query);
        _connection.Execute(query);

        _connection.InsertAll (new[]{
            new UserInfo{
                 userId       = g.googleId
                ,gameCode     = g.gameCode
                ,userName     = g.googleName
                ,nickName     = g.nickName
                ,deviceId     = deviceId
                ,curStage     = 1
                ,bestScore    = 0
                ,coin         = 0
                ,freeHp       = g.maxBonusHpCnt
                ,buyHp        = 0              
                ,giftHp       = 0              
                ,itemHp       = 0              
                ,freeSkip     = g.maxBonusSkipCnt 
                ,buySkip      = 0              
                ,giftSkip     = 0              
                ,itemSkip     = 0
                ,partMove     = "100000000"    
                ,noBannerYn   = "N"            
                ,userPicYn    = "N"
                // 오픈전 임시 tester              
                //,partMove     = "111111111"    
                //,noBannerYn   = "Y"            
                //,userPicYn    = "Y"
                ,fbShareDate  = "20140101"    
                ,friendYn     = "N"            
                ,giftYn       = "N"            
                ,achievement  = "00000000000000000000000000000000000000000000000000"   
                ,userBgCol    = 0              
                ,userBgImg    = 0              
                ,fbId         = ""             
                ,createDate   = " "
                ,createTime   = " " 
                ,updateDate   = ""  
                ,updateTime   = ""  
            }
        });

        _connection.Close();


            sql.Open(dbPathName);

        query = "delete from UserCorrectHis";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        query = "insert into UserCorrectHis                                 \n";
        query += "select a.stageNum                           as stageNum   \n"; 
        query += "      ,a.texName                            as texName    \n"; 
        query += "      ,a.texShowName                        as texShowName\n"; 
        query += "      ,a.turnTexNum                         as turnTexNum \n"; 
        query += "      ,'0'                                  as state      \n"; 
        query += "      ,a.largeCode                          as largeCode  \n"; 
        query += "      ,a.middleCode                         as middleCode \n"; 
        query += "      ,'N'                                  as comboYn    \n"; 
        query += "      ,strftime('%Y%m%d','now','localtime') as createDate \n"; 
        query += "      ,strftime('%H%M%S','now','localtime') as createTime \n"; 
        query += "      ,''                                   as updateDate \n"; 
        query += "      ,''                                   as updateTime \n"; 
        query += "from  TextureInfo a                                       \n";
        query += "group by a.stageNum                                       \n";
        query += "        ,a.texName                                        \n";
        query += "        ,a.texShowName                                    \n";
        query += "        ,a.largeCode                                      \n";
        query += "        ,a.middleCode                                     \n";
        query += "order by a.stageNum                                       \n";
        query += "        ,a.turnTexNum                                     \n";

        //Debug.Log(query);
        sql.ExecuteQuery(query);


        query = "delete from UserStageHis";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        query = "insert into UserStageHis ";
        query += "select                   ";
        query += "'" + g.googleId + "'";
        query += ",a.stageNum              ";
        query += ",0 as stageVitualMaxScore";
        query += ",0 as stageMaxScore      ";
        query += ",0 as stageScore         ";
        query += ",0 as stageMaxRank       ";
        query += ",0 as stageRank          ";
        query += ",case when a.stageNum < 10 then 'Y' else 'N' end as clearYn";
        // 오픈전 임시
        //query += ",'Y' as clearYn";
        query += ",min(a.largeCode)  as largeCode  ";
        query += ",min(a.middleCode) as middleCode ";
        query += ",strftime('%Y%m%d','now','localtime') ";
        query += ",strftime('%H%M%S','now','localtime') ";
        query += ",''                      ";
        query += ",''                      ";
        query += "from TextureInfo a       ";
        query += "group by a.stageNum      ";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        query = "delete from FbUserInfo";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        query = "delete from FbFriendInfo";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        query = "delete from FbGiftHpHis";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        query = "delete from UserGiftHis";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        query = "delete from UserStarAchievment";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        query = "delete from CrazyWordPopup";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        query  = "insert into CrazyWordPopup                             \n";
        query += "select                                                 \n";
        query += "'2015010101'                          as popupId       \n";
        query += ",'20150101'                           as validStartDate \n";
        query += ",'99991231'                           as validEndDate   \n";
        query += ",''                                   as pupupDownUrl  \n";
        query += ",''                                   as pupupTexName  \n";
        query += ",'Y'                                  as showYn        \n";
        query += ",strftime('%Y%m%d','now','localtime')                  \n";
        query += ",strftime('%H%M%S','now','localtime')                  \n";
        query += ",''                                                    \n";
        query += ",''                                                    \n";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        // imsi
        //query  = "insert into CrazyWordPopup values ('2015031601','20150316','20150416','http://crazyword.org/popupimage/2015031601.png','2015031601.png','Y',strftime('%Y%m%d','now','localtime'),strftime('%H%M%S','now','localtime'),'','')";
        //sql.ExecuteQuery(query);
        //query  = "insert into CrazyWordPopup values ('2015031602','20150316','20150416','http://crazyword.org/popupimage/2015031602.png','2015031602.png','Y',strftime('%Y%m%d','now','localtime'),strftime('%H%M%S','now','localtime'),'','')";
        //sql.ExecuteQuery(query);
        //query  = "insert into CrazyWordPopup values ('2015031603','20150316','20150416','http://crazyword.org/popupimage/2015031603.png','2015031603.png','Y',strftime('%Y%m%d','now','localtime'),strftime('%H%M%S','now','localtime'),'','')";
        //sql.ExecuteQuery(query);

        sql.Close();

    }

    public bool IsUserExist()
    {
        //print ("IsUserExist");

        sql.Open(dbPathName);

        query = "select count(*) cnt from UserInfo";
        //Debug.Log(query);
        _data = sql.ExecuteQuery(query); dr = _data.Rows[0];

        sql.Close();

        if (int.Parse(dr["cnt"].ToString()) > 0) return true;
        else return false;

    }


    public void UptUserInfo()
    {

        sql.Open(dbPathName);

        query = "update UserInfo set ";
        query += "freeHp     = " + g.freeHp + ",";
        query += "freeSkip   = " + g.freeSkip + ",";
        query += "curStage   = " + g.curStage + ",";
        query += "updateDate = strftime('%Y%m%d','now','localtime'),updateTime = strftime('%H%M%S','now','localtime') ";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();
    }

    ///* userName    */ query += "null,";
    ///* userPicUrl  */ query += "null,";
    ///* userFbUrl   */ query += "null,";
    ///* userGender  */ query += "null,";
    ///* userLocale  */ query += "null,";


    public void UptSkip(string skipKind)
    { // free,buy,gift

        sql.Open(dbPathName);

        query = "update UserInfo set ";
        if (skipKind == "free") query += "freeSkip = " + g.freeSkip + ", ";
        if (skipKind == "buy")  query += "buySkip  = " + g.buySkip + ", ";
        if (skipKind == "gift") query += "giftSkip = " + g.giftSkip + ", ";
        if (skipKind == "item") query += "itemSkip = " + g.itemSkip + ", ";
        query += "updateDate     = strftime('%Y%m%d','now','localtime'),updateTime = strftime('%H%M%S','now','localtime') ";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();
    }

    public void UptSkipImsi()
    { // free,buy,gift

        sql.Open(dbPathName);

        query = "update UserInfo set itemSkip = 999,itemHp=999";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();
    }


    public void UptHp(string hpKind)
    { // free,buy,gift

        sql.Open(dbPathName);

        query = "update UserInfo set ";
        if (hpKind == "free") query += "freeHp = " + g.freeHp + ", ";
        if (hpKind == "buy")  query += "buyHp  = " + g.buyHp + ", ";
        if (hpKind == "gift") query += "giftHp = " + g.giftHp + ", ";
        if (hpKind == "item") query += "itemHp = " + g.itemHp + ", ";
        query += "updateDate     = strftime('%Y%m%d','now','localtime'),updateTime = strftime('%H%M%S','now','localtime') ";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();
    }

    public void UptUserInfoSkip1()
    {
        sql.Open(dbPathName);

        query = "update UserInfo set ";
        query += "buySkip = buySkip+1, ";
        query += "updateDate     = strftime('%Y%m%d','now','localtime'),updateTime = strftime('%H%M%S','now','localtime') ";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();
    }

    public void UptUserInfoHp1()
    {
        sql.Open(dbPathName);

        query = "update UserInfo set ";
        query += "buyHp = buyHp+1, ";
        query += "updateDate     = strftime('%Y%m%d','now','localtime'),updateTime = strftime('%H%M%S','now','localtime') ";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();
    }


    public void ImsiUptHpSkip()
    { // free,buy,gift

        sql.Open(dbPathName);
        query = "update UserInfo set freeHp=0,buyHp=0,giftHp=0,itemHp=0,freeSkip=0,buySkip=0,giftSkip=0,itemSkip=0";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();
    }


    public void UptUserInfoBestScore()
    {
        sql.Open(dbPathName);

        query = "update UserInfo set ";
        query += "bestScore      = " + g.bestScore + ",";
        query += "updateDate     = strftime('%Y%m%d','now','localtime'),updateTime = strftime('%H%M%S','now','localtime')";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();
    }


    public void UptUserInfoCoin()
    {

        sql.Open(dbPathName);

        query = "update UserInfo set ";
        query += "coin           = " + g.coin + ",";
        query += "updateDate     = strftime('%Y%m%d','now','localtime'),updateTime = strftime('%H%M%S','now','localtime')";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();
    }

    public void UptUserInfoPartMove()
    {
        sql.Open(dbPathName);

        query = "update UserInfo set ";
        query += "partMove      = " + g.partMove + ",";
        query += "updateDate     = strftime('%Y%m%d','now','localtime'),updateTime = strftime('%H%M%S','now','localtime')";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();
    }


    public void UptUserInfoAllPack()
    {
        sql.Open(dbPathName);

        query = "update UserInfo set ";
        query += "partMove = '111111111',";
        query += "noBannerYn = 'Y',";
        query += "userPicYn = 'Y',";
        query += "updateDate     = strftime('%Y%m%d','now','localtime'),updateTime = strftime('%H%M%S','now','localtime')";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        g.partMove = "111111111";
        g.noBannerYn = "Y";
        g.userPicYn = "Y";

        sql.Close();
    }

    public void UptUserInfoNoBanner()
    {
        sql.Open(dbPathName);

        query = "update UserInfo set ";
        query += " noBannerYn     = 'Y',";
        query += " updateDate     = strftime('%Y%m%d','now','localtime'),updateTime = strftime('%H%M%S','now','localtime')";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();
    }


    public void UptUserInfoFbShareDate()
    {
        sql.Open(dbPathName);

        query = "update UserInfo set ";
        query += "fbShareDate    = '" + g.wwwDate + "',";
        query += "updateDate     = strftime('%Y%m%d','now','localtime'),updateTime = strftime('%H%M%S','now','localtime')";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        g.partMove = "111111111";
        g.noBannerYn = "Y";
        g.userPicYn = "Y";

        sql.Close();
    }


    public void UptUserInfoAchievement(string achievement)
    {
        sql.Open(dbPathName);

        query = "update UserInfo set ";
        query += " achievement    = '" + achievement + "',";
        query += " giftYn         = 'Y',";
        query += " updateDate     = strftime('%Y%m%d','now','localtime'),updateTime = strftime('%H%M%S','now','localtime')";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();
    }

    public void UptUserInfoUserBgCol()
    {
        sql.Open(dbPathName);

        query = "update UserInfo set ";
        query += "userBgCol      = " + g.userBgCol + ",";
        query += "updateDate     = strftime('%Y%m%d','now','localtime'),updateTime = strftime('%H%M%S','now','localtime')";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();
    }

    public void UptUserInfoUserBgImg()
    {
        sql.Open(dbPathName);

        query = "update UserInfo set ";
        query += " userBgImg      = " + g.userBgImg + ",";
        query += " updateDate     = strftime('%Y%m%d','now','localtime'),updateTime = strftime('%H%M%S','now','localtime')";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();
    }


    public void UptUserFbId(string fbId)
    {
        sql.Open(dbPathName);

        query = "update UserInfo set ";
        query += " fbId = '"+fbId+"',";
        query += " updateDate = strftime('%Y%m%d','now','localtime'),updateTime = strftime('%H%M%S','now','localtime') ";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();
    }

    public void UptUserNickName()
    {
        sql.Open(dbPathName);

        query = "update UserInfo set ";
        query += " nickName = '"+g.nickName+"',";
        query += " updateDate = strftime('%Y%m%d','now','localtime'),updateTime = strftime('%H%M%S','now','localtime') ";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();
    }

    public void UptUserInfoWww(
      string userId
    , int bestScore
    , int coin
    , string partMove
    , string noBannerYn
    , string userPicYn
    , string fbShareDate
    , string friendYn
    , string giftYn1
    , string achievement
    )
    {

        Debug.Log("local UptUserInfoWww");

        sql.Open(dbPathName);

        query = "update UserInfo set ";
        query += "bestScore    = (case when bestScore<"+bestScore+" then "+bestScore+" else bestScore end),";
        query += "coin         = '" + coin + "', ";
        query += "partMove     = '" + partMove + "', ";
        query += "noBannerYn   = '" + noBannerYn + "', ";
        query += "userPicYn    = '" + userPicYn + "', ";
        query += "fbShareDate  = '" + fbShareDate + "', ";
        query += "friendYn   = '" + friendYn + "', ";
        query += "giftYn   = '" + giftYn1 + "', ";
        query += "achievement   = '" + achievement + "', ";
        query += "updateDate   = strftime('%Y%m%d','now','localtime'),updateTime = strftime('%H%M%S','now','localtime') ";
        query += "where userId = '" + userId + "' ";

        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();
    }

    public void UptFbUserInfo()
    {

        sql.Open(dbPathName);

        query = "update FbUserInfo set ";
        query += " freeHp     = " + g.freeHp + ",";
        query += " freeSkip   = " + g.freeSkip + ",";
        query += " curStage   = " + g.curStage + ",";
        query += " updateDate = strftime('%Y%m%d','now','localtime'),updateTime = strftime('%H%M%S','now','localtime') ";
        query += " where userId = '" + g.userId + "' and userIdKind = 1 ";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();
    }



    //-------------------------------------------------------------------------
    //                             UserStageTurnHis
    //-------------------------------------------------------------------------


    //  public DataTable SelUserStageTurnHis(int gameKind){
    //      sql.Open(dbPathName);
    //      
    //      query =  "select * from UserStageTurnHis";
    //      query += " where stageNum = "+g.curStage;
    //      query += " and   turnNum  = "+g.stageTurn;
    //      query += " and   gameKind = "+gameKind;
    //
    //      //Debug.Log(query);
    //      _data = sql.ExecuteQuery (query);
    //
    //      sql.Close();
    //      
    //      return _data;
    //  }

    public DataTable SelUserStageTurnHisScore()
    {
        sql.Open(dbPathName);

        query = "select a.stageNum                  ";
        query += "      ,sum(a.gameScore) stageScore ";
        query += "      ,max(a.comboCnt)  maxCombo   ";
        query += "from UserStageTurnHis a            ";
        query += " where a.stageNum = " + g.curStage;
        query += " group by a.stageNum               ";

        //Debug.Log(query);
        _data = sql.ExecuteQuery(query);

        sql.Close();

        return _data;
    }


    public void DelUserStageTurnHis()
    {

        sql.Open(dbPathName);

        query = "delete from UserStageTurnHis ";
        query += " where stageNum = " + g.curStage;

        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();

    }

    public void UptUserId()
    {

        sql.Open(dbPathName);

        query = "update UserInfo set userId ='" + g.userId + "';";
        query += "update FbUserInfo set userId ='" + g.userId + "';";
        query += "update UserStageHis set userId ='" + g.userId + "';";

        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();

    }



    public void UptUserStageTurnHisScore()
    {

        sql.Open(dbPathName);

        query = "update UserStageTurnHis set ";
        query += " gameScore = 0";
        query += " where stageNum = " + g.curStage;
        query += " and   turnNum  = " + g.stageTurn;

        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();

    }



    public void InsUserStageTurnHis(int gameKind, int gameScore)
    {

        sql.Open(dbPathName);

        query = "delete from UserStageTurnHis ";
        query += " where stageNum = " + g.curStage;
        query += " and   turnNum  = " + g.stageTurn;
        query += " and   gameKind = " + gameKind;

        //Debug.Log(query);
        sql.ExecuteQuery(query);

        query = "insert into UserStageTurnHis values (";
        query += g.curStage + ",";                        // stageNum  
        query += g.stageTurn + ",";                       // turnNum   
        query += gameKind + ",";                          // gameKind  
        query += gameScore + ",";                         // gameScore 
        query += g.comboCnt + ",";                        // comboCnt  
        query += "'" + g.texName + "',";                  // texName   
        query += "strftime('%Y%m%d','now','localtime'),"; // createDate
        query += "strftime('%H%M%S','now','localtime'),"; // createTime
        query += "'',";                                   // updateDate
        query += "'')";                                   // updateTime
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();

    }



    //-------------------------------------------------------------------------
    //                           UserCorrectHis
    //-------------------------------------------------------------------------

    public DataTable SelUserCorrectHisCode()
    {
        sql.Open(dbPathName);

        query = "select stageNum,largeCode,middleCode \n";
        query += "from UserCorrectHis                 \n";
        query += "where turnTexNum = 1                \n";
        query += "order by stageNum                   \n";

        //Debug.Log(query);
        _data = sql.ExecuteQuery(query);

        sql.Close();

        return _data;
    }

    public int SelUserCorrectHisComboCnt()
    {
        sql.Open(dbPathName);

        query = "select count(comboYn) comboCnt \n";
        query += "from UserCorrectHis           \n";
        query += "where comboYn = 'Y'           \n";
        //Debug.Log(query);

        _data = sql.ExecuteQuery(query); dr = _data.Rows[0];
        int comboCnt = int.Parse(dr["comboCnt"].ToString());
        sql.Close();

        return comboCnt;
    }

    public int SelUserCorrectHisComboLargeCnt()
    {
        sql.Open(dbPathName);

        query = "select ifnull(count(*),0) noComboLargeCnt \n";
        query += "from (                         \n";
        query += "      select largeCode         \n";
        query += "      from UserCorrectHis      \n";
        query += "      where stageNum > 0       \n";
        query += "      and   comboYn = 'N'      \n";
        query += "      group by largeCode       \n";
        query += "      )                        \n";
        //Debug.Log(query);  

        _data = sql.ExecuteQuery(query); dr = _data.Rows[0];
        int comboCnt = int.Parse(dr["noComboLargeCnt"].ToString());
        sql.Close();

        return comboCnt;
    }


    public string SelUserCorrectHisComboYn()
    {
        sql.Open(dbPathName);

        query = "select comboYn \n";
        query += "from UserCorrectHis           \n";
        query += " where stageNum = " + g.curStage;
        query += " and   turnTexNum = " + g.stageTurn;
        //Debug.Log(query);

        _data = sql.ExecuteQuery(query); dr = _data.Rows[0];
        string comboYn= dr["comboYn"].ToString();
        sql.Close();

        return comboYn;
    }










    public void UptCorrectHis(int gameState)
    {

        sql.Open(dbPathName);

        query = "update UserCorrectHis set ";
        query += "state          = '" + gameState + "',";
        query += "updateDate     = strftime('%Y%m%d','now','localtime'),updateTime = strftime('%H%M%S','now','localtime') ";
        query += "where texName  = '" + g.texName + "' "
            ;

        //Debug.Log(query);

        sql.ExecuteQuery(query);

        sql.Close();
    }


    public void UptCorrectHisComboYn()
    {

        sql.Open(dbPathName);

        query = "update UserCorrectHis set ";
        query += " comboYn = 'Y',";
        query += " updateDate     = strftime('%Y%m%d','now','localtime'),updateTime = strftime('%H%M%S','now','localtime') ";
        query += " where stageNum = " + g.curStage;
        query += " and   texName = '" + g.texName + "'";
        ;

        //Debug.Log(query);

        sql.ExecuteQuery(query);

        sql.Close();
    }


    public DataTable SelTotalLetterCnt()
    {

        sql.Open(dbPathName);

        query = "select  sum(length(trim(texShowName))) totalLetterCnt ";
        query += "       ,count(texname) turnCnt ";
        query += "       ,sum(length(texName))/count(texname) avgLetterCnt ";
        query += "from   UserCorrectHis ";
        query += "where  stageNum = '" + g.curStage + "' ";

        //Debug.Log(query);
        _data = sql.ExecuteQuery(query); dr = _data.Rows[0];

        sql.Close();

        return _data;
    }

    public DataTable SelPartStageNum(int largeCode)
    {
        sql.Open(dbPathName);

        query = "select min(stageNum) minStageNum,max(stageNum) maxStageNum from UserCorrectHis where largeCode = " + largeCode;

        //Debug.Log(query);
        _data = sql.ExecuteQuery(query); dr = _data.Rows[0];

        sql.Close();

        return _data;
    }


    public DataTable SelPartStartStageNum()
    {
        sql.Open(dbPathName);

        query  = "select largeCode,min(stageNum) minStageNum,max(stageNum) maxStageNum ";
        query += " from UserCorrectHis where stageNum > 0 group by largeCode ";

        //Debug.Log(query);
        _data = sql.ExecuteQuery(query); dr = _data.Rows[0];

        sql.Close();

        return _data;
    }

    //-------------------------------------------------------------------------
    //                             UserStageHis
    //-------------------------------------------------------------------------

    public DataTable SelUserStageHis(int stageNum)
    {
        sql.Open(dbPathName);

        query = "select * from UserStageHis where stageNum=" + stageNum;
        //Debug.Log(query);
        _data = sql.ExecuteQuery(query); dr = _data.Rows[0];

        sql.Close();

        return _data;
    }

    public int SelUserStageHisTotalScore()
    {
        int totalScore = 0;
        sql.Open(dbPathName);

        query = "select sum(stageScore) totalScore from UserStageHis where stageNum >= 1";
        //Debug.Log(query);
        _data = sql.ExecuteQuery(query); dr = _data.Rows[0];
        totalScore = int.Parse(dr["totalScore"].ToString());
        sql.Close();

        return totalScore;
    }

    public DataTable SelUserStageHisRankCnt()
    {
        sql.Open(dbPathName);

        query  = "select ifnull(sum(stageRank),0) totalRankCnt,count(*) clearStageCnt \n";
        query += " from UserStageHis a                                      \n";
        query += " where stageNum > 0                                       \n";
        query += " and stageRank > 0                                        \n";
        //Debug.Log(query);
        _data = sql.ExecuteQuery(query);

        sql.Close();

        return _data;
    }

    public void UptUserStageHis()
    {

        sql.Open(dbPathName);

        query = "update UserStageHis set ";
        query += "stageMaxScore  = " + g.curStageMaxScore + ", ";
        query += "stageScore     = " + g.curStageScore + ", ";
        query += "stageMaxRank   = " + g.curStageMaxRank + ", ";
        query += "stageRank      = " + g.curStageRank + ", ";
        query += "updateDate     = strftime('%Y%m%d','now','localtime'),updateTime = strftime('%H%M%S','now','localtime') ";
        query += "where stageNum = '" + g.curStage + "' ";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();
    }

    public void UptUserStageHisStageClear(int minStageNum, int maxStageNum)
    {

        sql.Open(dbPathName);

        query = "update UserStageHis set ";
        query += "stageOpenYn   = 'Y', ";
        query += "updateDate     = strftime('%Y%m%d','now','localtime'),updateTime = strftime('%H%M%S','now','localtime') ";
        query += "where stageNum between " + minStageNum + " and " + maxStageNum;
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();
    }

    //--------------------------
    // UserStageHis Achievement
    //--------------------------
    public DataTable SelUserStageHisLarge()
    {
        sql.Open(dbPathName);

        print ("SelUserStageHis");

        query = "select min(largeCode) largeCode from UserCorrectHis where stageNum=" + g.curStage;
        //Debug.Log(query);
        _data = sql.ExecuteQuery(query);

        sql.Close();

        return _data;
    }

    public DataTable SelUserStageHisAchieve()
    {
        sql.Open(dbPathName);

        print ("SelUserStageHis");

        query  = " select case when count(*) = 0 then 'Y' else 'N' end as achieveYn \n";
        query += " from   UserStageHis              \n";
        query += " where  stageNum in (             \n";
        query += "        select distinct stageNum  \n";
        query += "        from UserCorrectHis       \n";
        query += "        where largeCode = (       \n";
        query += "           select min(largeCode)  \n";
        query += "           from UserCorrectHis    \n";
        query += "           where stageNum = " + g.curStage + " \n";
        query += "           )                      \n";
        query += "        )                         \n";
        query += " and    stageMaxRank < 5          \n";

        //Debug.Log(query);
        _data = sql.ExecuteQuery(query);

        sql.Close();

        return _data;
    }



    //----------------------------------------------------
    // UserGanaStageHis
    //----------------------------------------------------
    public DataTable SelUserGanaStageHis(int stageNum)
    {
        sql.Open(dbPathName);

        print ("SelUserStageHis");

        query = "select * from UserGanaStageHis where stageNum=" + stageNum;
        //Debug.Log(query);
        _data = sql.ExecuteQuery(query);

        sql.Close();

        return _data;
    }

    public DataTable SelUserGanaStageHisRank(int stageNum)
    {
        sql.Open(dbPathName);

        print ("SelUserStageHis");

        query = "select round(avg(stageRank)) avgRank from UserGanaStageHis \n";
        query += " where stageNum<=" + stageNum + "\n";
        query += " and stageRank > 0               \n";
        query += " group by stageRank";

        //Debug.Log(query);
        _data = sql.ExecuteQuery(query);

        sql.Close();

        return _data;
    }

    public void UptUserGanaStageHis()
    {

        sql.Open(dbPathName);

        query = "update UserGanaStageHis set ";
        query += "stageScore     = " + g.curStageScore + ", ";
        query += "stageRank      = " + g.curStageRank + ", ";
        query += "stageMaxScore  = " + g.curStageMaxScore + ", ";
        query += "updateDate     = strftime('%Y%m%d','now','localtime'),updateTime = strftime('%H%M%S','now','localtime') ";
        query += "where stageNum = '" + g.curStage + "' ";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();
    }


    //-------------------------------------------------------------------------
    //                             FbUserInfo
    //-------------------------------------------------------------------------
    public bool isFbUserInfo()
    {
        print ("isFbUserInfo");

        sql.Open(dbPathName);

        query = "select count(*) cnt from FbUserInfo";
        //Debug.Log(query);
        _data = sql.ExecuteQuery(query); dr = _data.Rows[0];

        sql.Close();

        if (int.Parse(dr["cnt"].ToString()) > 0) return true;
        else return false;
    }

    public void InsFbUserInfo(string fbId, string fbName, string fbGender, string fbUrl, string fbLocale)
    {
        sql.Open(dbPathName);

        query = "select strftime('%Y%m%d','now','localtime') createDate,strftime('%H%M%S','now','localtime') createTime";
        //Debug.Log(query);
        _data = sql.ExecuteQuery(query); dr = _data.Rows[0];

        string createDate = dr["createDate"].ToString();
        string createTime = dr["createTime"].ToString();

        sql.Close();


        var factory = new Factory();_connection = factory.Create(dbPathName);

        query = "delete from FbUserInfo";
        //Debug.Log("query=" + query);
        _connection.Execute(query);


        _connection.InsertAll (new[]{
            new FbUserInfo{
                 userId     = g.userId
                ,fbId       = fbId
                ,fbName     = fbName
                ,fbPicUrl   = ""
                ,fbUrl      = fbUrl
                ,fbGender   = fbGender
                ,fbLocale   = fbLocale
                ,createDate = createDate
                ,createTime = createTime
                ,updateDate = ""        
                ,updateTime = ""        
            }
        });

        _connection.Close();
    }


    public void UptFbUserInfoPic(string picUrl)
    {
        sql.Open(dbPathName);

        query = "update FbUserInfo set fbPicUrl = '" + picUrl + "',";
        query += "updateDate     = strftime('%Y%m%d','now','localtime'),updateTime = strftime('%H%M%S','now','localtime') ";

        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();

    }

    //-------------------------------------------------------------------------
    //                             FbFriendInfo
    //-------------------------------------------------------------------------

    public DataTable SelFbFriendInfo()
    {
        sql.Open(dbPathName);

        query = "select * from FbFriendInfo order by score desc";

        //Debug.Log(query);
        _data = sql.ExecuteQuery(query);

        sql.Close();

        return _data;
    }

    public DataTable SelFbFriendInfo2()
    {
        sql.Open(dbPathName);

        query = "select * from FbFriendInfo where fbId <> fbFriendId order by score desc";

        //Debug.Log(query);
        _data = sql.ExecuteQuery(query);

        sql.Close();

        return _data;
    }

    public void DelFbFriendInfo()
    {
        var factory = new Factory();_connection = factory.Create(dbPathName);

        query = "delete from FbFriendInfo";
        _connection.Execute(query);

        _connection.Close();
    }

    public void InsFbFriendInfo(string friendId, string friendName, string friendPicUrl)
    {

        sql.Open(dbPathName);

        query = "select strftime('%Y%m%d','now','localtime') createDate,strftime('%H%M%S','now','localtime') createTime";
        //Debug.Log(query);
        _data = sql.ExecuteQuery(query); dr = _data.Rows[0];

        string createDate = dr["createDate"].ToString();
        string createTime = dr["createTime"].ToString();

        sql.Close();

        var factory = new Factory();_connection = factory.Create(dbPathName);

        query = "delete from FbFriendInfo where fbId='"+g.fbId+"' and fbFriendId='"+friendId+"'";
        //Debug.Log("query=" + query);
        _connection.Execute(query);

        //Debug.Log("friendName=" + friendName);

        _connection.InsertAll (new[]{
            new FbFriendInfo{
                 fbId           = g.fbId
                ,fbFriendId     = friendId
                ,fbFriendName   = friendName
                ,fbFriendPicUrl = friendPicUrl      
                ,score          = 0
                ,createDate     = createDate
                ,createTime     = createTime
                ,updateDate     = ""        
                ,updateTime     = ""        
            }
        });

        _connection.Close();

    }

    public void UptFbFriendInfoScore(string fbFriendId,int bestScore)
    {
        sql.Open(dbPathName);

        query = "update FbFriendInfo set score ="+bestScore+","; 
        query +=" updateDate = strftime('%Y%m%d','now','localtime'),updateTime = strftime('%H%M%S','now','localtime')";
        query +=" where fbFriendId = '"+fbFriendId+"'";
        //Debug.Log("query=" + query);
       
        sql.ExecuteQuery(query);
        sql.Close();
    }


    //-------------------------------------------------------------------------
    //                                UserGiftHis
    //-------------------------------------------------------------------------

    public DataTable SelUserGiftHis()
    {
        sql.Open(dbPathName);

        //print ("SelUserStageHis");

        query  = "select a.*,ifnull(trim(b.fbFriendName),' ') fbFriendName,ifnull(trim(b.fbFriendPicUrl),' ') fbFriendPicUrl";
        query += " from UserGiftHis a left join FbFriendInfo b on a.fbFriendId = b.fbFriendId ";
        query += " where a.useYn='N' ";
        query += " order by a.receiveDate desc,a.receiveTime desc";

        //Debug.Log(query);
        _data = sql.ExecuteQuery(query);

        sql.Close();

        return _data;
    }

    public void DelUserGiftHis()
    {
        sql.Open(dbPathName);

        query = "delete from UserGiftHis";

        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();
    }

    public void InsUserGiftHis(int itemGroup, int itemCode, string fbFriendId)
    {
        sql.Open(dbPathName);

        if(itemGroup == 10) {
            query = "delete from UserGiftHis";
            query += " where userId    = '"+g.userId+"'";
            query += " and   itemGroup = " +itemGroup;
            query += " and   itemCode  = " +itemCode; 

            //Debug.Log(query);
            sql.ExecuteQuery(query);
        }

        query = "insert into UserGiftHis values (";
        query += "'" + g.userId + "',";                   // userId 
        query += itemGroup + ",";                         // itemGroup    
        query += itemCode + ",";                          // itemCode    
        query += "strftime('%Y%m%d','now','localtime'),"; // receiveDate    
        query += "strftime('%H%M%S','now','localtime'),"; // receiveTime    
        query += "'N',";                                  // useYn
        query += "'" + fbFriendId + "',";                 // fbFriendId
        query += "strftime('%Y%m%d','now','localtime'),"; // createDate    
        query += "strftime('%H%M%S','now','localtime'),"; // createTime    
        query += "'',";                                   // updateDate    
        query += "'')";                                   // updateTime    
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();

    }

    public void UptUserGiftHis(int itemGroup, int itemCode, string receiveDate, string receiveTime)
    {
        sql.Open(dbPathName);

        query  = "update UserGiftHis set useYn='Y'";
        query += ",updateDate     = strftime('%Y%m%d','now','localtime'),updateTime = strftime('%H%M%S','now','localtime') ";
        query += " where itemGroup="+itemGroup+" and itemcode="+itemCode;
        query += " and   receiveDate = '"+receiveDate+"' and receiveTime= '"+receiveTime+"'";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();

    }

    public void UptUserGiftHisAll()
    {
        sql.Open(dbPathName);

        query = "update UserGiftHis set useYn='Y'";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();

    }



    //-------------------------------------------------------------------------
    //                                UserGiftHis
    //-------------------------------------------------------------------------

    public bool SelUserStarAchievment(int comboCnt)
    {
        bool isAchieve = false;
        int cnt;
        int comboTotalCntRest = comboCnt % g.totalStar;
        int minQueryCnt = comboCnt - comboTotalCntRest;
        int maxQueryCnt = minQueryCnt + 9;

        //Debug.Log("comboTotalCntRest="+comboTotalCntRest+" minQueryCnt="+minQueryCnt+" maxQueryCnt="+maxQueryCnt);

        sql.Open(dbPathName);

        if(comboCnt >= 10) {
            query  = "select count(*) cnt from UserStarAchievment";
            query += " where comboCnt between "+minQueryCnt+" and "+maxQueryCnt;
            _data = sql.ExecuteQuery(query); dr = _data.Rows[0];
            cnt = int.Parse(dr["cnt"].ToString());

            //Debug.Log("cnt="+cnt);

            if(cnt == 0) {
                query = "delete from UserStarAchievment where comboCnt="+comboCnt; 
                //Debug.Log(query);
                sql.ExecuteQuery(query);
                query = "insert into UserStarAchievment values ("+comboCnt+",strftime('%Y%m%d','now','localtime'),strftime('%H%M%S','now','localtime'),'','')"; 
                //Debug.Log(query);
                sql.ExecuteQuery(query);
                isAchieve = true;
            } else isAchieve = false;
        }

        sql.Close();
        return isAchieve;
    }

    public void InsUserStarAchievment(int comboCnt)
    {
        sql.Open(dbPathName);


        query = "insert into UserStarAchievment values ("+comboCnt+",strftime('%Y%m%d','now','localtime'),strftime('%H%M%S','now','localtime'),'','')";
        sql.ExecuteQuery(query);

        sql.Close();

    }




    //-------------------------------------------------------------------------
    //                             Add hp,skip
    //-------------------------------------------------------------------------

    public DataTable SelAddSkipTime()
    {
        sql.Open(dbPathName);

        query = "select strftime('%s',datetime('now','localtime')) - strftime('%s',startTime) as addSkipSecond from AddSkip";
        //Debug.Log(query);
        _data = sql.ExecuteQuery(query); dr = _data.Rows[0];

        sql.Close();

        return _data;
    }

    public void UptAddSkip()
    {
        sql.Open(dbPathName);

        query = "update AddSkip set startTime = datetime('now','localtime')";

        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();
    }

    public DataTable SelAddHpTime()
    {
        sql.Open(dbPathName);


        query = "select strftime('%s',datetime('now','localtime')) - strftime('%s',startTime) as addHpSecond from AddHp";
        //Debug.Log(query);
        _data = sql.ExecuteQuery(query); dr = _data.Rows[0];

        sql.Close();

        return _data;
    }

    public void UptAddHp()
    {
        sql.Open(dbPathName);

        query = "update AddHp set startTime = datetime('now','localtime')";

        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();
    }



    //-------------------------------------------------------------------------
    //                             FbGiftHpHis
    //-------------------------------------------------------------------------

    public DataTable SelFbGiftHpHis()
    {
        sql.Open(dbPathName);

        query = "select * from FbGiftHpHis where createDate ='"+g.wwwDate+"'";
        //Debug.Log(query);
        _data = sql.ExecuteQuery(query);

        sql.Close();

        return _data;
    }

    public void InsFbGiftHpHis(string kind)
    {
        sql.Open(dbPathName);

        query = "delete from FbGiftHpHis";
        query += " where fbFriendId = '"+g.fbFriendId+"'";
        query += " and   kind       = '"+kind+"'";

        //Debug.Log(query);
        sql.ExecuteQuery(query);

        query = "insert into FbGiftHpHis values (";
        query += "'" + g.fbFriendId + "',";               // fbFriendId 
        query += "'" + g.wwwDate + "',";                  // createDate    
        query += "strftime('%H%M%S','now','localtime'),"; // createTime    
        query += "'" + kind + "',";                       // kind
        query += "'',";                                   // updateDate    
        query += "'')";                                   // updateTime    
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();

    }


    //-------------------------------------------------------------------------
    //                            Inform Popup
    //-------------------------------------------------------------------------

    public int SelUserFirstEnterPopup()
    {
        sql.Open(dbPathName);

        query = "select count(*) cnt from CrazyWordPopup where popupId = '2015010101' and showYn = 'Y'";
        //Debug.Log(query);
        _data = sql.ExecuteQuery(query); dr = _data.Rows[0];
        int cnt = int.Parse(dr["cnt"].ToString());

        sql.Close();

        return cnt;
    }

    public DataTable SelCrazyWordPopup()
    {
        sql.Open(dbPathName);

        query  = "select * from CrazyWordPopup                                 \n";
        query += "where popupId >= 20150301                                    \n";
        query += "and   validStartDate <= strftime('%Y%m%d','now','localtime') \n";
        query += "and   validEndDate >= strftime('%Y%m%d','now','localtime')   \n";
        query += "and   showYn = 'Y'                                           \n";
        //Debug.Log(query);
        _data = sql.ExecuteQuery(query);

        sql.Close();

        return _data;
    }

    public string SelCrazyWordPopupMaxId()
    {
        sql.Open(dbPathName);

        query = "select max(popupId) popupId from CrazyWordPopup";
        //Debug.Log(query);
        _data = sql.ExecuteQuery(query); dr = _data.Rows[0];
        string popupId = dr["popupId"].ToString();

        sql.Close();

        return popupId;
    }


    public void UptCrazyWordPopupShowN(string popupId)
    {

        sql.Open(dbPathName);

        query = "update CrazyWordPopup set ";
        query += " showYn = 'N'";
        query += ",updateDate     = strftime('%Y%m%d','now','localtime'),updateTime = strftime('%H%M%S','now','localtime') ";
        query += "where popupId = '" + popupId + "' ";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();
    }

    public void InsCrazyWordPopup(string popupId
                                ,string validStartDate
                                ,string validEndDate
                                ,string pupupDownUrl
                                ,string pupupTexName)
    {
        sql.Open(dbPathName);

        query = "insert into CrazyWordPopup values (";
        query += "'" + popupId + "',";                    // popupId      
        query += "'" + validStartDate + "',";             // validStartDate      
        query += "'" + validEndDate + "',";               // validEndDate      
        query += "'" + pupupDownUrl + "',";               // pupupDownUrl      
        query += "'" + pupupTexName + "',";               // pupupTexName      
        query += "'Y',";                                  // showYn       
        query += "strftime('%Y%m%d','now','localtime'),"; // createDate      
        query += "strftime('%H%M%S','now','localtime'),"; // createTime      
        query += "'',";                                   // updateDate      
        query += "'')";                                   // updateTime      

        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();
    }











    //-------------------------------------------------------------------------
    //                            WWW Modify Sqlite
    //-------------------------------------------------------------------------

    public void ExcuteWwwQuery(string wwwQuery)
    {
        sql.Open(dbPathName);
        Debug.Log(wwwQuery);
        sql.ExecuteQuery(wwwQuery);
        sql.Close();
    }

    public void InsCrazyWordModifySql(string curVersion, string wwwQuery)
    {
        sql.Open(dbPathName);

        print ("FbFriendInfo");
        query = "delete from CrazyWordModifySql where curVersion = '" + curVersion + "'";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        query = "insert into CrazyWordModifySql values (";
        query += "'" + curVersion + "',";                 // curVersion      
        query += "'" + wwwQuery + "',";                   // query           
        query += "'Y',";                                  // executeYn       
        query += "strftime('%Y%m%d','now','localtime'),"; // executeDate     
        query += "strftime('%H%M%S','now','localtime'),"; // executeTime     
        query += "'',";                                   // executeSuccessYn
        query += "strftime('%Y%m%d','now','localtime'),"; // createDate      
        query += "strftime('%H%M%S','now','localtime'),"; // createTime      
        query += "'',";                                   // updateDate      
        query += "'')";                                   // updateTime      
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();

        ExcuteWwwQuery(wwwQuery);

        UptCrazyWordModifySql(curVersion);



    }

    public DataTable SelCrazyWordModifySql()
    {
        sql.Open(dbPathName);

        query = "select max(curVersion) curVersion from CrazyWordModifySql";
        //Debug.Log(query);
        _data = sql.ExecuteQuery(query);

        sql.Close();

        return _data;
    }

    public void UptCrazyWordModifySql(string curVersion)
    {

        sql.Open(dbPathName);

        query = "update CrazyWordModifySql set ";
        query += " executeSuccessYn = 'Y'";
        query += ",updateDate     = strftime('%Y%m%d','now','localtime'),updateTime = strftime('%H%M%S','now','localtime') ";
        query += "where curVersion = '" + curVersion + "' ";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        sql.Close();
    }



    //----------------------------------------------------
    // Etc
    //----------------------------------------------------
    public DataTable SelLanguageAlphabet()
    {

        sql.Open(dbPathName);

        query = "select * from LanguageAlphabet ";
        query += " where language = '" + g.CrazyWordLanguage + "'";

        //Debug.Log(query);
        _data = sql.ExecuteQuery(query); dr = _data.Rows[0];

        sql.Close();

        return _data;

    }


    public DataTable SelDayhourmin()
    {

        sql.Open(dbPathName);

        query = "select strftime('%Y%m%d%H%M','now','localtime') dayhourmin";
        //Debug.Log(query);
        sql.ExecuteQuery(query);

        //Debug.Log(query);
        _data = sql.ExecuteQuery(query); dr = _data.Rows[0];

        sql.Close();

        return _data;
    }


    public string GetUniqueId()
    {

        Debug.Log(SystemInfo.deviceUniqueIdentifier);

        string key = "ID";

        var random = new System.Random();
        DateTime epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
        double timestamp = (System.DateTime.UtcNow - epochStart).TotalSeconds;

        Debug.Log("timestamp=" + timestamp);
        Debug.Log("timestamp=" + Math.Round(timestamp).ToString());

        string uniqueID = Application.systemLanguage //Language
            //+"-"+SystemInfo.deviceName //Device
            //+ "-" + String.Format("{0:X}", Convert.ToInt32(timestamp)) //Time
                + "-" + Math.Round(timestamp).ToString() //Time
                + "-" + String.Format("{0:X}", Convert.ToInt32(Time.time * 1000000)) //Time in game
                + "-" + String.Format("{0:X}", random.Next(1000000000)); //random number

        Debug.Log("Generated Unique ID: " + uniqueID);

        //if (PlayerPrefs.HasKey(key))
        //{
        //    uniqueID = PlayerPrefs.GetString(key);
        //}
        //else
        //{
        //    PlayerPrefs.SetString(key, uniqueID);
        //    PlayerPrefs.Save();
        //}

        return uniqueID;
    }

    public void SqliteDBConn()
    {
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
