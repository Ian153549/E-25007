using OpenCvSharp;
using MRLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NSAA_16Axis
{
    //public class RecipeTemplate
    //{
    //    public Mat LeftLowMaskMat;
    //    public Mat LeftLowWaferMat;
    //    public Mat RightLowMaskMat;
    //    public Mat RightLowWaferMat;

    //    public Mat LeftHighMaskMat;
    //    public Mat LeftHighWaferMat;
    //    public Mat RightHighMaskMat;
    //    public Mat RightHighWaferMat;

    //    public GrayImage LeftLowMaskMask;
    //    public GrayImage LeftLowWaferMask;
    //    public GrayImage RightLowMaskMask;
    //    public GrayImage RightLowWaferMask;

    //    public GrayImage LeftHighMaskMask;
    //    public GrayImage LeftHighWaferMask;
    //    public GrayImage RightHighMaskMask;
    //    public GrayImage RightHighWaferMask;
    //}

    [XmlRoot("Recipe")]
    public class Recipe
    {
        public bool Valid { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string TemplatePath { get; set; }

        public int RecipeNumber { get; set; }

        public string RecipeName { get; set; }

        public int ExposureType { get; set; }

        public double ExposureValue { get; set; }

        public int ExposureTime { get; set; }

        public double ExposurePower { get; set; }

        public double ExposurePowerRadio { get; set; }

        public int AlignDistance { get; set; }

        public int ExposureDistance { get; set; }

        public int ContactMode { get; set; }

        public int WaferSize { get; set; }

        public float OCRMinScore { get; set; }

        public int SpeedPercent { get; set; }

        public string JobName { get; set; }

        public int PAWaferType { get; set; }

        public int WaferMaterial { get; set; }

        public int Overpressure { get; set; }

        public int Gapcompensate { get; set; }

        public int WaferZHigh { get; set; }

        public int WaferLeveling { get; set; }

        public int HardContact { get; set; }

        public int WaferThinkness { get; set; }
        public int WaferRotate { get; set; }

        public double PAOCROrientation { get; set; }

        public double PAFinalWaferOrientation { get; set; }

        public double PA2FinalWaferOrientation { get; set; }

        public bool ByPassLX { get; set; }

        public double[] InX6Posi { get; set; }

        public int IAutoAdjustRecipe { get; set; }

        public int OCRTryCount { get; set; }
        public int MaskChangeCount { get; set; }

        public bool ContactConfirm { get; set; }

        public bool ContactShiftCompensation { get; set; }

        public int AlignMode { get; set; }

        public int Flag { get; set; }

        public int RobotArmSelect { get; set; }  //0: Both Arm ; 1: upper arm ; 2:lower arm

        public string MaskBarcode { get; set; }

        public int MaskCassetteSize { get; set; }

        public AlignCondition AlignC = new AlignCondition();

        public XYYPosition MaskPosition = new XYYPosition();

        public XYYPosition WaferPosition = new XYYPosition();

        public PARecipe PARecipe = new PARecipe();

        public RobotRecipe RobotRecipe = new RobotRecipe();

        public RobotRecipe MaskRobotRecipe = new RobotRecipe()
        {
            SpeedPercent = 1
        };

        public UpperMotorPosition UpperMPosition = new UpperMotorPosition();

        public UpperMotorPosition UpperMLowResPosition = new UpperMotorPosition();

        public UpperArmWECPosition UpperWECPos = new UpperArmWECPosition();

        public UpperArmWECPosition UpperLowResWECPos = new UpperArmWECPosition();

        public LowerMotorPosition LowerMPosition = new LowerMotorPosition();

        public LowerArmWECPosition LowerWECPos = new LowerArmWECPosition();

        public UpperMotorPosition QRCodePosition = new UpperMotorPosition();

        public MotorPosition MotorPositionPos = new MotorPosition();

        public DisPosition disPos = new DisPosition();

        [XmlIgnore]
        public Mat LeftLowMaskMat;
        [XmlIgnore]
        public Mat LeftLowWaferMat;
        [XmlIgnore]
        public Mat LeftHighMaskMat;
        [XmlIgnore]
        public Mat LeftHighWaferMat;
        [XmlIgnore]
        public Mat RightLowMaskMat;
        [XmlIgnore]
        public Mat RightLowWaferMat;
        [XmlIgnore]
        public Mat RightHighMaskMat;
        [XmlIgnore]
        public Mat RightHighWaferMat;
        [XmlIgnore]
        public GrayImage LeftLowMaskMask;
        public GrayImage LeftLowWaferMask;
        public GrayImage RightLowMaskMask;
        public GrayImage RightLowWaferMask;

        public GrayImage LeftHighMaskMask;
        public GrayImage LeftHighWaferMask;
        public GrayImage RightHighMaskMask;
        public GrayImage RightHighWaferMask;

        public string LeftLowMaskMatData
        {
            get
            {
                if (LeftLowMaskMat != null)
                {
                    if (LeftLowMaskMat.Empty())
                    {
                        return null;
                    }
                    Cv2.ImEncode(".png", LeftLowMaskMat,out byte[] imageData);
                    return Convert.ToBase64String(imageData);
                }
                return null;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    byte[] imageData = Convert.FromBase64String(value);
                    LeftLowMaskMat=Cv2.ImDecode(imageData, ImreadModes.Grayscale);
                }
            }
        }

        public string LeftLowWaferMatData
        {
            get
            {
                if (LeftLowWaferMat != null)
                {
                    if (LeftLowWaferMat.Empty())
                    {
                        return null;
                    }
                    Cv2.ImEncode(".png", LeftLowWaferMat,out byte[] imageData);
                    return Convert.ToBase64String(imageData);
                }
                return null;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    byte[] imageData = Convert.FromBase64String(value);
                    LeftLowWaferMat=Cv2.ImDecode(imageData, ImreadModes.Grayscale);
                }
            }
        }

        public string LeftHighMaskMatData
        {
            get
            {
                if (LeftHighMaskMat != null)
                {
                    if (LeftHighMaskMat.Empty())
                    {
                        return null;
                    }
                    Cv2.ImEncode(".png", LeftHighMaskMat, out byte[] imageData);
                    return Convert.ToBase64String(imageData);
                }
                return null;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    byte[] imageData = Convert.FromBase64String(value);
                    LeftHighMaskMat=Cv2.ImDecode(imageData, ImreadModes.Grayscale);
                }
            }
        }

        public string LeftHighWaferMatData
        {
            get
            {
                if (LeftHighWaferMat != null)
                {
                    if (LeftHighWaferMat.Empty())
                    {
                        return null;
                    }
                    Cv2.ImEncode(".png", LeftHighWaferMat, out byte[] imageData);
                    return Convert.ToBase64String(imageData);
                }
                return null;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    byte[] imageData = Convert.FromBase64String(value);
                    LeftHighWaferMat = Cv2.ImDecode(imageData, ImreadModes.Grayscale);
                }
            }
        }

        public string RightLowMaskMatData
        {
            get
            {
                if (RightLowMaskMat != null)
                {
                    if (RightLowMaskMat.Empty())
                    {
                        return null;
                    }
                    Cv2.ImEncode(".png", RightLowMaskMat, out byte[] imageData);
                    return Convert.ToBase64String(imageData);
                }
                return null;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    byte[] imageData = Convert.FromBase64String(value);
                    RightLowMaskMat = Cv2.ImDecode(imageData, ImreadModes.Grayscale);
                }
            }
        }

        public string RightLowWaferMatData
        {
            get
            {
                if (RightLowWaferMat != null)
                {
                    if (RightLowWaferMat.Empty())
                    {
                        return null;
                    }
                    Cv2.ImEncode(".png", RightLowWaferMat, out byte[] imageData);
                    return Convert.ToBase64String(imageData);
                }
                return null;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    byte[] imageData = Convert.FromBase64String(value);
                    RightLowWaferMat = Cv2.ImDecode(imageData, ImreadModes.Grayscale);
                }
            }
        }

        public string RightHighMaskMatData
        {
            get
            {
                if (RightHighMaskMat != null)
                {
                    if (RightHighMaskMat.Empty())
                    {
                        return null;
                    }
                    Cv2.ImEncode(".png", RightHighMaskMat, out byte[] imageData);
                    return Convert.ToBase64String(imageData);
                }
                return null;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    byte[] imageData = Convert.FromBase64String(value);
                    RightHighMaskMat = Cv2.ImDecode(imageData, ImreadModes.Grayscale);
                }
            }
        }

        public string RightHighWaferMatData
        {
            get
            {
                if (RightHighWaferMat != null)
                {
                    if (RightHighWaferMat.Empty())
                    {
                        return null;
                    }
                    Cv2.ImEncode(".png", RightHighWaferMat, out byte[] imageData);
                    return Convert.ToBase64String(imageData);
                }
                return null;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    byte[] imageData = Convert.FromBase64String(value);
                    RightHighWaferMat = Cv2.ImDecode(imageData, ImreadModes.Grayscale);
                }
            }
        }

        public Mat GetLeftLowMaskMat() => LeftLowMaskMat;

        public void SetLeftLowMaskMat(Mat mat) => LeftLowMaskMat = mat;

        public Mat GetLeftLowWaferMat() => LeftLowWaferMat;

        public void SetLeftLowWaferMat(Mat mat) => LeftLowWaferMat = mat;

        public Mat GetLeftHighMaskMat() => LeftHighMaskMat;

        public void SetLeftHighMaskMat(Mat mat) => LeftHighMaskMat = mat;

        public Mat GetLeftHighWaferMat() => LeftHighWaferMat;

        public void SetLeftHighWaferMat(Mat mat) => LeftHighWaferMat = mat;

        public Mat GetRightLowMaskMat() => RightLowMaskMat;

        public void SetRightLowMaskMat(Mat mat) => RightLowMaskMat = mat;

        public Mat GetRightLowWaferMat() => RightLowWaferMat;

        public void SetRightLowWaferMat(Mat mat) => RightLowWaferMat = mat;

        public Mat GetRightHighMaskMat() => RightHighMaskMat;

        public void SetRightHighMaskMat(Mat mat) => RightHighMaskMat = mat;

        public Mat GetRightHighWaferMat() => RightHighWaferMat;

        public void SetRightHighWaferMat(Mat mat) => RightHighWaferMat = mat;

        public Recipe()
        {
            Valid = false;
            FileName = "default";
            FilePath = "";
            TemplatePath = "";
            RecipeNumber = 1;
            RecipeName = "1";
            ExposureType = 0;
            ExposureValue = 10;
            ExposureTime = 1;
            ExposurePower = 1;
            ExposurePowerRadio = 70.0;
            AlignDistance = 60;
            ExposureDistance = 10;
            ContactMode = 2;
            WaferSize = 6;
            OCRMinScore = 150f;
            SpeedPercent = 2;
            JobName = "42";
            PAWaferType = 1;
            WaferMaterial = 0;
            Overpressure = 300;
            Gapcompensate = 0;
            WaferZHigh = 360600;
            WaferLeveling = 360600 - 700 * 8;
            HardContact = 50;
            WaferThinkness = 700;
            WaferRotate = 0;
            PAOCROrientation = 180;
            PAFinalWaferOrientation = 89.9;
            PA2FinalWaferOrientation = 180;
            ByPassLX = false;
            InX6Posi = new double[4] { 50.0, 50.0, 50.0, 50.0 };
            IAutoAdjustRecipe = 0;
            OCRTryCount = 3;
            MaskChangeCount = 10;
            ContactConfirm = false;
            ContactShiftCompensation = true;
            AlignMode = 1;
            Flag = 0;
            RobotArmSelect = 0;  //0: Both Arm ; 1: upper arm ; 2:lower arm
            MaskBarcode = "";
            MaskCassetteSize = 7;
            AlignC = new AlignCondition();
            LeftLowMaskMask = new GrayImage(4000, 3000);
            LeftLowWaferMask = new GrayImage(4000, 3000);
            LeftHighMaskMask = new GrayImage(4000, 3000);
            LeftHighWaferMask = new GrayImage(4000, 3000);
            RightLowMaskMask = new GrayImage(4000, 3000);
            RightLowWaferMask = new GrayImage(4000, 3000);
            RightHighMaskMask = new GrayImage(4000, 3000);
            RightHighWaferMask = new GrayImage(4000, 3000);
            LeftLowMaskMat = new Mat();
            LeftLowWaferMat = new Mat();
            LeftHighMaskMat = new Mat();
            LeftHighWaferMat = new Mat();
            RightLowMaskMat = new Mat();
            RightLowWaferMat = new Mat();
            RightHighMaskMat = new Mat();
            RightHighWaferMat = new Mat();
        }

        public void Init()
        {
            //LeftLowMaskMat = new Mat();
            //LeftLowWaferMat = new Mat();
            //RightLowMaskMat = new Mat();
            //RightLowWaferMat = new Mat();

            //LeftHighMaskMat = new Mat();
            //LeftHighWaferMat = new Mat();
            //RightHighMaskMat = new Mat();
            //RightHighWaferMat = new Mat();
        }

        public void AutoAdjust()
        {
            //MotorPosition moPos = this.MotorPositionPos;
            //ZoomLensInfo ZoomInfo = GV.ZoomLensInfo;

            //if (iAutoAdjustRecipe == 1)
            //{
            //    moPos.XR6 = GV.NowLocation[4];
            //    moPos.YR6 = GV.NowLocation[5];
            //    moPos.ZR6 = GV.NowLocation[12];
            //    moPos.XL6 = GV.NowLocation[2];
            //    moPos.YL6 = GV.NowLocation[3];
            //    moPos.ZL6 = GV.NowLocation[11];
            //    moPos.Y6 = GV.NowLocation[1];
            //    MaskPosition.X = GV.NowLocation[8];
            //    MaskPosition.Y1 = GV.NowLocation[9];
            //    MaskPosition.Y2 = GV.NowLocation[10];
            //}
            //else if (iAutoAdjustRecipe == 2)
            //{
            //    moPos.XR6 = (GV.NowLocation[4] + moPos.XR6) / 2;
            //    moPos.YR6 = (GV.NowLocation[5] + moPos.YR6) / 2;
            //    moPos.ZR6 = (GV.NowLocation[12] + moPos.ZR6) / 2;
            //    moPos.XL6 = (GV.NowLocation[2] + moPos.XL6) / 2;
            //    moPos.YL6 = (GV.NowLocation[3] + moPos.YL6) / 2;
            //    moPos.ZL6 = (GV.NowLocation[11] + moPos.ZL6) / 2;
            //    moPos.Y6 = (GV.NowLocation[1] + moPos.Y6) / 2;
            //    MaskPosition.X = (GV.NowLocation[8] + MaskPosition.X) / 2;
            //    MaskPosition.Y1 = (GV.NowLocation[9] + MaskPosition.Y1) / 2;
            //    MaskPosition.Y2 = (GV.NowLocation[10] + MaskPosition.Y2) / 2;
            //}

            //int Y = moPos.Y1 - moPos.Y6;
            //int XR = moPos.XR1 - moPos.XR6;
            //int YR = moPos.YR1 - moPos.YR6;
            //int ZR = moPos.ZR1 - moPos.ZR6;
            //int XL = moPos.XL1 - moPos.XL6;
            //int YL = moPos.YL1 - moPos.YL6;
            //int ZL = moPos.ZR1 - moPos.ZL6;

            //// double dYR = (YR - GV.AppSettingParm.RightYExposure) * ZoomInfo.RightMotorStepsPerumY + (Y - GV.AppSettingParm.StageYExposure) * ZoomInfo.StageMotorStepsPerumY;
            //// double dYL = (YL - GV.AppSettingParm.LeftYExposure) * ZoomInfo.LeftMotorStepsPerumY + (Y - GV.AppSettingParm.StageYExposure) * ZoomInfo.StageMotorStepsPerumY;
            //double dYR = YR * ZoomInfo.RightMotorStepsPerumY + Y * ZoomInfo.StageMotorStepsPerumY;
            //double dYL = YL * ZoomInfo.LeftMotorStepsPerumY + Y * ZoomInfo.StageMotorStepsPerumY;
            //double dY1 = (dYR + dYL) / 2;
            //int Y1 = (int)(dY1 / ZoomInfo.StageMotorStepsPerumY);
            //int YL1 = (int)((dYL - dY1) / ZoomInfo.LeftMotorStepsPerumY);
            //int YR1 = (int)((dYR - dY1) / ZoomInfo.RightMotorStepsPerumY);
            //// Y1 += GV.AppSettingParm.StageYExposure;
            //// YL1 += GV.AppSettingParm.LeftYExposure;
            //// YR1 += GV.AppSettingParm.RightYExposure;

            //double dXR1 = XR * ZoomInfo.RightMotorStepsPerumX;
            //double dXL1 = XL * ZoomInfo.LeftMotorStepsPerumX;
            //double dX1 = (dXR1 + dXL1) / 2;
            //int XL1 = XL - (int)(dX1 / ZoomInfo.LeftMotorStepsPerumX);
            //int XR1 = XR - (int)(dX1 / ZoomInfo.RightMotorStepsPerumX);
            ////int X1 = X - (int)(dX1 / ZoomInfo.StageMotorStepsPerumX);

            //disPos.XR6 = XR1;
            //disPos.YR6 = YR1;
            //disPos.ZR6 = moPos.ZR1 - moPos.ZR6;
            //disPos.XL6 = XL1;
            //disPos.YL6 = YL1;
            //disPos.ZL6 = moPos.ZL1 - moPos.ZL6;
            //disPos.Y6 = Y1;
        }

        //public void AutoAdjustLow()
        //{
        //    MotorPosition moPos = this.MotorPositionPos;
        //    ZoomLensInfo ZoomInfo = GV.ZoomLensInfo;

        //    if (iAutoAdjustRecipe == 1)
        //    {
        //        moPos.XR1 = GV.NowLocation[4];
        //        moPos.YR1 = GV.NowLocation[5];
        //        moPos.ZR1 = GV.NowLocation[12];
        //        moPos.XL1 = GV.NowLocation[2];
        //        moPos.YL1 = GV.NowLocation[3];
        //        moPos.ZR1 = GV.NowLocation[11];
        //        moPos.Y1 = GV.NowLocation[1];
        //    }
        //    else if (iAutoAdjustRecipe == 2)
        //    {
        //        moPos.XR1 = (GV.NowLocation[4] + moPos.XR1) / 2;
        //        moPos.YR1 = (GV.NowLocation[5] + moPos.YR1) / 2;
        //        moPos.ZR1 = (GV.NowLocation[12] + moPos.ZR1) / 2;
        //        moPos.XL1 = (GV.NowLocation[2] + moPos.XL1) / 2;
        //        moPos.YL1 = (GV.NowLocation[3] + moPos.YL1) / 2;
        //        moPos.ZL1 = (GV.NowLocation[11] + moPos.ZL1) / 2;
        //        //moPos.X1 = (GV.NowLocation[0] + moPos.X1) / 2;
        //        moPos.Y1 = (GV.NowLocation[1] + moPos.Y1) / 2;
        //    }

        //    // double dYR1 = (moPos.YR1 - GV.AppSettingParm.RightYExposure) * ZoomInfo.RightMotorStepsPerumY + (moPos.Y1 - GV.AppSettingParm.StageYExposure) * ZoomInfo.StageMotorStepsPerumY;
        //    // double dYL1 = (moPos.YL1 - GV.AppSettingParm.LeftYExposure) * ZoomInfo.LeftMotorStepsPerumY + (moPos.Y1 - GV.AppSettingParm.StageYExposure) * ZoomInfo.StageMotorStepsPerumY;
        //    // double dY1 = (dYR1 + dYL1) / 2;
        //    // int Y1 = (int)(dY1 / ZoomInfo.StageMotorStepsPerumY);
        //    // int YL1 = (int)((dYL1 - dY1) / ZoomInfo.LeftMotorStepsPerumY);
        //    // int YR1 = (int)((dYR1 - dY1) / ZoomInfo.RightMotorStepsPerumY);
        //    // Y1 += GV.AppSettingParm.StageYExposure;
        //    // YL1 += GV.AppSettingParm.LeftYExposure;
        //    // YR1 += GV.AppSettingParm.RightYExposure;

        //    double dYR1 = moPos.YR1 * ZoomInfo.RightMotorStepsPerumY + moPos.Y1 * ZoomInfo.StageMotorStepsPerumY;
        //    double dYL1 = moPos.YL1 * ZoomInfo.LeftMotorStepsPerumY + moPos.Y1 * ZoomInfo.StageMotorStepsPerumY;
        //    double dY1 = (dYR1 + dYL1) / 2;
        //    int Y1 = (int)(dY1 / ZoomInfo.StageMotorStepsPerumY);
        //    int YL1 = (int)((dYL1 - dY1) / ZoomInfo.LeftMotorStepsPerumY);
        //    int YR1 = (int)((dYR1 - dY1) / ZoomInfo.RightMotorStepsPerumY);

        //    // int Y1 = (int)(dY1 / ZoomInfo.StageMotorStepsPerumY) + (int)((GV.AppSettingParm.LeftYExposure * ZoomInfo.LeftMotorStepsPerumY) / ZoomInfo.StageMotorStepsPerumY);
        //    // int YL1 = (int)((dYL1 - dY1) / ZoomInfo.LeftMotorStepsPerumY) - GV.AppSettingParm.LeftYExposure;
        //    // int YR1 = (int)((dYR1 - dY1) / ZoomInfo.RightMotorStepsPerumY) - GV.AppSettingParm.LeftYExposure;

        //    double dXR1 = moPos.XR1 * ZoomInfo.RightMotorStepsPerumX;
        //    double dXL1 = moPos.XL1 * ZoomInfo.LeftMotorStepsPerumX;
        //    double dX1 = (Math.Abs(dXR1) + Math.Abs(dXL1)) / 2;
        //    dXR1 += dX1;
        //    dXL1 -= dX1;
        //    int XL1 = moPos.XL1 - (int)(dXL1 / ZoomInfo.LeftMotorStepsPerumX);
        //    int XR1 = moPos.XR1 - (int)(dXR1 / (ZoomInfo.RightMotorStepsPerumX + 0.001));
        //    //int X1 = moPos.X1 - (int)(dXR1 / ZoomInfo.StageMotorStepsPerumX);

        //    disPos.Y1 = Y1;
        //    disPos.YR1 = YR1;
        //    disPos.YL1 = YL1;
        //    //disPos.X1 = X1;
        //    disPos.XR1 = XR1;
        //    disPos.XL1 = XL1;

        //    moPos.Y1 = Y1;
        //    moPos.YR1 = YR1;
        //    moPos.YL1 = YL1;
        //    //moPos.X1 = X1;
        //    moPos.XR1 = XR1;
        //    moPos.XL1 = XL1;
        //}

        public void Save()
        {
            this.Valid = true;
            XmlSerializer ser = new XmlSerializer(typeof(Recipe));
            using (TextWriter writer = new StreamWriter(FileName))
            {
                ser.Serialize(writer, this);
                writer.Flush();
            }
        }

        //public void WriteToPlc(MPlcCommunicationS mcp)
        //{
        //    // mcp.WriteData16(16500, (short)this.RecipeNumber);
        //    // mcp.WriteData16(16501, numArray);
        //    // mcp.WriteData16(16581, (short)this.ExposureEnergy);
        //    // mcp.WriteData32(mcp.iAlignDistance, AlignDistance);
        //    // mcp.WriteData32(16585, new int[] { this.ExposureDistance });
        //    // mcp.WriteData16(16587, (short)(this.ContactMode + 1));
        //    // mcp.WriteData16(16589, (short)this.WaferSize);
        //}

        public void CopyFrom(Recipe Nrecipe)
        {
            this.Valid = Nrecipe.Valid;
            this.FileName = Nrecipe.FileName;
            this.RecipeNumber = Nrecipe.RecipeNumber;
            this.RecipeName = Nrecipe.RecipeName;
            this.ExposureType = Nrecipe.ExposureType;
            this.ExposureValue = Nrecipe.ExposureValue;
            this.ExposureTime = Nrecipe.ExposureTime;
            this.ExposurePower = Nrecipe.ExposurePower;
            this.ExposurePowerRadio = Nrecipe.ExposurePowerRadio;
            this.AlignDistance = Nrecipe.AlignDistance;
            this.ExposureDistance = Nrecipe.ExposureDistance;
            this.ContactMode = Nrecipe.ContactMode;
            this.WaferSize = Nrecipe.WaferSize;
            this.OCRMinScore = Nrecipe.OCRMinScore;
            this.SpeedPercent = Nrecipe.SpeedPercent;
            this.JobName = Nrecipe.JobName;
            this.PAWaferType = Nrecipe.PAWaferType;
            this.WaferMaterial = Nrecipe.WaferMaterial;
            this.Overpressure = Nrecipe.Overpressure;
            this.Gapcompensate = Nrecipe.Gapcompensate;
            this.WaferZHigh = Nrecipe.WaferZHigh;
            this.WaferLeveling = Nrecipe.WaferLeveling;
            this.HardContact = Nrecipe.HardContact;
            this.WaferThinkness = Nrecipe.WaferThinkness;
            this.WaferRotate = Nrecipe.WaferRotate;
            this.PAOCROrientation = Nrecipe.PAOCROrientation;
            this.PAFinalWaferOrientation = Nrecipe.PAFinalWaferOrientation;
            this.PA2FinalWaferOrientation = Nrecipe.PA2FinalWaferOrientation;
            this.ByPassLX = Nrecipe.ByPassLX;

            for (int i = 0; i < InX6Posi.Length; i++)
                this.InX6Posi[i] = Nrecipe.InX6Posi[i];

            this.IAutoAdjustRecipe = Nrecipe.IAutoAdjustRecipe;
            this.OCRTryCount = Nrecipe.OCRTryCount;
            this.MaskChangeCount = Nrecipe.MaskChangeCount;
            this.ContactConfirm = Nrecipe.ContactConfirm;
            this.ContactShiftCompensation = Nrecipe.ContactShiftCompensation;
            this.AlignMode = Nrecipe.AlignMode;
            this.Flag = Nrecipe.Flag;
            this.RobotArmSelect = Nrecipe.RobotArmSelect;
            this.MaskBarcode = Nrecipe.MaskBarcode;
            this.MaskCassetteSize = Nrecipe.MaskCassetteSize;

            //this.RobotRecipe = new RobotRecipe();
            //this.MaskRobotRecipe = new RobotRecipe();
            //this.PARecipe = new PARecipe();
            //this.UpperMPosition = new UpperMotorPosition();
            //this.UpperMLowResPosition = new UpperMotorPosition();
            //this.UpperWECPos = new UpperArmWECPosition();
            //this.UpperLowResWECPos = new UpperArmWECPosition();
            //this.LowerMPosition = new LowerMotorPosition();
            //this.LowerWECPos = new LowerArmWECPosition();
            //this.QRCodePosition = new UpperMotorPosition();
            //this.MotorPositionPos.XL1 = Nrecipe.MotorPositionPos.XL1;
            //this.MotorPositionPos.YL1 = Nrecipe.MotorPositionPos.YL1;
            //this.MotorPositionPos.XL1 = Nrecipe.MotorPositionPos.XL1;
            //this.MotorPositionPos.XR1 = Nrecipe.MotorPositionPos.XR1;
            //this.MotorPositionPos.YR1 = Nrecipe.MotorPositionPos.YR1;
            //this.MotorPositionPos.ZR1 = Nrecipe.MotorPositionPos.ZR1;
            //this.MotorPositionPos.XL6 = Nrecipe.MotorPositionPos.XL6;
            //this.MotorPositionPos.YL6 = Nrecipe.MotorPositionPos.YL6;
            //this.MotorPositionPos.ZL6 = Nrecipe.MotorPositionPos.ZL6;
            //this.MotorPositionPos.XR6 = Nrecipe.MotorPositionPos.XR6;
            //this.MotorPositionPos.YR6 = Nrecipe.MotorPositionPos.YR6;
            //this.MotorPositionPos.ZR6 = Nrecipe.MotorPositionPos.ZR6;
            //this.MotorPositionPos.Y1 = Nrecipe.MotorPositionPos.Y1;
            //this.MotorPositionPos.Y6 = Nrecipe.MotorPositionPos.Y6;
            //this.disPos = new DisPosition();
            //this.MaskPosition.X = Nrecipe.MaskPosition.X;
            //this.MaskPosition.Y1 = Nrecipe.MaskPosition.Y1;
            //this.MaskPosition.Y2 = Nrecipe.MaskPosition.Y2;
            //this.WaferPosition = new XYYPosition();
            //this.AlignC = new AlignCondition();
        }
    }

    public class UpperMotorPosition
    {
        public double Y;

        public double XL;

        public double YL;

        public double ZL;

        public double XR;

        public double YR;

        public double ZR;

        public double[] Positions
        {
            get
            {
                return new double[] { Y, XL, YL, ZL, XR, YR, ZR };
            }
        }

        public UpperMotorPosition()
        {
        }
    }

    public class UpperArmWECPosition
    {
        public double Z;

        public double X;

        public double Y;

        public double A;

        public UpperArmWECPosition()
        {
        }
    }

    public class MotorPosition
    {

        public int Y1;

        public int Y6;

        public int XL1;

        public int YL1;

        public int ZL1;

        public int XR1;

        public int YR1;

        public int ZR1;

        public int XL6;

        public int YL6;

        public int ZL6;

        public int XR6;

        public int YR6;

        public int ZR6;

        public int[] Positions
        {
            get
            {
                return new int[] { XL1, YL1, XL1, XR1, YR1, ZR1, XL6, YL6, ZL6, XR6, YR6, ZR6, Y1, Y6 };
            }
        }

        public MotorPosition()
        {
        }
    }

    public class DisPosition
    {
        public int Y1;

        public int Y6;

        public int XL1;

        public int YL1;

        public int ZL1;

        public int XR1;

        public int YR1;

        public int ZR1;

        public int XL6;

        public int YL6;

        public int ZL6;

        public int XR6;

        public int YR6;

        public int ZR6;

        public int[] Positions
        {
            get
            {
                return new int[] { XL1, YL1, ZL1, XR1, YR1, ZR1, XL6, YL6, ZL6, XR6, YR6, ZR6, Y1, Y6 };
            }
        }

        public DisPosition()
        {
            //XL6 = GV.ZoomLensInfo.iLHLdistanceX;
            //YL6 = GV.ZoomLensInfo.iLHLdistanceY;
            //ZL6 = GV.ZoomLensInfo.iLHLdistanceZ;
            //XL6 = GV.ZoomLensInfo.iRHLdistanceX;
            //YL6 = GV.ZoomLensInfo.iRHLdistanceY;
            //ZR6 = GV.ZoomLensInfo.iRHLdistanceZ;
            //Y6 = GV.ZoomLensInfo.iHLdistanceY;
        }
    }

    public class LowerMotorPosition
    {
        public double XL;

        public double YL;

        public double ZL;

        public double XR;

        public double YR;

        public double ZR;

        public double[] Positions
        {
            get
            {
                return new double[] { this.XL, this.YL, this.ZL, this.XR, this.YR, this.ZR };
            }
        }

        public LowerMotorPosition()
        {
        }
    }

    public class LowerArmWECPosition
    {
        public double Z;

        public double X;

        public double Y;

        public double A;

        public LowerArmWECPosition()
        {
        }
    }

    public class XYYPosition
    {
        public int X;

        public int Y1;

        public int Y2;

        public int[] Positions
        {
            get
            {
                return new int[] { X, Y1, Y2 };
            }
        }

        public XYYPosition()
        {

        }
    }

    public class RobotRecipe
    {
        public int SpeedPercent = 2;

        public MappingParams MPL = new MappingParams();
        public MappingParams MPR = new MappingParams();

        //public void WriteMappingParamsToRobot(WaferRobot robot, string station)
        //{
        //    //MappingParams mp = null;
        //    //if ("ej".Contains(station)) mp = MPL;
        //    //if ("EJ".Contains(station)) mp = MPR;

        //    //if (mp.T == 0 && mp.R == 0 && mp.Z == 0) return;
        //    //int[] pos = { mp.T, mp.R, mp.Z };
        //    //robot.WriteMappingStationPos(station, pos);
        //    //int[] osp = { mp.Thicknes, mp.Speed, mp.LowPos, mp.HighPos };
        //    //robot.WriteMappingStationTSLH(station, osp);
        //    //robot.SaveToFlash();
        //}
    }

    public class MappingParams
    {
        public int T = 0;
        public int R = 0;
        public int Z = 0;
        public int Thicknes = 0;
        public int Speed = 0;
        public int Pitch = 0;
        public int LowPos = 0;
        public int HighPos = 0;
    }

    public class PARecipe
    {
        public double PAFinalWaferOrientation = 180;
        public double PA2FinalWaferOrientation = 180;
    }
}
