using System;
using System.IO;
using Steamworks.Ugc;
using Steamworks.Data;
using UnityEngine;
using System.Xml.Serialization;
using HarmonyLib;
using BepInEx;
using BepInEx.Harmony;
using CustomInvitation;

namespace Steamworks_Updater {
    [BepInPlugin("LoR.uGuardian.Steamworks_Updater", "Steamworks Updater", "1.0")]
    public class Steamworks_Updater : BaseUnityPlugin {
        void Awake() {
            Harmony harmony = new Harmony("LoR.uGuardian.Steamworks_Updater");
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(MainManager))]
    [HarmonyPatch("UploadItem")]
    class UploadPatch {
        static bool Prefix(MainManager __instance) {
            UploadItem uplo = new UploadItem();
            UnityEngine.Debug.Log("Custom UploadItem call");
            uplo.Run(__instance);
            return false;
        }
    }
    public class UploadItem {
        public async void Run(MainManager inst) {
            UnityEngine.Debug.Log("Custom UploadItem init");
            string curWorkingDir = MainManager.instance.GetCurrentWorkingPath()+"/UploadID.xml";
            ulong _updateID = 0;
            try
			{
				CustomInvitation.NormalInvitation invitationMetaData = DataManager.instance.GetInvitationMetaData();
				invitationMetaData.workshopInfo.title = inst._workshopItemSetter.GetTitle();
				invitationMetaData.workshopInfo.desc = inst._workshopItemSetter.GetDescription();
				invitationMetaData.workshopInfo.tag = "Invitation";
				string previewImgAbsPath = invitationMetaData.workshopInfo.previewImgAbsPath;
				string currentWorkingPath = inst._workshopItemSetter.GetCurrentWorkingPath();
				LogManager.instance.PrintLog("Uploading..");
				LogManager.instance.PopupLog(false, "Uploading..");
				PublishResult pub = await Editor.NewCommunityFile.WithTitle(invitationMetaData.workshopInfo.title).WithDescription(invitationMetaData.workshopInfo.desc).WithTag(invitationMetaData.workshopInfo.tag).WithContent(currentWorkingPath).WithPreviewFile(previewImgAbsPath).SubmitAsync(new ProgressClass());
				if (pub.Success)
				{
					LogManager.instance.PopupLog(true, "Upload succeed!");
					LogManager.instance.PrintLogDone("Upload success!");
                    _updateID = pub.FileId.Value;
                    UnityEngine.Debug.Log("Output: "+_updateID);
                    NormalInvitation invInfo = new NormalInvitation();
                    invInfo.workshopInfo.UpdateID = Convert.ToInt64(_updateID);
                    try
                    {
                        using (StreamWriter streamWriter = new StreamWriter(curWorkingDir))
                        {
                            new XmlSerializer(typeof(NormalInvitation)).Serialize(streamWriter, invInfo);
                            UnityEngine.Debug.Log("Save: " + curWorkingDir);
                        }
                    }
                    catch (Exception arg2)
                    {
                        UnityEngine.Debug.Log("[UploadID.xml] Serialize Error: " + arg2);
                    }
				}
				else
				{
					LogManager.instance.PopupErrorLog(true, "Upload failed.");
					LogManager.instance.PrintLogError("Upload failed.");
				}
			}
			catch (Exception)
			{
				LogManager.instance.PopupErrorLog(true, "Upload error");
			}
        }
    }

    [HarmonyPatch(typeof(Steamworks.Ugc.Editor))]
    [HarmonyPatch("SubmitAsync")]
    class AsyncPatch {
        static void Prefix(ref PublishedFileId ___fileId, ref bool ___creatingNew) {
            UnityEngine.Debug.Log("SubmitAsync Prefix");
            string curWorkingDir = MainManager.instance.GetCurrentWorkingPath()+"/UploadID.xml";
            ulong _updateID = 0;
            try {
                using (StreamReader streamReader = new StreamReader(curWorkingDir)) {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(NormalInvitation));
                    NormalInvitation invInfo = (xmlSerializer.Deserialize(streamReader) as NormalInvitation);
                    _updateID = Convert.ToUInt64(invInfo.workshopInfo.UpdateID);
                }
                UnityEngine.Debug.Log("Input: "+_updateID);
            }
            catch (Exception arg) {
				UnityEngine.Debug.Log("[Steamworks_Updater] Deserialize Error: " + arg);
                UnityEngine.Debug.Log("Input: "+_updateID);
			}
            if (_updateID != 0) {
                ___creatingNew = false;
                ___fileId = _updateID;
            }
        }
    }

    public class NormalInvitation {
        public NormalInvitation()
		{
			this.workshopInfo = new WorkshopInfo();
		}
		[XmlElement("Workshop")]
		public WorkshopInfo workshopInfo;
    }

    public class WorkshopInfo
	{
		[XmlElement("UpdateID")]
        public long UpdateID;
	}
}