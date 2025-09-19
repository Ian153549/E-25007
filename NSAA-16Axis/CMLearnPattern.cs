using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using MRLibrary;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.IO;

namespace NSAA_16Axis
{
    public partial class CMLearnPattern : UserControl
    {
        public CMLearnPattern()
        {
            InitializeComponent();
            GV.LeftUpWindowOnLearnPage = LeftLearnPatternVideoWindow;
            GV.RightUpWindowOnLearnPage = RightLearnPatternVideoWindow;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);       
               
        }
        private void CMLearnPattern_Load(object sender, EventArgs e)
        {
            CheckLevel();
            gbLMask.Enabled = false;
            gbLWafer.Enabled = false;
            gbRMask.Enabled = false;
            gbRWafer.Enabled = false;
        }

        public void DrawMatchPosition()
        {
            OpenCV3MatchUMat matcher = new OpenCV3MatchUMat("DrawMatchPosition");

            while(!GV.AppEnding)
            {
                while (GV.TabOption == GV.Tab.Learn)
                {
                    if (radioButtonLowMagnification.Checked)
                    {
                        LeftLearnPatternVideoWindow.MaskMp = matcher.Match(GV.LeftUpCam.Grab(), GV.LeftLowMaskMat[GV.NowRecipeNumber]);
                        LeftLearnPatternVideoWindow.WaferMp = matcher.Match(GV.LeftUpCam.Grab(), GV.LeftLowWaferMat[GV.NowRecipeNumber]);
                        RightLearnPatternVideoWindow.MaskMp = matcher.Match(GV.RightUpCam.Grab(), GV.RightLowMaskMat[GV.NowRecipeNumber]);
                        RightLearnPatternVideoWindow.WaferMp = matcher.Match(GV.RightUpCam.Grab(), GV.RightLowWaferMat[GV.NowRecipeNumber]);
                    }
                    if (radioButtonHighMagnification.Checked)
                    {                     
                        LeftLearnPatternVideoWindow.MaskMp = matcher.Match(GV.LeftUpCam.Grab(), GV.LeftHighMaskMat[GV.NowRecipeNumber]);
                        LeftLearnPatternVideoWindow.WaferMp = matcher.Match(GV.LeftUpCam.Grab(), GV.LeftHighWaferMat[GV.NowRecipeNumber]);
                        RightLearnPatternVideoWindow.MaskMp = matcher.Match(GV.RightUpCam.Grab(), GV.RightHighMaskMat[GV.NowRecipeNumber]);
                        RightLearnPatternVideoWindow.WaferMp = matcher.Match(GV.RightUpCam.Grab(), GV.RightHighWaferMat[GV.NowRecipeNumber]);
                    }
                    Thread.Sleep(200);
                }
                Thread.Sleep(1000);
            } 
        }

        public void UpdateUI()
        {
            lbRecipeNumber.Text = "Recipe Number : " + GV.NowRecipeNumber.ToString();
            pbLMaskImage.Image = null; pbLWaferImage.Image = null; pbRMaskImage.Image = null; pbRWaferImage.Image = null;
            if (radioButtonLowMagnification.Checked)
            {
                if (GV.LeftLowMaskMat[GV.NowRecipeNumber] != null)
                    pbLMaskImage.Image = GV.LeftLowMaskMat[GV.NowRecipeNumber].ToBitmap();
                if (GV.LeftLowWaferMat[GV.NowRecipeNumber] != null)
                    pbLWaferImage.Image = GV.LeftLowWaferMat[GV.NowRecipeNumber].ToBitmap();
                if (GV.RightLowMaskMat[GV.NowRecipeNumber] != null)
                    pbRMaskImage.Image = GV.RightLowMaskMat[GV.NowRecipeNumber].ToBitmap();
                if (GV.RightLowWaferMat[GV.NowRecipeNumber] != null)
                    pbRWaferImage.Image = GV.RightLowWaferMat[GV.NowRecipeNumber].ToBitmap();
            }

            if (radioButtonHighMagnification.Checked)
            {
                if (GV.LeftHighMaskMat[GV.NowRecipeNumber] != null)
                    pbLMaskImage.Image = GV.LeftHighMaskMat[GV.NowRecipeNumber].ToBitmap();
                if (GV.LeftHighWaferMat[GV.NowRecipeNumber] != null)
                    pbLWaferImage.Image = GV.LeftHighWaferMat[GV.NowRecipeNumber].ToBitmap();
                if (GV.RightHighMaskMat[GV.NowRecipeNumber] != null)
                    pbRMaskImage.Image = GV.RightHighMaskMat[GV.NowRecipeNumber].ToBitmap();
                if (GV.RightHighWaferMat[GV.NowRecipeNumber] != null)
                    pbRWaferImage.Image = GV.RightHighWaferMat[GV.NowRecipeNumber].ToBitmap();
            }

            

        }


