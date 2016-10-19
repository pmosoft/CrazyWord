using UnityEngine;
using System.Collections.Generic;

public class TTSControl : MonoBehaviour 
{
	//private bool _initializeError = false;


    private bool isInitialize;
	
	public enum Languages
	{
		English,
		Korean,
		Chinese,
		Japanese,
		German,
		French,
		Italian,
	}
	
	public Languages languages;
	
    //private static TTSControl _instance;
    //public static TTSControl instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //        {
    //            _instance = GameObject.FindObjectOfType<TTSControl>();
    //            DontDestroyOnLoad(_instance.gameObject);
    //        }
    //        return _instance;
    //    }
    //}


    private static TTSControl _instance;
    public static TTSControl instance
    {
        get
        {
            if (!_instance)
            {
                _instance = GameObject.FindObjectOfType<TTSControl>();
                if (!_instance)
                {
                    GameObject container = new GameObject();
                    container.name = "TTSControl";
                    _instance = container.AddComponent(typeof(TTSControl)) as TTSControl;
                }
            }

            return _instance;
        }
    }
	
    //void Awake()
    //{
    //    if (_instance == null)
    //    {
    //        _instance = this;
    //        DontDestroyOnLoad(this);
    //    }
    //    else
    //    {
    //        if (this != _instance)
    //            Destroy(this.gameObject);
    //    }
    //}

    public void Init()
    {
        Debug.Log("TTSControl Init");

        //if(!isInitialize)
        //{
            DontDestroyOnLoad(this);

            if (g.CrazyWordLanguage == "Japanese") languages = Languages.Japanese;
            if (g.CrazyWordLanguage == "Chinese") languages = Languages.Chinese;
            if (g.CrazyWordLanguage == "Korean") languages = Languages.Korean;
            if (g.CrazyWordLanguage == "English") languages = Languages.English;
            if (g.CrazyWordLanguage == "German") languages = Languages.German;
            TTSManager.Initialize(transform.name, "OnTTSInit");
        //}
    }
	
	
	void Start () {
        //if(g.CrazyWordLanguage == "Japanese") languages = Languages.Japanese;
        //if(g.CrazyWordLanguage == "Chinese") languages = Languages.Chinese;
        //if(g.CrazyWordLanguage == "Korean") languages = Languages.Korean;
        //if(g.CrazyWordLanguage == "English") languages = Languages.English;
        //if(g.CrazyWordLanguage == "German") languages = Languages.German;

        //TTSManager.Initialize(transform.name, "OnTTSInit");
	}
	
	
	
	void OnTTSInit(string message)
	{
		int response = int.Parse(message);

        if (isInitialize == false)
        {
            switch (response)
            {
                case TTSManager.SUCCESS:

                    isInitialize = true;

                    if (TTSManager.IsInitialized())
                    {
                        for (int i = 0; i < TTSManager.GetAvailableLanguages().Count; i++)
                        {
                            if (TTSManager.GetAvailableLanguages()[i].Name == languages.ToString())
                            {
                                TTSManager.SetLanguage(TTSManager.GetAvailableLanguages()[i]);
                                break;
                            }
                        }
                    }


                    break;
                case TTSManager.ERROR:
                    //_initializeError = true;


                    break;
            }
        }
	}
	
	public void OnDestroy()
	{
		TTSManager.Shutdown();
	}
}
