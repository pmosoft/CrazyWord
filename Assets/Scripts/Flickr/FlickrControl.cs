using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System.IO;
using SQLite4Unity3d;

public class FlickrControl : MonoBehaviour
{
/*
// category
http://commons.wikimedia.org/w/api.php?action=query&list=categorymembers&cmtype=file&cmtitle=Category:sleeve
http://commons.wikimedia.org/w/api.php?action=query&list=categorymembers&cmtype=file&cmtitle=Category:Numbers
http://commons.wikimedia.org/w/api.php?action=query&list=categorymembers&cmtitle=Category:Physics
// category - titles
http://en.wikipedia.org/w/api.php?action=query&titles=San_Francisco&prop=images&imlimit=20&format=jsonfm
http://en.wikipedia.org/w/api.php?action=query&titles=Numbers&prop=images&imlimit=20&format=jsonfm
  
 * 
 * 
// image
http://commons.wikimedia.org/wiki/Category:Addition?uselang=ko#mediaviewer/File:-4%2B-3tiles.PNG
http://commons.wikimedia.org/w/api.php?action=query&titles=Image:Cscr-featured.svg&prop=imageinfo
http://commons.wikimedia.org/w/api.php?action=query&titles=Image:Commons-logo.svg&prop=imageinfo&iiprop=metadata&iimetadataversion=latest
http://upload.wikimedia.org/wikipedia/commons/thumb/f/f8/Wiktionary-logo-en.svg/17px-Wiktionary-logo-en.svg.png
http://commons.wikimedia.org/wiki/File:Swimsuit_drawing.png?uselang=ko
http://commons.wikimedia.org/wiki/File:Swimsuit_drawing.png?uselang=ko
http://commons.wikimedia.org/wiki/Category:numbers?uselang=ko#mediaviewer/File:Bivalvia_numbers.jpg
http://commons.wikimedia.org/wiki/Category:numbers?uselang=ko#mediaviewer/File:1 in Assamese Script.png

// 하위 카테고리를 recercieve로 검출요 
http://commons.wikimedia.org/w/api.php?action=query&list=categorymembers&cmtitle=Category:numbers  
http://commons.wikimedia.org/w/api.php?action=query&list=categorymembers&cmtype=file&cmtitle=Category:Numbers
http://commons.wikimedia.org/w/api.php?action=query&list=categorymembers&cmtype=file&cmtitle=Category:Rational numbers
http://commons.wikimedia.org/wiki/Category:numbers?uselang=ko#mediaviewer/File:100 Number Square-es.svg
http://upload.wikimedia.org/wikipedia/commons/6/6d/100_Number_Square-es.svg
http://commons.wikimedia.org/wiki/Category:numbers?uselang=ko#mediaviewer/File:Decimal-fraction equivalents--v0006.png


 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
// text
http://ko.wikipedia.org/w/api.php?action=query&list=search&srwhat=text&srsearch=computer 
http://ko.wikipedia.org/w/api.php?action=query&prop=revisions&titles=%EC%88%AB%EC%9E%90&rvprop=timestamp|content 

// search screen
http://commons.wikimedia.org/wiki/Category:Addition?uselang=ko
http://commons.wikimedia.org/wiki/Category:numbers?uselang=ko
http://commons.wikimedia.org/wiki/Category:africa?uselang=ko
http://commons.wikimedia.org/w/index.php?title=Special:Search&limit=20&offset=20&profile=images&search=sleeve&uselang=ko
// etc

https://en.wikipedia.org/w/api.php?action=query&titles=Albert%20Einstein&prop=info&inprop=protection%7Ctalkid 

http://commons.wikimedia.org/wiki/Category:San_Francisco?uselang=ko



http://commons.wikimedia.org/wiki/Category:tollgate?uselang=ko x
http://commons.wikimedia.org/wiki/Category:traffic?uselang=ko x

 * 
 */



    public class PhotoSearch
    {
        [PrimaryKey] public int largeCode { get; set; }
        [PrimaryKey] public int middleCode { get; set; }
        [PrimaryKey] public string texName { get; set; }
        [PrimaryKey] public string searchName { get; set; }
        [PrimaryKey] public string photoId { get; set; }
        [PrimaryKey] public int photoNum { get; set; }
        public string photoTitle { get; set; }
        public string photoTbURL { get; set; }
        public string photoImgURL { get; set; }
        public string photoHref { get; set; }
        public string useYn { get; set; }
        public string checkYn { get; set; }
        public string flickrUseYn { get; set; }
        public string createDate { get; set; }
        public string createTime { get; set; }
        public string updateDate { get; set; }
        public string updateTime { get; set; }
        public string userName { get; set; }
    }

    public class PhotoGetInfo
    {
        public string photoId { get; set; }
        public string photoOwner { get; set; }
        public string photoLicense { get; set; }
        public string createDate { get; set; }
        public string createTime { get; set; }
    }

    public EventCommon ev;
    public GameObject PopupBg8;


    string userName = "";

    //--------------------------------
    // sqlite
    //--------------------------------
    SqliteDatabase sql;
    string dbPathName;
    DataTable _data;
    string query;
    DataRow dr;

    /*

    create table PhotoSearchAll (
    largeCode    int           not null,
    middleCode   int           not null,
    texName      varchar(100)  not null,
    photoId      varchar(100)  not null,
    photoNum     int           not null,
    photoImgURL  varchar(500)  not null,  
    photoHref    varchar(500)  not null,
    photoOwner   varchar(100)  not null,
    photoLicense varchar(100)  not null,
    photoSource  varchar(100)    not null,
    createDate   varchar(8)    not null,
    createTime   varchar(6)    not null,
    userName     varchar(50)       null,
    primary key (largeCode,middleCode,texName,searchName,photoId,photoNum)
    );


    drop table PhotoWikiSearch;

    create table PhotoSearch (
    largeCode    int           not null,
    middleCode   int           not null,
    texName      varchar(100)  not null,
    catategyName varchar(100)  not null,
    primary key (largeCode,middleCode,texName,searchName,photoId,photoNum)
    );
 
    drop table PhotoSearch;

    create table PhotoSearch (
    largeCode    int           not null,
    middleCode   int           not null,
    texName      varchar(100)  not null,
    searchName   varchar(100)  not null,
    photoId      varchar(100)  not null,
    photoNum     int           not null,
    photoTitle   varchar(200)  not null,
    photoTbURL   varchar(500)  not null,  
    photoImgURL  varchar(500)  not null,  
    photoHref    varchar(500)  not null,
    useYn        varchar(1)    not null,  -- default:N
    checkYn      varchar(1)    not null,  -- default:N
    flickrUseYn  varchar(1)    not null,  -- default:Y
    createDate   varchar(8)    not null,
    createTime   varchar(6)    not null,
    userName     varchar(50)       null,
    primary key (largeCode,middleCode,texName,searchName,photoId,photoNum)
    );

    drop table PhotoGetInfo;

    create table PhotoGetInfo (
    photoId      varchar(100)  not null,
    photoOwner   varchar(100)  not null,
    photoLicense varchar(100)  not null,
    createDate   varchar(8)    not null,
    createTime   varchar(6)    not null,		
    primary key (photoId)
    );
 
 
    */
    private ISQLiteConnection _connection;

    void Awake() { SqliteDBConn(); }


