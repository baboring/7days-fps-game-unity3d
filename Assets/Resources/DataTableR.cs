using System.IO;
using System.Collections.Generic;
#if ( UNITY_EDITOR || UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9 )
using UnityEngine;
#endif
using CSV;



namespace SB {

	public class R {

		private static R _instance;
		public static R instance { get { if(null==_instance) Create(); return _instance;} }
		public R() {
			Assert = assert_;
		}

		public static bool Create(string path = "") {

			if (null != _instance)
				return false;
			_instance = new R();
			return _instance.LoadFromCSV(path);
		}

		private bool LoadFromCSV(string path) {

			if (path.Length > 0 && !(path[path.Length - 1] == '\\' || path[path.Length - 1] != '/'))
				path += '\\';
			CSV.csvTable table;
			Dictionary<string, CSV.csvTable> mapCSVTable;
			try {
				mapCSVTable = ReadCSV_FromAsset(path + "csv_Data/PropertyTable");
				Assert(null != mapCSVTable);
				if(null != mapCSVTable) { 
					if (mapCSVTable.TryGetValue("PropertyInfo", out table)) {
						var dic = this._data_PropertyInfo = new Dictionary<PropertyInfo.eID, R.PropertyInfo>();
						foreach (var rec in table.tRecords) {
							R.PropertyInfo newOne = new R.PropertyInfo(rec.tContents);
							dic.Add(newOne.id, newOne);
						}
					}
					if (mapCSVTable.TryGetValue("Speeches", out table)) {
						var dic = this._data_Speeches = new Dictionary<Speeches.eKey, R.Speeches>();
						foreach (var rec in table.tRecords) {
							R.Speeches newOne = new R.Speeches(rec.tContents);
							dic.Add(newOne.id, newOne);
						}
					}
					if (mapCSVTable.TryGetValue("Seq", out table)) {
						var dic = this._data_Seq = new Dictionary<Seq.eState, R.Seq>();
						foreach (var rec in table.tRecords) {
							R.Seq newOne = new R.Seq(rec.tContents);
							dic.Add(newOne.state, newOne);
						}
					}
				}
			}
			catch (System.Exception e) {
				throw e;
			}
			try {
				mapCSVTable = ReadCSV_FromAsset(path + "csv_Data/SecureInfo");
				Assert(null != mapCSVTable);
				if(null != mapCSVTable) { 
					if (mapCSVTable.TryGetValue("Secure", out table)) {
						var dic = this._data_Secure = new Dictionary<Secure.eKey, R.Secure>();
						foreach (var rec in table.tRecords) {
							R.Secure newOne = new R.Secure(rec.tContents);
							dic.Add(newOne.id, newOne);
						}
					}
					if (mapCSVTable.TryGetValue("illegalApp", out table)) {
						var dic = this._data_illegalApp = new Dictionary<string, R.illegalApp>();
						foreach (var rec in table.tRecords) {
							R.illegalApp newOne = new R.illegalApp(rec.tContents);
							dic.Add(newOne.Text, newOne);
						}
					}
				}
			}
			catch (System.Exception e) {
				throw e;
			}
			try {
				mapCSVTable = ReadCSV_FromAsset(path + "csv_Data/ConstantTable");
				Assert(null != mapCSVTable);
				if(null != mapCSVTable) { 
					if (mapCSVTable.TryGetValue("DefaultValue", out table)) {
						var dic = this._data_DefaultValue = new Dictionary<int, R.DefaultValue>();
						foreach (var rec in table.tRecords) {
							R.DefaultValue newOne = new R.DefaultValue(rec.tContents);
							dic.Add(newOne.IDX, newOne);
						}
					}
					if (mapCSVTable.TryGetValue("TestEnumTable", out table)) {
						var dic = this._data_TestEnumTable = new Dictionary<TestEnumTable.eID, R.TestEnumTable>();
						foreach (var rec in table.tRecords) {
							R.TestEnumTable newOne = new R.TestEnumTable(rec.tContents);
							dic.Add(newOne.id, newOne);
						}
					}
				}
			}
			catch (System.Exception e) {
				throw e;
			}
			return true;
		}
		public static Dictionary<string, CSV.csvTable> ReadCSV_FromAsset(string szFileName) {
				MemoryStream fs = null;
				try {
					TextAsset txtAsset = UnityEngine.Resources.Load(szFileName) as TextAsset;
					if(null != txtAsset) {
						fs = new MemoryStream(txtAsset.bytes);
						if (null != fs) {
							var mapCSVTable = CSV.Loader.ReadCSVFromStream(fs);
							fs.Close();							return mapCSVTable;
						}
					}
				}
				catch (System.Exception ex1) {
					if (null != fs)
						fs.Close();
					throw ex1;
				}
				return null;
		}

