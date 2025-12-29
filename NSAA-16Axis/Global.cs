using MRLibrary;
using OpenCvSharp;
using Sentech.StApiDotNET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace NSAA_16Axis
{
    class GV
    {
        public enum User { Operator, Engineer, Administrator };
        public static User UserLevel = User.Operator;

        public enum Mode { Production, Educational };
        public static Mode ModeSelected = Mode.Educational;
        public enum Tab { Align, Learn, Setting, BackLearn };
        public static Tab TabOption = Tab.Align;
        public enum Status { Standby, Align, ChangeRecipe, DownAlign, DownWaferAlign, DownStandby };
        public static Status MachineStatus = Status.Standby;
        public static volatile bool _isAIServiceRestarting = false;
        // public static ThreeAMLightSource Light = new ThreeAMLightSource();
        // public static CPLightSource Light = new CPLightSource("Light");
        public static NCPLightSource Light = new NCPLightSource("Light");
        public static NCPLightSource RingLight = new NCPLightSource("RingLight");
        // public static CPLightSource RingLight = new CPLightSource("RingLight");

        //Zoomlens
        //public static NarvitarMotorLens LeftZoomLens = new NarvitarMotorLens();
        public static MvotemZoom LeftZoomLens = new MvotemZoom();
        // public static NarvitarMotorLens RightZoomLens = new NarvitarMotorLens();
        public static MvotemZoom RightZoomLens = new MvotemZoom();

        // public static NarvitarMotorLensFake RightZoomLens = new NarvitarMotorLensFake();
        public static ZoomLensInfo ZoomLensInfo = new ZoomLensInfo();

        public static MPlcCommunication Plc = new MPlcCommunication();

        public static List<AlignCondition> AlignContionsList = new List<AlignCondition>();
        public static AlignCondition NowAlignCondition = new AlignCondition();

        public static AlignCondition[] acar = new AlignCondition[101];
        //public static Recipe _recipe = new Recipe();
        public static Recipe _recipe;
        //public static RecipeTemplate _recipeT;

        public static int inacp = 0;

        public static int UpBackAlign = 0;
        public static bool TestAlign = false;

        public static Mat[] LeftLowMaskMat = new Mat[200];
        public static Mat[] LeftLowWaferMat = new Mat[200];
        public static Mat[] RightLowMaskMat = new Mat[200];
        public static Mat[] RightLowWaferMat = new Mat[200];

        public static Mat[] LeftHighMaskMat = new Mat[200];
        public static Mat[] LeftHighWaferMat = new Mat[200];
        public static Mat[] RightHighMaskMat = new Mat[200];
        public static Mat[] RightHighWaferMat = new Mat[200];

        public static GrayImage[] LeftLowMaskMask = new GrayImage[200];
        public static GrayImage[] LeftLowWaferMask = new GrayImage[200];
        public static GrayImage[] RightLowMaskMask = new GrayImage[200];
        public static GrayImage[] RightLowWaferMask = new GrayImage[200];

        public static GrayImage[] LeftHighMaskMask = new GrayImage[200];
        public static GrayImage[] LeftHighWaferMask = new GrayImage[200];
        public static GrayImage[] RightHighMaskMask = new GrayImage[200];
        public static GrayImage[] RightHighWaferMask = new GrayImage[200];

        public static int NowRecipeNumber = 0;
        public static int ModifyRecipeNumber = 0;
        public static bool AppEnding = false;
        public static bool OnDrawMatch = false;
        public static bool OnLearnPattern = false;
        public static bool OnAlign = false;
        public static bool bBacksideCCD = false;

        public static BaslerCamParm BaslerCamParm = new BaslerCamParm();
        public static AIClassList AIClassList = new AIClassList();
        //public static PylonNetToMat LeftUpCam = new PylonNetToMat();
        //public static PylonNetToMat RightUpCam = new PylonNetToMat();

        public static CStApiAutoInit CStapi;
        public static CStSystem Csystem;
        public static SentechNetToMat LeftUpCam;
        public static SentechNetToMat RightUpCam;
        public static SentechNetToMat LeftBackCam;
        public static SentechNetToMat RightBackCam;

        public static int[] NowLocation = new int[20];

        //public static PylonNetToMat RightBackCam = new PylonNetToMat();
        //public static PylonNetToMat LeftSelCam = new PylonNetToMat();
        //public static PylonNetToMat RightSelCam = new PylonNetToMat();

        public enum AlignSide { Upside, Backside };
        public enum AlignView { Upside, Backside };

        public static int skView = 0;                  // skview = 0 , 上 CCD , skview = 1 , 下 CCD 
        public static int iStartAlign = 0;
        public static AlignSide AlignSideMode = AlignSide.Upside;
        public static SKZoomAndPanWindow LeftUpWindowOnAlignPage;
        public static SKZoomAndPanWindow RightUpWindowOnAlignPage;
        public static SKZoomAndPanWindow LeftUpWindowOnLearnPage;
        public static SKZoomAndPanWindow RightUpWindowOnLearnPage;
        public static SKZoomAndPanWindow LeftUpWindowOnParameterSettingPage;
        public static SKZoomAndPanWindow RightUpWindowOnParameterSettingPage;

        public static SKZoomAndPanWindow LeftDownWindowOnAlignPage;
        public static SKZoomAndPanWindow RightDownWindowOnAlignPage;
        public static SKZoomAndPanWindow BackLearnLeft;
        public static SKZoomAndPanWindow BackLearnRight;

        public static OpenCV3MatchUMat matcherLLW = null;
        public static OpenCV3MatchUMat matcherLLM = null;
        public static OpenCV3MatchUMat matcherLHW = null;
        public static OpenCV3MatchUMat matcherLHM = null;
        public static OpenCV3MatchUMat matcherRLW = null;
        public static OpenCV3MatchUMat matcherRLM = null;
        public static OpenCV3MatchUMat matcherRHW = null;
        public static OpenCV3MatchUMat matcherRHM = null;

        // public static SKZoomAndPanWindow LeftDownWindowOnParameterSettingPage;
        // public static SKZoomAndPanWindow RightDownWindowOnParameterSettingPage;

        public static int EnableTbStatus = 1;
        public static TextBox TbStatus;

        public static Mat DialogMat;
        public static Mat DialogMatCenter;
        public static Mat LeftMaskMat, RightMaskMat;
        public static Mat LeftCheckMat = null;
        public static Mat RightCheckMat = null;
        public static Mat UpMat;
        public static Mat DownMat;
        public static PointF UpCenter;
        public static PointF DownCenter;

        public static AppSettingParm AppSettingParm = new AppSettingParm();

        public static PLCAlarmCode PlcAlarmState = new PLCAlarmCode();

        public static DisplayLanguage Dlang = new DisplayLanguage();

        public static RecipeInfo NowRecipe;

        public static int PLCTimeOut = 30000;

        public static FormMain HwndFormMain = null;
        public static PlcStatus PlcStatusForm = null;

        public static int NowMagnification = 1;

        public static int NowWaferMask = 1;         //  1: Mask 2:Wafer

        public static int TickCount = 0;
        public static int iHmiNum = 0;
        public static int iAutoRun = 0;

        public static Thread thPlcStatus = null;

        public static Thread drawAlignMatch = null;

        public static Thread scanPlcThread = null;

        public static CCDPad CCDPadForm = null;
        public static Thread thCCDPad = null;

        public static PlcTest PlcTestForm = null;
        public static Thread thPlcTest = null;

        public static int[] OffsetHeight = new int[400];
        public static double[] LxOffset = new double[400];
        public static double[] LyOffset = new double[400];
        public static double[] RxOffset = new double[400];
        public static double[] RyOffset = new double[400];

        public static List<PointF> OffsetList = new List<PointF>();

        public static double R;
        public static double ThetaX;
        public static double ThetaY1;
        public static double ThetaY2;

        //public static OpenCV3MatchUMat matcher = null;

        public static Dictionary<int, string> HmiCodeD = new Dictionary<int, string>();

        public static Dictionary<int, string> PlcAlarmCodeD = new Dictionary<int, string>();

        public static Dictionary<int, string> XCodeD = new Dictionary<int, string>();

        public static Dictionary<int, string> YCodeD = new Dictionary<int, string>();

        public static string serviceName = "waitress-serve";
    }

    class GM
    {

        public delegate void DelegateUse(string message);

        public static void WriteToStatusTextBox(string message)
        {
            if (GV.TbStatus.InvokeRequired)
            {
                DelegateUse u = new DelegateUse(WriteToStatusTextBox);
            }
            else
            {
                if (GV.EnableTbStatus == 1)
                {
                    string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    if (message == GV.Dlang.strClear)
                        GV.TbStatus.Text = string.Empty;
                    else
                        GV.TbStatus.Text += time + "  " + message + Environment.NewLine;

                    GV.TbStatus.SelectionStart = GV.TbStatus.Text.Length;
                    GV.TbStatus.ScrollToCaret();
                    GV.TbStatus.Refresh();
                }

                LogActivities.GenerateLog(message);
            }
        }

        public static void DebugMessage(string message)
        {
            LogActivities.DebugLog(message);
        }

        public static void WriteToStatusTextBox1(int dis, string message)
        {
            if (dis == 1)
            {
                WriteToStatusTextBox(message);
            }
            else
            {
                LogActivities.GenerateLog(message);
            }
        }
        // public static DelegateUse WriteToStatusTextBox = new DelegateUse(WriteToStatusTextBox);

        public static void Quit()
        {
            // MessageBox1 msg1 = new MessageBox1(GV.Dlang.btSureQuit, GV.Dlang.btnQuit, MessageBoxIcon.Question);
            // DialogResult m1 = msg1.ShowDialog();
            // if (m1 == DialogResult.OK)
            if (MessageBox.Show(GV.Dlang.btSureQuit, GV.Dlang.btnQuit, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //GV.LeftUpVideoOnAlignPage.Exit();
                //GV.RightUpVideoOnAlignPage.Exit();
                //GV.LeftUpVideoOnLearnPage.Exit();
                //GV.RightUpVideoOnLearnPage.Exit();
                //GV.LeftUpVideoOnParameterSettingPage.Exit();
                //GV.RightUpVideoOnParameterSettingPage.Exit();
                GV.AppEnding = true;
                // WriteAlignConditionsAllListToXml("AlignConditions.xml");
                WriteAlignConditionsAcarToXml("AlignConditions.xml");
                GV.Light.ChangeBrightness("left", 0);
                GV.Light.ChangeBrightness("right", 0);
                GV.Light.ChangeBrightness("leftback", 0);
                GV.Light.ChangeBrightness("rightback", 0);
                GV.RingLight.ChangeBrightness("left", 0);
                GV.RingLight.ChangeBrightness("right", 0);

                GV.thCCDPad?.Abort();

                GV.thPlcTest?.Abort();

                GV.thPlcStatus?.Abort();

                GV.LeftZoomLens.Close();
                GV.RightZoomLens.Close();
                GV.Plc.Close();
                GV.Light.Close();
                GV.RingLight.Close();

                GV.LeftUpCam.Freeze();
                GV.RightUpCam.Freeze();
                GV.LeftBackCam.Freeze();
                GV.RightBackCam.Freeze();

                GV.drawAlignMatch?.Abort();

                GV.scanPlcThread?.Abort();

                if (GV.LeftUpCam.IsOpen)
                {
                    GV.LeftUpCam.StopCamera();
                }
                if (GV.RightUpCam.IsOpen)
                {
                    GV.RightUpCam.StopCamera();
                }
                if (GV.LeftBackCam.IsOpen)
                {
                    GV.LeftBackCam.StopCamera();
                }
                if (GV.RightBackCam.IsOpen)
                {
                    GV.RightBackCam.StopCamera();
                }

                //for (int i = 0; i < 200; i++)
                //{
                //    if (GV.LeftLowMaskMat[i] == null)
                //    {
                //        CvInvoke.Imwrite(GetTemplateFileName("LLM", i), GV.LeftLowMaskMat[1]);
                //        CvInvoke.Imwrite(GetTemplateFileName("LHM", i), GV.LeftHighMaskMat[1]);
                //        CvInvoke.Imwrite(GetTemplateFileName("LLW", i), GV.LeftLowWaferMat[1]);
                //        CvInvoke.Imwrite(GetTemplateFileName("LHW", i), GV.LeftHighWaferMat[1]);
                //        CvInvoke.Imwrite(GetTemplateFileName("RLM", i), GV.RightLowMaskMat[1]);
                //        CvInvoke.Imwrite(GetTemplateFileName("RHM", i), GV.RightHighMaskMat[1]);
                //        CvInvoke.Imwrite(GetTemplateFileName("RLW", i), GV.RightLowWaferMat[1]);
                //        CvInvoke.Imwrite(GetTemplateFileName("RHW", i), GV.RightHighWaferMat[1]);
                //        GV.LeftLowMaskMask[1].Save(GetTemplateFileName("MLLM", i), System.Drawing.Imaging.ImageFormat.Bmp);
                //        GV.LeftHighMaskMask[1].Save(GetTemplateFileName("MLHM", i), System.Drawing.Imaging.ImageFormat.Bmp);
                //        GV.LeftLowWaferMask[1].Save(GetTemplateFileName("MLLW", i), System.Drawing.Imaging.ImageFormat.Bmp);
                //        GV.LeftHighWaferMask[1].Save(GetTemplateFileName("MLHW", i), System.Drawing.Imaging.ImageFormat.Bmp);
                //        GV.RightLowMaskMask[1].Save(GetTemplateFileName("MRLM", i), System.Drawing.Imaging.ImageFormat.Bmp);
                //        GV.RightHighMaskMask[1].Save(GetTemplateFileName("MRHM", i), System.Drawing.Imaging.ImageFormat.Bmp);
                //        GV.RightLowWaferMask[1].Save(GetTemplateFileName("MRLW", i), System.Drawing.Imaging.ImageFormat.Bmp);
                //        GV.RightHighWaferMask[1].Save(GetTemplateFileName("MRHW", i), System.Drawing.Imaging.ImageFormat.Bmp);
                //    }
                //}

                //Easy.Terminate();

                Thread.Sleep(2000);

                // Process[] Parray = Process.GetProcesses(); 

                foreach (var process in Process.GetProcessesByName("DacianCheck1"))
                {
                    process.Kill();
                }

                // Application.Exit();
                Environment.Exit(Environment.ExitCode);
            }
        }

        private static string GetTemplateFileName(string patternAndSide, int EditRecipe)
        {
            string fileName = "Templates//" + patternAndSide + EditRecipe.ToString() + ".bmp";
            return fileName;
        }

        public static AlignCondition GetAlignCondition(int recipeNumber)
        {
            AlignCondition alignCondition = new AlignCondition() { RecipeNumber = 0 };


            foreach (var item in GV.AlignContionsList)
            {
                if (item.RecipeNumber == recipeNumber)
                {
                    alignCondition = item;
                    break;
                }
            }

            if (alignCondition.RecipeNumber == 0)
            {
                alignCondition.RecipeNumber = recipeNumber;
                GV.AlignContionsList.Add(alignCondition);
            }

            return alignCondition;
        }

        public static List<AlignCondition> ReadAlignConditionsXmlToList(string fileName)
        {
            List<AlignCondition> list;
            List<AlignCondition> alignConditionsL = new List<AlignCondition>();
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<AlignCondition>));

                using (FileStream stream = File.OpenRead(fileName))
                {
                    list = (List<AlignCondition>)serializer.Deserialize(stream);
                }

                foreach (AlignCondition nCondi in list)
                {
                    if (nCondi == null) continue;
                    AlignCondition NewOne = alignConditionsL.Find(x => x.RecipeNumber == nCondi.RecipeNumber);
                    if (NewOne == null)
                    {
                        alignConditionsL.Add((AlignCondition)nCondi.Clone());
                    }
                    else
                    {
                    }
                }

                for (int i = 1; i < 101; i++)
                {
                    GV.acar[i] = new AlignCondition();
                    AlignCondition NewOne = alignConditionsL.Find(x => x.RecipeNumber == i);
                    if (NewOne == null)
                    {
                        GV.acar[i].RecipeNumber = i;
                    }
                    else
                    {
                        NewOne.CopyTo(i);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ReadAlignConditionsXmlToList", MessageBoxButtons.OK, MessageBoxIcon.Error);
                GM.WriteToStatusTextBox(ex.Message);
                list = new List<AlignCondition>();
            }
            return alignConditionsL;
        }

        public static void ReadOffsetListXmlToList(string fileName)
        {
            try
            {
                List<OffsetF> templist = new List<OffsetF>();

                XmlSerializer serializer = new XmlSerializer(typeof(List<OffsetF>));

                using (FileStream stream = File.OpenRead(fileName))
                {
                    templist = (List<OffsetF>)serializer.Deserialize(stream);
                }

                foreach (OffsetF nOffset in templist)
                {
                    if ((nOffset.index > 0) && (nOffset.index < 400))
                    {
                        GV.OffsetHeight[nOffset.index] = (int)nOffset.AbsHeight;
                        GV.LxOffset[nOffset.index] = nOffset.lOffsetX;
                        GV.LyOffset[nOffset.index] = nOffset.lOffsetY;
                        GV.RxOffset[nOffset.index] = nOffset.rOffsetX;
                        GV.RyOffset[nOffset.index] = nOffset.rOffsetY;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ReadOffsetXmlToList", MessageBoxButtons.OK, MessageBoxIcon.Error);
                GM.WriteToStatusTextBox(ex.Message);
            }
        }

        public static void WriteAlignConditionsAllListToXml(string fileName)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(List<AlignCondition>));

                using (var stream = File.Create(fileName))
                {
                    serializer.Serialize(stream, GV.AlignContionsList);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "WriteAlignConditionListToXml", MessageBoxButtons.OK, MessageBoxIcon.Error);
                GM.WriteToStatusTextBox(ex.Message);
            }
        }

        public static void WriteOffsetAllListToXml(string fileName)
        {
            int i;
            List<OffsetF> tempOffsetList = new List<OffsetF>();

            try
            {
                var serializer = new XmlSerializer(typeof(List<OffsetF>));

                for (i = 0; i < 400; i++)
                {
                    OffsetF tempOffsetF = new OffsetF(i, GV.OffsetHeight[i], GV.LxOffset[i], GV.LyOffset[i], GV.RxOffset[i], GV.RyOffset[i]);
                    tempOffsetList.Add(tempOffsetF);
                }

                using (var stream = File.Create(fileName))
                {
                    serializer.Serialize(stream, tempOffsetList);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "WriteOffsetListToXml", MessageBoxButtons.OK, MessageBoxIcon.Error);
                GM.WriteToStatusTextBox(ex.Message);
            }
        }

        public static void WriteAlignConditionsAcarToXml(string fileName)
        {
            try
            {
                List<AlignCondition> tempAlignConditionList = new List<AlignCondition>();

                for (int i = 1; i < 101; i++)
                {
                    tempAlignConditionList.Add(GV.acar[i]);
                }

                var serializer = new XmlSerializer(typeof(List<AlignCondition>));

                using (var stream = File.Create(fileName))
                {
                    serializer.Serialize(stream, tempAlignConditionList);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "WriteAlignConditionListToXml", MessageBoxButtons.OK, MessageBoxIcon.Error);
                GM.WriteToStatusTextBox(ex.Message);
            }
        }

        public static void WriteAlignConditionsListToXml(string fileName, AlignCondition alignCondition, int recipeNumber)
        {
            try
            {
                /*
                if (recipeNumber < 0 || recipeNumber > 100)
                    throw new MRException("Wrong recipe number");

                List<AlignCondition> tempAlignConditionList = GV.AlignContionsList.ToList();

                foreach (var item in GV.AlignContionsList)
                {
                    if (item.RecipeNumber == recipeNumber)
                        tempAlignConditionList.RemoveAt(tempAlignConditionList.IndexOf(item));
                    // item.LeftLight = new bool[12];
                }

                tempAlignConditionList.Add(alignCondition);
                GV.AlignContionsList = tempAlignConditionList;
                // GV.NowAlignCondition = alignCondition.Clone();

                var serializer = new XmlSerializer(typeof(List<AlignCondition>));
                
                using (var stream = File.Create(fileName))
                {
                    serializer.Serialize(stream, GV.AlignContionsList);
                }
                */
                WriteAlignConditionsAcarToXml("AlignConditions.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "WriteAlignConditionListToXml", MessageBoxButtons.OK, MessageBoxIcon.Error);
                GM.WriteToStatusTextBox(ex.Message);
            }
        }

        public static void WriteAlignConditionsListToXml1(string fileName, AlignCondition alignCondition, int recipeNumber)
        {
            try
            {
                /*
                if (recipeNumber < 0 || recipeNumber > 100)
                    throw new MRException("Wrong recipe number");

                List<AlignCondition> tempAlignConditionList = GV.AlignContionsList.ToList();

                foreach (var item in GV.AlignContionsList)
                {
                    if (item.RecipeNumber == recipeNumber)
                        tempAlignConditionList.RemoveAt(tempAlignConditionList.IndexOf(item));
                 }

                tempAlignConditionList.Add((AlignCondition)alignCondition.Clone());
                GV.AlignContionsList = tempAlignConditionList;

                var serializer = new XmlSerializer(typeof(List<AlignCondition>));
                using (var stream = File.Create(fileName))
                {
                    serializer.Serialize(stream, GV.AlignContionsList);
                }
                */
                WriteAlignConditionsAcarToXml("AlignConditions.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "WriteAlignConditionListToXml", MessageBoxButtons.OK, MessageBoxIcon.Error);
                GM.WriteToStatusTextBox(ex.Message);
            }
        }

        public static ZoomLensInfo ReadZoomLensInfoXml(string fileName)
        {
            ZoomLensInfo zoomLensInfo;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ZoomLensInfo));
                using (FileStream stream = File.OpenRead(fileName))
                {
                    zoomLensInfo = (ZoomLensInfo)serializer.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ReadZoomLensInfoXml", MessageBoxButtons.OK, MessageBoxIcon.Error);
                GM.WriteToStatusTextBox(ex.Message);
                zoomLensInfo = new ZoomLensInfo();
            }
            return zoomLensInfo;
        }

        public static void WriteZoomLensInfoToXml(string fileName, ZoomLensInfo zoomLensInfo)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(ZoomLensInfo));
                using (var stream = File.Create(fileName))
                {
                    serializer.Serialize(stream, zoomLensInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "WriteZoomLensInfoToXml", MessageBoxButtons.OK, MessageBoxIcon.Error);
                GM.WriteToStatusTextBox(ex.Message);
            }
        }

        public static void WriteBalserCamParmXml(string fileName, BaslerCamParm baslerCamParm)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(BaslerCamParm));
                using (var stream = File.Create(fileName))
                {
                    serializer.Serialize(stream, baslerCamParm);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "WriteBalserCamParmToXml", MessageBoxButtons.OK, MessageBoxIcon.Error);
                GM.WriteToStatusTextBox(ex.Message);
            }
        }

        public static BaslerCamParm ReadBalserCamParmXml(string fileName)
        {
            BaslerCamParm baslerCamParm = new BaslerCamParm();
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(BaslerCamParm));
                using (FileStream stream = File.OpenRead(fileName))
                {
                    baslerCamParm = (BaslerCamParm)serializer.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ReadBalserCamParmXml", MessageBoxButtons.OK, MessageBoxIcon.Error);
                GM.WriteToStatusTextBox(ex.Message);
                baslerCamParm = new BaslerCamParm();
            }
            return baslerCamParm;
        }

        public static void WriteAppSettingParmXml(string fileName, AppSettingParm appSettingParm)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(AppSettingParm));
                using (var stream = File.Create(fileName))
                {
                    serializer.Serialize(stream, appSettingParm);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "WriteAppSettingParmXml", MessageBoxButtons.OK, MessageBoxIcon.Error);
                GM.WriteToStatusTextBox(ex.Message);
            }
        }

        public static AppSettingParm ReadAppSettingParmXml(string fileName)
        {
            AppSettingParm appSettingParm = new AppSettingParm();
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AppSettingParm));
                using (FileStream stream = File.OpenRead(fileName))
                {
                    appSettingParm = (AppSettingParm)serializer.Deserialize(stream);
                }
                GV.bBacksideCCD = appSettingParm.BackSideCamEnable;

                GV.R = appSettingParm.XYYTableSize / 2;
                GV.ThetaX = appSettingParm.ThetaX * Math.PI / 180;
                GV.ThetaY1 = appSettingParm.ThetaY1 * Math.PI / 180;
                GV.ThetaY2 = appSettingParm.ThetaY2 * Math.PI / 180;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ReadAppSettingParmXml", MessageBoxButtons.OK, MessageBoxIcon.Error);
                GM.WriteToStatusTextBox(ex.Message);
                appSettingParm = new AppSettingParm();
            }
            return appSettingParm;
        }

        public static DisplayLanguage ReadLanguageFile(string fileName)
        {
            DisplayLanguage DLanguage = new DisplayLanguage();
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(DisplayLanguage));
                using (FileStream stream = File.OpenRead(fileName))
                {
                    DLanguage = (DisplayLanguage)serializer.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ReadLanguageFile", MessageBoxButtons.OK, MessageBoxIcon.Error);
                GM.WriteToStatusTextBox(ex.Message);
                DLanguage = new DisplayLanguage();
            }
            return DLanguage;
        }

        public static void ReadMatTemplates()
        {
            try
            {
                for (int i = 0; i < 101; i++)
                {
                    string fileName = "Templates//" + "LLM" + i.ToString() + ".bmp";
                    if (File.Exists(fileName))
                    {
                        try
                        {
                            GV.LeftLowMaskMat[i] = Cv2.ImRead(fileName, ImreadModes.Grayscale);
                        }
                        catch (Exception)
                        {

                            // GV.LeftLowMaskMat[i] = new Mat(new Size(256, 256), DepthType.Cv8U, 1);
                        }
                    }
                    else
                    {
                        // GV.LeftLowMaskMat[i] = new Mat(new Size(256, 256), DepthType.Cv8U, 1);
                    }
                }
                try
                {
                    string fileNameL = "Templates//" + "LCheck87" + ".bmp";
                    GV.LeftCheckMat = Cv2.ImRead(fileNameL, ImreadModes.Grayscale);
                    string fileNameR = "Templates//" + "RCheck87" + ".bmp";
                    GV.RightCheckMat = Cv2.ImRead(fileNameR, ImreadModes.Grayscale);
                }
                catch (Exception)
                {

                }
                for (int i = 0; i < 101; i++)
                {
                    string fileName = "Templates//" + "LLW" + i.ToString() + ".bmp";
                    if (File.Exists(fileName))
                    {
                        try
                        {
                            GV.LeftLowWaferMat[i] = Cv2.ImRead(fileName, ImreadModes.Grayscale);
                        }
                        catch (Exception)
                        {

                            // GV.LeftLowWaferMat[i] = new Mat(new Size(256, 256), DepthType.Cv8U, 1);
                        }
                    }
                    else
                    {
                        // GV.LeftLowWaferMat[i] = new Mat(new Size(256, 256), DepthType.Cv8U, 1);
                    }
                }
                for (int i = 0; i < 101; i++)
                {
                    string fileName = "Templates//" + "RLM" + i.ToString() + ".bmp";
                    if (File.Exists(fileName))
                    {
                        try
                        {
                            GV.RightLowMaskMat[i] = Cv2.ImRead(fileName, ImreadModes.Grayscale);
                        }
                        catch (Exception)
                        {
                            // GV.RightLowMaskMat[i] = new Mat(new Size(256, 256), DepthType.Cv8U, 1);
                        }
                    }
                    else
                    {
                        // GV.RightLowMaskMat[i] = new Mat(new Size(256, 256), DepthType.Cv8U, 1);
                    }
                }
                for (int i = 0; i < 101; i++)
                {
                    string fileName = "Templates//" + "RLW" + i.ToString() + ".bmp";
                    if (File.Exists(fileName))
                    {
                        try
                        {
                            GV.RightLowWaferMat[i] = Cv2.ImRead(fileName, ImreadModes.Grayscale);
                        }
                        catch (Exception)
                        {
                            // GV.RightLowWaferMat[i] = new Mat(new Size(256, 256), DepthType.Cv8U, 1);
                        }
                    }
                    else
                    {
                        // GV.RightLowWaferMat[i] = new Mat(new Size(256, 256), DepthType.Cv8U, 1);
                    }
                }
                for (int i = 0; i < 101; i++)
                {
                    string fileName = "Templates//" + "LHM" + i.ToString() + ".bmp";
                    if (File.Exists(fileName))
                    {
                        try
                        {
                            GV.LeftHighMaskMat[i] = Cv2.ImRead(fileName, ImreadModes.Grayscale);
                        }
                        catch (Exception)
                        {
                            // GV.LeftHighMaskMat[i] = new Mat(new Size(256, 256), DepthType.Cv8U, 1);
                        }
                    }
                    else
                    {
                        // GV.LeftHighMaskMat[i] = new Mat(new Size(256, 256), DepthType.Cv8U, 1);
                    }
                }
                for (int i = 0; i < 101; i++)
                {
                    string fileName = "Templates//" + "LHW" + i.ToString() + ".bmp";
                    if (File.Exists(fileName))
                    {
                        try
                        {
                            GV.LeftHighWaferMat[i] = Cv2.ImRead(fileName, ImreadModes.Grayscale);
                        }
                        catch (Exception)
                        {
                            // GV.LeftHighWaferMat[i] = new Mat(new Size(256, 256), DepthType.Cv8U, 1);
                        }
                    }
                    else
                    {
                        // GV.LeftHighWaferMat[i] = new Mat(new Size(256, 256), DepthType.Cv8U, 1);
                    }
                }
                for (int i = 0; i < 101; i++)
                {
                    string fileName = "Templates//" + "RHM" + i.ToString() + ".bmp";
                    if (File.Exists(fileName))
                    {
                        try
                        {
                            GV.RightHighMaskMat[i] = Cv2.ImRead(fileName, ImreadModes.Grayscale);
                        }
                        catch (Exception)
                        {
                            // GV.RightHighMaskMat[i] = new Mat(new Size(256, 256), DepthType.Cv8U, 1);
                        }
                    }
                    else
                    {
                        // GV.RightHighMaskMat[i] = new Mat(new Size(256, 256), DepthType.Cv8U, 1);
                    }
                }
                for (int i = 0; i < 101; i++)
                {
                    string fileName = "Templates//" + "RHW" + i.ToString() + ".bmp";
                    if (File.Exists(fileName))
                    {
                        try
                        {
                            GV.RightHighWaferMat[i] = Cv2.ImRead(fileName, ImreadModes.Grayscale);
                        }
                        catch (Exception)
                        {
                            // GV.RightHighWaferMat[i] = new Mat(new Size(256, 256), DepthType.Cv8U, 1);
                        }
                    }
                    else
                    {
                        // GV.RightHighWaferMat[i] = new Mat(new Size(256, 256), DepthType.Cv8U, 1);
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public static void ReadMaskTemplates()
        {
            try
            {
                for (int i = 0; i < 101; i++)
                {
                    string fileName = "Templates//" + "MLLM" + i.ToString() + ".bmp";
                    if (File.Exists(fileName))
                    {
                        try
                        {
                            GV.LeftLowMaskMask[i] = new GrayImage(fileName);
                        }
                        catch (Exception)
                        {
                            if (GV.LeftLowMaskMat[i] != null)
                            {
                                GV.LeftLowMaskMask[i] = new GrayImage(GV.LeftLowMaskMat[i].Width, GV.LeftLowMaskMat[i].Height);
                                GV.LeftLowMaskMask[i].Fill(255);
                            }
                        }
                    }
                    else
                    {
                        if (GV.LeftLowMaskMat[i] != null)
                        {
                            GV.LeftLowMaskMask[i] = new GrayImage(GV.LeftLowMaskMat[i].Width, GV.LeftLowMaskMat[i].Height);
                            GV.LeftLowMaskMask[i].Fill(255);
                        }
                    }
                }

                for (int i = 0; i < 101; i++)
                {
                    string fileName = "Templates//" + "MLLW" + i.ToString() + ".bmp";
                    if (File.Exists(fileName))
                    {
                        try
                        {
                            GV.LeftLowWaferMask[i] = new GrayImage(fileName);
                        }
                        catch (Exception)
                        {

                            if (GV.LeftLowWaferMat[i] != null)
                            {
                                GV.LeftLowWaferMask[i] = new GrayImage(GV.LeftLowWaferMat[i].Width, GV.LeftLowWaferMat[i].Height);
                                GV.LeftLowWaferMask[i].Fill(255);
                            }
                        }
                    }
                    else
                    {
                        if (GV.LeftLowWaferMat[i] != null)
                        {
                            GV.LeftLowWaferMask[i] = new GrayImage(GV.LeftLowWaferMat[i].Width, GV.LeftLowWaferMat[i].Height);
                            GV.LeftLowWaferMask[i].Fill(255);
                        }
                    }
                }
                for (int i = 0; i < 101; i++)
                {
                    string fileName = "Templates//" + "MRLM" + i.ToString() + ".bmp";
                    if (File.Exists(fileName))
                    {
                        try
                        {
                            GV.RightLowMaskMask[i] = new GrayImage(fileName);
                        }
                        catch (Exception)
                        {
                            if (GV.RightLowMaskMat[i] != null)
                            {
                                GV.RightLowMaskMask[i] = new GrayImage(GV.RightLowMaskMat[i].Width, GV.RightLowMaskMat[i].Height);
                                GV.RightLowMaskMask[i].Fill(255);
                            }
                        }
                    }
                    else
                    {
                        if (GV.RightLowMaskMat[i] != null)
                        {
                            GV.RightLowMaskMask[i] = new GrayImage(GV.RightLowMaskMat[i].Width, GV.RightLowMaskMat[i].Height);
                            GV.RightLowMaskMask[i].Fill(255);
                        }

                    }
                }
                for (int i = 0; i < 101; i++)
                {
                    string fileName = "Templates//" + "MRLW" + i.ToString() + ".bmp";
                    if (File.Exists(fileName))
                    {
                        try
                        {
                            GV.RightLowWaferMask[i] = new GrayImage(fileName);
                        }
                        catch (Exception)
                        {
                            if (GV.RightLowWaferMat[i] != null)
                            {
                                GV.RightLowWaferMask[i] = new GrayImage(GV.RightLowWaferMat[i].Width, GV.RightLowWaferMat[i].Height);
                                GV.RightLowWaferMask[i].Fill(255);
                            }
                        }
                    }
                    else
                    {
                        if (GV.RightLowWaferMat[i] != null)
                        {
                            GV.RightLowWaferMask[i] = new GrayImage(GV.RightLowWaferMat[i].Width, GV.RightLowWaferMat[i].Height);
                            GV.RightLowWaferMask[i].Fill(255);
                        }
                    }
                }
                for (int i = 0; i < 101; i++)
                {
                    string fileName = "Templates//" + "MLHM" + i.ToString() + ".bmp";
                    if (File.Exists(fileName))
                    {
                        try
                        {
                            GV.LeftHighMaskMask[i] = new GrayImage(fileName);
                        }
                        catch (Exception)
                        {
                            if (GV.LeftHighMaskMat[i] != null)
                            {
                                GV.LeftHighMaskMask[i] = new GrayImage(GV.LeftHighMaskMat[i].Width, GV.LeftHighMaskMat[i].Height);
                                GV.LeftHighMaskMask[i].Fill(255);
                            }
                        }
                    }
                    else
                    {
                        if (GV.LeftHighMaskMat[i] != null)
                        {
                            GV.LeftHighMaskMask[i] = new GrayImage(GV.LeftHighMaskMat[i].Width, GV.LeftHighMaskMat[i].Height);
                            GV.LeftHighMaskMask[i].Fill(255);
                        }
                    }
                }
                for (int i = 0; i < 101; i++)
                {
                    string fileName = "Templates//" + "MLHW" + i.ToString() + ".bmp";
                    if (File.Exists(fileName))
                    {
                        try
                        {
                            GV.LeftHighWaferMask[i] = new GrayImage(fileName);
                        }
                        catch (Exception)
                        {
                            if (GV.LeftHighWaferMat[i] != null)
                            {
                                GV.LeftHighWaferMask[i] = new GrayImage(GV.LeftHighWaferMat[i].Width, GV.LeftHighWaferMat[i].Height);
                                GV.LeftHighWaferMask[i].Fill(255);
                            }
                        }
                    }
                    else
                    {
                        if (GV.LeftHighWaferMat[i] != null)
                        {
                            GV.LeftHighWaferMask[i] = new GrayImage(GV.LeftHighWaferMat[i].Width, GV.LeftHighWaferMat[i].Height);
                            GV.LeftHighWaferMask[i].Fill(255);
                        }
                    }
                }

                for (int i = 0; i < 101; i++)
                {
                    string fileName = "Templates//" + "MRHM" + i.ToString() + ".bmp";
                    if (File.Exists(fileName))
                    {
                        try
                        {
                            GV.RightHighMaskMask[i] = new GrayImage(fileName);
                        }
                        catch (Exception)
                        {
                            if (GV.RightHighMaskMat[i] != null)
                            {
                                GV.RightHighMaskMask[i] = new GrayImage(GV.RightHighMaskMat[i].Width, GV.RightHighMaskMat[i].Height);
                                GV.RightHighMaskMask[i].Fill(255);
                            }
                        }
                    }
                    else
                    {
                        if (GV.RightHighMaskMat[i] != null)
                        {
                            GV.RightHighMaskMask[i] = new GrayImage(GV.RightHighMaskMat[i].Width, GV.RightHighMaskMat[i].Height);
                            GV.RightHighMaskMask[i].Fill(255);
                        }
                    }
                }

                for (int i = 0; i < 101; i++)
                {
                    string fileName = "Templates//" + "MRHW" + i.ToString() + ".bmp";
                    if (File.Exists(fileName))
                    {
                        try
                        {
                            GV.RightHighWaferMask[i] = new GrayImage(fileName);
                        }
                        catch (Exception)
                        {
                            if (GV.RightHighWaferMat[i] != null)
                            {
                                GV.RightHighWaferMask[i] = new GrayImage(GV.RightHighWaferMat[i].Width, GV.RightHighWaferMat[i].Height);
                                GV.RightHighWaferMask[i].Fill(255);
                            }
                        }
                    }
                    else
                    {
                        if (GV.RightHighWaferMat[i] != null)
                        {
                            GV.RightHighWaferMask[i] = new GrayImage(GV.RightHighWaferMat[i].Width, GV.RightHighWaferMat[i].Height);
                            GV.RightHighWaferMask[i].Fill(255);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public static void ReadRecpeTemplate(int iRec)
        {
            try
            {
                string fileName = "Templates//" + "LLM" + iRec.ToString() + ".bmp";
                if (File.Exists(fileName))
                {
                    try
                    {
                        GV.LeftLowMaskMat[iRec] = Cv2.ImRead(fileName, ImreadModes.Grayscale);
                    }
                    catch (Exception)
                    {
                        DebugMessage(fileName + " fail");
                        GV.LeftLowMaskMat[iRec] = new Mat(new OpenCvSharp.Size(256, 256), MatType.CV_8U);
                    }
                }
                else
                {
                    DebugMessage(fileName + " missing");
                    GV.LeftLowMaskMat[iRec] = new Mat(new OpenCvSharp.Size(256, 256), MatType.CV_8U);
                }

                fileName = "Templates//" + "LLW" + iRec.ToString() + ".bmp";
                if (File.Exists(fileName))
                {
                    try
                    {
                        GV.LeftLowWaferMat[iRec] = Cv2.ImRead(fileName, ImreadModes.Grayscale);
                    }
                    catch (Exception)
                    {
                        DebugMessage(fileName + " fail");
                        GV.LeftLowWaferMat[iRec] = new Mat(new OpenCvSharp.Size(256, 256), MatType.CV_8U);
                    }
                }
                else
                {
                    DebugMessage(fileName + " missing");
                    GV.LeftLowWaferMat[iRec] = new Mat(new OpenCvSharp.Size(256, 256), MatType.CV_8U);
                }

                fileName = "Templates//" + "RLM" + iRec.ToString() + ".bmp";
                if (File.Exists(fileName))
                {
                    try
                    {
                        GV.RightLowMaskMat[iRec] = Cv2.ImRead(fileName, ImreadModes.Grayscale);
                    }
                    catch (Exception)
                    {
                        DebugMessage(fileName + " fail");
                        GV.RightLowMaskMat[iRec] = new Mat(new OpenCvSharp.Size(256, 256), MatType.CV_8U);
                    }
                }
                else
                {
                    DebugMessage(fileName + " missing");
                    GV.RightLowMaskMat[iRec] = new Mat(new OpenCvSharp.Size(256, 256), MatType.CV_8U);
                }

                fileName = "Templates//" + "RLW" + iRec.ToString() + ".bmp";
                if (File.Exists(fileName))
                {
                    try
                    {
                        GV.RightLowWaferMat[iRec] = Cv2.ImRead(fileName, ImreadModes.Grayscale);
                    }
                    catch (Exception)
                    {
                        DebugMessage(fileName + " fail");
                        GV.RightLowWaferMat[iRec] = new Mat(new OpenCvSharp.Size(256, 256), MatType.CV_8U);
                    }
                }
                else
                {
                    DebugMessage(fileName + " missing");
                    GV.RightLowWaferMat[iRec] = new Mat(new OpenCvSharp.Size(256, 256), MatType.CV_8U);
                }

                fileName = "Templates//" + "LHM" + iRec.ToString() + ".bmp";
                if (File.Exists(fileName))
                {
                    try
                    {
                        GV.LeftHighMaskMat[iRec] = Cv2.ImRead(fileName, ImreadModes.Grayscale);
                    }
                    catch (Exception)
                    {
                        DebugMessage(fileName + " fail");
                        GV.LeftHighMaskMat[iRec] = new Mat(new OpenCvSharp.Size(256, 256), MatType.CV_8U);
                    }
                }
                else
                {
                    DebugMessage(fileName + " missing");
                    GV.LeftHighMaskMat[iRec] = new Mat(new OpenCvSharp.Size(256, 256), MatType.CV_8U);
                }

                fileName = "Templates//" + "LHW" + iRec.ToString() + ".bmp";
                if (File.Exists(fileName))
                {
                    try
                    {
                        GV.LeftHighWaferMat[iRec] = Cv2.ImRead(fileName, ImreadModes.Grayscale);
                    }
                    catch (Exception)
                    {
                        DebugMessage(fileName + " fail");
                        GV.LeftHighWaferMat[iRec] = new Mat(new OpenCvSharp.Size(256, 256), MatType.CV_8U);
                    }
                }
                else
                {
                    DebugMessage(fileName + " missing");
                    GV.LeftHighWaferMat[iRec] = new Mat(new OpenCvSharp.Size(256, 256), MatType.CV_8U);
                }

                fileName = "Templates//" + "RHM" + iRec.ToString() + ".bmp";
                if (File.Exists(fileName))
                {
                    try
                    {
                        GV.RightHighMaskMat[iRec] = Cv2.ImRead(fileName, ImreadModes.Grayscale);
                    }
                    catch (Exception)
                    {
                        DebugMessage(fileName + " fail");
                        GV.RightHighMaskMat[iRec] = new Mat(new OpenCvSharp.Size(256, 256), MatType.CV_8U);
                    }
                }
                else
                {
                    DebugMessage(fileName + " missing");
                    GV.RightHighMaskMat[iRec] = new Mat(new OpenCvSharp.Size(256, 256), MatType.CV_8U);
                }

                fileName = "Templates//" + "RHW" + iRec.ToString() + ".bmp";
                if (File.Exists(fileName))
                {
                    try
                    {
                        GV.RightHighWaferMat[iRec] = Cv2.ImRead(fileName, ImreadModes.Grayscale);
                    }
                    catch (Exception)
                    {
                        DebugMessage(fileName + " fail");
                        GV.RightHighWaferMat[iRec] = new Mat(new OpenCvSharp.Size(256, 256), MatType.CV_8U);
                    }
                }
                else
                {
                    DebugMessage(fileName + " missing");
                    GV.RightHighWaferMat[iRec] = new Mat(new OpenCvSharp.Size(256, 256), MatType.CV_8U);
                }

                fileName = "Templates//" + "MLLM" + iRec.ToString() + ".bmp";
                if (File.Exists(fileName))
                {
                    try
                    {
                        GV.LeftLowMaskMask[iRec] = new GrayImage(fileName);
                        if ((GV.LeftLowMaskMask[iRec].Width != GV.LeftLowMaskMat[iRec].Width) || (GV.LeftLowMaskMask[iRec].Height != GV.LeftLowMaskMat[iRec].Height))
                        {
                            GV.LeftLowMaskMask[iRec] = new GrayImage(GV.LeftLowMaskMat[iRec].Width, GV.LeftLowMaskMat[iRec].Height);
                            GV.LeftLowMaskMask[iRec].Fill(255);
                        }
                    }
                    catch (Exception)
                    {
                        if (GV.LeftLowMaskMat[iRec] != null)
                        {
                            DebugMessage(fileName + " fail");
                            GV.LeftLowMaskMask[iRec] = new GrayImage(GV.LeftLowMaskMat[iRec].Width, GV.LeftLowMaskMat[iRec].Height);
                            GV.LeftLowMaskMask[iRec].Fill(255);
                        }
                    }
                }
                else
                {
                    if (GV.LeftLowMaskMat[iRec] != null)
                    {
                        DebugMessage(fileName + " missing");
                        GV.LeftLowMaskMask[iRec] = new GrayImage(GV.LeftLowMaskMat[iRec].Width, GV.LeftLowMaskMat[iRec].Height);
                        GV.LeftLowMaskMask[iRec].Fill(255);
                        GV.LeftLowMaskMask[iRec].Save(fileName, System.Drawing.Imaging.ImageFormat.Bmp);
                    }
                }

                fileName = "Templates//" + "MLLW" + iRec.ToString() + ".bmp";
                if (File.Exists(fileName))
                {
                    try
                    {
                        GV.LeftLowWaferMask[iRec] = new GrayImage(fileName);
                        if ((GV.LeftLowWaferMask[iRec].Width != GV.LeftLowWaferMat[iRec].Width) || (GV.LeftLowWaferMask[iRec].Height != GV.LeftLowWaferMat[iRec].Height))
                        {
                            GV.LeftLowWaferMask[iRec] = new GrayImage(GV.LeftLowWaferMat[iRec].Width, GV.LeftLowWaferMat[iRec].Height);
                            GV.LeftLowWaferMask[iRec].Fill(255);
                        }
                    }
                    catch (Exception)
                    {
                        if (GV.LeftLowWaferMat[iRec] != null)
                        {
                            DebugMessage(fileName + " missing");
                            GV.LeftLowWaferMask[iRec] = new GrayImage(GV.LeftLowWaferMat[iRec].Width, GV.LeftLowWaferMat[iRec].Height);
                            GV.LeftLowWaferMask[iRec].Fill(255);
                        }
                    }
                }
                else
                {
                    if (GV.LeftLowWaferMat[iRec] != null)
                    {
                        GV.LeftLowWaferMask[iRec] = new GrayImage(GV.LeftLowWaferMat[iRec].Width, GV.LeftLowWaferMat[iRec].Height);
                        GV.LeftLowWaferMask[iRec].Fill(255);
                    }
                }

                fileName = "Templates//" + "MRLM" + iRec.ToString() + ".bmp";
                if (File.Exists(fileName))
                {
                    try
                    {
                        GV.RightLowMaskMask[iRec] = new GrayImage(fileName);
                        if ((GV.RightLowMaskMask[iRec].Width != GV.RightLowMaskMat[iRec].Width) || (GV.RightLowMaskMask[iRec].Height != GV.RightLowMaskMat[iRec].Height))
                        {
                            GV.RightLowMaskMask[iRec] = new GrayImage(GV.RightLowMaskMat[iRec].Width, GV.RightLowMaskMat[iRec].Height);
                            GV.RightLowMaskMask[iRec].Fill(255);
                        }

                    }
                    catch (Exception)
                    {
                        if (GV.RightLowMaskMat[iRec] != null)
                        {
                            GV.RightLowMaskMask[iRec] = new GrayImage(GV.RightLowMaskMat[iRec].Width, GV.RightLowMaskMat[iRec].Height);
                            GV.RightLowMaskMask[iRec].Fill(255);
                        }
                    }
                }
                else
                {
                    if (GV.RightLowMaskMat[iRec] != null)
                    {
                        DebugMessage(fileName + " missing");
                        GV.RightLowMaskMask[iRec] = new GrayImage(GV.RightLowMaskMat[iRec].Width, GV.RightLowMaskMat[iRec].Height);
                        GV.RightLowMaskMask[iRec].Fill(255);
                        GV.RightLowMaskMask[iRec].Save(fileName, System.Drawing.Imaging.ImageFormat.Bmp);
                    }

                }

                fileName = "Templates//" + "MRLW" + iRec.ToString() + ".bmp";
                if (File.Exists(fileName))
                {
                    try
                    {
                        GV.RightLowWaferMask[iRec] = new GrayImage(fileName);
                        if ((GV.RightLowWaferMask[iRec].Width != GV.RightLowWaferMat[iRec].Width) || (GV.RightLowWaferMask[iRec].Height != GV.RightLowWaferMat[iRec].Height))
                        {
                            GV.RightLowWaferMask[iRec] = new GrayImage(GV.RightLowWaferMat[iRec].Width, GV.RightLowWaferMat[iRec].Height);
                            GV.RightLowWaferMask[iRec].Fill(255);
                        }
                    }
                    catch (Exception)
                    {
                        if (GV.RightLowWaferMat[iRec] != null)
                        {
                            GV.RightLowWaferMask[iRec] = new GrayImage(GV.RightLowWaferMat[iRec].Width, GV.RightLowWaferMat[iRec].Height);
                            GV.RightLowWaferMask[iRec].Fill(255);
                        }
                    }
                }
                else
                {
                    if (GV.RightLowWaferMat[iRec] != null)
                    {
                        DebugMessage(fileName + " missing");
                        GV.RightLowWaferMask[iRec] = new GrayImage(GV.RightLowWaferMat[iRec].Width, GV.RightLowWaferMat[iRec].Height);
                        GV.RightLowWaferMask[iRec].Fill(255);
                        GV.RightLowWaferMask[iRec].Save(fileName, System.Drawing.Imaging.ImageFormat.Bmp);
                    }
                }

                fileName = "Templates//" + "MLHM" + iRec.ToString() + ".bmp";
                if (File.Exists(fileName))
                {
                    try
                    {
                        GV.LeftHighMaskMask[iRec] = new GrayImage(fileName);
                        if ((GV.LeftHighMaskMask[iRec].Width != GV.LeftHighMaskMat[iRec].Width) || (GV.LeftHighMaskMask[iRec].Height != GV.LeftHighMaskMat[iRec].Height))
                        {
                            GV.LeftHighMaskMask[iRec] = new GrayImage(GV.LeftHighMaskMat[iRec].Width, GV.LeftHighMaskMat[iRec].Height);
                            GV.LeftHighMaskMask[iRec].Fill(255);
                        }
                    }
                    catch (Exception)
                    {
                        if (GV.LeftHighMaskMat[iRec] != null)
                        {
                            GV.LeftHighMaskMask[iRec] = new GrayImage(GV.LeftHighMaskMat[iRec].Width, GV.LeftHighMaskMat[iRec].Height);
                            GV.LeftHighMaskMask[iRec].Fill(255);
                        }
                    }
                }
                else
                {
                    if (GV.LeftHighMaskMat[iRec] != null)
                    {
                        DebugMessage(fileName + " missing");
                        GV.LeftHighMaskMask[iRec] = new GrayImage(GV.LeftHighMaskMat[iRec].Width, GV.LeftHighMaskMat[iRec].Height);
                        GV.LeftHighMaskMask[iRec].Fill(255);
                        GV.LeftHighMaskMask[iRec].Save(fileName, System.Drawing.Imaging.ImageFormat.Bmp);
                    }
                }

                fileName = "Templates//" + "MLHW" + iRec.ToString() + ".bmp";
                if (File.Exists(fileName))
                {
                    try
                    {
                        GV.LeftHighWaferMask[iRec] = new GrayImage(fileName);
                        if ((GV.LeftHighWaferMask[iRec].Width != GV.LeftHighWaferMat[iRec].Width) || (GV.LeftHighWaferMask[iRec].Height != GV.LeftHighWaferMat[iRec].Height))
                        {
                            GV.LeftHighWaferMask[iRec] = new GrayImage(GV.LeftHighWaferMat[iRec].Width, GV.LeftHighWaferMat[iRec].Height);
                            GV.LeftHighWaferMask[iRec].Fill(255);
                        }
                    }
                    catch (Exception)
                    {
                        if (GV.LeftHighWaferMat[iRec] != null)
                        {
                            GV.LeftHighWaferMask[iRec] = new GrayImage(GV.LeftHighWaferMat[iRec].Width, GV.LeftHighWaferMat[iRec].Height);
                            GV.LeftHighWaferMask[iRec].Fill(255);
                        }
                    }
                }
                else
                {
                    if (GV.LeftHighWaferMat[iRec] != null)
                    {
                        DebugMessage(fileName + " missing");
                        GV.LeftHighWaferMask[iRec] = new GrayImage(GV.LeftHighWaferMat[iRec].Width, GV.LeftHighWaferMat[iRec].Height);
                        GV.LeftHighWaferMask[iRec].Fill(255);
                        GV.LeftHighWaferMask[iRec].Save(fileName, System.Drawing.Imaging.ImageFormat.Bmp);
                    }
                }

                fileName = "Templates//" + "MRHM" + iRec.ToString() + ".bmp";
                if (File.Exists(fileName))
                {
                    try
                    {
                        GV.RightHighMaskMask[iRec] = new GrayImage(fileName);
                        if ((GV.RightHighMaskMask[iRec].Width != GV.RightHighMaskMat[iRec].Width) || (GV.RightHighMaskMask[iRec].Height != GV.RightHighMaskMat[iRec].Height))
                        {
                            GV.RightHighMaskMask[iRec] = new GrayImage(GV.RightHighMaskMat[iRec].Width, GV.RightHighMaskMat[iRec].Height);
                            GV.RightHighMaskMask[iRec].Fill(255);
                        }
                    }
                    catch (Exception)
                    {
                        if (GV.RightHighMaskMat[iRec] != null)
                        {
                            GV.RightHighMaskMask[iRec] = new GrayImage(GV.RightHighMaskMat[iRec].Width, GV.RightHighMaskMat[iRec].Height);
                            GV.RightHighMaskMask[iRec].Fill(255);
                        }
                    }
                }
                else
                {
                    if (GV.RightHighMaskMat[iRec] != null)
                    {
                        DebugMessage(fileName + " missing");
                        GV.RightHighMaskMask[iRec] = new GrayImage(GV.RightHighMaskMat[iRec].Width, GV.RightHighMaskMat[iRec].Height);
                        GV.RightHighMaskMask[iRec].Fill(255);
                        GV.RightHighMaskMask[iRec].Save(fileName, System.Drawing.Imaging.ImageFormat.Bmp);
                    }
                }

                fileName = "Templates//" + "MRHW" + iRec.ToString() + ".bmp";
                if (File.Exists(fileName))
                {
                    try
                    {
                        GV.RightHighWaferMask[iRec] = new GrayImage(fileName);
                        if ((GV.RightHighWaferMask[iRec].Width != GV.RightHighWaferMat[iRec].Width) || (GV.RightHighWaferMask[iRec].Height != GV.RightHighWaferMat[iRec].Height))
                        {
                            GV.RightHighWaferMask[iRec] = new GrayImage(GV.RightHighWaferMat[iRec].Width, GV.RightHighWaferMat[iRec].Height);
                            GV.RightHighWaferMask[iRec].Fill(255);
                        }
                    }
                    catch (Exception)
                    {
                        if (GV.RightHighWaferMat[iRec] != null)
                        {
                            DebugMessage(fileName + " missing");
                            GV.RightHighWaferMask[iRec] = new GrayImage(GV.RightHighWaferMat[iRec].Width, GV.RightHighWaferMat[iRec].Height);
                            GV.RightHighWaferMask[iRec].Fill(255);
                        }
                    }
                }
                else
                {
                    if (GV.RightHighWaferMat[iRec] != null)
                    {
                        DebugMessage(fileName + " missing");
                        GV.RightHighWaferMask[iRec] = new GrayImage(GV.RightHighWaferMat[iRec].Width, GV.RightHighWaferMat[iRec].Height);
                        GV.RightHighWaferMask[iRec].Fill(255);
                        GV.RightHighWaferMask[iRec].Save(fileName, System.Drawing.Imaging.ImageFormat.Bmp);
                    }
                }
            }
            catch (Exception)
            {

            }

        }

        public static void WriteRecipeXml(int iRecipe)
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string sRecipePath = Path.Combine(currentDirectory, "Recipe");
            string fileName = Path.Combine(sRecipePath, "Recipe" + iRecipe.ToString() + ".xml");
            //GV.acar[iRecipe].CopyToObj(ref GV._recipe.AlignC);
            WriteRecipeXml(fileName, GV._recipe);
        }

        public static void WriteRecipeXml(string fileName, Recipe recipe)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(Recipe));
                using (var stream = File.Create(fileName))
                {
                    serializer.Serialize(stream, recipe);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"序列化失敗：{ex.Message}");
                Console.WriteLine($"內部錯誤：{ex.InnerException?.Message}");
                MessageBox.Show(ex.Message, "WriteRecipeXml", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static Recipe ReadRecipeXml(int iRecipe)
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string sRecipePath = Path.Combine(currentDirectory, "Recipe");
            string fileName = Path.Combine(sRecipePath, "Recipe" + iRecipe.ToString() + ".xml");
            Recipe recipe = ReadRecipeXml(fileName);
            //recipe?.AlignC.CopyTo(iRecipe);
            return recipe;
        }

        public static Recipe ReadRecipeXml(string fileName)
        {
            Recipe recipe = new Recipe();

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Recipe));
                using (FileStream stream = File.OpenRead(fileName))
                {
                    recipe = (Recipe)serializer.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                string destinationFilePath = fileName + ".bak";

                try
                {
                    // Rename the file
                    File.Move(fileName, destinationFilePath);
                    Console.WriteLine("File renamed successfully.");
                }
                catch (IOException)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

            return recipe;
        }

        public static void SaveImageBeforeExposure(Mat lResult, Mat rResult)
        {
            DateTime now = DateTime.Now;
            string time = now.ToString("yyyyMMdd");

            string folderName = "Result//" + time;
            if (!Directory.Exists(folderName))
                Directory.CreateDirectory(folderName);

            //var jpgFiles = Directory.EnumerateFiles(folderName, "*.jpg");
            string lfileName = folderName + "//L-" + now.ToString("HHmmss") + ".jpg";
            string rfileName = folderName + "//R-" + now.ToString("HHmmss") + ".jpg";

            Cv2.ImWrite(lfileName, lResult);
            Cv2.ImWrite(rfileName, rResult);
        }

        public static string SaveCaptureImage(Mat lImage, Mat rImage)
        {
            DateTime now = DateTime.Now;
            string time = now.ToString("yyyyMMdd");
            string folderName = "Capture//" + time;
            if (!Directory.Exists(folderName))
                Directory.CreateDirectory(folderName);

            string time1 = now.ToString("HHmmss");
            string lfileName = folderName + "//L-" + time1 + ".jpg";
            string rfileName = folderName + "//R-" + time1 + ".jpg";

            Cv2.ImWrite(lfileName, lImage);
            Cv2.ImWrite(rfileName, rImage);

            return "L-" + time1 + " & " + "R-" + time1;
        }

        public static void SaveRecipeTemplate(Recipe recipe, int iHighLow)
        {
            DateTime now = DateTime.Now;
            string time = now.ToString("yyyyMMdd");
            string folderName = "Capture//" + time;
            if (!Directory.Exists(folderName))
                Directory.CreateDirectory(folderName);

            string time1 = now.ToString("HHmmss");

            if (iHighLow == 1)
            {
                Cv2.ImWrite(folderName + "//LM-" + time1 + ".bmp", recipe.LeftLowMaskMat);
                Cv2.ImWrite(folderName + "//RM-" + time1 + ".bmp", recipe.RightLowMaskMat);
                Cv2.ImWrite(folderName + "//LW-" + time1 + ".bmp", recipe.LeftLowWaferMat);
                Cv2.ImWrite(folderName + "//RW-" + time1 + ".bmp", recipe.RightLowWaferMat);
                recipe.LeftLowMaskMask.Save(folderName + "//MLM-" + time1 + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                recipe.RightLowMaskMask.Save(folderName + "//MRM-" + time1 + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                recipe.LeftLowWaferMask.Save(folderName + "//MLW-" + time1 + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                recipe.RightLowWaferMask.Save(folderName + "//MRW-" + time1 + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            }
            else if (iHighLow == 0)
            {
                Cv2.ImWrite(folderName + "//LM-" + time1 + ".bmp", recipe.LeftHighMaskMat);
                Cv2.ImWrite(folderName + "//RM-" + time1 + ".bmp", recipe.RightHighMaskMat);
                Cv2.ImWrite(folderName + "//LW-" + time1 + ".bmp", recipe.LeftHighWaferMat);
                Cv2.ImWrite(folderName + "//RW-" + time1 + ".bmp", recipe.RightHighWaferMat);
                recipe.LeftHighMaskMask.Save(folderName + "//MLM-" + time1 + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                recipe.RightHighMaskMask.Save(folderName + "//MRM-" + time1 + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                recipe.LeftHighWaferMask.Save(folderName + "//MLW-" + time1 + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                recipe.RightHighWaferMask.Save(folderName + "//MRW-" + time1 + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            }
            else if (iHighLow == 2)
            {
                //Cv2.ImWrite(folderName + "//LM-" + time1 + ".bmp", recipe.LeftLowWaferMat);
                //Cv2.ImWrite(folderName + "//RM-" + time1 + ".bmp", recipe.RightLowWaferMat);
                Cv2.ImWrite(folderName + "//LW-" + time1 + ".bmp", recipe.LeftHighWaferMat);
                Cv2.ImWrite(folderName + "//RW-" + time1 + ".bmp", recipe.RightHighWaferMat);
                //recipe.LeftLowWaferMask.Save(folderName + "//MLM-" + time1 + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                //recipe.RightLowWaferMask.Save(folderName + "//MRM-" + time1 + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                recipe.LeftHighWaferMask.Save(folderName + "//MLW-" + time1 + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                recipe.RightHighWaferMask.Save(folderName + "//MRW-" + time1 + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            }
        }

        public static string SaveCaptureImageTxt(Mat lImage, Mat rImage, string strL, string strR)
        {
            DateTime now = DateTime.Now;
            string time = now.ToString("yyyyMMdd");
            string folderName = "Capture//" + time;
            if (!Directory.Exists(folderName))
                Directory.CreateDirectory(folderName);

            string time1 = now.ToString("HHmmss");
            string lfileName = folderName + "//L-" + time1 + ".jpg";
            string rfileName = folderName + "//R-" + time1 + ".jpg";

            Cv2.ImWrite(lfileName, lImage);
            Cv2.ImWrite(rfileName, rImage);

            return "L-" + time1 + " & " + "R-" + time1;
        }

        public static string SaveCaptureImageXyy(Mat lImage, Mat rImage)
        {
            DateTime now = DateTime.Now;
            string time = now.ToString("yyyyMMdd");
            string folderName = "Capture//" + time;
            if (!Directory.Exists(folderName))
                Directory.CreateDirectory(folderName);

            string time1 = now.ToString("HHmmss");
            string lfileName = folderName + "//LXyy-" + time1 + ".jpg";
            string rfileName = folderName + "//RXyy-" + time1 + ".jpg";

            Cv2.ImWrite(lfileName, lImage);
            Cv2.ImWrite(rfileName, rImage);

            return "LXyy-" + time1 + " & " + "RXyy-" + time1;
        }

        public static void ModelToCSV(string FilePath, AlignResultData data)
        {
            using (var file = new StreamWriter(FilePath, true))
            {
                //file.WriteLineAsync(data.ToString());
                file.WriteLine(data.ToString());
            }
        }

        internal static void ReadOffsetTxT(string v)
        {
            using (StreamReader sr = File.OpenText(v))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    string[] sr1 = s.Split(',');
                    int index = Int32.Parse(sr1[3]) + 1;
                    if ((index > 0) && (index < 400))
                    {
                        GV.LxOffset[index] = double.Parse(sr1[5]);
                        GV.LyOffset[index] = double.Parse(sr1[7]);
                        GV.RxOffset[index] = double.Parse(sr1[11]);
                        GV.RyOffset[index] = double.Parse(sr1[13]);
                    }
                }
            }
        }

        public static PLCAlarmCode ReadPLCAlarmCodeXml(string fileName, string Language)
        {
            int i;
            PLCAlarmCode plcAlarmCodeRU = new PLCAlarmCode();
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(PLCAlarmCode));
                using (FileStream stream = File.OpenRead(fileName))
                {
                    plcAlarmCodeRU = (PLCAlarmCode)serializer.Deserialize(stream);
                }

                for (i = 0; i < 160; i++)
                {
                    if (plcAlarmCodeRU.AlarmCode[i] == null)
                    {
                        plcAlarmCodeRU.AlarmCode[i] = new CAlarmState();
                    }
                    else
                    {
                        try
                        {
                            //if (Language == "RU")
                            //    GV.PlcAlarmCodeD_RU.Add(plcAlarmCodeRU.AlarmCode[i].iNumber, plcAlarmCodeRU.AlarmCode[i].iState);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }

                for (i = 0; i < 50; i++)
                {
                    if (plcAlarmCodeRU.HmiState[i] == null)
                    {
                        plcAlarmCodeRU.HmiState[i] = new CAlarmState();
                    }
                    else
                    {
                        try
                        {
                            //if (Language == "RU")
                            //    GV.HmiCodeD_RU.Add(plcAlarmCodeRU.HmiState[i].iNumber, plcAlarmCodeRU.HmiState[i].iState);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
                //Read XState 
                for (i = 0; i < 36; i++)
                {
                    if (plcAlarmCodeRU.XState[i] == null)
                    {
                        plcAlarmCodeRU.XState[i] = new CAlarmState();
                    }
                    else
                    {
                        try
                        {
                            //if (Language == "RU")
                            //    GV.XCodeD_RU.Add(plcAlarmCodeRU.XState[i].iNumber, plcAlarmCodeRU.XState[i].iState);
                        }
                        catch (Exception)
                        {

                        }
                    }

                }
                //Read YState 
                for (i = 0; i < 32; i++)
                {

                    if (plcAlarmCodeRU.YState[i] == null)
                    {
                        plcAlarmCodeRU.YState[i] = new CAlarmState();
                    }
                    else
                    {
                        try
                        {
                            //if (Language == "RU")
                            //    GV.YCodeD_RU.Add(plcAlarmCodeRU.YState[i].iNumber, plcAlarmCodeRU.YState[i].iState);
                        }
                        catch (Exception) { }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ReadPLCAlarmCodeXml", MessageBoxButtons.OK, MessageBoxIcon.Error);
                plcAlarmCodeRU = new PLCAlarmCode();
            }

            return plcAlarmCodeRU;
        }

        public static PLCAlarmCode ReadPLCAlarmCodeXml(string fileName)
        {
            int i;
            PLCAlarmCode plcAlarmCode = new PLCAlarmCode();
            //WritePLCAlarmCodeXmlA(fileName+"N", plcAlarmCode);
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(PLCAlarmCode));
                using (FileStream stream = File.OpenRead(fileName))
                {
                    plcAlarmCode = (PLCAlarmCode)serializer.Deserialize(stream);
                }

                if (plcAlarmCode.AlarmCode.Length == 0)
                {
                    plcAlarmCode = new PLCAlarmCode();
                    WritePLCAlarmCodeXml(fileName, plcAlarmCode);
                }

                GV.PlcAlarmCodeD.Clear();
                for (i = 0; i < 160; i++)
                {
                    if (plcAlarmCode.AlarmCode[i] == null)
                    {
                        plcAlarmCode.AlarmCode[i] = new CAlarmState();
                    }
                    else
                    {
                        try
                        {
                            GV.PlcAlarmCodeD.Add(plcAlarmCode.AlarmCode[i].iNumber, plcAlarmCode.AlarmCode[i].iState);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }

                GV.HmiCodeD.Clear();
                for (i = 0; i < 180; i++)
                {
                    if (plcAlarmCode.HmiState[i] == null)
                    {
                        plcAlarmCode.HmiState[i] = new CAlarmState();
                    }
                    else
                    {
                        try
                        {
                            GV.HmiCodeD.Add(plcAlarmCode.HmiState[i].iNumber, plcAlarmCode.HmiState[i].iState);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }

                //Read XState 
                GV.XCodeD.Clear();
                for (i = 0; i < 36; i++)
                {
                    if (plcAlarmCode.XState[i] == null)
                    {
                        plcAlarmCode.XState[i] = new CAlarmState();
                    }
                    else
                    {
                        try
                        {
                            GV.XCodeD.Add(plcAlarmCode.XState[i].iNumber, plcAlarmCode.XState[i].iState);
                        }
                        catch (Exception)
                        {

                        }
                    }

                }

                //Read YState 
                GV.YCodeD.Clear();
                for (i = 0; i < 32; i++)
                {

                    if (plcAlarmCode.YState[i] == null)
                    {
                        plcAlarmCode.YState[i] = new CAlarmState();
                    }
                    else
                    {
                        try
                        {
                            GV.YCodeD.Add(plcAlarmCode.YState[i].iNumber, plcAlarmCode.YState[i].iState);
                        }
                        catch (Exception) { }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ReadPLCAlarmCodeXml", MessageBoxButtons.OK, MessageBoxIcon.Error);
                plcAlarmCode = new PLCAlarmCode();
            }

            return plcAlarmCode;
        }

        public static void WritePLCAlarmCodeXmlA(string fileName, PLCAlarmCode plcAlarmCode)
        {
            int i;
            for (i = 0; i < 160; i++)
            {
                if (plcAlarmCode.AlarmCode[i] == null)
                {
                    plcAlarmCode.AlarmCode[i] = new CAlarmState();
                    plcAlarmCode.AlarmCode[i].iNumber = i;
                    plcAlarmCode.AlarmCode[i].iState = i.ToString();
                }
            }
            for (i = 0; i < 50; i++)
            {
                if (plcAlarmCode.HmiState[i] == null)
                {
                    plcAlarmCode.HmiState[i] = new CAlarmState();
                    plcAlarmCode.HmiState[i].iNumber = i;
                    plcAlarmCode.HmiState[i].iState = i.ToString();
                }
            }
            for (i = 0; i < 36; i++)
            {
                if (plcAlarmCode.XState[i] == null)
                {
                    plcAlarmCode.XState[i] = new CAlarmState();
                    plcAlarmCode.XState[i].iNumber = i;
                    plcAlarmCode.XState[i].iState = i.ToString();
                }
            }
            for (i = 0; i < 32; i++)
            {
                if (plcAlarmCode.YState[i] == null)
                {
                    plcAlarmCode.YState[i] = new CAlarmState();
                    plcAlarmCode.YState[i].iNumber = i;
                    plcAlarmCode.YState[i].iState = i.ToString();
                }
            }
            WritePLCAlarmCodeXml(fileName, plcAlarmCode);
        }

        public static void WritePLCAlarmCodeXml(string fileName, PLCAlarmCode PlcAlarmCode)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(PLCAlarmCode));
                using (var stream = File.Create(fileName))
                {
                    serializer.Serialize(stream, PlcAlarmCode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "WritePLCAlarmCodeXml", MessageBoxButtons.OK, MessageBoxIcon.Error);
                GM.WriteToStatusTextBox(ex.Message);
            }
        }

        //public static void LearnTemplate(int iNowReci)
        //{
        //    if (GV.skView == 1)
        //    {
        //        if (GV.NowMagnification == 1)
        //        {
        //            if ((GV.LeftLowMaskMat[iNowReci] != null) && (GV.LeftLowMaskMask[iNowReci] != null))
        //            {
        //                Learn(GV.m_MatcherLM, ImageConvert.MatToGrayImage(GV.LeftLowMaskMat[iNowReci]), GV.LeftLowMaskMask[iNowReci]);
        //            }
        //            if ((GV.LeftLowWaferMat[iNowReci] != null) && (GV.LeftLowWaferMask[iNowReci] != null))
        //            {
        //                Learn(GV.m_MatcherLW, ImageConvert.MatToGrayImage(GV.LeftLowWaferMat[iNowReci]), GV.LeftLowWaferMask[iNowReci]);
        //            }
        //            if ((GV.RightLowMaskMat[iNowReci] != null) && (GV.RightLowMaskMask[iNowReci] != null))
        //            {
        //                Learn(GV.m_MatcherRM, ImageConvert.MatToGrayImage(GV.RightLowMaskMat[iNowReci]), GV.RightLowMaskMask[iNowReci]);
        //            }
        //            if ((GV.RightLowWaferMat[iNowReci] != null) && (GV.RightLowWaferMask[iNowReci] != null))
        //            {
        //                Learn(GV.m_MatcherRW, ImageConvert.MatToGrayImage(GV.RightLowWaferMat[iNowReci]), GV.RightLowWaferMask[iNowReci]);
        //            }
        //        }
        //        else if (GV.NowMagnification == 2)
        //        {
        //            if ((GV.LeftHighMaskMat[iNowReci] != null) && (GV.LeftHighMaskMask[iNowReci] != null))
        //            {
        //                Learn(GV.m_MatcherLM, ImageConvert.MatToGrayImage(GV.LeftHighMaskMat[iNowReci]), GV.LeftHighMaskMask[iNowReci]);
        //            }
        //            if ((GV.LeftHighWaferMat[iNowReci] != null) && (GV.LeftHighWaferMask[iNowReci] != null))
        //            {
        //                Learn(GV.m_MatcherLW, ImageConvert.MatToGrayImage(GV.LeftHighWaferMat[iNowReci]), GV.LeftHighWaferMask[iNowReci]);
        //            }
        //            if ((GV.RightHighMaskMat[iNowReci] != null) && (GV.RightHighMaskMask[iNowReci] != null))
        //            {
        //                Learn(GV.m_MatcherRM, ImageConvert.MatToGrayImage(GV.RightHighMaskMat[iNowReci]), GV.RightHighMaskMask[iNowReci]);
        //            }
        //            if ((GV.RightHighWaferMat[iNowReci] != null) && (GV.RightHighWaferMask[iNowReci] != null))
        //            {
        //                Learn(GV.m_MatcherRW, ImageConvert.MatToGrayImage(GV.RightHighWaferMat[iNowReci]), GV.RightHighWaferMask[iNowReci]);
        //            }
        //        }

        //    }
        //    else if (GV.skView == 2)
        //    {
        //        if ((GV.LeftLowWaferMat[iNowReci] != null) && (GV.LeftLowWaferMask[iNowReci] != null))
        //        {
        //            Learn(GV.m_MatcherLM, ImageConvert.MatToGrayImage(GV.LeftLowWaferMat[iNowReci]), GV.LeftLowWaferMask[iNowReci]);
        //        }
        //        if ((GV.LeftHighWaferMat[iNowReci] != null) && (GV.LeftHighWaferMask[iNowReci] != null))
        //        {
        //            Learn(GV.m_MatcherLW, ImageConvert.MatToGrayImage(GV.LeftHighWaferMat[iNowReci]), GV.LeftHighWaferMask[iNowReci]);
        //        }
        //        if ((GV.RightLowWaferMat[iNowReci] != null) && (GV.RightLowWaferMask[iNowReci] != null))
        //        {
        //            Learn(GV.m_MatcherRM, ImageConvert.MatToGrayImage(GV.RightLowWaferMat[iNowReci]), GV.RightLowWaferMask[iNowReci]);
        //        }
        //        if ((GV.RightHighWaferMat[iNowReci] != null) && (GV.RightHighWaferMask[iNowReci] != null))
        //        {
        //            Learn(GV.m_MatcherRW, ImageConvert.MatToGrayImage(GV.RightHighWaferMat[iNowReci]), GV.RightHighWaferMask[iNowReci]);
        //        }
        //    }
        //}

        //public static void Learn(EMatcher eMatch, GrayImage pattern, GrayImage dontCare)
        //{
        //    lock (eMatch)
        //    {
        //        GrayImage grayImage = new GrayImage(pattern);
        //        if (dontCare != null)
        //        {
        //            grayImage.SetMask(dontCare);
        //        }
        //        EImageBW8 imageBW8 = ImageConvert.GrayImageToImageBW8(grayImage);
        //        eMatch.LearnPattern(imageBW8);
        //    }
        //}
    }

    public class AlignCondition
    {
        public DateTime LastModifyTime;
        public float MaskSocre = 0.6F;
        public float WaferScore = 0.6F;
        public double XOffset = 0F;
        public double YLOffset = 0F;
        public double YROffset = 0F;
        public int MaxAlignTimes = 10;
        public int MinAlignTimes = 0;
        public double XPrecision = 2F;
        public double YPrecision = 1F;
        public double ThetaPrecision = 1.0;
        public double MaxExpansion = 3F;
        public int RecipeNumber = 1;
        public int UpBackAlign = 1;     // 1 = 上對位， 2 下對位
        public int UpBotMask = 1;       // 下對位時，1 = 上光罩， 2 = 下光罩;
        public int AdjuestZ = 0;
        public int PatternShift = 4;    // 對位 Pattern 移動方向 0 - down , 1 - up , 2 - Left , 3 - Right
        public int LMaskAdjX = 0;
        public int LMaskAdjY = 0;
        public int RMaskAdjX = 0;
        public int RMaskAdjY = 0;

        public OpenCV3MatchUMat.AlignAlgorithm LLMaskAlgorithm = OpenCV3MatchUMat.AlignAlgorithm.TemplateMatch;
        public OpenCV3MatchUMat.AlignAlgorithm LLWaferAlgorithm = OpenCV3MatchUMat.AlignAlgorithm.TemplateMatch;
        public OpenCV3MatchUMat.AlignAlgorithm RLMaskAlgorithm = OpenCV3MatchUMat.AlignAlgorithm.TemplateMatch;
        public OpenCV3MatchUMat.AlignAlgorithm RLWaferAlgorithm = OpenCV3MatchUMat.AlignAlgorithm.TemplateMatch;

        public OpenCV3MatchUMat.AlignAlgorithm LHMaskAlgorithm = OpenCV3MatchUMat.AlignAlgorithm.TemplateMatch;
        public OpenCV3MatchUMat.AlignAlgorithm LHWaferAlgorithm = OpenCV3MatchUMat.AlignAlgorithm.TemplateMatch;
        public OpenCV3MatchUMat.AlignAlgorithm RHMaskAlgorithm = OpenCV3MatchUMat.AlignAlgorithm.TemplateMatch;
        public OpenCV3MatchUMat.AlignAlgorithm RHWaferAlgorithm = OpenCV3MatchUMat.AlignAlgorithm.TemplateMatch;


        public int LLWaferAIClassId { get; set; } = -1;
        public int LHWaferAIClassId { get; set; } = -1;

        public int RLWaferAIClassId { get; set; } = -1;
        public int RHWaferAIClassId { get; set; } = -1;
        public int LLMaskAIClassId { get; set; } = -1;
        public int LHMaskAIClassId { get; set; } = -1;
        public int RLMaskAIClassId { get; set; } = -1;
        public int RHMaskAIClassId { get; set; } = -1;


        public int LLWaferBlockSize = 27;
        public int RLWaferBlockSize = 27;
        public int LHWaferBlockSize = 27;
        public int RHWaferBlockSize = 27;

        public int LLBitwiseNot = 0;
        public int RLBitwiseNot = 0;
        public int LHBitwiseNot = 0;
        public int RHBitwiseNot = 0;

        public bool IsLLMaskPatternMaskUse = false;
        public bool IsLLWaferPatternMaskUse = false;
        public bool IsLHMaskPatternMaskUse = false;
        public bool IsLHWaferPatternMaskUse = false;

        public bool IsRLMaskPatternMaskUse = false;
        public bool IsRLWaferPatternMaskUse = false;
        public bool IsRHMaskPatternMaskUse = false;
        public bool IsRHWaferPatternMaskUse = false;

        public int AlignLowMagnification = 0;
        public int LowMagnificationLZ = 0;
        public int LowMagnificationRZ = 0;
        public int AlignHighMagnification = 0;
        public int HighMagnificationLZ = 0;
        public int HighMagnificationRZ = 0;

        public int PatternCenterDistanceUm = 75000;
        public int DPatternCenterDistanceUm = 150000;

        public bool[] LeftLight = { true, true, true, true, true, true, true, true, true, true, true, true };
        public bool[] RightLight = { true, true, true, true, true, true, true, true, true, true, true, true };
        public bool[] LRingLight = { false, false, false, false, false, false, false, false, false, false, false, false };
        public bool[] RRingLight = { false, false, false, false, false, false, false, false, false, false, false, false };
        public int[] LeftBrightness = { 10, 10, 10, 10, 10, 10, 11, 31, 26, 77, 110, 130 };
        public int[] RightBrightness = { 10, 10, 10, 10, 10, 10, 11, 31, 26, 77, 110, 130 };
        public int[] LeftRingBrightness = { 10, 10, 10, 10, 10, 10, 11, 31, 26, 77, 110, 130 };
        public int[] RightRingBrightness = { 10, 10, 10, 10, 10, 10, 11, 31, 26, 77, 110, 130 };

        public int LBMaskBright = 80;
        public int RBMaskBright = 80;
        public int LBWaferBright = 80;
        public int RBWaferBright = 80;

        public bool bLShowMask = false;
        public bool bRShowMask = false;

        public double dLalpha = 1.0;
        public double dRalpha = 1.0;

        public void CopyToObj(ref AlignCondition align)
        {
            align.LastModifyTime = this.LastModifyTime;
            align.MaskSocre = this.MaskSocre;
            align.WaferScore = this.WaferScore;
            align.XOffset = this.XOffset;
            align.YLOffset = this.YLOffset;
            align.YROffset = this.YROffset;
            align.MaxAlignTimes = this.MaxAlignTimes;
            align.MinAlignTimes = this.MinAlignTimes;
            align.XPrecision = this.XPrecision;
            align.YPrecision = this.YPrecision;
            align.ThetaPrecision = this.ThetaPrecision;
            align.MaxExpansion = this.MaxExpansion;
            align.RecipeNumber = this.RecipeNumber;
            align.UpBackAlign = this.UpBackAlign;       // 1 = 上對位， 2 下對位
            align.UpBotMask = this.UpBotMask;
            align.AdjuestZ = this.AdjuestZ;
            align.PatternShift = this.PatternShift;      // 對位 Pattern 移動方向 0 - down , 1 - up , 2 - Left , 3 - Right
            align.LMaskAdjX = this.LMaskAdjX;
            align.LMaskAdjY = this.LMaskAdjY;
            align.RMaskAdjX = this.RMaskAdjX;
            align.RMaskAdjY = this.RMaskAdjY;

            align.LLMaskAlgorithm = this.LLMaskAlgorithm;
            align.LLWaferAlgorithm = this.LLWaferAlgorithm;
            align.RLMaskAlgorithm = this.RLMaskAlgorithm;
            align.RLWaferAlgorithm = this.RLWaferAlgorithm;

            align.LHMaskAlgorithm = this.LHMaskAlgorithm;
            align.LHWaferAlgorithm = this.LHWaferAlgorithm;
            align.RHMaskAlgorithm = this.RHMaskAlgorithm;
            align.RHWaferAlgorithm = this.RHWaferAlgorithm;

            align.IsLLMaskPatternMaskUse = this.IsLLMaskPatternMaskUse;
            align.IsLLWaferPatternMaskUse = this.IsLLWaferPatternMaskUse;
            align.IsLHMaskPatternMaskUse = this.IsLHMaskPatternMaskUse;
            align.IsLHWaferPatternMaskUse = this.IsLHMaskPatternMaskUse;

            align.IsRLMaskPatternMaskUse = this.IsRLMaskPatternMaskUse;
            align.IsRLWaferPatternMaskUse = this.IsRLWaferPatternMaskUse;
            align.IsRHMaskPatternMaskUse = this.IsRHMaskPatternMaskUse;
            align.IsRHWaferPatternMaskUse = this.IsRHWaferPatternMaskUse;

            align.AlignLowMagnification = this.AlignLowMagnification;
            align.LowMagnificationLZ = this.LowMagnificationLZ;
            align.LowMagnificationRZ = this.LowMagnificationRZ;
            align.AlignHighMagnification = this.AlignHighMagnification;
            align.HighMagnificationLZ = this.HighMagnificationLZ;
            align.HighMagnificationRZ = this.HighMagnificationRZ;

            align.PatternCenterDistanceUm = this.PatternCenterDistanceUm;
            align.DPatternCenterDistanceUm = this.DPatternCenterDistanceUm;

            align.LBMaskBright = this.LBMaskBright;
            align.RBMaskBright = this.RBMaskBright;
            align.LBWaferBright = this.LBWaferBright;
            align.RBWaferBright = this.RBWaferBright;

            align.bLShowMask = this.bLShowMask;
            align.bRShowMask = this.bRShowMask;

            align.dLalpha = this.dLalpha;
            align.dRalpha = this.dRalpha;

            for (int j = 0; j < 12; j++)
            {
                align.LeftLight[j] = this.LeftLight[j];
                align.RightLight[j] = this.RightLight[j];
                align.LRingLight[j] = this.LRingLight[j];
                align.RRingLight[j] = this.RRingLight[j];
                align.LeftBrightness[j] = this.LeftBrightness[j];
                align.RightBrightness[j] = this.RightBrightness[j];
                align.LeftRingBrightness[j] = this.LeftRingBrightness[j];
                align.RightRingBrightness[j] = this.RightRingBrightness[j];
            }
        }

        public AlignCondition Clone()
        {
            AlignCondition dNow = (AlignCondition)this.MemberwiseClone();

            for (int i = 0; i < 12; i++)
            {
                dNow.LeftLight[i] = this.LeftLight[i];
                dNow.RightLight[i] = this.RightLight[i];
                dNow.LRingLight[i] = this.LRingLight[i];
                dNow.RRingLight[i] = this.RRingLight[i];
                dNow.LeftBrightness[i] = this.LeftBrightness[i];
                dNow.RightBrightness[i] = this.RightBrightness[i];
                dNow.LeftRingBrightness[i] = this.LeftRingBrightness[i];
                dNow.RightRingBrightness[i] = this.RightRingBrightness[i];
            }
            return dNow;
        }

        public void CopyTo(int i)
        {
            GV.acar[i].LastModifyTime = this.LastModifyTime;
            GV.acar[i].MaskSocre = this.MaskSocre;
            GV.acar[i].WaferScore = this.WaferScore;
            GV.acar[i].XOffset = this.XOffset;
            GV.acar[i].YLOffset = this.YLOffset;
            GV.acar[i].YROffset = this.YROffset;
            GV.acar[i].MaxAlignTimes = this.MaxAlignTimes;
            GV.acar[i].MinAlignTimes = this.MinAlignTimes;
            GV.acar[i].XPrecision = this.XPrecision;
            GV.acar[i].YPrecision = this.YPrecision;
            GV.acar[i].ThetaPrecision = this.ThetaPrecision;
            GV.acar[i].MaxExpansion = this.MaxExpansion;
            GV.acar[i].RecipeNumber = this.RecipeNumber;
            GV.acar[i].UpBackAlign = this.UpBackAlign;       // 1 = 上對位， 2 下對位
            GV.acar[i].UpBotMask = this.UpBotMask;
            GV.acar[i].AdjuestZ = this.AdjuestZ;
            GV.acar[i].PatternShift = this.PatternShift;      // 對位 Pattern 移動方向 0 - down , 1 - up , 2 - Left , 3 - Right
            GV.acar[i].LMaskAdjX = this.LMaskAdjX;
            GV.acar[i].LMaskAdjY = this.LMaskAdjY;
            GV.acar[i].RMaskAdjX = this.RMaskAdjX;
            GV.acar[i].RMaskAdjY = this.RMaskAdjY;

            GV.acar[i].LLMaskAlgorithm = this.LLMaskAlgorithm;
            GV.acar[i].LLWaferAlgorithm = this.LLWaferAlgorithm;
            GV.acar[i].RLMaskAlgorithm = this.RLMaskAlgorithm;
            GV.acar[i].RLWaferAlgorithm = this.RLWaferAlgorithm;

            GV.acar[i].LHMaskAlgorithm = this.LHMaskAlgorithm;
            GV.acar[i].LHWaferAlgorithm = this.LHWaferAlgorithm;
            GV.acar[i].RHMaskAlgorithm = this.RHMaskAlgorithm;
            GV.acar[i].RHWaferAlgorithm = this.RHWaferAlgorithm;

            GV.acar[i].IsLLMaskPatternMaskUse = this.IsLLMaskPatternMaskUse;
            GV.acar[i].IsLLWaferPatternMaskUse = this.IsLLWaferPatternMaskUse;
            GV.acar[i].IsLHMaskPatternMaskUse = this.IsLHMaskPatternMaskUse;
            GV.acar[i].IsLHWaferPatternMaskUse = this.IsLHMaskPatternMaskUse;

            GV.acar[i].IsRLMaskPatternMaskUse = this.IsRLMaskPatternMaskUse;
            GV.acar[i].IsRLWaferPatternMaskUse = this.IsRLWaferPatternMaskUse;
            GV.acar[i].IsRHMaskPatternMaskUse = this.IsRHMaskPatternMaskUse;
            GV.acar[i].IsRHWaferPatternMaskUse = this.IsRHWaferPatternMaskUse;

            GV.acar[i].AlignLowMagnification = this.AlignLowMagnification;
            GV.acar[i].LowMagnificationLZ = this.LowMagnificationLZ;
            GV.acar[i].LowMagnificationRZ = this.LowMagnificationRZ;
            GV.acar[i].AlignHighMagnification = this.AlignHighMagnification;
            GV.acar[i].HighMagnificationLZ = this.HighMagnificationLZ;
            GV.acar[i].HighMagnificationRZ = this.HighMagnificationRZ;

            GV.acar[i].PatternCenterDistanceUm = this.PatternCenterDistanceUm;
            GV.acar[i].DPatternCenterDistanceUm = this.DPatternCenterDistanceUm;

            GV.acar[i].LBMaskBright = this.LBMaskBright;
            GV.acar[i].RBMaskBright = this.RBMaskBright;
            GV.acar[i].LBWaferBright = this.LBWaferBright;
            GV.acar[i].RBWaferBright = this.RBWaferBright;

            GV.acar[i].bLShowMask = this.bLShowMask;
            GV.acar[i].bRShowMask = this.bRShowMask;

            GV.acar[i].dLalpha = this.dLalpha;
            GV.acar[i].dRalpha = this.dRalpha;

            for (int j = 0; j < 12; j++)
            {
                GV.acar[i].LeftLight[j] = this.LeftLight[j];
                GV.acar[i].RightLight[j] = this.RightLight[j];
                GV.acar[i].LRingLight[j] = this.LRingLight[j];
                GV.acar[i].RRingLight[j] = this.RRingLight[j];
                GV.acar[i].LeftBrightness[j] = this.LeftBrightness[j];
                GV.acar[i].RightBrightness[j] = this.RightBrightness[j];
                GV.acar[i].LeftRingBrightness[j] = this.LeftRingBrightness[j];
                GV.acar[i].RightRingBrightness[j] = this.RightRingBrightness[j];
            }
        }


        /*
        public AlignCondition()
        {
            LeftLight = new bool[12];
            RightLight = new bool[12];
            LRingLight = new bool[12];
            RRingLight = new bool[12];

            LeftBrightness = new int[12];
            RightBrightness = new int[12];
            LeftRingBrightness = new int[12];
            RightRingBrightness = new int[12];
        }
        */
    }

    public struct StAlignCondition
    {
        public float MaskSocre;
        public float WaferScore;
        public double XOffset;
        public double YOffset;
        public double ThetaOffset;
        public int MaxAlignTimes;
        public int MinAlignTimes;
        public double XPrecision;
        public double YPrecision;
        public double ThetaPrecision;
        public double MaxExpansion;
        public int RecipeNumber;
        public int UpBackAlign; // 1 = 上對位， 2 下對位
        public int PatternShift;   // 對位 Pattern 移動方向 0 - down , 1 - up , 2 - Left , 3 - Right


        public OpenCV3MatchUMat.AlignAlgorithm LLMaskAlgorithm;
        public OpenCV3MatchUMat.AlignAlgorithm LLWaferAlgorithm;
        public OpenCV3MatchUMat.AlignAlgorithm RLMaskAlgorithm;
        public OpenCV3MatchUMat.AlignAlgorithm RLWaferAlgorithm;

        public OpenCV3MatchUMat.AlignAlgorithm LHMaskAlgorithm;
        public OpenCV3MatchUMat.AlignAlgorithm LHWaferAlgorithm;
        public OpenCV3MatchUMat.AlignAlgorithm RHMaskAlgorithm;
        public OpenCV3MatchUMat.AlignAlgorithm RHWaferAlgorithm;

        public bool IsLLMaskPatternMaskUse;
        public bool IsLLWaferPatternMaskUse;
        public bool IsLHMaskPatternMaskUse;
        public bool IsLHWaferPatternMaskUse;

        public bool IsRLMaskPatternMaskUse;
        public bool IsRLWaferPatternMaskUse;
        public bool IsRHMaskPatternMaskUse;
        public bool IsRHWaferPatternMaskUse;

        public int AlignLowMagnification;
        public int AlignHighMagnification;

        public int PatternCenterDistanceUm;

        public bool[] LeftLight;
        public bool[] RightLight;
        public bool[] LRingLight;
        public bool[] RRingLight;
        public int[] LeftBrightness;
        public int[] RightBrightness;
        public int[] LeftRingBrightness;
        public int[] RightRingBrightness;

        public int LBMaskBright;
        public int RBMaskBright;
        public int LBWaferBright;
        public int RBWaferBright;

        public bool bLShowMask;
        public bool bRShowMask;

        public double dLalpha;
        public double dRalpha;

        /*
        public stAlignCondition()
        {
            this.MaskSocre = 0.6F;
            this.WaferScore = 0.6F;
            this.XOffset = 0F;
            this.YOffset = 0F;
            this.ThetaOffset = 0D;
            this.MaxAlignTimes = 10;
            this.XPrecision = 2F;
            this.YPrecision = 1F;
            this.ThetaPrecision = 0.001D;
            this.MaxExpansion = 3F;
            this.RecipeNumber = -1;
            this.UpBackAlign = 1; // 1 = 上對位， 2 下對位
            this.PatternShift = 0;   // 對位 Pattern 移動方向 0 - down , 1 - up , 2 - Left , 3 - Right

            this.LLMaskAlgorithm = OpenCV3MatchUMat.AlignAlgorithm.TemplateMatch;
            this.LLWaferAlgorithm = OpenCV3MatchUMat.AlignAlgorithm.TemplateMatch;
            this.RLMaskAlgorithm = OpenCV3MatchUMat.AlignAlgorithm.TemplateMatch;
            this.RLWaferAlgorithm = OpenCV3MatchUMat.AlignAlgorithm.TemplateMatch;

            this.LHMaskAlgorithm = OpenCV3MatchUMat.AlignAlgorithm.TemplateMatch;
            this.LHWaferAlgorithm = OpenCV3MatchUMat.AlignAlgorithm.TemplateMatch;
            this.RHMaskAlgorithm = OpenCV3MatchUMat.AlignAlgorithm.TemplateMatch;
            this.RHWaferAlgorithm = OpenCV3MatchUMat.AlignAlgorithm.TemplateMatch;

            this.IsLLMaskPatternMaskUse = false;
            this.IsLLWaferPatternMaskUse = false;
            this.IsLHMaskPatternMaskUse = false;
            this.IsLHWaferPatternMaskUse = false;

            this.IsRLMaskPatternMaskUse = false;
            this.IsRLWaferPatternMaskUse = false;
            this.IsRHMaskPatternMaskUse = false;
            this.IsRHWaferPatternMaskUse = false;

            this.AlignLowMagnification = 0;
            this.AlignHighMagnification = 0;

            this.PatternCenterDistanceUm = 75000;

            this.LeftLight = new bool[] { false, false, false, false, false, false, false, false, false, false, false, false };
            this.RightLight = new bool[] { false, false, false, false, false, false, false, false, false, false, false, false };
            this.LRingLight = new bool[] { false, false, false, false, false, false, false, false, false, false, false, false };
            this.RRingLight = new bool[] { false, false, false, false, false, false, false, false, false, false, false, false };
            this.LeftBrightness = new int[] { 10, 10, 10, 10, 10, 10, 11, 31, 26, 77, 110, 130 };
            this.RightBrightness = new int[] { 10, 10, 10, 10, 10, 10, 11, 31, 26, 77, 110, 130 };
            this.LeftRingBrightness = new int[] { 10, 10, 10, 10, 10, 10, 11, 31, 26, 77, 110, 130 };
            this.RightRingBrightness = new int[] { 10, 10, 10, 10, 10, 10, 11, 31, 26, 77, 110, 130 };

            this.LBMaskBright = 15;
            this.RBMaskBright = 15;
            this.LBWaferBright = 15;
            this.RBWaferBright = 15;

            this.bLShowMask = false;
            this.bRShowMask = false;

            this.dLalpha = 100.0;
            this.dRalpha = 100.0;
        }
        */
    }

    public class ZoomLensInfo
    {
        public int PatternWidth = 20;
        public double[] LeftUmPerPixelX = new double[12];
        public double[] RightUmPerPixelX = new double[12];
        public double[] LeftUmPerPixelY = new double[12];
        public double[] RightUmPerPixelY = new double[12];
        public int[] LeftMagnificationMotorSteps = new int[12];
        public int[] RightMagnificationMotorSteps = new int[12];
        public double[] LeftMotorStepsPerPixelX = new double[12];
        public double[] LeftMotorStepsPerPixelY = new double[12];
        public double[] RightMotorStepsPerPixelX = new double[12];
        public double[] RightMotorStepsPerPixelY = new double[12];
        public double MotorStepsPerPixelDegree;
        public double BMSPPDegree;
        public int LRUpCenterDistance = 233000;
        public int LRBackCenterDistance = 113000;

        public double[] LeftCameraMotorStepsPerPixelX = new double[12];
        public double[] RightCameraMotorStepsPerPixelX = new double[12];
        public double[] LeftCameraMotorStepsPerPixelY = new double[12];
        public double[] RightCameraMotorStepsPerPixelY = new double[12];

        public double[] LeftCameraAdjustX = new double[12];
        public double[] LeftCameraAdjustY = new double[12];
        public double[] LeftCameraAdjustZ = new double[12];
        public double[] RightCameraAdjustX = new double[12];
        public double[] RightCameraAdjustY = new double[12];
        public double[] RightCameraAdjustZ = new double[12];

        public double LeftDownUmPerPixelX = 0.3496503496503497;
        public double LeftDownUmPerPixelY = 0.3597122302158273;
        public double RightDownUmPerPixelX = 0.3703703703703704;
        public double RightDownUmPerPixelY = 0.3636363636363636;
        public double LeftDownUmPerPixelZ = 0.1;
        public double RightDownUmPerPixelZ = 0.1;
        public double XyyUmPerStepX = 0.1;
        public double XyyUmPerStepY1 = 0.1;
        public double XyyUmPerStepY2 = 0.1;
        public double LeftDownUmPerStepX = 0.1;
        public double LeftDownUmPerStepY = 0.1;
        public double RightDownUmPerStepX = 0.1;
        public double RightDownUmPerStepY = 0.1;
        public double[] LeftDownMotorStepsPerPixelX = new double[12];
        public double[] RightDownMotorStepsPerPixelX = new double[12];
        public double[] LeftDownMotorStepsPerPixelY = new double[12];
        public double[] RightDownMotorStepsPerPixelY = new double[12];
        public double[] LeftDownAdjustX = new double[12];
        public double[] LeftDownAdjustY = new double[12];
        public double[] LeftDownAdjustZ = new double[12];
        public double[] RightDownAdjustX = new double[12];
        public double[] RightDownAdjustY = new double[12];
        public double[] RightDownAdjustZ = new double[12];
        public double StageMotorStepsPerumX = 0.1;
        public double StageMotorStepsPerumY1 = 0.1;
        public double StageMotorStepsPerumY2 = 0.1;
        public double LeftMotorStepsPerumX = 0.1;
        public double LeftMotorStepsPerumY = 0.1;
        public double RightMotorStepsPerumX = 0.1;
        public double RightMotorStepsPerumY = 0.1;

        public int StepsOfSearchingWafer = 300;
    }

    public class BaslerCamParm
    {
        public double LeftUpGamma = 0;
        public double LeftUpGain = 0;
        public double LeftUpBlackLevel = 0;
        public double LeftUpExposureTime = 0;
        public double LeftUpFrameRate = 15;
        public bool LeftUpReserveX = false;
        public bool LeftUpReserveY = false;

        public double RightUpGamma = 0;
        public double RightUpGain = 0;
        public double RightUpBlackLevel = 0;
        public double RightUpExposureTime = 0;
        public double RightUpFrameRate = 15;
        public bool RightUpReserveX = false;
        public bool RightUpReserveY = false;

        public double LeftBackGamma = 0;
        public double LeftBackGain = 0;
        public double LeftBackBlackLevel = 0;
        public double LeftBackExposureTime = 0;
        public double LeftBackFrameRate = 10;
        public bool LeftBackReserveX = false;
        public bool LeftBackReserveY = false;

        public double RightBackGamma = 0;
        public double RightBackGain = 0;
        public double RightBackBlackLevel = 0;
        public double RightBackExposureTime = 0;
        public double RightBackFrameRate = 10;
        public bool RightBackReserveX = false;
        public bool RightBackReserveY = false;
    }

    public class AppSettingParm
    {       
        public int Author  = 0;
        public bool ProgramMode = false;
        public bool Emulation = false;
        public bool DebugMode = false;
        public bool InTestProgram = false;
        public bool EnableInitialZoomLensWhenStart = false;
        public bool EnableContactShiftCompensation = false;
        public bool EnableAutoSearch = false;
        public bool EnableDownThruHome = false;
        public bool BackSideCamEnable = false;
        public int SearchingSteps = 1000;

        public bool LightComPortEnable = false;
        public string LightComPort = "COM3";
        public bool RingLightPortEnable = false;
        public string RingLightPort = "COM4";
        public bool LeftZoomLensEnable = false;
        public string LeftZoomLensComPort = "COM1";
        public bool RightZoomLensEnable = false;
        public string RightZoomLensComPort = "COM2";
        public bool LeftUpCamEnable = false;
        public string LeftUpCamSerialNumber = "22270741";
        public bool RightUpCamEnable = false;
        public string RightUpCamSerialNumber = "22223159";
        public bool LeftBackCamEnable = false;
        public string LeftBackCamSerialNumber = "22270741";
        public int LeftCameraRotation = 0;
        public int LeftCameraMirror = 0;
        public bool RightBackCamEnable = false;
        public string RightBackCamSerialNumber = "22223159";
        public int RightCameraRotation = 0;
        public int RightCameraMirror = 0;
        public bool PlcEnable = true;
        public bool PlcStatusEnable = false;
        public string PlcIp = "192.168.1.101";
        public int PlcPort = 8501;

        public int DefaultUserLevel = 3;

        public string EngineerPassword = "1234";
        public string AdministratorPassword = "5978";
        public string ProductionModePassword = "5566948";
        public string Language = "default";
        public string LanguageFile = "defaultlng";

        public int XYYTableSize = 245000;
        public int XYYTableSpeed = 10000;
        public int TableMinSpeed = 300;
        public double XYYStepX = 0.1;
        public double XYYStepY1 = 0.1;
        public double XYYStepY2 = 0.1;
        public int LogAge = 30;
        public int ThetaX = 45;
        public int ThetaY1 = 135;
        public int ThetaY2 = 225;

        public int PLCTimeOut = 20;
        public int MaskImageWait = 2500;
        public int StartUpBack = 1;
        public int CaputerImageWait = 30;

        public int BackWait = 3600;
        public int RestartWait = 7200;

        public double _LxShift = -2.05;
        public double _LyShift = -4.15;
        public double _RxShift = 4.19;
        public double _RyShift = 2.58;

        public double LMaskScore = 1.0;
        public double RMaskScore = 1.0;

        public double LMaskScoreW = 1.0;
        public double RMaskScoreW = 1.0;

        public int LxShift4 = 0;
        public int LyShift4 = 0;
        public int RxShift4 = 0;
        public int RyShift4 = 0;

        public int LxShift6 = 0;
        public int LyShift6 = 0;
        public int RxShift6 = 0;
        public int RyShift6 = 0;

        public string[] sNowState = {"Please put the Wafer on the Chuck correctly, and press the \"Pick and Place Material\" button after placing it!",
                                     "Pease push the Feed Stage to the bottom and press \"Feed Stage lock\"",
                                     "Please press \"Level\" to level",
                                     "Leveling/unleveling operation~please wait",
                                     "The leveling is completed, please carry out the mark alignment, and press the \"Contact\" button after completion,Start contact and exposure",
                                     "Contact/disconnect action~ please wait",
                                     "After the contact is complete, press the \"Exposure\" button to expose",
                                     "Exposure ~ please wait",
                                     "Exposure complete ~ please wait",
                                     "The exposure is complete~ Please take out the Wafer!",
                                     "Mask mark take a picture!!",
                                     "Put wafer in chuck!!"
        };
        
    }

    public class MRException : Exception, ISerializable
    {
        public MRException(string message) : base(message)
        {
            GM.WriteToStatusTextBox(message);
        }
    }

    public class AlignResultData
    {
        public int AlignTimes = 0;
        public double Ldx = 0D;
        public double Ldy = 0D;
        public double Rdx = 0D;
        public double Rdy = 0D;
        public string Nowtime
        {
            get { return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); }
        }
        public double PrecisionX = 0D;
        public double PrecisionY = 0D;
        public double Expansion = 0D;
        public string ResultFileName = string.Empty;

        public AlignResultData(double ldx, double ldy, double rdx, double rdy, double precisionX, double precisionY, double expansion)
        {
            Ldx = ldx;
            Ldy = ldy;
            Rdx = rdx;
            Rdy = rdy;
            PrecisionX = precisionX;
            PrecisionY = precisionY;
            Expansion = expansion;
        }
        public AlignResultData()
        {

        }

        public override string ToString()
        {
            return $"{this.Nowtime},{this.Ldx},{this.Ldy},{this.Rdx},{this.Rdy},{this.PrecisionX},{this.PrecisionY},{this.Expansion},{this.ResultFileName}";
        }
    }

    public class RecipeInfo
    {
        public int RecipeNumber;        // Recipe 編號
        public int iUpDownSide;         // 上下CCD 對位 : 0 上CCD 對位 1 下CCD 對位
        public int iWaferSize;          // 1 - 2" 2 - 3" 3 - 4" 4 - 6"
        public int iUpCCDLeftX;         // 上CCD 左X 預定位 : +- 2000000 整數 
        public int iUpCCDLeftY;         // 上CCD 左Y 預定位 : +- 2000000 整數 
        public int iUpCCDRightX;        // 上CCD 右X 預定位 : +- 2000000 整數
        public int iUpCCDRightY;        // 上CCD 右X 預定位 : +- 2000000 整數
        public int iUpCCDBigY;          // 上CCD 大Y 預定位 : +- 2000000 整數

        public int iBackCCDLeftX;           // 下CCD 左X 預定位 : +- 2000000 整數
        public int iBackCCDLeftY;           // 下CCD 左Y 預定位 : +- 2000000 整數
        public int iBackCCDLeftZ;           // 下CCD 左Z 預定位 : +- 2000000 整數
        public int iBackCCDRightX;          // 下CCD 右X 預定位 : +- 2000000 整數
        public int iBackCCDRightY;          // 下CCD 右X 預定位 : +- 2000000 整數
        public int iBackCCDRightZ;          // 下CCD 右Z 預定位 : +- 2000000 整數

        public int iWECX;               // WEC X 預定位 : +- 2000000 整數
        public int iWECY1;              // WEC X 預定位 : +- 2000000 整數
        public int iWECY2;              // WEC X 預定位 : +- 2000000 整數
        public int iWECZ;               // WEC X 預定位 : +- 2000000 整數

        public int iAlignGap;           // 對位間隙 : 0~1000 單位 um

        public int iExposureGap;        // 曝光間隙 : 0~1000 單位 um
        public int iExposurePower;      // 曝光能量 : 0~10000 單位 mJ

        public int iContactMode;        // 接觸模式 : 1 hard contact  2 soft contact 3 Proximity
        public int iUnLevelingTime;     // 平整次數 : 0~10 單位 : 次數

        public float MaskScore = 0.7F;
        public float WaferScore = 0.7F;
        public double XOffset = 0F;
        public double YOffset = 0F;
        public double ThetaOffset = 0D;
        public int MaxAlignTimes = 10;
        public int MinAlignTimes = 0;
        public double XPrecision = 2F;
        public double YPrecision = 0.5F;
        public double ThetaPrecision = 0.00001D;
        public double MaxExpansion = 3F;

        public int AlignLowMagnification = 0;

        public int AlignHighMagnification = 0;

        public int PatternCenterDistanceUm = 0;

        public int[] LeftBrightness = { 3, 3, 3, 3, 4, 8, 11, 31, 26, 77 };
        public int[] RightBrightness = { 3, 3, 3, 3, 4, 8, 11, 31, 26, 77 };

        public int LeftBackBrightness = 15;
        public int RightBackBrightness = 15;

        public string RecipeID = "";  // Recipe 名稱
        public string MaskQRCode = "123";
        public string WaferIDJobName = "";
    }

    public struct OffsetF
    {
        public OffsetF(int iindex, int iAbs, double lx, double ly, double rx, double ry)
        {
            index = iindex;
            AbsHeight = iAbs;
            lOffsetX = lx;
            lOffsetY = ly;
            rOffsetX = rx;
            rOffsetY = ry;
        }

        public int index;
        public int AbsHeight;
        public double lOffsetX;
        public double lOffsetY;
        public double rOffsetX;
        public double rOffsetY;
    }

    public class DisplayLanguage
    {
        public string mainAlign = "Align";
        public string mainLearnPattern = "LearnPattern";
        public string MemoryPatternBottom = "MemoryPattern Bottom";
        public string mainParametersSetting = "Parameters Setting";
        public string btLogin = "Log in";
        public string btQuit = "Quit";
        public string gbAlignCondition = "Align Condition";
        public string lbMaskScore = "Mask Score";
        public string lbWaferScore = "Wafer Score";
        public string lbXOffset = "X Offset(um)";
        public string lbYOffset = "Y Offset(um)";
        public string lbThetaOffset = "ThetaOffset";
        public string lbMarkDist = "Mark DIST(um)";
        public string lbMaxAlignTimes = "Max Align Times";
        public string lbStdXPre = "Standard X Precision(um)";
        public string lbStdYPre = "Standard Y Precision(um)";
        public string lbStdThetaPre = "Standard Theta Precision";
        public string lbStdMaxExpansion = "Standard Max Expansion(um)";
        public string strRecipeNumber = "Recipe Number  : ";
        public string strSave = "Save";
        public string btCapture = "Capture";
        public string btTestMode = "Cycle Test";
        public string lbCompany = "M&&R Nano Technology co. Ltd";
        public string strCreateMark = "Create Mark";
        public string strAlgo = "ALGO.";
        public string strWafer = "Wafer";
        public string strMask = "Mask";
        public string groupBoxMagnification = "Magnification";
        public string radioButtonLowMagnification = "Low";
        public string radioButtonHighMagnification = "High";
        public string UpBackgroupBox = "Up/Back side select";
        public string UpsideCCD = "UpsideCCD";
        public string BacksideCCD = "BacksideCCD";
        public string gbMeasurePatternDistance = "Meas. pattern distance";
        public string strStep = "Steps :";
        public string btCalPatternDistance = "Measure";
        public string gbLeftZoomLens = "Left Zoom Lens";
        public string gbRightZoomLens = "Right Zoom Lens";
        public string gpcamaraParameter = "Camera Parmeters";
        public string radioButtonLeftCamera = "Left Camera";
        public string radioButtonRightCamera = "Right Camera";
        public string radioButtonLeftBackCam = "Left Back Cam";
        public string radioButtonRightBackCam = "Right Back Cam";
        public string btResotre = "Restore";
        public string strGain = "Gain";
        public string strGamma = "Gamma";
        public string strBlackLevel = "BlackLevel";
        public string strExposureTime = "ExposureTime";
        public string lbLCCDLight = "Left Brightness :";
        public string lbRCCDLight = "Right Brightness :";
        public string gbZoomLensParam = "Align Magnification";
        public string lbLowMagnification = "Align LowMagnification  :";
        public string lbHighMagnification = "Align HighMagnification :";
        public string btnSaveAlignMagnification = "Save Align Magnification";
        public string gbMachingSetting = "Maching setting";
        public string btnQuit = "Quit";
        public string btnManualPlc = "PLC";
        public string btnTuneXyyTable = "Tune Table";
        public string btTuneLens = "Tune Lens";
        public string buttonSetting = "Setting";
        public string buttonTuneLight = "TuneLight";
        public string btSetting = "Setting";
        public string buttonInitialLens = "Initial lens";
        public string frmLogin = "Log In";
        public string fbtLogin = "Log In";
        public string fbtCancel = "Cancel";
        public string fbtBack = "Back";
        public string frmManualPLCCom = "ManualPlcCom";
        public string fIPAddress = "IP Address";
        public string fPLCPort = "Port";
        public string fConnect = "Connect";
        public string fWrite = "Write";
        public string fRead = "Read";
        public string fSend = "Send";
        public string frmTuneTable = "DialogTuneTable";
        public string frdCamera = "Camera";
        public string frdTable = "Table";
        public string fgbZoomLens = "Zoom Lens";
        public string flbMoveGoto = "Goto";
        public string flbPettrnCenterDis = "Pattern Center Distance(mm) :";
        public string flbRecord = "Record :";
        public string fgbXYYTable = "XYY Table";
        public string strGo = "Go";
        public string strManual = "Manual";
        public string fgbCamera = "CameraAxisMotor";
        public string frdTheta = "Theta";
        public string flabelLeftResult = "Left steps per pixel : ";
        public string flabelRightResult = "Right steps per pixel : ";
        public string flLeftX = "LeftX";
        public string flRightX = "RightX";
        public string flLeftY = "LeftY";
        public string flRightY = "RightY";
        public string frmTuneLens = "DialogTuneLens";
        public string frbLeftLens = "Left zoom lens";
        public string frbRightLens = "Right zoom lens";
        public string fgbParameter = "Parameter";
        public string flbPatternWidth = "Pattern width(um) :";
        public string frbLive = "Live";
        public string frbMeasure = "Measure";
        public string fbtnPreCalculate = "Precalculate";
        public string fbtnCalAndSave = "Calculate and Save";
        public string flbX1 = "X1 (pixel)";
        public string flbX2 = "X2 (pixel)";
        public string flbX3 = "X3 (pixel)";
        public string flbX4 = "X4 (pixel)";
        public string flbX5 = "X5 (pixel)";
        public string flbX6 = "X6 (pixel)";
        public string flbX7 = "X7 (pixel)";
        public string flbX8 = "X8 (pixel)";
        public string flbX9 = "X9 (pixel)";
        public string flbX10 = "X10 (pixel)";
        public string flbX11 = "X11 (pixel)";
        public string flbX12 = "X12 (pixel)";
        public string frmAppSetting = "AppSetting";
        public string fcbWheatherInitialLens = "Initial zoom lens when start";
        public string fcbDownThruHome = "Lens down through home";
        public string fcbEnableContactShift = "Contact shift compensation";
        public string fcbEnableAutoSearch = "Auto searching";
        public string flbSearchSteps = "Searching steps :";
        public string fbtnAbout = "About";
        public string fbtnChangeEngineerPassword = "Change Engineer Password";
        public string fbtnChangeAdminPassword = "Change Admin Password";
        public string fbtnChangeProductionPassword = "Change Production Mode Password";
        public string flbOldPassword = "Old Password :";
        public string flbNewPassword = "New Password :";
        public string flbVerify = "Verify :";
        public string fbtnConfirm = "Confirm";
        public string frmUpdateLog = "DialogUpdatingLog";
        public string frmTuneLight = "DialogTuneLight";
        public string strbtnSaveLLW = "Sure to save Left Low wafer template ?";
        public string strbtnSaveLHW = "Sure to save Left High wafer template ?";
        public string strbtnSaveRLW = "Sure to save Right Low wafer template ?";
        public string strbtnSaveRHW = "Sure to save Right High wafer template ?";
        public string strbtnSaveLLM = "Sure to save Left Low mask template ?";
        public string strbtnSaveLHM = "Sure to save Left High mask template ?";
        public string strbtnSaveRLM = "Sure to save Right Low mask template ?";
        public string strbtnSaveRHM = "Sure to save Right High mask template ?";
        public string strSureSaveLLW = "LLWaferTemplateSave";
        public string strSureSaveLHW = "LHWaferTemplateSave";
        public string strSureSaveRLW = "RLWaferTemplateSave";
        public string strSureSaveRHW = "RHWaferTemplateSave";
        public string strSureSaveLLM = "LLMaskTemplateSave";
        public string strSureSaveLHM = "LHMaskTemplateSave";
        public string strSureSaveRLM = "RLMaskTemplateSave";
        public string strSureSaveRHM = "RHMaskTemplateSave";
        public string strAdmin = "Admin";
        public string strAdminLogout = "Admin Log Out";
        public string strEngineer = "Engineer";
        public string strEngineerLogout = "Engineer Log out";
        public string strOperator = "Operator";
        public string btSureLogout = "Sure to log out ?";
        public string btLogout = "Log Out";
        public string btSureQuit = "Sure to quit ?";
        public string btSureSaveAlignCondition = "Sure to save align conditions ?";
        public string btSaveAlignCon = "SaveAlignConditions";
        public string btAbort = "Abort";
        public string ProgramName = "M&R Semi-Auto Exposure System";
        public string ChangeLanguage = "Change To Default Language";
        public string fcbEmulationMode = "Emulation Mode";
        public string strChangeEmulationMode = "Do U Want Change to Emulation Mode";
        public string strInitialDevice = "InitialDevice";
        public string strWrongPassword = "Wrong Password!";
        public string strDown = "Down";
        public string strUp = "Up";
        public string strLeft = "Left";
        public string strRight = "Right";
        public string strCenter = "Center";
        public string strDelete = "Delete";
        public string strDefault = "Default";
        public string strRingLight = "RingLight";
        public string strCoaLight = "CoaLight";
        public string strTop = "Top";
        public string strBack = "Bottom";
        public string strLive = "Live";
        public string strRecord = "Record";
        public string strMax = "Max:";
        public string strMin = "Min:";
        public string strLight = "Light";
        public string strMaskImage = "MaskImage";
        public string strbtOK = "OK";
        public string strPLCStatus = "PLC Status Windows";
        public string strConnecting = "Connecting cameras...";
        public string strOK = "OK";
        public string strConnectPlc = "Connecting plc...";
        public string strConnectLenLight = "Connecting lens light...";
        public string strConnectLenRingLight = "Connecting len ring light ...";
        public string strConnectLeftZoom = "Connecting left zoom lens...";
        public string strConnectRightZoom = "Connecting right zoom lens...";
        public string strInitialLenLight = "Initializing lens light...";
        public string strInitialCameras = "Initializing cameras...";
        public string strInitialZoomLens = "Initializing zoom lens ...";
        public string strLaunchSucc = "Launch device success.";
        public string strWelcome = "Welcome to M&R auto align system.";
        public string strSureDeleteRecipe = "Sure to Delete Recipe : {0}";
        public string strDeleteRecipe = "Delete Recipe";
        public string strSureDefaultRecipe = "Sure Set All Light Setting to Recipe default value: {0} ??";
        public string strSetRecipe = "Set Recipe";
        public string strLowMagnification = "Low Magnification = {0}";
        public string strHighMagnification = "High Magnification = {0}";
        public string strPatternShift = "Pattern Shift = {0}";
        public string strTopAlignMode = "And Set Recipe to TOP Alignmode!!";
        public string strSaveMagnification = "Save magnification";
        public string strSureSave = "Sure to save";
        public string strTopCCD = "Top CCD";
        public string strBottomCCD = "Bottom CCD";
        public string strLLTopMaskSave = "Left Low Top Mask";
        public string strLLTopMask = "Sure to save Left Low magnification top mask template";
        public string strLHTopMaskSave = "Left High Top Mask";
        public string strLHTopMask = "Sure to save Left High magnification top mask template";
        public string strRLTopMaskSave = "Right Low Top Mask";
        public string strRLTopMask = "Sure to save Right Low magnification top mask template";
        public string strRHTopMaskSave = "Right High Top Mask";
        public string strRHTopMask = "Sure to save Right High magnification top mask template";
        public string strLBottomMaskSave = "Left Bottom Mask Template Save";
        public string strLBottomMask = "Sure to save left bottom mask template ?";
        public string strRBottomMaskSave = "Right Bottom Mask Template Save";
        public string strRBottomMask = "Sure to save right bottom mask template ?";
        public string strLWaferSave = "Left Bottom Wafer Template Save";
        public string strLWafer = "Sure to save left bottom wafer template ?";
        public string strRWaferSave = "Right Bottom Wafer Template Save";
        public string strRWafer = "Sure to save right bottom wafer template ?";
        public string strLightLTopMaskSave = "Top Low Mask Light";
        public string strLightLTopMask = "Sure to save low magnification top mask light setting ?";
        public string strLightHTopMaskSave = "Top High Mask Light";
        public string strLightHTopMask = "Sure to save high magnification top mask light setting ?";
        public string strLightBottomMaskSave = "Bottom Mask Light";
        public string strLightBottomMask = "Sure to save bottom mask light setting ?";
        public string strLightWaferSave = "Wafer Light";
        public string strLightWafer = "Sure to save wafer light setting ?";
        public string strClear = "clear";
        public string strErrorCreateFirst = "Error : Please Create (high/low) pattern first";
        public string strErrorCreateBottomFirst = "Error : Please learn (high/low) mask  or back mask or wafer pattern first";
        public string strMessageLowMagnification = "--------------------------Low Mag.-----------------------";
        public string strMessageZoomOut = "LENS ZOOMING OUT...";
        public string strMessageMaskCenter = "Mask centering...";
        public string strMessageWaferSearch = "Wafer searching...";
        public string strMessagesHighMagnification = "--------------------------High Mag.------------------------";
        public string strMessageZoomIn = "LENS ZOOMING IN...";
        public string strMessageAlign = "----------------------------Align------------------------";
        public string strMessageBottomMaskAlign = "-------------------Bottom Mask Algin--------------------";
        public string strMessageElaspedTime = "Elasped : {0} ms";
        public string strMessageLeftMaskWaferScore = "L Mask / Wafer score : {0} , {1}";
        public string strMessageRightMaskWaferScore = "R Mask / Wafer score : {0} , {1}";
        public string strErrorCantFindMask = "Error : Can't find mask!!";
        public string strMessageWaferScore = "L / R Wafer score : {0} {1}";
        public string strErrorCantFindWafer = "Can't find wafer !!";
        public string strMessageFindWaferSucc = "Wafer searching successfully";
        public string strErrorHighCantFindMask = "Error : Can't find mask template at high magnification";
        public string strMessageLShiftum = "L shift(um) : ({0}, {1})";
        public string strMessageRShiftum = "R shift(um) : ({0}, {1})";
        public string strMessageTableStep = "Table(steps)";
        public string strMessageAlign1 = "Align ======== 1 - ";
        public string strErrorHighCantFindWafer = "Error : Can't find wafer template at high magnification";
        public string strMessageStage1done = "Stage 1 done";
        public string strErrorMaxAlign = "Error : Over max align times";
        public string strMessageAlgin2 = "Align ======== 2 - ";
        public string strMessageStage2done = "Stage 2 done";
        public string strErrorCantBottomMask = "Bottom CCD Mask Can't find";
        public string strMessageMaskScore = "L Mask / R Mask score : ";
        public string strErrorLowCantFindMask = "Error : Can't find mask template at low magnification";
        public string strMessageCheckMaskAlign = "--------------------Check Mask Align------------------------";
        public string strMessageBottomWaferAlign = "--------------------Bottom Wafer Align------------------------";
        public string strMessageMaskShift = "LX:{0:F}, RX:{1:F}, LY:{2:F}, RY:{3:F}";
        public string strMessageBottomAlign1 = "Bottom Align ======== 1 - ";
        public string strMessageBottomAlign2 = "Bottom Align ======== 2 - ";
        public string strErrorCantBottomWafer = "Bottom CCD wafer Can't find";
        public string strMessagePrecisionX = "PrecisionX(um) : ";
        public string strMessagePrecisionY = "PrecisionY(um) : ";
        public string strMessageRotation = "Rotation(deg) : ";
        public string strMessageExpansion = "Expansion(um) : ";
        public string strMessageCameraMoveError = "CCD Move Error!";
        public string strFindCenter = "FindCenter";
        public string strTemplate = "Template";
        public string strEdge = "Edge";
        public string strAlignTest = "AlignTest";
        public string strCapture = "Capture";
        public string strChuckAlign = "ChuckAlign";
        public string strCapMask = "CapMask";
        public string strMaskCenter = "MaskCenter";
        public string strLMaskX = "LMask-X";
        public string strLMaskY = "LMask-Y";
        public string strRMaskX = "RMask-X";
        public string strRMaskY = "RMask-Y";
        public string[] sNowState = {"Please put the Wafer on the Chuck correctly, and press the \"Pick and Place Material\" button after placing it!",
                                     "Pease push the Feed Stage to the bottom and press \"Feed Stage lock\"",
                                     "Please press \"Level\" to level",
                                     "Leveling/unleveling operation~please wait",
                                     "The leveling is completed, please carry out the mark alignment, and press the \"Contact\" button after completion,Start contact and exposure",
                                     "Contact/disconnect action~ please wait",
                                     "After the contact is complete, press the \"Exposure\" button to expose",
                                     "Exposure ~ please wait",
                                     "Exposure complete ~ please wait",
                                     "The exposure is complete~ Please take out the Wafer!",
                                     "Mask mark take a picture!!",
                                     "Put wafer in chuck!!"
        };
        public string strTopMask = "Mask";
        public string strBottomMask = "Bond";
        public string strAI = "AI Identify";
        public string strImageLabel = "Label Image";
        public string strLanguageSelect = "Language Select";
        public string strEnglish = "English";
        public string strTChinese = "Traditional Chinese";
        public string strSChinese = "Simplified Chinese";
        public string strMessage1 = "The labeled information has been saved!";
        public string strLabelImageTitle = "Label Image - Recipe";
        public string strLabelImagePath = "Path";
        public string strBtnPrev = "Previous";
        public string strBtnNext = "Next";
        public string strBtnSave = "Save";
        public string strBtnClear = "Clear All";
        public string strBtnAddClass = "Add";
        public string strBtnDeleteClass = "Delete";
        public string strBtnTrain = "Train Model";
        public string strBtnDeleteImage = "Delete Image";
        public string strLabelClass = "Class:";
        public string strLabelNewClass = "New Class:";
        public string strConfirmClearAll = "Are you sure you want to clear all annotations?\n\n{0} annotation boxes will be cleared.";
        public string strConfirmClear = "Confirm Clear";
        public string strNoAnnotation = "No annotations to delete!";
        public string strClassExists = "Class '{0}' already exists!";
        public string strEnterClassName = "Please enter a class name!";
        public string strDeleteClassTitle = "Delete Class - Recipe";
        public string strSelectClassToDelete = "Select classes to delete: (Recipe{0})";
        public string strConfirmDelete = "Confirm Delete";
        public string strNoClassSelected = "No class selected!";
        public string strConfirmDeleteClass = "Are you sure you want to delete the following {0} classes?\n\n{1}\n\nWarning: This will affect annotated images!";
        public string strDeleteSuccess = "Successfully deleted {0} classes!";
        public string strComplete = "Complete";
        public string strConfirmDeleteImage = "Are you sure you want to delete the current image?";
        public string strCannotUndo = "Warning: This action cannot be undone!";
        public string strImageDeleted = "Image deleted!\nNo images left in this folder.";
        public string strDeleteComplete = "Delete Complete";
        public string strNoImageFound = "No image files found!";
        public string strTrainingConfirm = "Preparing to train model for Recipe {0}\n\nDataset: {1}\nBase Model: {2}\nParameters: epochs=50, imgsz=2000\n\nTraining will run in background.\nContinue?";
        public string strConfirmTraining = "Confirm Training";
        public string strTrainingStarted = "Recipe {0} background training started!\n\nTraining will run in background.\nYou will be notified when complete.";
        public string strTrainingLaunched = "Training Launched";
        public string strTrainingComplete = "Recipe {0} model training complete!\n\nNew model saved to:\n{1}\n\nTraining results at:\n{2}";
        public string strTrainingSuccess = "Training Success";
        public string strHint = "Hint";
        public string strError = "Error";
        public string strWarning = "Warning";
        public string strPermissionError = "Permission Error";
        public string strNoPermission = "No permission to access folder: {0}\nError: {1}";
        public string strLoadImageError = "Error loading images: {0}";
        public string strSaveError = "Error saving: {0}";
        public string strDeleteError = "Error deleting: {0}";
        public string strTraining = "Training...";
        public string gbWECLocation = "WEC Location";
        public string lbCurrentPosition = "Current Position";
        public string lbTargetPosition = "Target Position";
        public string lbChuckX = "Chuck X";
        public string lbChuckY1 = "Chuck Y1";
        public string lbChuckY2 = "Chuck Y2";
        public string lbChuckZ = "Chuck Z";
        public string lbChuckR = "Chuck R";
        public string btChuckUpdate = "Update";
        public string btChuckGo = "Go";
        public string gbCCDLocation = "CCD Location";
        public string lbBigY = "Big Y";
        public string lbLeftX = "Left X";
        public string lbLeftY = "Left Y";
        public string lbLeftZ = "Left Z";
        public string lbRightX = "Right X";
        public string lbRightY = "Right Y";
        public string lbRightZ = "Right Z";
        public string btCCDUpdate = "Update";
        public string btCCDGo = "Go";
        public string strConfirmUpdateWEC = "Confirm to update current position to target position?";
        public string strConfirmUpdateCCD = "Confirm to update current CCD position to target position?";
        public string strUpdateSuccess = "Update successful";
        public string strUpdateWECLocation = "Update WEC Location";
        public string strUpdateCCDLocation = "Update CCD Location";
    }

    public class PLCAlarmCode
    {
        public CAlarmState[] AlarmCode = new CAlarmState[160];
        public CAlarmState[] HmiState = new CAlarmState[180];
        public CAlarmState[] XState = new CAlarmState[36];
        public CAlarmState[] YState = new CAlarmState[32];
    }

    public class CAlarmState
    {
        public int iNumber;
        public string iState;

        public CAlarmState()
        {
            iNumber = 0;
            iState = "";
        }
    }

    class YoloBox
    {
        public int ClassId;
        public float X, Y, W, H;
        public string ClassName { get; set; }
    }
}