    // Use this for initialization
    void Start()
    {
        userName = "psh";
        //userName = "ljy";
        //userName = "kds";

        //StartCoroutine( FlickrSearch(1,1,"ADDITION","ADDITION") );	
        //StartCoroutine( FlickrGetInfoBulk() );	

        //StartCoroutine( FlickrDownload(1,1,"number") );	

        //StartCoroutine( MakeTexGrid(1,1,"number") );	
        //StartCoroutine( FlickInfo("8906598833") );	

        //StartCoroutine( FlickrGetInfoBulk2() );	

        //StartCoroutine( FlickInfo2 ( "3293465641" ) );
        StartCoroutine( FlickInfo2 ( "16485994271" ) );

        //StartCoroutine( FlickInfo2 ( "1817309626" ) );
        //StartCoroutine( FlickInfo2 ( "4466728814" ) );
        //StartCoroutine( FlickInfo2 ( "14147313530" ) );



        //TexViewerStart();

        //StartCoroutine(FlickrDownloadCheckBulk(4, 6));

        //StartCoroutine(FlickrSearchBulk(3,0));

        //StartCoroutine(FlickrDownloadBulk(8,4,0," "));

        //StartCoroutine( FlickrSearchBulk(4,6) );	

        //StartCoroutine(FlickrSearch(2, 4, "UMBRELLA", "UMBRELLA"));
        //StartCoroutine(FlickrSearch(2, 4, "WALLET", "WALLET"));

        //StartCoroutine( FlickrDownloadHighBulk(2) );

        
    }


    //-------------------------------------------------------------------
    //                       Texture Viewer
    //-------------------------------------------------------------------

    //public UIGrid texUIGrid;
    public GameObject ScrollViewPanel;
    public GameObject TexPrefab,GridPrefab;
    GameObject TexClone,GridClone;
    UITexture texUITexture;
    UILabel texInfoUILabel, texDescUILabel;
    UISprite texCheckUISprite;
    public UISprite checkUISprite;

    string texInfo;
    UIEventTrigger texUIEventTrigger;

    public UILabel notifyUILabel,flickrUseYnUILabel;

    int largeCode, middleCode;
    string texName, searchName, photoId, photoNum, photoTitle, photoTbURL, photoImgURL, photoHref, useYn, checkYn, flickrUseYn;

    int page;

    public UILabel largeUILabel, middleUILabel, texNameLabel, searchNameUILabel, searchNameInputUILabel;
    public UIPopupList largeUIPopupList, middleUIPopupList, texNameUIPopupList, searchNameUIPopupList, pageUIPopupList;

    public struct TexInfo
    {
        public int largeCode, middleCode;
        public string texName, searchName, photoId;
        public string photoNum, photoTitle,photoTbURL,photoImgURL,photoHref,useYn,checkYn,flickrUseYn;

        public TexInfo(int largeCode, int middleCode, string texName, string searchName
            , string photoId, string photoNum, string photoTitle,string photoTbURL,string photoImgURL,string photoHref
            ,string useYn
            ,string checkYn
            ,string flickrUseYn
            )
        {
            this.largeCode = largeCode;
            this.middleCode = middleCode;
            this.texName = texName;
            this.searchName = searchName;
            this.photoId = photoId;
            this.photoNum = photoNum;
            this.photoTitle = photoTitle;
            this.photoTbURL = photoTbURL;
            this.photoImgURL = photoImgURL;
            this.photoHref = photoHref;
            this.useYn = useYn;
            this.checkYn = checkYn;
            this.flickrUseYn = flickrUseYn;
        }
    }
    public List<TexInfo> texInfos = new List<TexInfo>();


    //----------------------------
    // TexViewerStart
    //----------------------------
    void TexViewerStart()
    {

        _data = SelPhotoSearchCode();

        if (_data.Rows.Count > 0)
        {
            dr = _data.Rows[0];
            largeCode = int.Parse(dr["largeCode"].ToString());
            middleCode = int.Parse(dr["middleCode"].ToString());
            texName = dr["texName"].ToString();

            // TexName
            _data = SelTextureMeaning(largeCode, middleCode);
            print(_data.Rows.Count);
            texNameUIPopupList.items.Clear();
            for (int i = 0; i < _data.Rows.Count; i++)
            {
                dr = _data.Rows[i];
                texNameUIPopupList.items.Add(dr["texName"].ToString());
            }

            // SechName
            texName = texNameUIPopupList.items[0];
            _data = SelPhotoSearchSearchName(largeCode,middleCode,texName);
            searchNameUIPopupList.items.Clear();
            for (int i = 0; i < _data.Rows.Count; i++) {
                dr = _data.Rows[i];
                searchNameUIPopupList.items.Add(dr["searchName"].ToString());
            }
            searchNameUIPopupList.value = searchNameUIPopupList.items[0];


            //searchName = dr["searchName"].ToString();
        }
        else
        {
            largeCode = 1;
            middleCode = 1;
            texName = "no data";
        }

        largeUIPopupList.value = largeUIPopupList.items[largeCode - 1];
        middleUIPopupList.value = middleUIPopupList.items[middleCode - 1];
        texNameUIPopupList.value = texName;
        //texNameLabel.text = texName;
        //searchNameUILabel.text = texName;
    }

    //----------------------------
    // MakeTexGrid
    //----------------------------
    public IEnumerator MakeTexGrid(int largeCode, int middleCode, string texName, string searchName, int page)
    {
        if(!isBulk) {
            print("MakeTexGrid");

            PopupBg8.SetActive(true);

            string useYn;
            if (checkUISprite.spriteName == "bgediteoption3") useYn = "Y"; else useYn = "N";
            flickrUseYnUILabel.text = "Use:Y";

            _data = SelPhotoSearch(largeCode, middleCode, texName, searchName, useYn, page, " ");

		    Transform[] trans = ScrollViewPanel.GetComponentsInChildren<Transform>(); 
		    foreach (Transform tr in trans) {
			    if(tr.gameObject.tag != "Grid") Destroy(tr.gameObject); 
		    }
            texInfos.Clear();
            for (int i = 0; i < _data.Rows.Count; i++)
            {
                dr = _data.Rows[i];

                searchName = dr["searchName"].ToString();
                photoId = dr["photoId"].ToString();
                photoNum = dr["photoNum"].ToString();
                photoTitle = dr["photoTitle"].ToString();
                photoTbURL = dr["photoTbURL"].ToString();
                photoImgURL = dr["photoImgURL"].ToString();
                photoHref = dr["photoHref"].ToString();
                useYn = dr["useYn"].ToString();
                checkYn = dr["checkYn"].ToString();
                flickrUseYn = dr["flickrUseYn"].ToString();

                texInfos.Add(new TexInfo(largeCode, middleCode, texName, searchName, photoId,photoNum, photoTitle,photoTbURL,photoImgURL,photoHref,useYn,checkYn,flickrUseYn));

                //ScrollViewPanel
                if(i%5==0) {
                    GridClone = Instantiate(GridPrefab) as GameObject;
                    GridClone.transform.parent = ScrollViewPanel.transform;
                    GridClone.transform.localPosition = new Vector3(-1000, 500-( 380*(i/5) ), 0f);
                    GridClone.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    GridClone.transform.localScale = Vector3.one;
                }

                TexClone = Instantiate(TexPrefab) as GameObject;
                TexClone.transform.parent = GridClone.transform;
                print("380*("+i+"%5)="+380*(i%5));
                TexClone.transform.localPosition = new Vector3(380*(i%5), 0, 0f);
                TexClone.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                TexClone.transform.localScale = Vector3.one;
                TexClone.transform.name = i.ToString();
                //TexClone.transform.name = photoId;

                //--------------------------
                // TexInfo
                //--------------------------
                texInfo = largeCode + "-" + middleCode + ":" + texName + "\n";
                texInfoUILabel = TexClone.transform.FindChild("Top").FindChild("TexInfo").GetComponent<UILabel>();
                texInfoUILabel.text = texInfo;

                //--------------------------
                // TexDesc
                //--------------------------
                texInfo = photoTitle;
                texDescUILabel = TexClone.transform.FindChild("TexDesc").GetComponent<UILabel>();
                texDescUILabel.text = texInfo;

                //--------------------------
                // Check
                //--------------------------
                texUIEventTrigger = TexClone.transform.FindChild("Top").FindChild("Check").GetComponent<UIEventTrigger>();
                EventDelegate.Add(texUIEventTrigger.onPress, ev.EventOnPress);

                EventDelegate eventClick = new EventDelegate(this, "EventTexCheck");
                EventDelegate.Parameter param = new EventDelegate.Parameter();
                param.obj = TexClone.transform;
                param.expectedType = typeof(Transform);
                eventClick.parameters[0] = param;
                EventDelegate.Add(texUIEventTrigger.onRelease, eventClick);

                texCheckUISprite = TexClone.transform.FindChild("Top").FindChild("Check").GetComponent<UISprite>();
                if (useYn == "Y") texCheckUISprite.spriteName = "bgediteoption3"; else texCheckUISprite.spriteName = "bgediteoption1";

                //--------------------------
                // Texture
                //--------------------------
                texUITexture = TexClone.transform.FindChild("Texture").GetComponent<UITexture>();

                string texPath, texFileName;
                WWW www;

                Texture2D wwwTexture;

                texPath = Application.persistentDataPath + "/Flickr/" + largeCode;
                texPath += "/" + middleCode + "/" + texName + "/";
                texFileName = texName + "_" + photoId + ".jpg";

                print("i=" + i + " " + texFileName);

                FileInfo FI = new FileInfo(texPath + texFileName);
                if (FI.Exists == true) {
                    notifyUILabel.text = (i+1).ToString();
                    www = new WWW("file:///" + texPath + texFileName);
                    yield return www;
                    wwwTexture = new Texture2D(272, 272, TextureFormat.ARGB32, false);
                    www.LoadImageIntoTexture(wwwTexture);
                    texUITexture.mainTexture = wwwTexture;
                    //yield return new WaitForSeconds(0.01f);
                } else {
                    yield return null;
                    notifyUILabel.text = "파일을 다운받으세요";
                }

            }
            print("i=finish");
            if(flickrUseYn == "N") {
                notifyUILabel.text = "적합 이미지 미존재";
                flickrUseYnUILabel.text = "Use:N";
            } 
            //SpringPanel springPanel; 
            //springPanel = ScrollViewPanel.GetComponent<SpringPanel>();
            //springPanel.target = new Vector3(0, 0, 0);


            //notifyUILabel.text = "finish";
            PopupBg8.SetActive(false);
        }
    }