		Dictionary<string, string[]> tableMap = new Dictionary<string, string[]> {
			{ "csv_Data/PropertyTable.csv",new string[] {"PropertyInfo", "Speeches", "Seq", }},
			{ "csv_Data/SecureInfo.csv",new string[] {"Secure", "illegalApp", }},
			{ "csv_Data/ConstantTable.csv",new string[] {"DefaultValue", "TestEnumTable", }},
		};

		void  assert_( bool proposition, string szFormat = null, params object[] p ) {
			if (!proposition)
				throw new System.ApplicationException("internal error or data corrupt!");
		}
		public delegate void AssertFunc( bool proposition, string szFormat = null, params object[] p );
		private AssertFunc Assert		{ get; set; }
		// -----------------------------------------------
		public static PropertyInfo GetPropertyInfo(PropertyInfo.eID key) {
			if (instance._data_PropertyInfo.ContainsKey(key))
				return _instance._data_PropertyInfo[key];
			return null;
		}
		public static bool TryGetPropertyInfo(PropertyInfo.eID key,out PropertyInfo val) {
			return (_instance._data_PropertyInfo.TryGetValue(key, out val));
		}
		public static Speeches GetSpeeches(Speeches.eKey key) {
			if (instance._data_Speeches.ContainsKey(key))
				return _instance._data_Speeches[key];
			return null;
		}
		public static bool TryGetSpeeches(Speeches.eKey key,out Speeches val) {
			return (_instance._data_Speeches.TryGetValue(key, out val));
		}
		public static Seq GetSeq(Seq.eState key) {
			if (instance._data_Seq.ContainsKey(key))
				return _instance._data_Seq[key];
			return null;
		}
		public static bool TryGetSeq(Seq.eState key,out Seq val) {
			return (_instance._data_Seq.TryGetValue(key, out val));
		}

		/// PropertyTable - class declearation
		public class PropertyInfo {

			public readonly eID id;
			public readonly float sightRange;
			public readonly float attackRange;
			public readonly float wander_min_range;
			public readonly float wander_max_range;
			public readonly float searchRange;
			public readonly float chaseRange;
			public readonly float awayRange;
			public readonly float stepAngle;
			public readonly float eyeLevel;
			public readonly float angularSpeed;
			public readonly float walkSpeed;
			public readonly float runSpeed;
			public readonly float stoppingDist;
			public readonly float acceleration;
			public readonly float attack_power;
			public readonly float life;

			public enum eID : int {
				None,
				Bullet,
				Hunter,
				Bot_X,
				Player,
			}


			public PropertyInfo (List<CSV.varient> tContents) {
				this.id = tContents[0].ToEnum<PropertyInfo.eID>();
				this.sightRange = tContents[1];
				this.attackRange = tContents[2];
				this.wander_min_range = tContents[3];
				this.wander_max_range = tContents[4];
				this.searchRange = tContents[5];
				this.chaseRange = tContents[6];
				this.awayRange = tContents[7];
				this.stepAngle = tContents[8];
				this.eyeLevel = tContents[9];
				this.angularSpeed = tContents[10];
				this.walkSpeed = tContents[11];
				this.runSpeed = tContents[12];
				this.stoppingDist = tContents[13];
				this.acceleration = tContents[14];
				this.attack_power = tContents[15];
				this.life = tContents[16];
			}

		}

		public class Speeches {

			public readonly eKey id;
			public readonly string StrValue;
			public readonly string Search;
			public readonly string StrHelp;
			public readonly string StrMotion;
			public readonly string json;

			public enum eKey : int {
				Welcome,
				Greeting,
				AskHelp,
				AskHelp2,
				PainTrackerIntro,
				PainTrackerTouch,
				PainTrackerHowPain,
				AnythingElse,
				WhichPart,
				HowAreYou,
			}


