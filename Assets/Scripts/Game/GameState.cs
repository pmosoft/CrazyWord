using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {

	//----------------------------------------------------------------------
	//                           GameControlZone
	//----------------------------------------------------------------------

	//---------------------------------------------------------
	// BackgroudPanel
	//---------------------------------------------------------
	private GameObject BackgroundPanel;
	public GameObject BlankSceneBg;
	public GameObject GameTipBg;

	//---------------------------------------------------------
	// BoardPanel
	//---------------------------------------------------------
	public GameObject BoardPanel;

	public GameObject ClickWordLabel;
	public GameObject WordCnt;
	public GameObject BackSpace;

	public GameObject ComboGame;
	public GameObject HardGame;
	public GameObject EasyGame;
	public GameObject LevelSkip;

	public GameObject HomeNavi;
	public GameObject MapNavi;

	//---------------------------------------------------------
	// TexturePanel
	//---------------------------------------------------------
	public GameObject TexturePanel;
	public GameObject PictureFrame;
	public GameObject GameTexture;

	public GameObject CCSprite1,CCSprite2,CCSprite3,CCSprite4;


	//---------------------------------------------------------
	// TimeSliderPanel
	//---------------------------------------------------------
	public GameObject TimeSliderPanel;
	public GameObject StartTimeSlider;
	public GameObject EndTimeSlider;

	//---------------------------------------------------------
	// GamePanel
	//-----------------	----------------------------------------
	public GameObject GamePanel;
	public GameObject GameBackgroud;
	public GameObject GameBackgroudCombo;
	public GameObject Grid;
	public GameObject ShowWord;

	public GameObject GameInterlude; 

	public GameShooting gameShooting;
	public GameRain gameRain;
	public GameMole gameMole;
	public GameBlank gameBlank;
	public GameHint gameHint;


	public GameBasic gameBasic;
	public GameComponent gameComponent;
	public GameControl gameControl;

	//---------------------------------------------------------
	// GameMolePanel
	//---------------------------------------------------------
	public GameObject MoleInCorrect;

	//---------------------------------------------------------
	// GameBlankPanel
	//---------------------------------------------------------
	public GameObject BlankInCorrect;

	//---------------------------------------------------------
	// GameBonusRulePanel
	//---------------------------------------------------------
	public GameObject GameBonusRulePanel;

	//---------------------------------------------------------
	// GameBonusPanel
	//---------------------------------------------------------
	public GameObject BonusInCorrect;
	public GameObject BonusGameEnd;

	//---------------------------------------------------------
	// BottomPanel
	//---------------------------------------------------------
	public GameObject BottomPanel;
	public GameObject ProgressBar;


	//---------------------------------------------------------
	// BannerPanel
	//---------------------------------------------------------

	//---------------------------------------------------------
	// AccountPanel
	//---------------------------------------------------------
	public GameObject AccountPanel;
	public GameObject AccountBestScoreUp;
	public GameObject AccountRankingUp;

	//---------------------------------------------------------
	// DownloadPanel
	//---------------------------------------------------------
	public GameObject DownloadPanel;


	public void StateGameLevel(string kind){
		Debug.Log("Gamekind="+kind);
		if     (kind=="memory")  {g.isGameMemory=true ;g.isGameShooting=false;g.isGameRain=false;g.isGameMole=true ;g.isGameBlank=false;g.isGameHint=false;g.isGameBonus=false;g.isGameTexture=false;g.isGameTurning=true;}
		else if(kind=="shooting"){g.isGameMemory=false;g.isGameShooting=true ;g.isGameRain=false;g.isGameMole=false;g.isGameBlank=false;g.isGameHint=false;g.isGameBonus=false;g.isGameTexture=false;g.isGameTurning=true;}
		else if(kind=="rain")    {g.isGameMemory=false;g.isGameShooting=false;g.isGameRain=true ;g.isGameMole=false;g.isGameBlank=false;g.isGameHint=false;g.isGameBonus=false;g.isGameTexture=false;g.isGameTurning=true;}
		else if(kind=="mole")    {g.isGameMemory=false;g.isGameShooting=false;g.isGameRain=false;g.isGameMole=true ;g.isGameBlank=false;g.isGameHint=false;g.isGameBonus=false;g.isGameTexture=false;g.isGameTurning=true;}
		else if(kind=="blank")   {g.isGameMemory=false;g.isGameShooting=false;g.isGameRain=false;g.isGameMole=false;g.isGameBlank=true ;g.isGameHint=false;g.isGameBonus=false;g.isGameTexture=false;g.isGameTurning=true;}
		else if(kind=="hint")    {g.isGameMemory=false;g.isGameShooting=false;g.isGameRain=false;g.isGameMole=false;g.isGameBlank=false;g.isGameHint=true ;g.isGameBonus=false;g.isGameTexture=false;g.isGameTurning=true;}
		else if(kind=="bonus")   {g.isGameMemory=false;g.isGameShooting=false;g.isGameRain=false;g.isGameMole=false;g.isGameBlank=false;g.isGameHint=false;g.isGameBonus=true ;g.isGameTexture=false;g.isGameTurning=true;}
	}

	public void StateDownLoadTexture(bool tf){

		DownloadPanel.SetActive(tf);

		BoardPanel.SetActive(!tf);
		TexturePanel.SetActive(!tf);
		TimeSliderPanel.SetActive(!tf);
		GamePanel.SetActive(!tf);
		BottomPanel.SetActive(!tf);
	}

	public void StateShowTexture(){

		WordCnt.SetActive(true);
		ClickWordLabel.SetActive(true);
		ComboGame.SetActive(true);
		BackSpace.SetActive(false);
		HardGame.SetActive(false);
		EasyGame.SetActive(false);
		LevelSkip.SetActive(false);

		TexturePanel.SetActive(true);
		PictureFrame.SetActive(true);
		GameTexture.SetActive(true);
		ShowWord.SetActive(false);

		TimeSliderPanel.SetActive(false);

		GameBackgroud.SetActive(false);
		GameBackgroudCombo.SetActive(false);
		Grid.SetActive(false);
		GameInterlude.SetActive(false);
		BonusGameEnd.SetActive(false);

		AccountPanel.SetActive(false);

        bool tf; if(g.curStage < 1) tf=false; else tf=true;
        CCSprite1.SetActive(tf);CCSprite2.SetActive(tf);CCSprite3.SetActive(tf);CCSprite4.SetActive(tf);
	}
	
	public void StateMemoryGamePanel(){

		StateLevelGame("memory");
		StateMemoryGameBoardPanel();
		StateGameCommon();


	}

	public void StateShootingGamePanel(){
		StateLevelGame("shoot");
		StateEasyGameBoardPanel();
		StateGameCommon();
	}
	public void StateRainGamePanel(){
		StateLevelGame("rain");
		StateEasyGameBoardPanel();
		StateGameCommon();
	}
	public void StateMoleGamePanel(){
		
		StateLevelGame("mole");
		StateEasyGameBoardPanel();
		StateGameCommon();
	}
	public void StateBlankGamePanel(){
		
		StateLevelGame("blank");
		StateEasyGameBoardPanel();
		StateGameCommon();
	}
	public void StateHintGamePanel(){
		StateLevelGame("hint");
		StateMemoryGameBoardPanel();
		StateGameCommon();
	}
	public void StateGameCommon(){
		TimeSliderPanel.SetActive(true);
		
		GameTexture.SetActive(true);
		PictureFrame.SetActive(true);
		ShowWord.SetActive(false);
		
		GamePanel.SetActive(true);
		GameBackgroud.SetActive(true);
		if(g.isCombo) GameBackgroudCombo.SetActive(true); else GameBackgroudCombo.SetActive(false);
		Grid.SetActive(true);

		GameInterlude.SetActive(false);

	}
	public void StateLevelGame(string kind){
		if(kind!="shoot") gameShooting.gameObject.SetActive(false);
		if(kind!="rain" ) gameRain.gameObject.SetActive(false);
		if(kind!="mole" ) gameMole.gameObject.SetActive(false);
		if(kind!="blank") gameBlank.gameObject.SetActive(false);
		//if(kind!="hint")  gameHint.gameObject.SetActive(false);
		//if(kind=="memory") null
	}
	public void StateEasyGameBoardPanel() {

		WordCnt.SetActive(true);
		LevelSkip.SetActive(true);

		BackSpace.SetActive(false);
		ComboGame.SetActive(false);
		HardGame.SetActive(false);
		EasyGame.SetActive(false);
	}

	public void StateMemoryGameBoardPanel() {

		WordCnt.SetActive(true);
		BackSpace.SetActive(true);

		ComboGame.SetActive(false);
		HardGame.SetActive(false);
		EasyGame.SetActive(false);
		LevelSkip.SetActive(false);
	}

	public void StateGameInterlude(){

		GameInterlude.SetActive(true);

		StateLevelGame("memory");
		TimeSliderPanel.SetActive(false);
		MoleInCorrect.SetActive(false);
		BonusGameEnd.SetActive(false);
	}

	public void StateBonusGame(){
		TimeSliderPanel.SetActive(false);
		GamePanel.SetActive(false);
		
		GameBackgroud.SetActive(true);
		PictureFrame.SetActive(false);
		GameTexture.SetActive(false);
		ShowWord.SetActive(false);
		
		GameBonusRulePanel.SetActive(true);
	}
	
	
	public void StateBonusGameStart(){
		
		GamePanel.SetActive(true);
		
		GameBackgroud.SetActive(true);
		GameBackgroudCombo.SetActive(false);
		
		TimeSliderPanel.SetActive(true);
		
		Grid.SetActive(true);
		PictureFrame.SetActive(false);
		GameTexture.SetActive(false);
		ShowWord.SetActive(false);
		
		GameInterlude.SetActive(false);
		GameBonusRulePanel.SetActive(false);
		
	}
	public void StateBonusGameEnded(){
		BonusGameEnd.SetActive(true);
		
		TimeSliderPanel.SetActive(false);
		GameBackgroud.SetActive(false);
		Grid.SetActive(false);
		BonusInCorrect.SetActive (false);
	}

	public void StatePopUpAccount(){

		AccountPanel.SetActive(true);
		AccountBestScoreUp.SetActive(false);
		AccountRankingUp.SetActive(false);

		ClickWordLabel.SetActive(false);

		TexturePanel.SetActive(false);
		TimeSliderPanel.SetActive(false);
		GamePanel.SetActive(false);

	}


	


}