    public void OnChangeLarge()
    {
        if(!isBulk) ev.EventOnPress();
        EventMiddleQuery();
        EventTexNameQuery();
        checkUISprite.spriteName = "bgediteoption1";
        pageUIPopupList.value = "1";

    }

    public void OnChangeMiddle()
    {
        if(!isBulk) ev.EventOnPress();
        EventTexNameQuery();
        checkUISprite.spriteName = "bgediteoption1";
        pageUIPopupList.value = "1";

    }

    //----------------------------
    // EventMiddleQuery
    //----------------------------
    public void EventMiddleQuery()
    {

        print("EventMiddleQuery");

        ev.EventOnRelease();
        if (ev.isCurPrevDist && !isBulk)
        {
            largeCode = int.Parse(largeUILabel.text);
            _data = SelTextureMeaningMiddle(largeCode);

            middleUIPopupList.items.Clear();

            for (int i = 0; i < _data.Rows.Count; i++) {
                dr = _data.Rows[i];
                middleUIPopupList.items.Add(dr["middleCode"].ToString());
            }
            middleUIPopupList.value = middleUIPopupList.items[0];

            EventTexNameQuery();
        }


    }

    //----------------------------
    // EventTexNameQuery
    //----------------------------
    public void EventTexNameQuery()
    {

        print("EventTexNameQuery");

        ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            largeCode = int.Parse(largeUILabel.text);
            middleCode = int.Parse(middleUILabel.text);

            // TexName
            _data = SelTextureMeaningTexName(largeCode,middleCode);
            texNameUIPopupList.items.Clear();
            for (int i = 0; i < _data.Rows.Count; i++) {
                dr = _data.Rows[i];
                print("texName"+dr["texName"].ToString());
                texNameUIPopupList.items.Add(dr["texName"].ToString());
            }
            texNameUIPopupList.value = texNameUIPopupList.items[0];

            // SechName
            texName = texNameUIPopupList.items[0];
            _data = SelPhotoSearchSearchName(largeCode,middleCode,texName);
            searchNameUIPopupList.items.Clear();
            for (int i = 0; i < _data.Rows.Count; i++) {
                dr = _data.Rows[i];
                searchNameUIPopupList.items.Add(dr["searchName"].ToString());
            }
            searchNameUIPopupList.value = searchNameUIPopupList.items[0];

            checkUISprite.spriteName = "bgediteoption1";
            pageUIPopupList.value = "1";

        }
    }
     
    public void OnChangeTexName()
    {
        if(!isBulk) ev.EventOnPress();
        EventSearchNameQuery();
        checkUISprite.spriteName = "bgediteoption1";
        pageUIPopupList.value = "1";
    }

    //----------------------------
    // EventTexNameQuery
    //----------------------------
    public void EventSearchNameQuery()
    {

        print("EventSearchNameQuery");

        ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            largeCode = int.Parse(largeUILabel.text);
            middleCode = int.Parse(middleUILabel.text);
            middleCode = int.Parse(middleUILabel.text);
            texName = texNameUIPopupList.value;

            _data = SelPhotoSearchSearchName(largeCode,middleCode,texName);
            searchNameUIPopupList.items.Clear();
            for (int i = 0; i < _data.Rows.Count; i++) {
                dr = _data.Rows[i];
                searchNameUIPopupList.items.Add(dr["searchName"].ToString());
            }
            searchNameUIPopupList.value = searchNameUIPopupList.items[0];

            checkUISprite.spriteName = "bgediteoption1";
            pageUIPopupList.value = "1";
        }
    }

    //----------------------------
    // EventShowTex
    //----------------------------
    public void EventShowTex()
    {

        print("EventShowTex");
        if(!isBulk) {
        //ev.EventOnRelease();
        //if (ev.isCurPrevDist)
        //{
            notifyUILabel.text = "";
            
            largeCode = int.Parse(largeUILabel.text);
            middleCode = int.Parse(middleUILabel.text);
            texName = texNameLabel.text;
            searchName = searchNameUILabel.text;
            page = int.Parse(pageUIPopupList.value);
            StartCoroutine(MakeTexGrid(largeCode, middleCode, texName, searchName, page));
        //}
        }
    }

    //----------------------------
    // EventCheck
    //----------------------------
    public void EventCheck()
    {

        print("EventCheck");

        ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            if (checkUISprite.spriteName == "bgediteoption3") checkUISprite.spriteName = "bgediteoption1";
            else checkUISprite.spriteName = "bgediteoption3";
        }
    }

    //----------------------------
    // EventTexCheck
    //----------------------------
    bool isCheck = false; bool isEventTexCheck = false;
    public void EventTexCheck(Transform gameo)
    {

        print("EventTexCheck");
        isEventTexCheck = true;

        texCheckUISprite = gameo.FindChild("Top").FindChild("Check").GetComponent<UISprite>();
        if (texCheckUISprite.spriteName == "bgediteoption3") isCheck = true; else isCheck = false;

        int idx = int.Parse(gameo.name);

        largeCode = texInfos[idx].largeCode;
        middleCode = texInfos[idx].middleCode;
        texName = texInfos[idx].texName;
        searchName = texInfos[idx].searchName;
        photoId = texInfos[idx].photoId;

        ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            if (!isCheck)
            {
                texCheckUISprite.spriteName = "bgediteoption3";
                _data = SelPhotoGetInfo(photoId);

                isBulk = false;
                int rowCnt = _data.Rows.Count;
                //if (rowCnt == 0) FlickInfo(photoId);
                StartCoroutine ( InsPhotoSearchWWW(idx) );
            }
            else
            {
                StartCoroutine ( DelPhotoSearchWWW(idx) );
            }
            //audioSource.clip = menuClip; audioSource.Play();
            //g.friendListIdx = int.Parse(gameo.name.Substring(6,1));
            //g.fbFriendId = fbFriendIds[g.friendListIdx];
        }
    }
    public IEnumerator InsPhotoSearchWWW(int idx) {
        //print("www InsPhotoSearchWWW");
        //WWWForm form = new WWWForm();
        //form.AddField("largeCode"  ,texInfos[idx].largeCode);
        //form.AddField("middleCode" ,texInfos[idx].middleCode);
        //form.AddField("texName"    ,texInfos[idx].texName);
        //form.AddField("searchName" ,texInfos[idx].searchName);
        //form.AddField("photoId"    ,texInfos[idx].photoId);
        //form.AddField("photoNum"   ,texInfos[idx].photoNum);   
        //form.AddField("photoTitle" ,texInfos[idx].photoTitle);
        //form.AddField("photoTbURL" ,texInfos[idx].photoTbURL);
        //form.AddField("photoImgURL",texInfos[idx].photoImgURL);
        //form.AddField("photoHref"  ,texInfos[idx].photoHref);
        //form.AddField("useYn"      ,texInfos[idx].useYn); 
        //form.AddField("checkYn"    ,texInfos[idx].checkYn); 
        //form.AddField("flickrUseYn",texInfos[idx].flickrUseYn);
        //form.AddField("userName", userName); 

        //print("photoTitle="+photoTitle);

        //WWW www = new WWW("http://crazyword.org/InsPhotoSearch.asp", form);

        //yield return www;
        //if (www.isDone && www.error == null) {
        //    print("InsPhotoSearchWWW www.text=" + www.text);
            UptPhotoSearch(largeCode, middleCode, texName, photoId, "Y");

            searchName = texInfos[idx].searchName;
            photoId = texInfos[idx].photoId;
            
            page = int.Parse( pageUIPopupList.value );

            useYn = "Y"; 
            //useYn = " "; 

            StartCoroutine( FlickrDownload(largeCode,middleCode,texName,searchName,page,useYn,photoId," ") );
        //} else {
        //    texCheckUISprite.spriteName = "bgediteoption1";
        //    print("InsPhotoSearchWWW www.error=" + www.error);
        //}
        yield return null;
    }
    public IEnumerator DelPhotoSearchWWW(int idx) {
        print("www DelPhotoSearchWWW");
        WWWForm form = new WWWForm();
        form.AddField("largeCode"  ,texInfos[idx].largeCode);
        form.AddField("middleCode" ,texInfos[idx].middleCode);
        form.AddField("texName"    ,texInfos[idx].texName);
        form.AddField("searchName" ,texInfos[idx].searchName);
        form.AddField("photoId"    ,texInfos[idx].photoId);

        WWW www = new WWW("http://crazyword.org/DelPhotoSearch.asp", form);

        yield return www;
        if (www.isDone && www.error == null) {
            print("DelPhotoSearchWWW www.text=" + www.text);
            texCheckUISprite.spriteName = "bgediteoption1";
            UptPhotoSearch(largeCode, middleCode, texName, photoId, "N");
        } else {
            print("DelPhotoSearchWWW www.error=" + www.error);
        }

        isEventTexCheck = false;
    }

    //----------------------------
    // EventFlickUseYn
    //----------------------------
    public void EventFlickUseYn() {
        print("EventFlickUseYn");

        largeCode = int.Parse(largeUILabel.text);
        middleCode = int.Parse(middleUILabel.text);
        texName = texNameLabel.text;

        ev.EventOnRelease();
        if (ev.isCurPrevDist) {
            StartCoroutine ( InsPhotoSearchWWW() );
        }
    }
    public IEnumerator InsPhotoSearchWWW() {
        print("www InsPhotoSearchWWW");
        WWWForm form = new WWWForm();
        form.AddField("largeCode"  ,largeCode);
        form.AddField("middleCode" ,middleCode);
        form.AddField("texName"    ,texName);
        form.AddField("searchName" ,texName);
        form.AddField("photoId"    ," ");
        form.AddField("photoNum"   ," ");   
        form.AddField("photoTitle" ," ");
        form.AddField("photoTbURL" ," ");
        form.AddField("photoImgURL"," ");
        form.AddField("photoHref"  ," ");
        form.AddField("useYn"      ,"N"); 
        form.AddField("checkYn"    ,"Y"); 
        form.AddField("flickrUseYn","N"); 

        WWW www = new WWW("http://crazyword.org/InsPhotoSearch.asp", form);

        yield return www;
        if (www.isDone && www.error == null) {
            print("InsPhotoSearchWWW www.text=" + www.text);
            flickrUseYnUILabel.text = "Use:N";
            UptPhotoSearchFlickrUseYn(largeCode,middleCode,texName);
        } else {
            print("InsPhotoSearchWWW www.error=" + www.error);
        }
    }


    //----------------------------
    // EventFlickLoad
    //----------------------------
    public void EventFlickLoad()
    {

        print("EventFlickLoad");

        ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            largeCode = int.Parse(largeUILabel.text);
            middleCode = int.Parse(middleUILabel.text);
            texName = texNameLabel.text;
            searchName = searchNameInputUILabel.text;

            StartCoroutine(FlickrSearch(largeCode, middleCode, texName, searchName));
        }
    }


    //----------------------------
    // EventTexDownLoad
    //----------------------------
    public void EventTexDownLoad()
    {
        print("EventTexDownLoad");

        ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            largeCode = int.Parse(largeUILabel.text);
            middleCode = int.Parse(middleUILabel.text);
            texName = texNameLabel.text;
            searchName = searchNameUILabel.text;
            page = int.Parse( pageUIPopupList.value );
            photoId = " ";
            string useYn;
            if (checkUISprite.spriteName == "bgediteoption3") {
                useYn = "Y"; 
                StartCoroutine( FlickrDownload(largeCode,middleCode,texName,searchName,page,useYn,photoId," ") );
            } else {
                useYn = "N";
                StartCoroutine(FlickrDownload(largeCode, middleCode, texName, searchName, 1, useYn, photoId, " "));
                StartCoroutine(FlickrDownload(largeCode, middleCode, texName, searchName, 2, useYn, photoId, " "));
                StartCoroutine(FlickrDownload(largeCode, middleCode, texName, searchName, 3, useYn, photoId, " "));
                StartCoroutine(FlickrDownload(largeCode, middleCode, texName, searchName, 4, useYn, photoId, " "));
                StartCoroutine(FlickrDownload(largeCode, middleCode, texName, searchName, 5, useYn, photoId, " "));
            }
        }
    }



    //----------------------------
    // EventSearchWordQuery
    //----------------------------
    public void EventSearchWordQuery()
    {

        print("EventSearchWordQuery");

        ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            largeCode = int.Parse(largeUILabel.text);
            middleCode = int.Parse(middleUILabel.text);
            texName = SelPhotoSearchTexName(largeCode, middleCode);
            texNameLabel.text = texName;
            searchNameUILabel.text = texName;
        }
    }

    //-------------------------------------------------------------------
    //                        Flickr Api
    //-------------------------------------------------------------------

    //---------------------------------------
    // FlickrSearch batch            
    //---------------------------------------
    IEnumerator FlickrDownloadBulk(int largeCode1, int middleCode1, int page1, string highYn)
    {
        isBulk = true;
        print("FlickrDownloadBulk");
        DataTable _data = SelTextureMeaning(largeCode1,middleCode1);
        for (int i = 0; i < _data.Rows.Count; i++)
        {
            DataRow dr = _data.Rows[i];
            int largeCode = int.Parse(dr["largeCode"].ToString());
            int middleCode = int.Parse(dr["middleCode"].ToString());
            string texName = dr["texName"].ToString();
            string searchName = texName;
            //print(i+" "+largeCode + " " + middleCode + " " + texName);

            yield return StartCoroutine(FlickrDownload(largeCode, middleCode, texName, searchName, page1, " ", " ", highYn));
            
            //yield return new WaitForSeconds(10f);

        }
    }


    IEnumerator FlickrDownloadHighBulk(int largeCode1)
    {
        isBulk = true;
        print("FlickrDownloadBulk");
        DataTable _data = SelPhotoSearchUseY(largeCode1);
        for (int i = 0; i < _data.Rows.Count; i++)
        {
            DataRow dr = _data.Rows[i];
            int largeCode = int.Parse(dr["largeCode"].ToString());
            int middleCode = int.Parse(dr["middleCode"].ToString());
            string texName = dr["texName"].ToString();
            string searchName = texName;
            string photoId = dr["photoId"].ToString();

            //print(i+" "+largeCode + " " + middleCode + " " + texName);

            yield return StartCoroutine(FlickrDownload(largeCode, middleCode, texName, searchName, 0, "Y", photoId, "Y"));
            
            //yield return new WaitForSeconds(10f);
        }
    }


    IEnumerator FlickrDownloadCheckBulk(int largeCode1, int middleCode1)
    {
        isBulk = true;
        print("FlickrDownloadCheckBulk");
        DataTable _data = SelTextureMeaning(largeCode1, middleCode1);

        string texPath;
        DirectoryInfo di;

        for (int i = 0; i < _data.Rows.Count; i++)
        {
            DataRow dr = _data.Rows[i];
            int largeCode = int.Parse(dr["largeCode"].ToString());
            int middleCode = int.Parse(dr["middleCode"].ToString());
            string texName = dr["texName"].ToString();
            string searchName = texName;
            texPath = Application.persistentDataPath + "/Flickr/" + largeCode;
            texPath += "/" + middleCode + "/" + texName + "/";

            di = new DirectoryInfo(texPath);
            if (di.Exists == false) di.Create();

            FileInfo[] files = di.GetFiles();
            if (files.Length <= 245)
            {
                useYn = "N";
                print(texPath + "=" + files.Length);
                yield return StartCoroutine(FlickrDownload(largeCode, middleCode, texName, searchName, 0, " ", " ", " "));
            }
        }
        print("FlickrDownloadCheckBulk end");
    }


    //---------------------------------------
    // FlickrSearch batch            
    //---------------------------------------
    bool isBulk = false;
    IEnumerator FlickrSearchBulk(int largeCode1, int middleCode1)
    {

        print("FlickrSearchBulk");
        isBulk = true;
        DataTable _data = SelTextureMeaning(largeCode1,middleCode1);
        for (int i = 0; i < _data.Rows.Count; i++)
        {
            DataRow dr = _data.Rows[i];
            int largeCode = int.Parse(dr["largeCode"].ToString());
            int middleCode = int.Parse(dr["middleCode"].ToString());
            string texName = dr["texName"].ToString();
            string searchName = texName;

            print(i+" "+largeCode + " " + middleCode + " " + texName);

            //yield return StartCoroutine(FlickrSearch(largeCode, middleCode, texName, searchName));
            StartCoroutine(FlickrSearch(largeCode, middleCode, texName, searchName));

            yield return new WaitForSeconds(1f);
        }
    }


    //---------------------------------------
    // FlickrSearch             
    //---------------------------------------
    public IEnumerator FlickrSearch(int largeCode, int middleCode, string texName, string searchName)
    {

        PopupBg8.SetActive(true);
        //string tagsName = "ADD";
        //string tagsName = "Number";

        searchName = searchName.ToUpper();
        print("FlickrSearch:" + searchName);
        WWWForm form = new WWWForm();
        form.AddField("method", "flickr.photos.search");
        form.AddField("api_key", "bf0b4822ed59cb01261094b58685cdd7");
        form.AddField("format", "json");
        form.AddField("page", "1");
        form.AddField("per_page", "500");
        form.AddField("sort", "relevance");
        form.AddField("license", "4,5,6");
        form.AddField("text", searchName);
        //form.AddField("text", "된장");

        //form.AddField("tags", tagsName);
        //form.AddField("tags", searchName);

        /*
          https://www.flickr.com/services/rest/?method=flickr.photos.search&api_key=bf0b4822ed59cb01261094b58685cdd7&format=json&text=addtion
          https://www.flickr.com/services/rest/?method=flickr.photos.getInfo&api_key=bf0b4822ed59cb01261094b58685cdd7&format=json&photo_id=16485994271
          https://www.flickr.com/services/rest/?method=flickr.photos.getInfo&api_key=bf0b4822ed59cb01261094b58685cdd7&format=json&photo_id=8317351311
         * 
         * "9668363908"
            "6148180362"
"9358374341"
"5849179473"

         * 
         * 
        */

        WWW www = new WWW("https://www.flickr.com/services/rest", form);

        yield return www;

        string photoId, photoTitle, photoTbURL, photoImgURL, photoHref;
        string photoOwner, photoFarm, photoServer, photoSecret;


        // Success
        if (www.isDone && www.error == null)
        {
            print("www FlickrSearch=" + www.text);
            var FlickrSearch = JSON.Parse(www.text);
            int rowCnt = FlickrSearch["photos"]["photo"].Count;
            print("rowCnt=" + rowCnt);

            DelPhotoSearch(largeCode, middleCode, texName, searchName);

            int iCnt = 1;
            for (int i = 0; i < rowCnt; i++)
            {
                photoId = FlickrSearch["photos"]["photo"][i]["id"];
                photoTitle = FlickrSearch["photos"]["photo"][i]["title"];
                if (photoTitle == null) photoTitle = " ";
                photoTitle = photoTitle.Replace("'","");

                photoOwner = FlickrSearch["photos"]["photo"][i]["owner"];
                photoFarm = FlickrSearch["photos"]["photo"][i]["farm"];
                photoServer = FlickrSearch["photos"]["photo"][i]["server"];
                photoSecret = FlickrSearch["photos"]["photo"][i]["secret"];

                photoTbURL = "http://farm" + photoFarm + ".static.flickr.com/" + photoServer + "/" + photoId + "_" + photoSecret + "_t.jpg";
                photoImgURL = "http://farm" + photoFarm + ".static.flickr.com/" + photoServer + "/" + photoId + "_" + photoSecret + "_z.jpg";
                photoHref = "http://www.flickr.com/photos/" + photoOwner + "/" + photoId;

                //print("photoId    ="+photoId    );
                //print("photoTitle ="+photoTitle );
                //print("photoOwner ="+photoOwner );
                //print("photoFarm  ="+photoFarm  );
                //print("photoServer="+photoServer);
                //print("photoSecret="+photoSecret);
                //print("photoTbURL ="+photoTbURL );
                //print("photoImgURL="+photoImgURL);
                //print("photoHref  ="+photoHref  );

                if (photoTitle.ToUpper().IndexOf(searchName) > -1)
                {
                    InsPhotoSearch(
                         largeCode
                        , middleCode
                        , texName
                        , searchName
                        , photoId
                        , iCnt
                        , photoTitle
                        , photoTbURL
                        , photoImgURL
                        , photoHref
                        );

                    iCnt++;
                }
            }

            for (int i = 0; i < rowCnt; i++)
            {
                photoId = FlickrSearch["photos"]["photo"][i]["id"];
                photoTitle = FlickrSearch["photos"]["photo"][i]["title"];
                if (photoTitle == null) photoTitle = " ";
                photoTitle = photoTitle.Replace("'","");

                photoOwner = FlickrSearch["photos"]["photo"][i]["owner"];
                photoFarm = FlickrSearch["photos"]["photo"][i]["farm"];
                photoServer = FlickrSearch["photos"]["photo"][i]["server"];
                photoSecret = FlickrSearch["photos"]["photo"][i]["secret"];

                photoTbURL = "http://farm" + photoFarm + ".static.flickr.com/" + photoServer + "/" + photoId + "_" + photoSecret + "_t.jpg";
                photoImgURL = "http://farm" + photoFarm + ".static.flickr.com/" + photoServer + "/" + photoId + "_" + photoSecret + "_z.jpg";
                photoHref = "http://www.flickr.com/photos/" + photoOwner + "/" + photoId;

                //print("photoId    ="+photoId    );
                //print("photoTitle ="+photoTitle );
                //print("photoOwner ="+photoOwner );
                //print("photoFarm  ="+photoFarm  );
                //print("photoServer="+photoServer);
                //print("photoSecret="+photoSecret);
                //print("photoTbURL ="+photoTbURL );
                //print("photoImgURL="+photoImgURL);
                //print("photoHref  ="+photoHref  );

                if(photoTitle.ToUpper().IndexOf(searchName) == -1 && photoTitle.Length < 30) {

                    InsPhotoSearch(
                         largeCode
                        , middleCode
                        , texName
                        , searchName
                        , photoId
                        , iCnt
                        , photoTitle
                        , photoTbURL
                        , photoImgURL
                        , photoHref
                        );

                    iCnt++;
                }
            }


            print("iCnt=" + iCnt);

            //StartCoroutine( FlickrDownload(largeCode,middleCode,texName,1) );
            notifyUILabel.text = "FlickrSearch Done";
            if(!isBulk){
                EventSearchNameQuery();
                searchNameUIPopupList.value = searchName;
                checkUISprite.spriteName = "bgediteoption1";
                pageUIPopupList.value = "1";
            }
        }
        else
        {
            print("www.error=" + www.error);
            notifyUILabel.text = "FlickrSearch Err";
        }

        PopupBg8.SetActive(false);

    }



    //---------------------------------------
    // FlickrDownload            
    //---------------------------------------
    public IEnumerator FlickrDownload(int largeCode1, int middleCode1, string texName1, string searchName1, int page, string useYn, string photoId1, string highYn)
    {
        print("FlickrDownload:"+largeCode1+" "+middleCode1+" "+texName1+" highYn="+highYn);
        PopupBg8.SetActive(true);

        string texPath, texFileName;
        DirectoryInfo di;

        texPath = Application.persistentDataPath + "/Flickr/" + largeCode1;
        texPath += "/" + middleCode1 + "/" + texName1 + "/";

        di = new DirectoryInfo(texPath);
        if (di.Exists == false) di.Create();

        //FileInfo[] files = di.GetFiles();
        //foreach (FileInfo F in files)
        //{
        //    F.Delete();
        //}

        DataTable _data;
        DataRow dr;
        _data = SelPhotoSearch(largeCode1, middleCode1, texName1, searchName1, useYn, page, photoId1);
        //print("_data.Rows.Count=" + _data.Rows.Count);

        int iCnt = 0;
        //------------------------------
        // start www download
        //------------------------------
        for (int i = 0; i < _data.Rows.Count; i++)
        {
            int largeCode, middleCode;
            string texName, photoId, photoTbURL, photoImgURL;

            WWW www;
            Texture2D downTexture;
            int texWidth, texHeight;

            dr = _data.Rows[i];

            largeCode = int.Parse(dr["largeCode"].ToString());
            middleCode = int.Parse(dr["middleCode"].ToString());
            texName = dr["texName"].ToString();
            photoId = dr["photoId"].ToString();
            photoTbURL = dr["photoTbURL"].ToString();
            photoImgURL = dr["photoImgURL"].ToString();

            print(largeCode+" "+middleCode+" "+texName+" "+photoId+":"+iCnt);

            //www = new WWW( photoImgURL );
            if(useYn == "Y" || highYn == "Y") {
                //print("high");
                //print("photoImgURL="+photoImgURL);
                www = new WWW(photoImgURL);
            } else { 
                //print("low");
                www = new WWW(photoTbURL);
            }
            yield return www;

            texWidth = www.texture.width > 256 ? 256 : www.texture.width;
            texHeight = www.texture.height > 256 ? 256 : www.texture.height;

            downTexture = new Texture2D(texWidth, texHeight, TextureFormat.ARGB32, false);
            www.LoadImageIntoTexture(downTexture);
            byte[] bytes = downTexture.EncodeToJPG();

            texFileName = texName + "_" + photoId + ".jpg";
            //print(texFileName);
            File.WriteAllBytes(texPath + texFileName, bytes);

            iCnt++;
            notifyUILabel.text = iCnt.ToString();
        }

        //print("finish download");
        if(isEventTexCheck && !isBulk) EventShowTex();
        PopupBg8.SetActive(false);
        isEventTexCheck = false;


    }

    //---------------------------------------
    // FlickrSearch batch            
    //---------------------------------------
    IEnumerator FlickrGetInfoBulk()
    {
        isBulk = true;
        print("FlickrSearchBulk");
        DataTable _data = SelPhotoSearchPhotoId();
        for (int i = 0; i < _data.Rows.Count; i++) {
            DataRow dr = _data.Rows[i];
            yield return StartCoroutine( FlickInfo ( dr["photoId"].ToString() ) );
            //StartCoroutine( FlickInfo ( dr["photoId"].ToString() ) );

            yield return new WaitForSeconds(1f);
            
        }
    }

    IEnumerator FlickrGetInfoBulk2()
    {
        isBulk = true;
        print("FlickrSearchBulk");
        DataTable _data = SelTextureInfoPhotoId();
        for (int i = 0; i < _data.Rows.Count; i++) {
            DataRow dr = _data.Rows[i];
            yield return StartCoroutine( FlickInfo2 ( dr["photoId"].ToString() ) );
            //StartCoroutine( FlickInfo ( dr["photoId"].ToString() ) );

            yield return new WaitForSeconds(1f);
            
        }
    }


    //---------------------------------------
    // FlickInfo         
    //---------------------------------------
    public IEnumerator FlickInfo(string photoId)
    {
        print("FlickInfo "+photoId);
        WWWForm form = new WWWForm();
        form.AddField("method", "flickr.photos.getInfo");
        form.AddField("api_key", "bf0b4822ed59cb01261094b58685cdd7");
        form.AddField("format", "json");
        form.AddField("photo_id", photoId);

        /*
          https://www.flickr.com/services/rest/?method=flickr.photos.search&api_key=bf0b4822ed59cb01261094b58685cdd7&text=number&format=json
          https://www.flickr.com/services/rest/?method=flickr.photos.getInfo&api_key=bf0b4822ed59cb01261094b58685cdd7&photo_id=16485994271&format=json
          
         4:cc by 5:cc by sa  6:cc by nd
        */

        WWW www = new WWW("https://www.flickr.com/services/rest", form);

        yield return www;

        // Success
        if (www.isDone && www.error == null)
        {
            print("www FlickrSearch=" + www.text);
            var FlickrGetInfo = JSON.Parse(www.text);
            int rowCnt = FlickrGetInfo["photo"].Count;
            print("rowCnt=" + rowCnt);

            string photoOwner = FlickrGetInfo["photo"]["owner"]["username"];
            string photoLicense = FlickrGetInfo["photo"]["license"];

            print("photoOwner=" + photoOwner);
            print("photoLicense=" + photoLicense);

            if (rowCnt > 0) InsPhotoGetInfo(photoId, photoOwner, photoLicense);

        }
        else
        {
            print("www.error=" + www.error);
        }
    }

    public IEnumerator FlickInfo2(string photoId)
    {
        print("FlickInfo "+photoId);
        WWWForm form = new WWWForm();
        form.AddField("method", "flickr.photos.getInfo");
        form.AddField("api_key", "bf0b4822ed59cb01261094b58685cdd7");
        form.AddField("format", "json");
        form.AddField("photo_id", photoId);

        /*
          https://www.flickr.com/services/rest/?method=flickr.photos.search&api_key=bf0b4822ed59cb01261094b58685cdd7&text=number&format=json
          https://www.flickr.com/services/rest/?method=flickr.photos.getInfo&api_key=bf0b4822ed59cb01261094b58685cdd7&photo_id=16485994271&format=json
             4:cc by 5:cc by sa  6:cc by nd
        */

        WWW www = new WWW("https://www.flickr.com/services/rest", form);

        yield return www;

        // Success
        if (www.isDone && www.error == null)
        {
            print("www FlickrSearch=" + www.text);
            var FlickrGetInfo = JSON.Parse(www.text);
            int rowCnt = FlickrGetInfo["photo"].Count;
            print("rowCnt=" + rowCnt);

            if(rowCnt > 0) {

                string photoOwner = FlickrGetInfo["photo"]["owner"]["username"];
                string nsid = FlickrGetInfo["photo"]["owner"]["nsid"];
                string photoLicense = FlickrGetInfo["photo"]["license"];

                string photoFarm = FlickrGetInfo["photo"]["farm"];
                string photoServer = FlickrGetInfo["photo"]["server"];
                string photoSecret = FlickrGetInfo["photo"]["secret"];

                string photoTbURL = "http://farm" + photoFarm + ".static.flickr.com/" + photoServer + "/" + photoId + "_" + photoSecret + "_t.jpg";
                string photoImgURL = "http://farm" + photoFarm + ".static.flickr.com/" + photoServer + "/" + photoId + "_" + photoSecret + "_z.jpg";
                string photoHref = "http://www.flickr.com/photos/" + nsid + "/" + photoId;

                photoOwner = photoOwner.Replace("'","");

                print("photoId    ="+photoId    );
                print("photoOwner ="+photoOwner );
                print("nsid ="+nsid );
                //print("photoFarm  ="+photoFarm  );
                //print("photoServer="+photoServer);
                //print("photoSecret="+photoSecret);
                //print("photoTbURL ="+photoTbURL );
                print("photoImgURL="+photoImgURL);
                print("photoHref  ="+photoHref  );
                print("photoLicense=" + photoLicense);
            
                UptTextureInfoPhotoId(photoId,photoImgURL,photoOwner,photoLicense);
            }
        }
        else
        {
            print("www.error=" + www.error);
        }
    }

    //----------------------------------------------------
    //                        sql
    //----------------------------------------------------

    //---------------------------------
    // PhotoSearch
    //---------------------------------
    public void DelPhotoSearch(
         int largeCode
        , int middleCode
        , string texName
        , string searchName
        )
    {

        sql.Open(dbPathName);

        query = "delete from PhotoSearch ";
        query += " where largeCode  =  " + largeCode;
        query += " and   middleCode =  " + middleCode;
        query += " and   upper(texName) = '" + texName.ToUpper() + "'";
        query += " and   upper(searchName) = '" + searchName.ToUpper() + "'";

        //print(query);
        sql.ExecuteQuery(query);

        sql.Close();
    }

    public void InsPhotoSearch(
         int largeCode
        , int middleCode
        , string texName
        , string searchName
        , string photoId
        , int photoNum
        , string photoTitle
        , string photoTbURL
        , string photoImgURL
        , string photoHref
        )
    {
        var factory = new Factory(); _connection = factory.Create(dbPathName);

        query = "delete from PhotoSearch ";
        query += " where largeCode  =  " + largeCode;
        query += " and   middleCode =  " + middleCode;
        query += " and   texName    = '" + texName + "'";
        query += " and   photoId    = '" + photoId + "'";

        //print("query=" + query);
        _connection.Execute(query);

        string toDate = System.DateTime.Now.ToString("yyyyMMdd");
        string toTime = System.DateTime.Now.ToString("HHmmss");

        _connection.InsertAll(new[]{
            new PhotoSearch{
                 largeCode   = largeCode 
                ,middleCode  = middleCode
                ,texName     = texName    
                ,searchName  = searchName    
                ,photoId     = photoId    
                ,photoNum    = photoNum
                ,photoTitle  = photoTitle 
                ,photoTbURL  = photoTbURL 
                ,photoImgURL = photoImgURL
                ,photoHref   = photoHref  
                ,useYn       = "N"
                ,checkYn     = "N"
                ,flickrUseYn = "Y"
                ,createDate  = toDate
                ,createTime  = toTime 
                ,updateDate  = " "
                ,updateTime  = " " 
                ,userName    = userName
            }    
        });
        _connection.Close();
    }

    public DataTable SelPhotoSearch(int largeCode, int middleCode, string texName, string searchName, string useYn, int page, string photoId)
    {
        int page1=1,page2=50;
        if     (page==0){page1=  1;page2=250;}
        else if(page==1){page1=  1;page2= 50;} else if(page==2){page1= 51;page2=100;}
        else if(page==3){page1=101;page2=150;} else if(page==4){page1=151;page2=200;}
        else if(page==5){page1=201;page2=250;} else if(page==6){page1=251;page2=300;}
        else if(page==7){page1=301;page2=350;} else if(page==8){page1=351;page2=400;}
        else if(page==9){page1=401;page2=450;} else if(page==10){page1=451;page2=500;}

        sql.Open(dbPathName);
        query = "select * from PhotoSearch ";
        if (largeCode > 0) query += " where largeCode  =  " + largeCode;
        if (middleCode > 0) query += " and   middleCode =  " + middleCode;
        if (texName != "0") query += " and   texName    = '" + texName + "'";
        if (searchName != "0" && useYn != "Y") query += " and   searchName = '" + searchName + "'";
        if (useYn == "Y") query += " and   useYn    = '" + useYn + "'";

        if (photoId != " ") query += " and   photoId    = '" + photoId + "'";
        if (useYn != "Y") query += " and   photoNum between " + page1 + " and " + page2;
        query += " order by largeCode,middleCode,texName,cast(photoId as int) asc";

        print(query);
        _data = sql.ExecuteQuery(query);
        sql.Close();

        return _data;
    }

    public DataTable SelPhotoSearchSearchName(int largeCode, int middleCode, string texName)
    {
        sql.Open(dbPathName);
        query  = " select searchName                      \n";
        query += " from PhotoSearch                       \n";
        query += " where largeCode = " + largeCode + "    \n";
        query += " and middleCode = " + middleCode + "    \n";
        query += " and   texName    = '" + texName + "'   \n";
        query += " group by searchName                    \n";

        print(query);
        _data = sql.ExecuteQuery(query);
        sql.Close();

        return _data;
    }

    public string SelPhotoSearchTexName(int largeCode, int middleCode)
    {
        sql.Open(dbPathName);
        query = " select largeCode,middleCode,min(texName) texName                   \n";
        query += " from (select largeCode,middleCode,texName                          \n";
        query += "             ,count(case when useYn = 'Y' then 1 else null end) cnt \n";
        query += "       from PhotoSearch                                             \n";
        query += "       where largeCode = " + largeCode + "                          \n";
        query += "       and middleCode = " + middleCode + "                          \n";
        query += "       group by largeCode,middleCode,texName                        \n";
        query += "       having count(case when useYn = 'Y' then 1 else null end) < 4 \n";
        query += "      )                                                             \n";
        query += " group by largeCode,middleCode                                      \n";

        print(query);
        _data = sql.ExecuteQuery(query);
        string texName = "no data";
        if (_data.Rows.Count > 0)
        {
            dr = _data.Rows[0];
            texName = dr["texName"].ToString();
        }
        sql.Close();

        return texName;
    }

    public DataTable SelPhotoSearchCode()
    {
        sql.Open(dbPathName);
        query = " select substr(min(a.largeCode||a.middleCode||a.texName),1,1)   as largeCode  \n";
        query += "       ,substr(min(a.largeCode||a.middleCode||a.texName),2,1)   as middleCode \n";
        query += "       ,substr(min(a.largeCode||a.middleCode||a.texName),3,200) as texName    \n";
        query += " from PhotoSearch a                                                           \n";
        query += " group by a.largeCode,a.middleCode,a.texName                                  \n";
        query += " having count(case when a.useYn = 'Y' then 1 else null end) < 4               \n";
        print(query);
        _data = sql.ExecuteQuery(query);

        sql.Close();

        return _data;
    }

    public DataTable SelPhotoSearchPhotoId()
    {
        sql.Open(dbPathName);
        query = "select a.photoId photoId from (select PhotoId from PhotoSearch where useYn = 'Y') a";
        query += " left join PhotoGetInfo b on a.photoId = b.photoId";
        query += " where b.photoId is null";
        print(query);
        _data = sql.ExecuteQuery(query);

        sql.Close();

        return _data;
    }

    public DataTable SelTextureInfoPhotoId()
    {
        /*
            drop table TextureInfoPhotoId;
            create table TextureInfoPhotoId as
            select 
             replace(substr(subUrl01,instr(subUrl01,'/')+1,15),'/','') photoId
            ,*
            from
               (
                select 
                 substr(photoLink,length(photoLink)-12,20) subUrl01
                ,*
                from TextureInfo
                where photoLink like '%www.flickr.com%'
                and photoSource <> 'flickr'
                and texUrl not like '%farm%'
               )
            ;

            commit;
        */

        sql.Open(dbPathName);
        query = "select photoId from TextureInfoPhotoId where texUrl not like '%farm%' ";
        print(query);
        _data = sql.ExecuteQuery(query);

        sql.Close();

        return _data;
    }

    public void UptTextureInfoPhotoId(string photoId, string texUrl, string photoOwner,string photoLicense)
    {
        sql.Open(dbPathName);
        query = "update TextureInfoPhotoId set ";
        query += " texUrl='"+texUrl+"'";
        //query += ",photoOwner='"+photoOwner+"'";
        query += ",photoLicense='"+photoLicense+"'";
        query += " where photoId='"+photoId+"'";

        print(query);
        _data = sql.ExecuteQuery(query);

        sql.Close();;
    }






    public DataTable SelPhotoSearchUseY(int largeCode)
    {
        sql.Open(dbPathName);
        query = "select * from PhotoSearch where useYn = 'Y'";
        if (largeCode > 0) query += " and largeCode = " + largeCode + "    \n";
        
        print(query);
        _data = sql.ExecuteQuery(query);
        sql.Close();

        return _data;
    }



    public void UptPhotoSearch(int largeCode, int middleCode, string texName, string photoId, string useYn)
    {
        sql.Open(dbPathName);
        query = "update PhotoSearch set ";
        query += " useYn = '" + useYn + "'";
        query += " ,updateDate = strftime('%Y%m%d','now','localtime'),updateTime = strftime('%H%M%S','now','localtime') ";
        query += " ,userName = '" + userName + "'";
        query += " where largeCode  =  " + largeCode;
        query += " and   middleCode =  " + middleCode;
        query += " and   texName    = '" + texName + "'";
        query += " and   photoId    = '" + photoId + "'";
        print(query);
        sql.ExecuteQuery(query);
        sql.Close();
    }

    public void UptPhotoSearchFlickrUseYn(int largeCode, int middleCode, string texName)
    {
        sql.Open(dbPathName);
        query = "update PhotoSearch set ";
        query += " flickrUseYn = 'N'";
        query += " updateDate = strftime('%Y%m%d','now','localtime'),updateTime = strftime('%H%M%S','now','localtime') ";
        query += " userName = '" + userName + "'";
        query += " where largeCode  =  " + largeCode;
        query += " and   middleCode =  " + middleCode;
        query += " and   texName    = '" + texName + "'";
        print(query);
        sql.ExecuteQuery(query);
        sql.Close();
    }


    //---------------------------------
    // PhotoGetInfo
    //---------------------------------
    public void InsPhotoGetInfo(string photoId, string photoOwner, string photoLicense)
    {
        var factory = new Factory(); _connection = factory.Create(dbPathName);

        query = "delete from PhotoGetInfo ";
        query += " where photoId    = '" + photoId + "'";

        print("query=" + query);
        _connection.Execute(query);

        _connection.InsertAll(new[]{
            new PhotoGetInfo{
                 photoId     = photoId    
                ,photoOwner  = photoOwner 
                ,photoLicense = photoLicense 
                ,createDate  = " "
                ,createTime  = " " 
            }    
        });

        _connection.Close();
    }

    public DataTable SelPhotoGetInfo(string photoId)
    {
        sql.Open(dbPathName);
        query = "select * from PhotoGetInfo ";
        query += " where photoId = '" + photoId + "'";

        print(query);
        _data = sql.ExecuteQuery(query);
        sql.Close();

        return _data;
    }


    //---------------------------------
    // PhotoGetInfo
    //---------------------------------

    //---------------------------------
    // SelTextureMeaning
    //---------------------------------    
    public DataTable SelTextureMeaning(int largeCode,int middleCode)
    {
        sql.Open(dbPathName);
        query = "select largeCode,middleCode,replace(texName,'_',' ') texName        \n";
        query += " from TextureMeaning                                               \n";
        query += " where largeCode > 0 and language = 'Korean' and languageClass = 1 \n";
        if (largeCode > 0) query += " and   largeCode = " + largeCode + "                               \n";
        if (middleCode > 0) query += " and   middleCode =  " + middleCode;
        //query += " and texName >= 'POT'                                            \n";
        query += " group by largeCode,middleCode,texName                             \n";
        query += " order by largeCode,middleCode,texName                             \n";
        print(query);
        _data = sql.ExecuteQuery(query);
        sql.Close();

        return _data;
    }

    public DataTable SelTextureMeaningMiddle(int largeCode)
    {
        sql.Open(dbPathName);
        query = "select middleCode                                                   \n";
        query += " from TextureMeaning                                               \n";
        query += " where largeCode > 0 and language = 'Korean' and languageClass = 1 \n";
        query += " and   largeCode = " + largeCode + "                               \n";
        query += " group by middleCode                                               \n";
        query += " order by middleCode                                               \n";

        print(query);
        _data = sql.ExecuteQuery(query);
        sql.Close();

        return _data;
    }

    public DataTable SelTextureMeaningTexName(int largeCode, int middleCode)
    {
        sql.Open(dbPathName);
        query = "select replace(texName,'_',' ') texName                             \n";
        query += " from TextureMeaning                                               \n";
        query += " where largeCode > 0 and language = 'Korean' and languageClass = 1 \n";
        query += " and   largeCode = " + largeCode + "                               \n";
        query += " and   middleCode = " + middleCode + "                             \n";
        query += " group by texName                                                  \n";
        query += " order by texName                                                  \n";

        print(query);
        _data = sql.ExecuteQuery(query);
        sql.Close();

        return _data;
    }






    void SqliteDBConn()
    {

        //dbPathName = Application.dataPath + "/Resources/Sqlite/CrazyWordJapanV1.db";

        dbPathName = Application.persistentDataPath + "/CrazyWordJapanV1.db";


        // 바이나리안의 eng.db를 외부폴더에 복사 한다.
        if (!System.IO.File.Exists(dbPathName))
        {
            //Debug.Log("Exists ******  " + dbPathName);
            TextAsset ta = (TextAsset)Resources.Load("Sqlite/CrazyWordJapanV1.db");
            System.IO.File.WriteAllBytes(dbPathName, ta.bytes); // 64MB limit on File.WriteAllBytes.
            ta = null;
        }
        else
        {
            //Debug.Log("Exists ******  " + dbPathName);
        }

        sql = new SqliteDatabase();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }




}