			public Speeches (List<CSV.varient> tContents) {
				this.id = tContents[0].ToEnum<Speeches.eKey>();
				this.StrValue = tContents[1];
				this.Search = tContents[2];
				this.StrHelp = tContents[3];
				this.StrMotion = tContents[4];
				this.json = tContents[5];
			}

		}

		public class Seq {

			public readonly eState state;
			public readonly string speak;
			public readonly string lookat;
			public readonly string cases;
			public readonly string condition;
			public readonly string cond_set;
			public readonly string elseCase;
			public readonly Seq.eState next;

			public enum eState : int {
				Sleep,
				Greet,
				AskDoForYou,
				AskWhich,
				AskHowDensity,
				AskConfirm,
				Finish,
				AskCallup,
				CallupEmergency,
			}


			public Seq (List<CSV.varient> tContents) {
				this.state = tContents[0].ToEnum<Seq.eState>();
				this.speak = tContents[1];
				this.lookat = tContents[2];
				this.cases = tContents[3];
				this.condition = tContents[4];
				this.cond_set = tContents[5];
				this.elseCase = tContents[6];
				this.next = tContents[7].ToEnum<Seq.eState>();
			}

		}

		private Dictionary<PropertyInfo.eID, PropertyInfo> _data_PropertyInfo;
		public static Dictionary<PropertyInfo.eID, PropertyInfo> AllPropertyInfo {
			get { return _instance._data_PropertyInfo; }
		}
		private Dictionary<Speeches.eKey, Speeches> _data_Speeches;
		public static Dictionary<Speeches.eKey, Speeches> AllSpeeches {
			get { return _instance._data_Speeches; }
		}
		private Dictionary<Seq.eState, Seq> _data_Seq;
		public static Dictionary<Seq.eState, Seq> AllSeq {
			get { return _instance._data_Seq; }
		}



		// -----------------------------------------------
		public static Secure GetSecure(Secure.eKey key) {
			if (instance._data_Secure.ContainsKey(key))
				return _instance._data_Secure[key];
			return null;
		}
		public static bool TryGetSecure(Secure.eKey key,out Secure val) {
			return (_instance._data_Secure.TryGetValue(key, out val));
		}
		public static illegalApp GetillegalApp(string key) {
			if (instance._data_illegalApp.ContainsKey(key))
				return _instance._data_illegalApp[key];
			return null;
		}
		public static bool TryGetillegalApp(string key,out illegalApp val) {
			return (_instance._data_illegalApp.TryGetValue(key, out val));
		}

		/// SecureInfo - class declearation
		public class Secure {

			public readonly eKey id;
			public readonly string Text;

			public enum eKey : int {
				SignatureHash,
			}


			public Secure (List<CSV.varient> tContents) {
				this.id = tContents[0].ToEnum<Secure.eKey>();
				this.Text = tContents[1];
			}

		}

		public class illegalApp {

			public readonly string Text;
			public readonly int id;

			public illegalApp (List<CSV.varient> tContents) {
				this.Text = tContents[0];
				this.id = tContents[1];
			}

		}

		private Dictionary<Secure.eKey, Secure> _data_Secure;
		public static Dictionary<Secure.eKey, Secure> AllSecure {
			get { return _instance._data_Secure; }
		}
		private Dictionary<string, illegalApp> _data_illegalApp;
		public static Dictionary<string, illegalApp> AllillegalApp {
			get { return _instance._data_illegalApp; }
		}



		// -----------------------------------------------
		public static DefaultValue GetDefaultValue(int key) {
			if (instance._data_DefaultValue.ContainsKey(key))
				return _instance._data_DefaultValue[key];
			return null;
		}
		public static bool TryGetDefaultValue(int key,out DefaultValue val) {
			return (_instance._data_DefaultValue.TryGetValue(key, out val));
		}
		public static TestEnumTable GetTestEnumTable(TestEnumTable.eID key) {
			if (instance._data_TestEnumTable.ContainsKey(key))
				return _instance._data_TestEnumTable[key];
			return null;
		}
		public static bool TryGetTestEnumTable(TestEnumTable.eID key,out TestEnumTable val) {
			return (_instance._data_TestEnumTable.TryGetValue(key, out val));
		}

		/// ConstantTable - class declearation
		public class DefaultValue {