        public void CheckLevel()
        {
            Invoke((MethodInvoker)delegate ()
            {
                UpdateUI();
                if (GV.UserLevel == GV.User.Operator)
                {
                    gbLWafer.Enabled = false; gbLMask.Enabled = false; gbRWafer.Enabled = false; gbRMask.Enabled = false;
                    numericUpDownMeasurePatternDistanceSteps.Enabled = false;
                    groupBoxMeasurePatternDistance.Enabled = false;
                    numericUpDownMeasurePatternDistanceSteps.Enabled = false;
                }
                else
                {
                    gbLWafer.Enabled = true; gbLMask.Enabled = true; gbRWafer.Enabled = true; gbRMask.Enabled = true;
                    numericUpDownMeasurePatternDistanceSteps.Enabled = true;
                    groupBoxMeasurePatternDistance.Enabled = true;
                    numericUpDownMeasurePatternDistanceSteps.Enabled = true;
                }

                if (GV.MachineStatus == GV.Status.Align)
                    groupBoxMagnification.Enabled = false;
                if (GV.MachineStatus == GV.Status.Standby)
                    groupBoxMagnification.Enabled = true;
            });
            
        }


        private string GetTemplateFileName(string patternAndSide)
        {
            string fileName = "Templates//" + patternAndSide + GV.NowRecipeNumber.ToString() + ".bmp";
            return fileName;
        }

        private OpenCV3MatchUMat.AlignAlgorithm Algoritm(int a)
        {
            if (a == 0)
                return OpenCV3MatchUMat.AlignAlgorithm.TemplateMatch;
            else if (a == 1)
                return OpenCV3MatchUMat.AlignAlgorithm.EdgeMatch;
            else
                return OpenCV3MatchUMat.AlignAlgorithm.TemplateMatch;

        }

