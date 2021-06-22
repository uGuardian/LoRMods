using System;
using System.Collections.Generic;
using System.IO;
using HarmonyLib;
using UnityEngine;

namespace ArbiterFragment
{
    public class PassiveAbility_181005 : PassiveAbilityBase
    {
        public override string debugDesc
        {
            get
            {
                return "At emotion level " + fragmentEmoLevel + " changes Binah to buffed version";
            }
        }
        public override void OnWaveStart()
		{
            if (!this.owner.passiveDetail.HasPassive<PassiveAbility_10010>())
            {
                Debug.LogError("ArbiterFragment: Fragment only works on Binah!");
                this.owner.passiveDetail.DestroyPassive(this);
            }
            if (this.owner.emotionDetail.EmotionLevel < fragmentEmoLevel)
            {
                fragmentActivated = 0;
            }
            // Debug.LogError("ArbiterFragment: fragmentActivated = " + fragmentActivated);
            if (fragmentActivated == 2)
            {
                fragmentActivated = 1;
                fragmentStartBP = true;
            }
            if (fragmentEmoLevel < 1 || fragmentEmoLevel > 5)
            {
                Debug.LogError("ArbiterFragment: Invalid Emotion Threshold, assuming 3");
                fragmentEmoLevel = 3;
            }
            if (fragmentHP < 0)
            {
                Debug.LogError("ArbiterFragment: Invalid Bonus Health, assuming 15");
                fragmentHP = 15;
            }
            if (fragmentBP < 0)
            {
                Debug.LogError("ArbiterFragment: Invalid Bonus Stagger, assuming 15");
                fragmentBP = 15;
            }
            OnRoundEndTheLast();
		}
        public override void OnRoundEndTheLast()
        {
            if (this.owner.emotionDetail.EmotionLevel >= fragmentEmoLevel && fragmentActivated < 2)
            {
                // Debug.LogError("ArbiterFragment: Buffing Binah");
                PassiveAbilityBase oldPassive = this.owner.passiveDetail.PassiveList.Find((PassiveAbilityBase x) => x is PassiveAbility_10011);
                this.owner.passiveDetail.DestroyPassive(oldPassive);
                this.owner.passiveDetail.AddPassive(new PassiveAbility_180005());
                List<BattleDiceCardModel> hand = this.owner.allyCardDetail.GetHand();
                List<BattleDiceCardModel> deck = this.owner.allyCardDetail.GetDeck();
                deck.AddRange(this.owner.allyCardDetail.GetDiscarded());
                deck.AddRange(this.owner.allyCardDetail.GetUse());
                this.owner.allyCardDetail.ExhaustAllCards();
                foreach (BattleDiceCardModel card in hand)
                {
                    int id = card.GetID();
                    string name = card.GetName();
                    // Debug.LogError("ArbiterFragment: Exchanging (hand) " + name);
                    switch (id)
                    {
                        case 607201:
                            this.owner.allyCardDetail.AddNewCard(706201, false);
                            break;
                        case 607202:
                            this.owner.allyCardDetail.AddNewCard(706202, false);
                            break;
                        case 607203:
                            this.owner.allyCardDetail.AddNewCard(706203, false);
                            break;
                        case 607204:
                            this.owner.allyCardDetail.AddNewCard(706204, false);
                            break;
                        case 607205:
                            this.owner.allyCardDetail.AddNewCard(706205, false);
                            break;
                        default:
                            Debug.LogError("ArbiterFragment: Non-Binah card, adding " + name + " back to hand");
                            this.owner.allyCardDetail.AddNewCard(id, false);
                            break;
                    }
                }
                foreach (BattleDiceCardModel card in deck)
                {
                    int id = card.GetID();
                    string name = card.GetName();
                    // Debug.LogError("ArbiterFragment: Exchanging (deck) " + name);
                    switch (id)
                    {
                        case 607201:
                            this.owner.allyCardDetail.AddNewCardToDeck(706201, false);
                            break;
                        case 607202:
                            this.owner.allyCardDetail.AddNewCardToDeck(706202, false);
                            break;
                        case 607203:
                            this.owner.allyCardDetail.AddNewCardToDeck(706203, false);
                            break;
                        case 607204:
                            this.owner.allyCardDetail.AddNewCardToDeck(706204, false);
                            break;
                        case 607205:
                            this.owner.allyCardDetail.AddNewCardToDeck(706205, false);
                            break;
                        default:
                            Debug.LogError("ArbiterFragment: Non-Binah card, adding " + name + " back to deck");
                            this.owner.allyCardDetail.AddNewCard(id, false);
                            break;
                    }
                }
                Hide();
                if (fragmentActivated == 0 && fragmentActivationHP == true) {
                    this.owner.RecoverHP(fragmentHP);
                }
                if (fragmentActivationBP == true || fragmentStartBP == true)
                {
                    this.owner.breakDetail.RecoverBreak(fragmentBP);
                    fragmentStartBP = false;
                }
                fragmentActivated = 2;
            }
        }
        public override int GetMaxHpBonus()
        {
            if (fragmentActivated > 0)
            {
                return fragmentHP;
            } else {
                return 0;
            }
        }
        public override int GetMaxBpBonus()
        {
            if (fragmentActivated > 0)
            {
                return fragmentBP;
            } else {
                return 0;
            }
        }
        public static int? ArbiterFragmentConfig(string settingKey)
		{
			string configFile = (Application.dataPath + "/BaseMods/Arbiter_Fragment/Arbiter_Fragment.ini");
			string[] config = new string[12];
			if (!File.Exists(configFile))
			{
                Debug.LogError("ArbiterFragment: Regenerating ini file");
                config[0] = ("All settings are case sensitive, and are assumed to be default if incorrect or missing");

				config[2] = ("FragmentEmoLevel");
				config[3] = ("3");
                config[4] = ("FragmentBonusHP");
				config[5] = ("15");
                config[6] = ("FragmentBonusStagger");
				config[7] = ("15");
                config[8] = ("FragmentActivationHP");
                config[9] = ("1");
                config[10] = ("FragmentActivationStagger");
                config[11] = ("1");
				File.WriteAllLines(configFile, config);
			} else {
				config = File.ReadAllLines(configFile);
			}
			try {
                // Debug.LogError("FragmentConfigIndex: " + config[Array.IndexOf(config, settingKey)]);
                // Debug.LogError("FragmentConfigIndex: " + config[Array.IndexOf(config, settingKey)+1]);
				int settingResult = System.Convert.ToInt32(config[Array.IndexOf(config, settingKey)+1]);
				Debug.LogError("ArbiterFragment: " + settingKey + " = " + settingResult);
				return settingResult;
			} catch (Exception ex) {
				Debug.LogError(ex.Message + Environment.NewLine + ex.StackTrace);
				Debug.LogError("ArbiterFragment: Error occured in config check for variable " + settingKey + ", Assuming default");
				return null;
			}
		}
        private static bool? ArbiterFragmentConfigBool(string settingKey)
        {
            try {
                int? config = ArbiterFragmentConfig(settingKey);
                if (config == null) {
                    throw new ArgumentException("Config Returned Null");
                }
                bool settingResult = System.Convert.ToBoolean(config);
                Debug.LogError("ArbiterFragment: " + settingKey + "(bool) = " + settingResult);
                return System.Convert.ToBoolean(settingResult);
            } catch {
				Debug.LogError("ArbiterFragment: Error occured in boolean conversion for variable " + settingKey + "(bool), Assuming default");
				return null;
            }
        }

        private static int fragmentEmoLevel = ArbiterFragmentConfig("FragmentEmoLevel") ?? 3;
        private static int fragmentHP = ArbiterFragmentConfig("FragmentBonusHP") ?? 15;
        private static int fragmentBP = ArbiterFragmentConfig("FragmentBonusStagger") ?? 15;
        private static bool fragmentActivationHP = ArbiterFragmentConfigBool("FragmentActivationHP") ?? true;
        private static bool fragmentActivationBP = ArbiterFragmentConfigBool("FragmentActivationStagger") ?? true;
        private static byte fragmentActivated = 0;
        private bool fragmentStartBP = false;
    }
}