			public readonly int IDX;
			public readonly eKey id;
			public readonly int Value;
			public readonly string StrValue;

			public enum eKey : int {
				DEFAULT_HELPER_COOLTIME = 1,
				MAX_ITEM_GRADE = 2,
				MAX_TOTAL_GIFT = 3,
				MAX_MILEAGE_DIA = 4,
				MAX_BUY_BAGSLOT = 5,
				TIME_TIME_CHARGE_STAMINA = 6,
				TIME_SEND_GIFTPOINT = 7,
				PLAYER_DEFAULT_MAX_HP = 8,
				PLAYER_DEFAULT_REGEN_HP = 9,
				PLAYER_DEFAULT_MAX_MP = 10,
				PLAYER_DEFAULT_REGEN_MP = 11,
				PLAYER_DEFAULT_ATTACK = 12,
				PLAYER_DEFAULT_ATTACK_SPEED = 13,
				PLAYER_DEFAULT_DEFENCE = 14,
				PLAYER_DEFAULT_DEFENCE_ADAPT_RATE = 15,
				PLAYER_DEFAULT_MOVE_SPEED = 16,
				PLAYER_DEFAULT_NORMAL_LOOTING_RANGE = 17,
				PLAYER_DEFAULT_BEAD_LOOTING_RANGE = 18,
				SKIN_OPEN_RUNE_LEVEL = 19,
				SKIN_OPEN_SLOT2_LEVEL = 20,
				SKIN_OPEN_SLOT3_LEVEL = 21,
				HP_POTION_COOLTIME = 22,
				MP_POTION_COOLTIME = 23,
				PW_POTION_COOLTIME = 24,
				CO_OP_CLEAR_GIFT_POINT = 25,
				CO_OP_UNKNOWN_GIFT_POINT = 26,
				MAPTIME_SINGLE_BONUS_LIMIT = 27,
				MAPTIME_CO_OP_BONUS_LIMIT = 28,
				ARENA_BONUS_POINT_TIME = 29,
				ARENA_WINNER_GAME_POINT = 30,
				ARENA_LOSE_GAME_POINT = 31,
				ARENA_TIME_BONUS_POINT = 32,
				CRITICAL_DAMAGE_RATIO = 33,
				VARIABLE_DAMAGE_RANGE = 34,
				EVENT_MON_ARRIVE_MIN_TIME = 35,
				EVENT_MON_ARRIVE_MAX_TIME = 36,
				BARUNSON_SID = 37,
				BARUNSON_WEB_ROOT_DEV = 38,
				BARUNSON_WEB_ROOT_LIVE = 39,
				BARUNSON_WEB_VIEW = 40,
				BARUNSON_WEB_CHECK_BANNER = 41,
				BARUNSON_WEB_PUBLIC_CAFE = 42,
				BARUNSON_WEB_CENTER_ROOT_DEV = 43,
				BARUNSON_WEB_CENTER_ROOT_LIVE = 44,
				BARUNSON_WEB_CENTER_1_1 = 45,
				BARUNSON_WEB_AGREE_SERVICE = 46,
				BARUNSON_WEB_AGREE_PRIVATE = 47,
				ARENA_CONTINUOUS_BONUS_MAX = 48,
				DELAY_TIME_TO_CLEARVIEW = 49,
				DELAY_TIME_TO_GAMEOVERVIEW = 50,
				DELAY_TIME_TO_TUTO_CLEARVIEW = 51,
				KAKAO_INVITE_REWARD = 52,
				ARENA_PLAY_LIMIT_TIME = 53,
				GOLDMAP_WAVE_LIMIT_TIME = 54,
				MAX_MILEAGE_GATCHA = 55,
				MAX_UltimateCooltimeReduceRatio = 56,
				MAX_HelperCooltimeReduceRatio = 57,
				MAX_DamageReduceRatio = 58,
				MAX_AttackSpeedRatio = 59,
				MAX_MpReducedConsumptionRatio = 60,
				MAX_GoldDropAmountRatio = 61,
				MAX_GoldRush_HelperCnt = 62,
				GoldRush_AmountPerGold = 63,
				GoldRush_GoldDrop_MaxRange = 64,
				GoldRush_Helper_Min_AtkDist = 65,
				GoldRush_Helper_Min_FollowDist = 66,
				MAP_SPAWN_AREABOX_RATIO = 67,
				ANNOUNCEBOX_AUTO_DESTRUCT_TIME = 68,
				GAME_CAMERA_DIST_OPTION_ON = 69,
				GAME_CAMERA_DIST_OPTION_OFF = 70,
				ACCOUNT_BONUS_STAT_PER_LV = 71,
				CO_OP_REVIVE_REQUIRE_TIME = 72,
				BOSS_GUADIAN_SCORE = 73,
				BOSS_GUADIAN_EXP = 74,
				LIMIT_SKIN_MOVESTAT_LEVEL = 75,
				SKIN_MOVE_STAT_DIA = 76,
				SKIN_RESET_STAT_DIA = 77,
			}