        private void btLWaferTemplateSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Sure to save left wafer template ?", "LMaskTemplateSave", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            if (radioButtonLowMagnification.Checked)
            {
                //GV.LeftLowWaferMat[GV.NowRecipeNumber] = new Mat(GV.LeftUpCam.Grab(), LeftLearnPatternVideoWindow.GetWaferRectangle());
                //pbLWaferImage.Image = GV.LeftLowWaferMat[GV.NowRecipeNumber].ToBitmap();
                //Cv2.ImWrite(GetTemplateFileName("LLW"), GV.LeftLowWaferMat[GV.NowRecipeNumber]);
                //GV.NowAlignCondition.LLWaferAlgorithm = Algoritm(cbLWaferAlgorithm.SelectedIndex);
            }
            else if (radioButtonHighMagnification.Checked)
            {
                //GV.LeftHighWaferMat[GV.NowRecipeNumber] = new Mat(GV.LeftUpCam.Grab(), LeftLearnPatternVideoWindow.GetWaferRectangle());
                //pbLWaferImage.Image = GV.LeftHighWaferMat[GV.NowRecipeNumber].ToBitmap();
                //Cv2.ImWrite(GetTemplateFileName("LHW"), GV.LeftHighWaferMat[GV.NowRecipeNumber]);
                //GV.NowAlignCondition.LHWaferAlgorithm = Algoritm(cbLWaferAlgorithm.SelectedIndex);
            }
            else
            {
                MessageBox.Show("Select magnification first.", "LWaferTemplateSave", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            GM.WriteAlignConditionsListToXml("AlignConditions.xml", GV.NowAlignCondition, GV.NowRecipeNumber);

        }

        private void btLMaskTemplateSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Sure to save left mask template ?", "LMaskTemplateSave", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            if (radioButtonLowMagnification.Checked)
            {
                //GV.LeftLowMaskMat[GV.NowRecipeNumber] = new Mat(GV.LeftUpCam.Grab(), LeftLearnPatternVideoWindow.GetMaskRectangle());
                //pbLMaskImage.Image = GV.LeftLowMaskMat[GV.NowRecipeNumber].ToBitmap();
                //CvInvoke.Imwrite(GetTemplateFileName("LLM"), GV.LeftLowMaskMat[GV.NowRecipeNumber]);
                //GV.NowAlignCondition.LLMaskAlgorithm = Algoritm(cbLMaskAlgorithm.SelectedIndex);
            }
            else if (radioButtonHighMagnification.Checked)
            {
                //GV.LeftHighMaskMat[GV.NowRecipeNumber] = new Mat(GV.LeftUpCam.Grab(), LeftLearnPatternVideoWindow.GetMaskRectangle());
                //pbLMaskImage.Image = GV.LeftHighMaskMat[GV.NowRecipeNumber].ToBitmap();
                //Cv2.ImWrite(GetTemplateFileName("LHM"), GV.LeftHighMaskMat[GV.NowRecipeNumber]);
                //GV.NowAlignCondition.LHMaskAlgorithm = Algoritm(cbLMaskAlgorithm.SelectedIndex);
            }
            else
            {
                MessageBox.Show("Select magnification first.", "LMaskTemplateSave", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            GM.WriteAlignConditionsListToXml("AlignConditions.xml", GV.NowAlignCondition, GV.NowRecipeNumber);
        }

        private void btRWaferTempalteSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Sure to save right wafer template ?", "RWaferTemplateSave", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            if (radioButtonLowMagnification.Checked)
            {
                //GV.RightLowWaferMat[GV.NowRecipeNumber] = new Mat(GV.RightUpCam.Grab(), RightLearnPatternVideoWindow.GetWaferRectangle());
                //pbRWaferImage.Image = GV.RightLowWaferMat[GV.NowRecipeNumber].ToBitmap();
                //CvInvoke.Imwrite(GetTemplateFileName("RLW"), GV.RightLowWaferMat[GV.NowRecipeNumber]);
                //GV.NowAlignCondition.RLWaferAlgorithm = Algoritm(cbRWaferAlgorithm.SelectedIndex);
            }
            else if (radioButtonHighMagnification.Checked)
            {
                //GV.RightHighWaferMat[GV.NowRecipeNumber] = new Mat(GV.RightUpCam.Grab(), RightLearnPatternVideoWindow.GetWaferRectangle());
                //pbRWaferImage.Image = GV.RightHighWaferMat[GV.NowRecipeNumber].ToBitmap();
                //CvInvoke.Imwrite(GetTemplateFileName("RHW"), GV.RightHighWaferMat[GV.NowRecipeNumber]);
                //GV.NowAlignCondition.RHWaferAlgorithm = Algoritm(cbRWaferAlgorithm.SelectedIndex);
            }
            else
            {
                MessageBox.Show("Select magnification first.", "RWaferTemplateSave", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
           
            GM.WriteAlignConditionsListToXml("AlignConditions.xml", GV.NowAlignCondition, GV.NowRecipeNumber);
        }

        private void btRMaskTemplateSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Sure to save right mask template ?", "RMaskTemplateSave", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            if (radioButtonLowMagnification.Checked)
            {
                //GV.RightLowMaskMat[GV.NowRecipeNumber] = new Mat(GV.RightUpCam.Grab(), RightLearnPatternVideoWindow.GetMaskRectangle());
                //pbRMaskImage.Image = GV.RightLowMaskMat[GV.NowRecipeNumber].ToBitmap();
                //CvInvoke.Imwrite(GetTemplateFileName("RLM"), GV.RightLowMaskMat[GV.NowRecipeNumber]);
                //GV.NowAlignCondition.RLMaskAlgorithm = Algoritm(cbRMaskAlgorithm.SelectedIndex);
            }
            else if (radioButtonHighMagnification.Checked)
            {
                //GV.RightHighMaskMat[GV.NowRecipeNumber] = new Mat(GV.RightUpCam.Grab(), RightLearnPatternVideoWindow.GetMaskRectangle());
                //pbRMaskImage.Image = GV.RightHighMaskMat[GV.NowRecipeNumber].ToBitmap();
                //CvInvoke.Imwrite(GetTemplateFileName("RHM"), GV.RightHighMaskMat[GV.NowRecipeNumber]);
                //GV.NowAlignCondition.RHMaskAlgorithm = Algoritm(cbRMaskAlgorithm.SelectedIndex);
            }
            else
            {
                MessageBox.Show("Select magnification first.", "RMaskTemplateSave", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            
            GM.WriteAlignConditionsListToXml("AlignConditions.xml", GV.NowAlignCondition, GV.NowRecipeNumber);
        }

        private void btLMaskLocationUp_Click(object sender, EventArgs e)
        {
            LeftLearnPatternVideoWindow.MaskMove(0, -1);
        }

        private void btLMaskLocationDown_Click(object sender, EventArgs e)
        {
            LeftLearnPatternVideoWindow.MaskMove(0, 1);
        }

        private void btLMaskLocationLeft_Click(object sender, EventArgs e)
        {
            LeftLearnPatternVideoWindow.MaskMove(-1, 0);
        }

        private void btLMaskLocationRight_Click(object sender, EventArgs e)
        {
            LeftLearnPatternVideoWindow.MaskMove(1, 0);
        }

        private void btLWaferLocationUp_Click(object sender, EventArgs e)
        {
            LeftLearnPatternVideoWindow.WaferMove(0,-1);
        }

        private void btLWaferLocationDown_Click(object sender, EventArgs e)
        {
            LeftLearnPatternVideoWindow.WaferMove(0, 1);
        }

        private void btLWaferLocationLeft_Click(object sender, EventArgs e)
        {
            LeftLearnPatternVideoWindow.WaferMove(-1, 0);
        }

        private void btLWaferLocationRight_Click(object sender, EventArgs e)
        {
            LeftLearnPatternVideoWindow.WaferMove(1, 0);
        }

        private void btRMaskLocationUp_Click(object sender, EventArgs e)
        {
            RightLearnPatternVideoWindow.MaskMove(0, -1);
        }

        private void btRMaskLocationDown_Click(object sender, EventArgs e)
        {
            RightLearnPatternVideoWindow.MaskMove(0, 1);
        }

        private void btRMaskLocationLeft_Click(object sender, EventArgs e)
        {
            RightLearnPatternVideoWindow.MaskMove(-1, 0);
        }

        private void btRMaskLocationRight_Click(object sender, EventArgs e)
        {
            RightLearnPatternVideoWindow.MaskMove(1, 0);
        }

        private void btRWaferLocationUp_Click(object sender, EventArgs e)
        {
            RightLearnPatternVideoWindow.WaferMove(0, -1);
        }

        private void btRWaferLocationDown_Click(object sender, EventArgs e)
        {
            RightLearnPatternVideoWindow.WaferMove(0, 1);
        }

        private void btRWaferLocationLeft_Click(object sender, EventArgs e)
        {
            RightLearnPatternVideoWindow.WaferMove(-1, 0);
        }

        private void btRWaferLocationRight_Click(object sender, EventArgs e)
        {
            RightLearnPatternVideoWindow.WaferMove(1, 0);
        }

        private async void RadioButtonLowMagnification_Click(object sender, EventArgs e)
        {
            try
            {
                groupBoxMagnification.Enabled = false;
                gbLMask.Enabled = true;
                gbLWafer.Enabled = true;
                gbRMask.Enabled = true;
                gbRWafer.Enabled = true;
                UpdateUI();
                await Task.Run(() =>
                {
                    Task leftLensZoomIn = Task.Factory.StartNew(() =>
                    {
                        GV.Light.ChangeBrightness("left", GV.NowAlignCondition.LeftBrightness[GV.NowAlignCondition.AlignLowMagnification]);
                        //GV.LeftZoomLens.MoveGoto(GV.ZoomLensInfo.LeftMagnificationMotorSteps, GV.NowAlignCondition.AlignLowMagnification);
                    });

                    Task rightLensZoomIn = Task.Factory.StartNew(() =>
                    {
                        GV.Light.ChangeBrightness("right", GV.NowAlignCondition.RightBrightness[GV.NowAlignCondition.AlignLowMagnification]);
                        //GV.RightZoomLens.MoveGoto(GV.ZoomLensInfo.RightMagnificationMotorSteps, GV.NowAlignCondition.AlignLowMagnification);
                    });
                    leftLensZoomIn.Wait(100000);
                    rightLensZoomIn.Wait(100000);
                });
                groupBoxMagnification.Enabled = true;

                cbLMaskAlgorithm.SelectedIndex = (int)GV.NowAlignCondition.LLMaskAlgorithm;
                cbLWaferAlgorithm.SelectedIndex = (int)GV.NowAlignCondition.LLWaferAlgorithm;
                cbRMaskAlgorithm.SelectedIndex = (int)GV.NowAlignCondition.RLMaskAlgorithm;
                cbRWaferAlgorithm.SelectedIndex = (int)GV.NowAlignCondition.RLWaferAlgorithm;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void RadioButtonHighMagnification_Click(object sender, EventArgs e)
        {
            try
            {
                groupBoxMagnification.Enabled = false;
                gbLMask.Enabled = true;
                gbLWafer.Enabled = true;
                gbRMask.Enabled = true;
                gbRWafer.Enabled = true;
                UpdateUI();
                await Task.Run(() =>
                {
                    Task leftLensZoomIn = Task.Factory.StartNew(() =>
                    {
                        GV.Light.ChangeBrightness("left", GV.NowAlignCondition.LeftBrightness[GV.NowAlignCondition.AlignHighMagnification]);
                        GV.LeftZoomLens.MoveGoto(GV.ZoomLensInfo.LeftMagnificationMotorSteps, GV.NowAlignCondition.AlignHighMagnification);
                    });

                    Task rightLensZoomIn = Task.Factory.StartNew(() =>
                    {
                        GV.Light.ChangeBrightness("right", GV.NowAlignCondition.RightBrightness[GV.NowAlignCondition.AlignHighMagnification]);
                        GV.RightZoomLens.MoveGoto(GV.ZoomLensInfo.RightMagnificationMotorSteps, GV.NowAlignCondition.AlignHighMagnification);
                    });
                    leftLensZoomIn.Wait(100000);
                    rightLensZoomIn.Wait(100000);
                });
                groupBoxMagnification.Enabled = true;

                cbLMaskAlgorithm.SelectedIndex = (int)GV.NowAlignCondition.LHMaskAlgorithm;
                cbLWaferAlgorithm.SelectedIndex = (int)GV.NowAlignCondition.LHWaferAlgorithm;
                cbRMaskAlgorithm.SelectedIndex = (int)GV.NowAlignCondition.RHMaskAlgorithm;
                cbRWaferAlgorithm.SelectedIndex = (int)GV.NowAlignCondition.RHWaferAlgorithm;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void UnableMag()
        {
            groupBoxMagnification.Enabled = false;
        }
        public void EnableMag()
        {
            groupBoxMagnification.Enabled = true;
        }

        //private void ButtonCalMarkDistance_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (!(GV.Plc.Send("RD MR31302") == "1\r\n" && GV.Plc.Send("RD MR31400") == "0\r\n"))
        //        {
        //            throw new MRException("Please measure in the correct status");
        //        }                    
        //        MatchPosition LWaferStart = new MatchPosition();
        //        MatchPosition LWaferEnd = new MatchPosition();
        //        MatchPosition RWaferStart = new MatchPosition();
        //        MatchPosition RWaferEnd = new MatchPosition();

        //        // OpenCV3MatchUMat matcher = new OpenCV3MatchUMat();

        //        int rotateSteps = (int)numericUpDownMeasurePatternDistanceSteps.Value;
        //        double rotateDegree = rotateSteps / GV.ZoomLensInfo.MotorStepsPerPixelDegree;

        //        if (GV.LeftUpCam.Grab() == null || GV.RightUpCam.Grab() == null)
        //            throw new MRException("Can't find pattern.");
        //        if (rotateSteps <= 0)
        //            throw new MRException("Please type the correct steps.");

        //        //LWaferStart = GV.matcher.MatchWithOption(GV.LeftUpCam.Grab(), GV.LeftLowWaferMat[GV.NowRecipeNumber], GV.NowAlignCondition.LLWaferAlgorithm);
        //        //RWaferStart = GV.matcher.MatchWithOption(GV.RightUpCam.Grab(), GV.RightLowWaferMat[GV.NowRecipeNumber], GV.NowAlignCondition.RLWaferAlgorithm);
        //        GV.matcher.MatchWithOption(GV.LeftUpCam.Grab(), GV.LeftLowWaferMat[GV.NowRecipeNumber], GV.LeftLowWaferMask[GV.NowRecipeNumber], GV.NowAlignCondition.LLWaferAlgorithm, ref LWaferStart);
        //        GV.matcher.MatchWithOption(GV.RightUpCam.Grab(), GV.RightLowWaferMat[GV.NowRecipeNumber], GV.RightLowWaferMask[GV.NowRecipeNumber], GV.NowAlignCondition.RLWaferAlgorithm, ref RWaferStart);

        //        //Move
        //        GV.Plc.RecipeModeXyyTableMove(0, 0, rotateSteps); 
        //        // LWaferEnd = GV.matcher.MatchWithOption(GV.LeftUpCam.Grab(), GV.LeftLowWaferMat[GV.NowRecipeNumber], GV.NowAlignCondition.LLWaferAlgorithm);
        //        // RWaferEnd = GV.matcher.MatchWithOption(GV.RightUpCam.Grab(), GV.RightLowWaferMat[GV.NowRecipeNumber], GV.NowAlignCondition.RLWaferAlgorithm);
        //        GV.matcher.MatchWithOption(GV.LeftUpCam.Grab(), GV.LeftLowWaferMat[GV.NowRecipeNumber], GV.LeftLowWaferMask[GV.NowRecipeNumber], GV.NowAlignCondition.LLWaferAlgorithm, ref LWaferEnd);
        //        GV.matcher.MatchWithOption(GV.RightUpCam.Grab(), GV.RightLowWaferMat[GV.NowRecipeNumber], GV.RightLowWaferMask[GV.NowRecipeNumber], GV.NowAlignCondition.RLWaferAlgorithm, ref RWaferEnd);

        //        //Heron's Foprmula   A2 +B2 -C2 = 2ABcosC
        //        double c = Math.Sqrt(Math.Pow(LWaferEnd.X - LWaferStart.X, 2) + Math.Pow(LWaferEnd.Y - LWaferStart.Y, 2)) + Math.Sqrt(Math.Pow(RWaferEnd.X - RWaferStart.X, 2) + Math.Pow(RWaferEnd.Y - RWaferStart.Y, 2));
        //        double cosc = Math.Cos(rotateDegree / 180 * Math.PI);
        //        double markDistance = Math.Sqrt((c * c) / ((1 - cosc) * 2)) * GV.ZoomLensInfo.LeftUmPerPixelX[GV.LeftZoomLens.Magnification];

        //        if (MessageBox.Show("Mark distance = " + (markDistance / 1000).ToString("F2") + " um , sure to save", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        //            return;

        //        // GV.NowAlignCondition.PatternCenterDistanceUm = (int)Math.Round(markDistance);
        //        GM.WriteAlignConditionsListToXml("AlignConditions.xml", GV.NowAlignCondition, GV.NowRecipeNumber);
        //    }
        //    catch(Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());
        //    }
        //}

        private void buttonCreateLWaferPatternMask_Click(object sender, EventArgs e)
        {
            if (!radioButtonHighMagnification.Checked && !radioButtonLowMagnification.Checked)
                return;
            Mat mat = radioButtonHighMagnification.Checked ? GV.LeftHighMaskMat[GV.NowRecipeNumber] : GV.LeftLowMaskMat[GV.NowRecipeNumber];
            DialogCreatePatternMask d = new DialogCreatePatternMask(mat);
            d.ShowDialog();
        }

        private void buttonCreateLMaskPatternMask_Click(object sender, EventArgs e)
        {

        }

        private void buttonCreateRWaferPatternMask_Click(object sender, EventArgs e)
        {

        }

        private void buttonCreateRMaskPatternMask_Click(object sender, EventArgs e)
        {

        }

        private void radioButtonLowMagnification_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
