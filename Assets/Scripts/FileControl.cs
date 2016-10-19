using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class FileControl : MonoBehaviour {

	//--------------------------------
	// sqlite
	//--------------------------------
	SqliteDatabase sql;
	string dbPathName;
  	DataTable _data;
	string query;
	DataRow dr;

	public struct TexInventory {
		public int volNum,largeCode,middleCode,startRow,endRow;
		public TexInventory(int volNum,int largeCode,int middleCode,int startRow,int endRow)
		{
			this.volNum = volNum;
			this.largeCode = largeCode;
			this.middleCode = middleCode;
			this.startRow = startRow;
			this.endRow = endRow;
		}
	}

	void Awake() {
		print ("Awake");

		//--------------------------------
		// sqlite db connection
		//--------------------------------
		SqliteDBConn();

	}

	// Use this for initialization
	void Start () {
        string tableName = "";

        MoveFile();

        //-----------------------
        // 0 step
        //----------------------
        //InsertTextureMeaning();

        //-----------------------
        // 3 step
        //----------------------
        //tableName = "TextureInfo_f";

        ////tableName = "TextureInfo_m";
		////InsertTextureInfo(tableName);
		////UptTextureInfoTexName(tableName,"Japanese",1);

        //----------------------
        // 4 step
        //----------------------
        //InsLanguageAlphabet("Japanese",1);
        //SelLanguageAlphabet("Japanese",1);

	}

	public void InsertTextureInfo(string tableName){

		sql.Open(dbPathName);
		
		query  = "delete from "+tableName; 
		print(query);
		sql.ExecuteQuery(query);

        if(tableName == "TextureInfo_f") {
            query = "insert into TextureInfo_f                              \n";
            query += " select                                               \n";   
            query += " 1                                    as volNum       \n";
            query += ",b.stageNum                           as stageNum     \n";
            query += ",b.turnTexNum                         as turnTexNum   \n";
            query += ",b.largeCode                          as largeCode    \n";
            query += ",b.middleCode                         as middleCode   \n";
            query += ",b.texName                            as texName      \n";
            query += ",b.texName                            as texShowName  \n";
            query += ",b.meaningKind                        as texShowKind  \n";
            query += ",a.photoNum                           as texNum       \n";
            query += ",a.photoImgURL                        as texUrl       \n";
            //query += ",a.photoTbURL                         as texUrl       \n";
            query += ",'Y'                                  as texUrlYn     \n";
            query += ",'N'                                  as downYn       \n";
            query += ",a.photoHref                          as photoLink    \n";
            query += ",c.photoOwner                         as photoOwner   \n";
            query += ",c.photoLicense                       as photoLicense \n";
            query += ",'flickr'                             as photoSource  \n";
            query += ",strftime('%Y%m%d','now','localtime') as createDate   \n";
            query += ",strftime('%H%M%S','now','localtime') as createTime   \n";
            query += ",''                                   as updateDate   \n";
            query += ",''                                   as updateTime   \n";
            query += "from PhotoSearch a, TextureMeaning b, PhotoGetInfo c  \n";
            //query += "   ,(select largeCode,middleCode,texName,count(*) cnt  \n";
            //query += "    from PhotoSearch                                  \n";
            //query += "    where useYn = 'Y'                                 \n";
            //query += "    group by largeCode,middleCode,texName             \n";
            //query += "    having count(*) = 4) d                            \n";
            query += "where a.largeCode  = b.largeCode                      \n";
            query += "and   a.middleCode = b.middleCode                     \n";
            query += "and   replace(replace(a.texName,'_',''),' ','')  = replace(replace(b.texName,'_',''),' ','') \n";
            query += "and   a.photoId    = c.photoId                        \n";
            //query += "and   a.largeCode  = d.largeCode                      \n";
            //query += "and   a.middleCode = d.middleCode                     \n";
            //query += "and   a.texName    = d.texName                        \n";
            query += "and   a.useYn      = 'Y'                              \n";
            query += "and   b.language   = 'Korean'                         \n";
            query += "and   b.languageClass = 1                             \n";
            query += "and   b.largeCode  > 0                                \n";
            query += "order by b.stageNum, b.turnTexNum, a.texName          \n";         
        } else {
            query =  "insert into TextureInfo_m                             \n";
            query += " select                                               \n";   
            query += " 1                                    as volNum       \n";
            query += ",b.stageNum                           as stageNum     \n";
            query += ",b.turnTexNum                         as turnTexNum   \n";
            query += ",b.largeCode                          as largeCode    \n";
            query += ",b.middleCode                         as middleCode   \n";
            query += ",b.texName                            as texName      \n";
            query += ",b.texName                            as texShowName  \n";
            query += ",b.meaningKind                        as texShowKind  \n";
            query += ",a.texNum                             as texNum       \n";
            query += ",a.texUrl                             as texUrl       \n";
            query += ",'Y'                                  as texUrlYn     \n";
            query += ",'N'                                  as downYn       \n";
            query += ",a.photoLink                          as photoLink    \n";
            query += ",a.photoOwner                         as photoOwner   \n";
            query += ",a.photoLicense                       as photoLicense \n";
            query += ",'manual'                             as photoSource  \n";
            query += ",strftime('%Y%m%d','now','localtime') as createDate   \n";
            query += ",strftime('%H%M%S','now','localtime') as createTime   \n";
            query += ",''                                   as updateDate   \n";
            query += ",''                                   as updateTime   \n";
            query += "from PhotoManual a, TextureMeaning b                  \n";
            query += "where a.largeCode  = b.largeCode                      \n";
            query += "and   a.middleCode = b.middleCode                     \n";
            query += "and   replace(replace(a.texName,'_',''),' ','')  = replace(replace(b.texName,'_',''),' ','') \n";
            query += "and   b.language   = 'Korean'                         \n";
            query += "and   b.languageClass = 1                             \n";
            query += "and   b.largeCode  > 0                                \n";
            query += "order by b.stageNum, b.turnTexNum, a.texName          \n";                 
        
        }                

        print(query);
        sql.ExecuteQuery(query);

        sql.ExecuteQuery("drop table if exists TextureInfoTexNum01");

        //---------------------------
        // update texNum
        //---------------------------

        query = " create table TextureInfoTexNum01 as                   \n";
        query += " select *									            \n";
        query += " from                                                 \n";
        query += "    (select stageNum,turnTexNum,texName,count(*) cnt  \n";
        query += "     from "+tableName+"                               \n";
        query += "     group by stageNum,turnTexNum,texName             \n";
        query += "     order by stageNum,turnTexNum,texName             \n";
        query += "     ) a, numtab b                                    \n";
        query += " where b.num <= a.cnt                                 \n";

        print(query);
        sql.ExecuteQuery(query);

        sql.ExecuteQuery("drop table if exists TextureInfo2");

        query = " create table TextureInfo2 as              \n";
        query += " select rowid rnum,*                      \n";
        query += " from "+tableName+"                       \n";
        query += " order by stageNum,turnTexNum,texName     \n";

        print(query);
        sql.ExecuteQuery(query);

        sql.ExecuteQuery("delete from "+tableName);

        query = "insert into "+tableName+"                             \n";
        query += "select                                               \n";
        query += " 1                                    as volNum      \n";
        query += ",a.stageNum                           as stageNum    \n";
        query += ",a.turnTexNum                         as turnTexNum  \n";
        query += ",a.largeCode                          as largeCode   \n";
        query += ",a.middleCode                         as middleCode  \n";
        query += ",a.texName                            as texName     \n";
        query += ",a.texName                            as texShowName \n";
        query += ",a.texShowKind                        as texShowKind \n";
        query += ",b.num                                as texNum      \n";
        query += ",a.texUrl                             as texUrl      \n";
        query += ",a.texUrlYn                           as texUrlYn    \n";
        query += ",a.downYn                             as downYn      \n";
        query += ",a.photoLink                          as photoLink   \n";
        query += ",a.photoOwner                         as photoOwner  \n";
        query += ",a.photoLicense                       as photoLicense\n";
        query += ",a.photoSource                        as photoSource \n";
        query += ",strftime('%Y%m%d','now','localtime') as createDate  \n";
        query += ",strftime('%H%M%S','now','localtime') as createTime  \n";
        query += ",''                                   as updateDate  \n";
        query += ",''                                   as updateTime  \n";
        query += " from  TextureInfo2 a,                               \n";
        query += "      (select rowid rnum,*                           \n";
        query += "       from  TextureInfoTexNum01                     \n";
        query += "       order by stageNum,turnTexNum,texName) b       \n";
        query += " where a.rnum = b.rnum                               \n";

        print(query);
        sql.ExecuteQuery(query);


        //if(tableName == "TextureInfo_f") {
        //    //---------------------------
        //    // delete TextureInfo_f 
        //    //---------------------------
        //    sql.ExecuteQuery("drop table if exists TextureInfo_fd");

        //    query  = "create table TextureInfo_fd as              \n";
        //    query += "select largeCode,middleCode,texName         \n";
        //    query += "from TextureInfo_f                          \n";    
        //    query += "group by largeCode,middleCode,texName       \n"; 
        //    query += "having count(*) < 4                         \n";

        //    print(query);
        //    sql.ExecuteQuery(query);


        //    query  = "delete from TextureInfo_f                   \n";           
        //    query += " where exists (                             \n";
        //    query += "  select largeCode,middleCode,texName       \n";
        //    query += "  from TextureInfo_fd                       \n";       
        //    query += "  where largeCode = TextureInfo_f.largeCode \n";
        //    query += "  and middleCode = TextureInfo_f.middleCode \n";
        //    query += "  and texName = TextureInfo_f.texName       \n";
        //    query += "  group by largeCode,middleCode,texName     \n";
        //    query += "  having count(*) < 4                       \n";
        //    query += "  )                                         \n";

        //    print(query);
        //    sql.ExecuteQuery(query);
        //}

        //---------------------------
        // delete TextureInfo 
        //---------------------------
        query  = "delete from TextureInfo                     \n";           
        query += " where exists (                             \n";
        query += "  select largeCode,middleCode,texName,texNum \n";
        query += "  from "+tableName+"                        \n";
        query += "  where largeCode = TextureInfo.largeCode   \n";
        query += "  and middleCode = TextureInfo.middleCode   \n";
        query += "and   replace(replace(texName,'_',''),' ','')  = replace(replace(TextureInfo.texName,'_',''),' ','') \n";
        query += "and   texNum  = TextureInfo.texNum          \n";
        query += "  group by largeCode,middleCode,texName     \n";
        query += "  )                                         \n";

        print(query);
        sql.ExecuteQuery(query);
        //---------------------------
        // insert TextureInfo 
        //---------------------------
        query  = "insert into TextureInfo select * from "+tableName;
        print(query);
        sql.ExecuteQuery(query);

        //sql.ExecuteQuery("insert into TextureInfo select * from TextureInfoGana");

        sql.Close();

	}

    public void UptTextureInfoTexName(string tableName,string language, int languageClass)
    {
        sql.Open(dbPathName);

        query = "update TextureInfo                                         \n";
        query += "set texShowName =(select meaning                          \n";
        query += "              from  TextureMeaning                        \n";
        query += "              where volNum = TextureInfo.volNum           \n";
        query += "              and   largeCode = TextureInfo.largeCode     \n";
        query += "              and   middleCode = TextureInfo.middleCode   \n";
        query += "              and   texName = TextureInfo.texName         \n";
        query += "              and   language = '" + language + "'          \n";
        query += "              and   languageClass = " + languageClass + ") \n";

        query += "   ,texShowKind =(select meaningKind                      \n";
        query += "              from  TextureMeaning                        \n";
        query += "              where volNum = TextureInfo.volNum           \n";
        query += "              and   largeCode = TextureInfo.largeCode     \n";
        query += "              and   middleCode = TextureInfo.middleCode   \n";
        query += "              and   texName = TextureInfo.texName         \n";
        query += "              and   language = '" + language + "'          \n";
        query += "              and   languageClass = " + languageClass + ") \n";

        query += "	where exists (select meaning                            \n";
        query += "	              from  TextureMeaning                      \n";
        query += "	              where volNum = TextureInfo.volNum         \n";
        query += "	              and   largeCode = TextureInfo.largeCode   \n";
        query += "	              and   middleCode = TextureInfo.middleCode \n";
        query += "	              and   texName = TextureInfo.texName       \n";
        query += "                and   language = '" + language + "'         \n";
        query += "                and   languageClass = " + languageClass + ")    \n";

        print(query);
        sql.ExecuteQuery(query);

        query = "update TextureInfo set texShowKind = 1 where largeCode in (-3,-2) \n";
        print(query);
        sql.ExecuteQuery(query);

        query = "update TextureInfo set texShowKind = 2 where largeCode in (-1,0) \n";
        print(query);
        sql.ExecuteQuery(query);

        sql.Close();

    }


    public void SelTextureInfo()
    {
        sql.Open(dbPathName);

        query = "update TextureInfo set texShowKind = 2 where largeCode in (-1,0) \n";
        print(query);
        sql.ExecuteQuery(query);

        sql.Close();

    }

	void MoveFile() {

		int stageNum;
		string srcPath,desPath,desPath2;
        DirectoryInfo di;
		
		for (int i = 1; i <= 200; i++) {
			
			stageNum = i;
					
			srcPath  = Application.persistentDataPath + "/Textures/v"+g.volumn.ToString("000");
			srcPath += "/s"+stageNum.ToString("000")+"/output/";

			desPath  = Application.persistentDataPath + "/Textures/v002";
			desPath += "/s"+stageNum.ToString("000")+"/";

			desPath2  = Application.persistentDataPath + "/Textures/v003";
			desPath2 += "/s"+stageNum.ToString("000")+"/";
	

		    DirectoryInfo dir = new DirectoryInfo(srcPath);
			di = new DirectoryInfo(desPath);
			if (di.Exists == false) di.Create();

			di = new DirectoryInfo(desPath2);
			if (di.Exists == false) di.Create();

		    FileInfo[] info = dir.GetFiles("*.*");

		    foreach(FileInfo f in info){ ;
                f.CopyTo(desPath+f.Name,true);
                f.CopyTo(desPath2+f.Name+".bytes",true);
            }			
		}
	}



	void InsTextureFileNm(string fileName){
		
		sql.Open(dbPathName);
		
		print ("DBSelUserInfo");
		
		query  = "delete from TextureFileNm where fileName ='"+fileName+"'";
		print(query);
		sql.ExecuteQuery(query);
		
		query  = "insert into TextureFileNm values ('"+fileName+"')";
		print(query);
		sql.ExecuteQuery(query);
		
		sql.Close();
		
	}

	
	void DelTextureFileNm(){
		
		sql.Open(dbPathName);

		query  = "delete from TextureFileNm ";
		print(query);
		_data = sql.ExecuteQuery(query);
		
		sql.Close();
		
	}


	void SelTextureFileNm(){
		
		sql.Open(dbPathName);


		query  = "select * from TextureFileNm order by length(fileName)";
		print(query);
		_data = sql.ExecuteQuery(query);

		fileNames.Clear();
		for(int i=0;i < _data.Rows.Count;i++){
			dr = _data.Rows[i];
			fileNames.Add( dr["fileName"].ToString() );
		}
		
		sql.Close();
		
	}


    //----------------------------------------------------
    // LanguageAlphabet
    //----------------------------------------------------

    public void InsLanguageAlphabet(string language, int languageClass)
    {

        string alphabet = "";

        sql.Open(dbPathName);

        query = "select a1 from (";
        for (int i = 1; i < 30; i++)
        {
            query += "select substr(meaning," + i + ",1) a1 FROM TextureMeaning where language = '" + language + "' and languageClass=" + languageClass + " group by substr(meaning," + i + ",1) union all ";
        }
        query += "select substr(meaning,30,1) a1 FROM TextureMeaning where language = '" + language + "' and languageClass=" + languageClass + " group by substr(meaning,30,1) ) a group by a.a1 order by a.a1";
        print(query);
        _data = sql.ExecuteQuery(query);

        for (int i = 0; i < _data.Rows.Count; i++)
        {
            dr = _data.Rows[i];
            alphabet += dr["a1"].ToString();
        }

        print("alphabet=" + alphabet);

        query = "delete from LanguageAlphabet ";
        query += " where language = '" + language + "'";
        query += " and   languageClass = " + languageClass;

        print(query);
        sql.ExecuteQuery(query);


        //query  = "update LanguageAlphabet set alphabet = '0123456789CDOWX가각간'";


        query = "insert into LanguageAlphabet values (";
        /* language   */
        query += "'" + language + "',";
        /* languageClass*/
        query += languageClass + ",";
        /* alphabet    */
        query += "'" + alphabet + "',";
        /* createDate  */
        query += "strftime('%Y%m%d','now','localtime'),";
        /* createTime  */
        query += "strftime('%H%M%S','now','localtime'),";
        /* updateDate  */
        query += "'',";
        /* updateTime  */
        query += "'')";

        print(query);
        sql.ExecuteQuery(query);

        sql.Close();
    }

    public void SelLanguageAlphabet(string language, int languageClass)
    {

        string alphabet = "";

        sql.Open(dbPathName);

        query = "select * from LanguageAlphabet ";
        query += " where language = '" + language + "'";
        query += " and   languageClass = " + languageClass;

        print(query);
        _data = sql.ExecuteQuery(query); dr = _data.Rows[0];
        alphabet += dr["alphabet"].ToString();

        print("alphabet=" + alphabet);

        sql.Close();
    }


    public void InsertTextureMeaning()
    {
        sql.Open(dbPathName);

        query = "";

        /*
-- loading from excel
TextureMeaningTemp

update TextureMeaningTemp 
set texName = trim(texName),kr = trim(kr),jp1 = trim(jp1),jp2 = trim(jp2)
,example1 = trim(example1),example1Meaning = trim(example1Meaning)
,example2 = trim(example2),example2Meaning = trim(example2Meaning)
;


drop table if exists TextureMeaningTemp2;

create table TextureMeaningTemp2 as
select rowid rnum,((rowid-1)/5)+1 as stageNum,* from TextureMeaningTemp;

drop table if exists TextureMeaningTemp3;

create table TextureMeaningTemp3 as
select a.stageNum,b.num
from
	 (select stageNum,count(*) cnt
	  from TextureMeaningTemp2
	  group by stageNum) a,
  	  numtab b
where b.num <= 5
order by a.stageNum
;

drop table if exists TextureMeaningTemp4;
create table TextureMeaningTemp4 as
select b.num turnTexNum,a.*
from TextureMeaningTemp2 a,
     (select rowid rnum,* from TextureMeaningTemp3) b
where a.rnum =b.rnum
;

delete from TextureMeaning;

insert into TextureMeaning
select 1 volNum
,largeCode
,middleCode
,upper(replace(trim(texName),' ','_'))
,'Korean' language
,1 languageClass
,upper(replace(trim(kr),' ','_')) meaning
,1 showKind
,ttsKind
,stageNum
,turnTexNum
,' ' example1
,' ' example1Meaning
,' ' example2
,' ' example2Meaning
,strftime('%Y%m%d','now','localtime'),strftime('%H%M%S','now','localtime'),'',''
from TextureMeaningTemp4
union all
select 1 volNum
,largeCode
,middleCode
,upper(replace(trim(texName),' ','_'))
,'Japanese' language
,1 languageClass
,upper(replace(trim(jp1),' ','_')) meaning
,showKind
,ttsKind
,stageNum
,turnTexNum
,example1
,example1Meaning
,example2
,example2Meaning
,strftime('%Y%m%d','now','localtime'),strftime('%H%M%S','now','localtime'),'',''
from TextureMeaningTemp4
union all
select 1 volNum
,largeCode
,middleCode
,upper(replace(trim(texName),' ','_'))
,'Japanese' language
,2 languageClass
,upper(replace(trim(jp2),' ','_')) meaning
,showKind
,ttsKind
,stageNum
,turnTexNum
,example1
,example1Meaning
,example2
,example2Meaning
,strftime('%Y%m%d','now','localtime'),strftime('%H%M%S','now','localtime'),'',''
from TextureMeaningTemp4
;

insert into TextureMeaning select * from TextureMeaningGana;

commit;
         
select * from TextureMeaning;

-- verify
select * from TextureInfo
where texUrl like '%/>%'

-- verify stage 5
select stageNum,count(*) cnt
from (
select stageNum,texName cnt
from TextureInfo
group by stageNum,texName
) a
group by stageNum
having count(*) < 5
;

select cnt,count(*)
from (
select stageNum,turnTexNum,texName,count(*) cnt
from TextureInfo
where stageNum > 0
group by stageNum,turnTexNum,texName
having count(*) < 8
) a
group by cnt

select cnt,count(*) from (

select largeCode,middleCode,smallCode,count(*) cnt
from TextureInventory
where texUrl not like 'data%'
group by largeCode,middleCode,smallCode

) a
group by cnt
order by cnt


select cnt,count(*) from (

select largeCode,middleCode,texName,count(*) cnt
from TextureInfo
group by largeCode,middleCode,texName

) a
group by cnt
order by cnt

select texUrlYn,downYn,count(*) from TextureInfo group by texUrlYn,downYn;

select * from TextureInfo where downYn = 'N';

select                                                                      
 a.volNum                                                                   
,b.stageNum                                                                 
,b.turnTexNum                                                               
,a.largeCode                                                                
,a.middleCode                                                               
,a.texName                                                                  
,a.texName                                                                  
,b.meaningKind texShowKind                                                  
,a.texNum                                                                   
,a.texUrl                                                                   
,'Y' texUrlYn                                                               
,'N' downYn                                                                 
,strftime('%Y%m%d','now','localtime')                                       
,strftime('%H%M%S','now','localtime')                                       
,''                                                                         
,''                                                                         
 from TextureInventory a, TextureMeaning b                                  
 where a.volNum=b.volNum                                                    
 and a.largeCode=b.largeCode                                                
 and a.middleCode=b.middleCode                                              
 and a.texName=b.texName                                                    
 and a.texNum <= 30                                                         
 and a.texUrl not like 'data%'                                              
 and a.texUrl not like 'img%'                                               
 and length(a.texUrl) < 300                                                 
 and ( length(a.texUrl) - length(trim(replace(a.texUrl,'https',''))) ) <= 5 
 and ( length(texUrl) - length(trim(replace(texUrl,'gstatic',''))) ) < 10   
 and b.language = 'Korean'                                                  
and a.largeCode = 3 and a.middleCode = 3
 order by b.stageNum, b.turnTexNum, a.texName                               
;


select * from TextureInventory;

select *
from
(
select distinct texName from TextureInventory a
where a.largeCode = 3 and a.middleCode = 3
) a
left join
(
select distinct texName from TextureMeaning a
where a.largeCode = 3 and a.middleCode = 3
) b
on a.texName = b.texName
left join
(
select distinct texName from TextureInfo a
where a.largeCode = 3 and a.middleCode = 3
) c
on a.texName = c.texName







select * from  TextureMeaning 
where trim(meaning) = ''






        */
        //"insert into delete from LanguageAlphabet ";

        print(query);
        sql.ExecuteQuery(query);

        sql.Close();

    }

	


	int volNum,largeCode,middleCode;
	string path;
	string texUrl = "";
	string line = "";
	string texName = "";


	List<string> fileNames = new List<string>();
	void InsertTextureFile (int volNum,int largeCode, int middleCode ) {

        print("largeCode=" + largeCode + " middleCode=" + middleCode);

		this.volNum = volNum; this.largeCode = largeCode; this.middleCode = middleCode;

		path = Application.persistentDataPath + "/word/";
		path += largeCode.ToString("00")+middleCode.ToString("00")+"/";
		print ("path="+path);

		DirectoryInfo dir = new DirectoryInfo(path);
		FileInfo[] info = dir.GetFiles("*.html");

		DelTextureFileNm();
		foreach(FileInfo f in info){ InsTextureFileNm(f.Name);
        }
		SelTextureFileNm();


		int smallCode = 0; 
		for (int i=0;i<fileNames.Count;i++) {	
//			texName = f.Name.Substring(0,f.Name.IndexOf(".")-1).ToUpper();
//			texNum  = f.Name.Substring(f.Name.IndexOf(".")-1,1);
//			query   = "insert into TextureName02 values('"+texName+"',"+texNum+")";
//			print (query);
			print("fileNm="+fileNames[i]);
 

			texName = fileNames[i].Substring(0,fileNames[i].IndexOf(".")).ToUpper();

            DelTextureInventoryTexName(volNum
                        , largeCode
                        , middleCode
                        , texName.Replace(" ", "_")
            );

			StreamReader sr = new StreamReader(path + fileNames[i]);
			if(sr == null)
			{
				print("Error : " + path + fileNames[i]);
			}
			else
			{
				line = sr.ReadLine();
				smallCode++;
				int texNum=0;
				while (line != null)
				{
					if(line.Length > 0){
						texNum++;
						//print (line.Length + " line="+line);
						texUrl = line.Replace("<img src=\"","");
						texUrl = texUrl.Replace("\">","");
						print (texUrl.Length + " texUrl="+texUrl);
                        //if (texUrl.Length < 300)
                        //{
                            InsTextureInventory(volNum
                                                , largeCode
                                                , middleCode
                                                , smallCode
                                                , texName.Replace(" ", "_")
                                                , texNum
                                                , texUrl);
                        //}

					}

					line = sr.ReadLine();
					
				}
				sr.Close();
				// print("Loaded " + Application.dataPath + "/Resources/db/" + fileName);
			}

		}

	}

	public void DelTextureInventory()
	{
		sql.Open(dbPathName);
		
		query  = "delete from TextureInventory "; 
		print(query);
		sql.ExecuteQuery(query);
		
		sql.Close();
		
	}


    public void DelTextureInventoryTexName(
          int volNum
        , int largeCode
        , int middleCode
        , string texName)
    {
        sql.Open(dbPathName);

        query = "delete from TextureInventory ";
        query += " where volNum   =" + volNum;
        query += " and largeCode  =" + largeCode;
        query += " and middleCode =" + middleCode;
        query += " and texName    ='" + texName + "'";
        print(query);
        sql.ExecuteQuery(query);

        sql.Close();

    }


	public void InsTextureInventory(
		int    volNum     
		,int    largeCode  
		,int    middleCode 
		,int    smallCode  
		,string texName    
		,int    texNum     
		,string texUrl)
	{
		sql.Open(dbPathName);

        print("InsTextureInventory");
		
	
		query  = "insert into TextureInventory values ("; 
		/* volNum      */ query += volNum     +",";
		/* largeCode   */ query += largeCode  +",";
		/* middleCode  */ query += middleCode +",";
		/* smallCode   */ query += smallCode  +",";
		/* texName     */ query += "'"+texName+"',";
		/* texNum      */ query += texNum     +",";
		/* texUrl      */ query += "'"+texUrl +"',";
		/* stageNum    */ query += "0,";
		/* turnTexNum  */ query += smallCode  +",";
		/* createDate  */ query += "strftime('%Y%m%d','now','localtime'),";
		/* createTime  */ query += "strftime('%H%M%S','now','localtime'),";
		/* updateDate  */ query += "'',";
		/* updateTime  */ query += "'')";
		
		print(query);
		sql.ExecuteQuery(query);
		
		sql.Close();
		
        

	}


	//-----------------------------
	// file delete
	//-----------------------------
	void fileDelete (string filpathename) {
		File.Delete(filpathename);
	}


	void DropTable (string tableName) {
		sql.Open(dbPathName);
		query = ("drop table " + tableName);
		sql.ExecuteQuery(query);
		print (query);
		sql.Close();	
	}
	
	void CreateTable () {
		
		sql.Open(dbPathName);
		query = ("create table ");
		sql.ExecuteQuery(query);
		print (query);
		sql.Close();	
	}

	void SqliteDBConn(){
		
        string path = Application.persistentDataPath + "/fb/com/dmp/";
        DirectoryInfo di = new DirectoryInfo(path);
        if (di.Exists == false) di.Create();

        dbPathName = Application.persistentDataPath + "/fb/com/dmp/czw00100400015jb1";
        //print (dbPathName);
        
        if (!System.IO.File.Exists(dbPathName)) {
            //Debug.Log("Exists ******  " + dbPathName);
            TextAsset ta = (TextAsset)Resources.Load("Sqlite/czw00100400015jb1.db");
            System.IO.File.WriteAllBytes(dbPathName, ta.bytes); // 64MB limit on File.WriteAllBytes.
            ta = null;
        } else {
            Debug.Log("Exists ******  " + dbPathName);
        }
        sql = new SqliteDatabase();
		
	} 

}