			public class c  {
				public const int DEFAULT_HELPER_COOLTIME = 1;
				public const int MAX_ITEM_GRADE = 2;
				public const int MAX_TOTAL_GIFT = 3;
				public const int MAX_MILEAGE_DIA = 4;
				public const int MAX_BUY_BAGSLOT = 5;
				public const int TIME_TIME_CHARGE_STAMINA = 6;
				public const int TIME_SEND_GIFTPOINT = 7;
				public const int PLAYER_DEFAULT_MAX_HP = 8;
				public const int PLAYER_DEFAULT_REGEN_HP = 9;
				public const int PLAYER_DEFAULT_MAX_MP = 10;
				public const int PLAYER_DEFAULT_REGEN_MP = 11;
				public const int PLAYER_DEFAULT_ATTACK = 12;
				public const int PLAYER_DEFAULT_ATTACK_SPEED = 13;
				public const int PLAYER_DEFAULT_DEFENCE = 14;
				public const int PLAYER_DEFAULT_DEFENCE_ADAPT_RATE = 15;
				public const int PLAYER_DEFAULT_MOVE_SPEED = 16;
				public const int PLAYER_DEFAULT_NORMAL_LOOTING_RANGE = 17;
				public const int PLAYER_DEFAULT_BEAD_LOOTING_RANGE = 18;
				public const int SKIN_OPEN_RUNE_LEVEL = 19;
				public const int SKIN_OPEN_SLOT2_LEVEL = 20;
				public const int SKIN_OPEN_SLOT3_LEVEL = 21;
				public const int HP_POTION_COOLTIME = 22;
				public const int MP_POTION_COOLTIME = 23;
				public const int PW_POTION_COOLTIME = 24;
				public const int CO_OP_CLEAR_GIFT_POINT = 25;
				public const int CO_OP_UNKNOWN_GIFT_POINT = 26;
				public const int MAPTIME_SINGLE_BONUS_LIMIT = 27;
				public const int MAPTIME_CO_OP_BONUS_LIMIT = 28;
				public const int ARENA_BONUS_POINT_TIME = 29;
				public const int ARENA_WINNER_GAME_POINT = 30;
				public const int ARENA_LOSE_GAME_POINT = 31;
				public const int ARENA_TIME_BONUS_POINT = 32;
				public const int CRITICAL_DAMAGE_RATIO = 33;
				public const int VARIABLE_DAMAGE_RANGE = 34;
				public const int EVENT_MON_ARRIVE_MIN_TIME = 35;
				public const int EVENT_MON_ARRIVE_MAX_TIME = 36;
				public const int BARUNSON_SID = 37;
				public const int BARUNSON_WEB_ROOT_DEV = 38;
				public const int BARUNSON_WEB_ROOT_LIVE = 39;
				public const int BARUNSON_WEB_VIEW = 40;
				public const int BARUNSON_WEB_CHECK_BANNER = 41;
				public const int BARUNSON_WEB_PUBLIC_CAFE = 42;
				public const int BARUNSON_WEB_CENTER_ROOT_DEV = 43;
				public const int BARUNSON_WEB_CENTER_ROOT_LIVE = 44;
				public const int BARUNSON_WEB_CENTER_1_1 = 45;
				public const int BARUNSON_WEB_AGREE_SERVICE = 46;
				public const int BARUNSON_WEB_AGREE_PRIVATE = 47;
				public const int ARENA_CONTINUOUS_BONUS_MAX = 48;
				public const int DELAY_TIME_TO_CLEARVIEW = 49;
				public const int DELAY_TIME_TO_GAMEOVERVIEW = 50;
				public const int DELAY_TIME_TO_TUTO_CLEARVIEW = 51;
				public const int KAKAO_INVITE_REWARD = 52;
				public const int ARENA_PLAY_LIMIT_TIME = 53;
				public const int GOLDMAP_WAVE_LIMIT_TIME = 54;
				public const int MAX_MILEAGE_GATCHA = 55;
				public const int MAX_UltimateCooltimeReduceRatio = 56;
				public const int MAX_HelperCooltimeReduceRatio = 57;
				public const int MAX_DamageReduceRatio = 58;
				public const int MAX_AttackSpeedRatio = 59;
				public const int MAX_MpReducedConsumptionRatio = 60;
				public const int MAX_GoldDropAmountRatio = 61;
				public const int MAX_GoldRush_HelperCnt = 62;
				public const int GoldRush_AmountPerGold = 63;
				public const int GoldRush_GoldDrop_MaxRange = 64;
				public const int GoldRush_Helper_Min_AtkDist = 65;
				public const int GoldRush_Helper_Min_FollowDist = 66;
				public const int MAP_SPAWN_AREABOX_RATIO = 67;
				public const int ANNOUNCEBOX_AUTO_DESTRUCT_TIME = 68;
				public const int GAME_CAMERA_DIST_OPTION_ON = 69;
				public const int GAME_CAMERA_DIST_OPTION_OFF = 70;
				public const int ACCOUNT_BONUS_STAT_PER_LV = 71;
				public const int CO_OP_REVIVE_REQUIRE_TIME = 72;
				public const int BOSS_GUADIAN_SCORE = 73;
				public const int BOSS_GUADIAN_EXP = 74;
				public const int LIMIT_SKIN_MOVESTAT_LEVEL = 75;
				public const int SKIN_MOVE_STAT_DIA = 76;
				public const int SKIN_RESET_STAT_DIA = 77;
			}

			public DefaultValue (List<CSV.varient> tContents) {
				this.IDX = tContents[0];
				this.id = tContents[1].ToEnum<DefaultValue.eKey>();
				this.Value = tContents[2];
				this.StrValue = tContents[3];
			}

		}

		public class TestEnumTable {

			public readonly eID id;
			public readonly int Value;
			public readonly string StrValue;

			public enum eID : int {
				PLAYER_DEFAULT_ATTACK_SPEED,
				PLAYER_DEFAULT_DEFENCE,
				PLAYER_DEFAULT_DEFENCE_ADAPT_RATE,
				PLAYER_DEFAULT_MOVE_SPEED,
				PLAYER_DEFAULT_NORMAL_LOOTING_RANGE,
				PLAYER_DEFAULT_BEAD_LOOTING_RANGE,
				SKIN_OPEN_RUNE_LEVEL,
				SKIN_OPEN_SLOT2_LEVEL,
				SKIN_OPEN_SLOT3_LEVEL,
				HP_POTION_COOLTIME,
				MP_POTION_COOLTIME,
				PW_POTION_COOLTIME,
				CO_OP_CLEAR_GIFT_POINT,
				CO_OP_UNKNOWN_GIFT_POINT,
				MAPTIME_SINGLE_BONUS_LIMIT,
				MAPTIME_CO_OP_BONUS_LIMIT,
				ARENA_BONUS_POINT_TIME,
				ARENA_WINNER_GAME_POINT,
				ARENA_LOSE_GAME_POINT,
				ARENA_TIME_BONUS_POINT,
				CRITICAL_DAMAGE_RATIO,
			}


			public TestEnumTable (List<CSV.varient> tContents) {
				this.id = tContents[0].ToEnum<TestEnumTable.eID>();
				this.Value = tContents[1];
				this.StrValue = tContents[2];
			}

		}

		private Dictionary<int, DefaultValue> _data_DefaultValue;
		public static Dictionary<int, DefaultValue> AllDefaultValue {
			get { return _instance._data_DefaultValue; }
		}
		private Dictionary<TestEnumTable.eID, TestEnumTable> _data_TestEnumTable;
		public static Dictionary<TestEnumTable.eID, TestEnumTable> AllTestEnumTable {
			get { return _instance._data_TestEnumTable; }
		}



	}
}
// end of class

