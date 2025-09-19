using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NSAA_16Axis
{
    public partial class CCDPad : Form
    {
        Thread thWaitRun;
        readonly Thread thUpdateValue;

        int isBigY = 0;
        int isRightX = 0;
        int isRightY = 0;
        int isLeftX = 0;
        int isLeftY = 0;
        int isChuckX = 0;
        int isChuckY1 = 0;
        int isChuckY2 = 0;
        int isChuckZ = 0;
        int isDownLeftX = 0;
        int isDownLeftY = 0;
        int isDownLeftZ = 0;
        int isDownRightX = 0;
        int isDownRightY = 0;
        int isDownRightZ = 0;
        int isUpDown = 1;

        bool bWaitRun = true;
        // bool bWaitStop = true;

        public static int iUpDown = 1;
        public static int iUpDownChuck = 1;
        public static int iMaskWafer = 1;

        public static int[] iRMemory = new int[50];
        public static int[] iWMemory = new int[GV.Plc.iWCount];
        public static int iRCount = GV.Plc.iRCount;
        public static int iWCount = GV.Plc.iWCount;

        public CCDPad()
        {
            InitializeComponent();

            comboBoxBigYS.SelectedIndex = 0;
            comboBoxBigYB.SelectedIndex = 0;
            comboBoxLeftXB.SelectedIndex = 0;
            comboBoxLeftXS.SelectedIndex = 0;
            comboBoxLeftYB.SelectedIndex = 0;
            comboBoxLeftYS.SelectedIndex = 0;
            comboBoxRightXB.SelectedIndex = 0;
            comboBoxRightXS.SelectedIndex = 0;
            comboBoxRightYB.SelectedIndex = 0;
            comboBoxRightYS.SelectedIndex = 0;
            comboBoxChuckXB.SelectedIndex = 0;
            comboBoxChuckXS.SelectedIndex = 0;
            comboBoxChuckYB.SelectedIndex = 0;
            comboBoxChuckYS.SelectedIndex = 0;
            comboBoxChuckZS.SelectedIndex = 0;
            comboBoxChuckZB.SelectedIndex = 0;
            comboBoxRotationS.SelectedIndex = 0;
            comboBoxRotationB.SelectedIndex = 0;

            Array.Clear(iRMemory, 0, 50);
            Array.Clear(iWMemory, 0, iWCount);

            thUpdateValue = new Thread(UpdateValueMethod);
            thUpdateValue.Start();
        }

        public void WaitRun()
        {
            int iRet = 0;
            int iRAdd = 0;
            int iWaitTime = 0;
            int iAdd = 0;
            bWaitRun = true;

            GV.Plc.WriteData32(GV.Plc.iWMemoryAddress, iWMemory);

            if (GV.Plc.ReadMemory(GV.Plc.iUpCCDStart))
            {
                iAdd = GV.Plc.iUpCCDAlignStart;
                iRAdd = GV.Plc.iUpCCDEnd;
            } else if (GV.Plc.ReadMemory(GV.Plc.iChuckStart))
            {
                iAdd = GV.Plc.iBackCCDAlignStart;
                iRAdd = GV.Plc.iChuckEnd;
            }
            else if (GV.Plc.ReadMemory(GV.Plc.iDownCCDStart))
            {
                iAdd = GV.Plc.iBackMaskStart;
                iRAdd = GV.Plc.iDownCCDEnd;
            } else if (GV.Plc.ReadMemory(GV.Plc.iCCDAdjust))
            {
                iAdd = GV.Plc.iCCDAdjMoveStart;
                iRAdd = GV.Plc.iCCDAdjOk;
            }

            GV.Plc.SendM(iAdd, 1);

            while (bWaitRun)
            {
                iRet = GV.Plc.ReciveM(iRAdd);

                if (iRet == 1)
                {
                    bWaitRun = false;
                }
                else
                {
                    Thread.Sleep(250);
                    iWaitTime += 1;
                    if (iWaitTime > 40)
                    {
                        bWaitRun = false;
                    }
                }
            }
            try
            {
                Invoke(new Action(() => AllEnable()));
            } catch(Exception) 
            { 
            }
        }

        public void UpdateValueMethod()
        {

            bool bwait = true;
            int iBigY = 0;
            int iRightX = 0;
            int iRightY = 0;
            int iLeftX = 0;
            int iLeftY = 0;
            int iChuckX = 0;
            int iChuckY1 = 0;
            int iChuckY2 = 0;
            int iChuckZ = 0;
            int iDownLeftX = 0;
            int iDownLeftY = 0;
            int iDownLeftZ = 0;
            int iDownRightX = 0;
            int iDownRightY = 0;
            int iDownRightZ = 0;

            try
            {
                while (bwait)
                {
                    int iUpdate = 0;

                    int[] TempArry = GV.Plc.ReadData32(GV.Plc.iRMemoryAddress, GV.Plc.iRCount);
                    Array.Copy(TempArry, iRMemory, iRCount);
                    iBigY = TempArry[GV.Plc.iiDUpBigY];
                    iRightX = TempArry[GV.Plc.iiDUpRightX];
                    iRightY = TempArry[GV.Plc.iiDUpRightY];
                    iLeftX = TempArry[GV.Plc.iiDUpLeftX];
                    iLeftY = TempArry[GV.Plc.iiDUpLeftY];
                    iChuckX = TempArry[GV.Plc.iiDChuckX];
                    iChuckY1 = TempArry[GV.Plc.iiDChuckY1];
                    iChuckY2 = TempArry[GV.Plc.iiDChuckY2];
                    iChuckZ = TempArry[GV.Plc.iiDChuckZ];
                    iDownLeftX = TempArry[GV.Plc.iiDDownLeftX];
                    iDownLeftY = TempArry[GV.Plc.iiDDownLeftY];
                    iDownLeftZ = TempArry[GV.Plc.iiDDownLeftZ];
                    iDownRightX = TempArry[GV.Plc.iiDDownRightX];
                    iDownRightY = TempArry[GV.Plc.iiDDownRightY];
                    iDownRightZ = TempArry[GV.Plc.iiDDownRightZ];

                    if ((iBigY != 0) && (iBigY != isBigY))
                    {
                        isBigY = iBigY;
                        iUpdate = 1;
                    }

                    if ((iRightX != 0) && (iRightX != isRightX))
                    {
                        isRightX = iRightX;
                        iUpdate = 1;
                    }

                    if ((iRightY != 0) && (iRightY != isRightY))
                    {
                        isRightY = iRightY;
                        iUpdate = 1;
                    }

                    if ((iLeftX != 0) && (iLeftX != isLeftX))
                    {
                        isLeftX = iLeftX;
                        iUpdate = 1;
                    }

                    if ((iLeftY != 0) && (iLeftY != isLeftY))
                    {
                        isLeftY = iLeftY;
                        iUpdate = 1;
                    }

                    if ((iChuckX != 0) && (iChuckX != isChuckX))
                    {
                        isChuckX = iChuckX;
                        iUpdate = 1;
                    }

                    if ((iChuckY1 != 0) && (iChuckY1 != isChuckY1))
                    {
                        isChuckY1 = iChuckY1;
                        iUpdate = 1;
                    }

                    if ((iChuckY2 != 0) && (iChuckY2 != isChuckY2))
                    {
                        isChuckY2 = iChuckY2;
                        iUpdate = 1;
                    }

                    if ((iChuckZ != 0) && (iChuckZ != isChuckZ))
                    {
                        isChuckZ = iChuckZ;
                        iUpdate = 1;
                    }

                    if ((iDownLeftX != 0) && (iDownLeftX != isDownLeftX))
                    {
                        isDownLeftX = iDownLeftX;
                        iUpdate = 1;
                    }

                    if ((iDownLeftY != 0) && (iDownLeftY != isDownLeftY))
                    {
                        isDownLeftY = iDownLeftY;
                        iUpdate = 1;
                    }

                    if ((iDownLeftZ != 0) && (iDownLeftZ != isDownLeftZ))
                    {
                        isDownLeftZ = iDownLeftZ;
                        iUpdate = 1;
                    }

                    if ((iDownRightX != 0) && (iDownRightX != isDownRightX))
                    {
                        isDownRightX = iDownRightX;
                        iUpdate = 1;
                    }

                    if ((iDownRightY != 0) && (iDownRightY != isDownRightY))
                    {
                        isDownRightY = iDownRightY;
                        iUpdate = 1;
                    }

                    if ((iDownRightZ != 0) && (iDownRightZ != isDownRightZ))
                    {
                        isDownRightZ = iDownRightZ;
                        iUpdate = 1;
                    }

                    if (iUpDown != isUpDown)
                    {
                        isUpDown = iUpDown;
                        iUpdate = 1;
                    }

                    if (iUpdate == 1)
                    {
                        if (iUpDown == 1)
                        {
                            BigY.Invoke((Action)delegate
                            {
                                BigY.Text = "大 Y : " + iBigY.ToString();
                                labelRightX.Text = "右 X : " + iRightX.ToString();
                                labelRightY.Text = "右 Y : " + iRightY.ToString();
                                labelLeftX.Text = "左 X : " + iLeftX.ToString();
                                labelLeftY.Text = "左 Y : " + iLeftY.ToString();
                                labelChuckX.Text = "Chuck X: " + iChuckX.ToString();
                                labelChuckY1.Text = "Chuck Y1: " + iChuckY1.ToString();
                                labelChuckY2.Text = "Chuck Y2: " + iChuckY2.ToString();
                                labelChuckZ.Text = "Chuck Z: " + iChuckZ.ToString();
                            });

                        }
                        else if (iUpDown == 2)
                        {
                            lbLeftZ.Invoke((Action)delegate
                            {
                                lbLeftZ.Text = "左 Z :" + iDownLeftZ.ToString();
                                lbRightZ.Text = "右 Z :" + iDownRightZ.ToString();
                                labelRightX.Text = "右 X : " + iDownRightX.ToString();
                                labelRightY.Text = "右 Y : " + iDownRightY.ToString();
                                labelLeftX.Text = "左 X : " + iDownLeftX.ToString();
                                labelLeftY.Text = "左 Y : " + iDownLeftY.ToString();
                                labelChuckX.Text = "Chuck X: " + iChuckX.ToString();
                                labelChuckY1.Text = "Chuck Y1: " + iChuckY1.ToString();
                                labelChuckY2.Text = "Chuck Y2: " + iChuckY2.ToString();
                                labelChuckZ.Text = "Chuck Z: " + iChuckZ.ToString();
                            });
                        }
                        
                    }
                    Thread.Sleep(250);
                }
            }
            catch (Exception)
            {

            }
        } 

        private void BigYSmallDown_Click(object sender, EventArgs e)
        {
            Moving();

            int iRet = iWMemory[GV.Plc.iiDSBigY];

            Int32.TryParse(comboBoxBigYS.SelectedItem.ToString(), out int iMoving);

            iWMemory[GV.Plc.iiDSBigY] += iMoving;

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        public void Moving()
        {
            // int[] TempArray = GV.Plc.ReadData32(GV.Plc.iRMemoryAddress, GV.Plc.iRCount);
            
            Array.Copy(iRMemory, iWMemory, iRCount);

            ReLoad.Enabled = false;
            BigYSmallUp.Enabled = false;
            BigYSmallDown.Enabled = false;
            BigYLargeUp.Enabled = false;
            BigYLargeDown.Enabled = false;
            buttonRightYDownB.Enabled = false;
            buttonRightXRightB.Enabled = false;
            buttonRightYUpB.Enabled = false;
            buttonRightXLeftB.Enabled = false;
            buttonRightYDownS.Enabled = false;
            buttonRightXRightS.Enabled = false;
            buttonRightYUpS.Enabled = false;
            buttonRightXLeftS.Enabled = false;
            buttonChuckYDownB.Enabled = false;
            buttonChuckXRightB.Enabled = false;
            buttonChuckYUpB.Enabled = false;
            buttonChuckXLeftB.Enabled = false;
            buttonChuckYDownS.Enabled = false;
            buttonChuckXRightS.Enabled = false;
            buttonChuckYUpS.Enabled = false;
            buttonChuckXLeftS.Enabled = false;
            buttonLeftYDownB.Enabled = false;
            buttonLeftXRightB.Enabled = false;
            buttonLeftYUpB.Enabled = false;
            buttonLeftXLeftB.Enabled = false;
            buttonLeftYDownS.Enabled = false;
            buttonLeftXRightS.Enabled = false;
            buttonLeftYUpS.Enabled = false;
            buttonLeftXLeftS.Enabled = false;
            comboBoxBigYS.Enabled = false;
            comboBoxBigYB.Enabled = false;
            buttonStop.Enabled = true;
        }

        public void AllEnable()
        {
            iUpDownChuck = iUpDown;
            buttonStop.Enabled = false;
            BigYSmallUp.Enabled = true;
            BigYSmallDown.Enabled = true;
            BigYLargeUp.Enabled = true;
            BigYLargeDown.Enabled = true;
            buttonRightYDownB.Enabled = true;
            buttonRightXRightB.Enabled = true;
            buttonRightYUpB.Enabled = true;
            buttonRightXLeftB.Enabled = true;
            buttonRightYDownS.Enabled = true;
            buttonRightXRightS.Enabled = true;
            buttonRightYUpS.Enabled = true;
            buttonRightXLeftS.Enabled = true;
            buttonChuckYDownB.Enabled = true;
            buttonChuckXRightB.Enabled = true;
            buttonChuckYUpB.Enabled = true;
            buttonChuckXLeftB.Enabled = true;
            buttonChuckYDownS.Enabled = true;
            buttonChuckXRightS.Enabled = true;
            buttonChuckYUpS.Enabled = true;
            buttonChuckXLeftS.Enabled = true;
            buttonLeftYDownB.Enabled = true;
            buttonLeftXRightB.Enabled = true;
            buttonLeftYUpB.Enabled = true;
            buttonLeftXLeftB.Enabled = true;
            buttonLeftYDownS.Enabled = true;
            buttonLeftXRightS.Enabled = true;
            buttonLeftYUpS.Enabled = true;
            buttonLeftXLeftS.Enabled = true;
            comboBoxBigYS.Enabled = true;
            comboBoxBigYB.Enabled = true;
            ReLoad.Enabled = true;

            GV.Plc.SendM(GV.Plc.iPadMoveUp, 0);
            GV.Plc.SendM(GV.Plc.iPadMoveDown, 0);
            GV.Plc.SendM(GV.Plc.iPadMoveChuck, 0);
        }

        private void BigYSmallUp_Click(object sender, EventArgs e)
        {
            Moving();


            int iRet = iWMemory[GV.Plc.iiDSBigY];

            Int32.TryParse(comboBoxBigYS.SelectedItem.ToString(), out int iMoving);

            iWMemory[GV.Plc.iiDSBigY] -= iMoving;

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void BigYLargeDown_Click(object sender, EventArgs e)
        {
            Moving();

            int iRet = iWMemory[GV.Plc.iiDSBigY];

            Int32.TryParse(comboBoxBigYB.SelectedItem.ToString(), out int iMoving);

            iWMemory[GV.Plc.iiDSBigY] += iMoving;

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void BigYLargeUp_Click(object sender, EventArgs e)
        {
            Moving();

            int iRet = iWMemory[GV.Plc.iiDSBigY];

            Int32.TryParse(comboBoxBigYB.SelectedItem.ToString(), out int iMoving);

            iWMemory[GV.Plc.iiDSBigY] -= iMoving;

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void ButtonStop_Click(object sender, EventArgs e)
        {
            GV.Plc.MoveStop();

            Invoke(new Action(() => AllEnable()));
        }

        private void ButtonLeftYUpS_Click(object sender, EventArgs e)
        {
            Moving();

            int iRet = iWMemory[GV.Plc.iiDSUpLeftY];

            Int32.TryParse(comboBoxLeftYS.SelectedItem.ToString(), out int iMoving);

            if (iUpDown == 1)
            {
                iWMemory[GV.Plc.iiDSUpLeftY] -= iMoving;
            }
            else if (iUpDown == 2)
            {
                iWMemory[GV.Plc.iiDDownLeftY] -= iMoving;
            }

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void ButtonLeftYDownS_Click(object sender, EventArgs e)
        {
            Moving();

            int iRet = iWMemory[GV.Plc.iiDSUpLeftY];

            Int32.TryParse(comboBoxLeftYS.SelectedItem.ToString(), out int iMoving);

            if (iUpDown == 1)
            {
                iWMemory[GV.Plc.iiDSUpLeftY] += iMoving;
            }
            else if (iUpDown == 2)
            {
                iWMemory[GV.Plc.iiDSDownLeftY] += iMoving;
            }

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void ButtonLeftYUpB_Click(object sender, EventArgs e)
        {
            Moving();


            int iRet = iWMemory[GV.Plc.iiDSUpLeftY];

            Int32.TryParse(comboBoxLeftYB.SelectedItem.ToString(), out int iMoving);

            if (iUpDown == 1)
            {
                iWMemory[GV.Plc.iiDSUpLeftY] -= iMoving;
            }
            else if (iUpDown == 2)
            {
                iWMemory[GV.Plc.iiDSDownLeftY] -= iMoving;
            }  

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void ButtonLeftYDownB_Click(object sender, EventArgs e)
        {
            Moving();

            int iRet = iWMemory[GV.Plc.iiDSUpLeftY];

            Int32.TryParse(comboBoxLeftYB.SelectedItem.ToString(), out int iMoving);

            if (iUpDown == 1)
            {
                iWMemory[GV.Plc.iiDSUpLeftY] += iMoving;
            }
            else if (iUpDown == 2)
            {
                iWMemory[GV.Plc.iiDSDownLeftY] += iMoving;
            }
            
            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void ButtonLeftXLeftS_Click(object sender, EventArgs e)
        {
            Moving();

            int iRet = iWMemory[GV.Plc.iiDSUpLeftX];

            Int32.TryParse(comboBoxLeftXS.SelectedItem.ToString(), out int iMoving);

            if (iUpDown == 1)
            {
                iWMemory[GV.Plc.iiDSUpLeftX] += iMoving;
            }
            else if (iUpDown == 2)
            {
                iWMemory[GV.Plc.iiDDownLeftX] += iMoving;
            }

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void ButtonLeftXRightS_Click(object sender, EventArgs e)
        {
            Moving();


            int iRet = iWMemory[GV.Plc.iiDSUpLeftX];

            Int32.TryParse(comboBoxLeftXS.SelectedItem.ToString(), out int iMoving);

            if (iUpDown == 1)
            {
                iWMemory[GV.Plc.iiDSUpLeftX] -= iMoving;
            } else if (iUpDown == 2)
            {
                iWMemory[GV.Plc.iiDSDownLeftX] -= iMoving;
            }
            
            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void ButtonLeftXRightB_Click(object sender, EventArgs e)
        {
            Moving();

            int iRet = iWMemory[GV.Plc.iiDSUpLeftX];

            Int32.TryParse(comboBoxLeftXB.SelectedItem.ToString(), out int iMoving);

            if (iUpDown == 1)
            {
                iWMemory[GV.Plc.iiDSUpLeftX] -= iMoving;
            }
            else if (iUpDown == 2)
            {
                iWMemory[GV.Plc.iiDSDownLeftX] -= iMoving;
            }
            
            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();

        }

        private void ButtonLeftXLeftB_Click(object sender, EventArgs e)
        {
            Moving();

            int iRet = iWMemory[GV.Plc.iiDSUpLeftX];

            Int32.TryParse(comboBoxLeftXB.SelectedItem.ToString(), out int iMoving);

            if (iUpDown == 1)
            {
                iWMemory[GV.Plc.iiDSUpLeftX] += iMoving;
            }
            else if (iUpDown == 2)
            {
                iWMemory[GV.Plc.iiDSDownLeftX] += iMoving;
            }    

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void ButtonChuckZUpS_Click(object sender, EventArgs e)
        {
            Moving();

            int iRet = GV.Plc.ReciveD(GV.Plc.iDSSChuckZ);

            Int32.TryParse(comboBoxChuckZS.SelectedItem.ToString(), out int iMoving);

            GV.Plc.McSend(GV.Plc.iDSChuckZ, iRet - iMoving);

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void ButtonChuckYDownS_Click(object sender, EventArgs e)
        {
            Moving();

            // iUpDownChuck = 3;

            int iRet = iWMemory[GV.Plc.iiDSChuckY1];

            Int32.TryParse(comboBoxChuckYS.SelectedItem.ToString(), out int iMoving);

            iWMemory[GV.Plc.iiDSChuckY1] += iMoving;
            iWMemory[GV.Plc.iiDSChuckY2] += iMoving;
            //iWMemory[GV.Plc.iiDSChuckXSpeed] = 10;
            //iWMemory[GV.Plc.iiDSChuckY1Speed] = 300;
            //iWMemory[GV.Plc.iiDSChuckY2Speed] = 300;

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();

            // GV.Plc.AlignXyyTableMovePad(0, iMoving, 0);

            // AllEnable();
        }

        private void ButtonChuckZUpB_Click(object sender, EventArgs e)
        {
            Moving();


            int iRet = GV.Plc.ReciveD(GV.Plc.iDSSChuckZ);

            Int32.TryParse(comboBoxChuckZB.SelectedItem.ToString(), out int iMoving);

            GV.Plc.McSend(GV.Plc.iDSChuckZ, iRet - iMoving);

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void ButtonChuckZDownB_Click(object sender, EventArgs e)
        {
            Moving();

            int iRet = GV.Plc.ReciveD(GV.Plc.iDSSChuckZ);

            Int32.TryParse(comboBoxChuckZB.SelectedItem.ToString(), out int iMoving);

            GV.Plc.McSend(GV.Plc.iDSChuckZ, iRet + iMoving);

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void ButtonRightYUpB_Click(object sender, EventArgs e)
        {
            Moving();

            int iRet = iWMemory[GV.Plc.iiDSUpRightY];

            Int32.TryParse(comboBoxRightYB.SelectedItem.ToString(), out int iMoving);

            if (iUpDown == 1)
            {
                iWMemory[GV.Plc.iiDSUpRightY] -= iMoving;
            }
            else if (iUpDown == 2)
            {
                iWMemory[GV.Plc.iiDSDownRightY] -= iMoving;
            }        

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void ButtonRightYDownB_Click(object sender, EventArgs e)
        {
            Moving();

            int iRet = iWMemory[GV.Plc.iiDSUpRightY];

            Int32.TryParse(comboBoxRightYB.SelectedItem.ToString(), out int iMoving);

            if (iUpDown == 1)
            {
                iWMemory[GV.Plc.iiDSUpRightY] += iMoving;
            }
            else if (iUpDown == 2)
            {
                iWMemory[GV.Plc.iiDSDownRightY] += iMoving;
            }

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void ButtonRightYUpS_Click(object sender, EventArgs e)
        {
            Moving();

            int iRet = iWMemory[GV.Plc.iiDSUpRightY];

            Int32.TryParse(comboBoxRightYS.SelectedItem.ToString(), out int iMoving);

            if (iUpDown == 1)
            {
                iWMemory[GV.Plc.iiDSUpRightY] -= iMoving;
            }
            else if (iUpDown == 2)
            {
                iWMemory[GV.Plc.iiDSDownRightY] -= iMoving;
            }

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void ButtonRightYDownS_Click(object sender, EventArgs e)
        {
            Moving();

            int iRet = iWMemory[GV.Plc.iiDSUpRightY];

            Int32.TryParse(comboBoxRightYS.SelectedItem.ToString(), out int iMoving);

            if (iUpDown == 1)
            {
                iWMemory[GV.Plc.iiDSUpRightY] += iMoving;
            }
            else if (iUpDown == 2)
            {
                iWMemory[GV.Plc.iiDSDownRightY] += iMoving;
            }

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void ButtonRightXRightB_Click(object sender, EventArgs e)
        {
            Moving();

            int iRet = iWMemory[GV.Plc.iiDSUpRightX];

            Int32.TryParse(comboBoxRightXB.SelectedItem.ToString(), out int iMoving);

            if (iUpDown == 1)
            {
                iWMemory[GV.Plc.iiDSUpRightX] += iMoving;
            }
            else if (iUpDown == 2)
            {
                iWMemory[GV.Plc.iiDSDownRightX] += iMoving;
            }
           
            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void ButtonRightXLeftB_Click(object sender, EventArgs e)
        {
            Moving();

            int iRet = iWMemory[GV.Plc.iiDSUpRightX];

            Int32.TryParse(comboBoxRightXB.SelectedItem.ToString(), out int iMoving);

            if (iUpDown == 1)
            {
                iWMemory[GV.Plc.iiDSUpRightX] -= iMoving;
            }
            else if (iUpDown == 2)
            {
                iWMemory[GV.Plc.iiDSDownRightX] -= iMoving;
            }

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void ButtonRightXRightS_Click(object sender, EventArgs e)
        {
            Moving();

            int iRet = iWMemory[GV.Plc.iiDSUpRightX];

            Int32.TryParse(comboBoxRightXS.SelectedItem.ToString(), out int iMoving);

            if (iUpDown == 1)
            {
                iWMemory[GV.Plc.iiDSUpRightX] += iMoving;
            }
            else if (iUpDown == 2)
            {
                iWMemory[GV.Plc.iiDSDownRightX] += iMoving;
            }  

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void ButtonRightXLeftS_Click(object sender, EventArgs e)
        {
            Moving();

            int iRet = iWMemory[GV.Plc.iiDSUpRightX];

            Int32.TryParse(comboBoxRightXS.SelectedItem.ToString(), out int iMoving);

            if (iUpDown == 1)
            {
                iWMemory[GV.Plc.iiDSUpRightX] -= iMoving;
            }
            else if (iUpDown == 2)
            {
                iWMemory[GV.Plc.iiDSDownRightX] -= iMoving;
            }

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void ButtonChuckZDownS_Click(object sender, EventArgs e)
        {
            Moving();

            int iRet = GV.Plc.ReciveD(GV.Plc.iDSSChuckZ);

            Int32.TryParse(comboBoxChuckZS.SelectedItem.ToString(), out int iMoving);

            GV.Plc.McSend(GV.Plc.iDSChuckZ, iRet + iMoving);

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void ButtonChuckYUpB_Click(object sender, EventArgs e)
        {
            Moving();

            // iUpDownChuck = 3;

            int iRet = iWMemory[GV.Plc.iiDSChuckY1];

            Int32.TryParse(comboBoxChuckYB.SelectedItem.ToString(), out int iMoving);

            iWMemory[GV.Plc.iiDSChuckY1] -= iMoving;
            iWMemory[GV.Plc.iiDSChuckY2] -= iMoving;
            //iWMemory[GV.Plc.iiDSChuckXSpeed] = 10;
            //iWMemory[GV.Plc.iiDSChuckY1Speed] = 300;
            //iWMemory[GV.Plc.iiDSChuckY2Speed] = 300;

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();

            // GV.Plc.AlignXyyTableMovePad(0, -iMoving, 0);

            // AllEnable();
        }

        private void ButtonChuckXLeftB_Click(object sender, EventArgs e)
        {
            Moving();

            // iUpDownChuck = 3;

            int iRet = iWMemory[GV.Plc.iiDSChuckX];

            Int32.TryParse(comboBoxChuckXB.SelectedItem.ToString(), out int iMoving);

            iWMemory[GV.Plc.iiDSChuckX] -= iMoving;
            //iWMemory[GV.Plc.iiDSChuckXSpeed] = 300;
            //iWMemory[GV.Plc.iiDSChuckY1Speed] = 10;
            //iWMemory[GV.Plc.iiDSChuckY2Speed] = 10;

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();

            // GV.Plc.AlignXyyTableMovePad(-iMoving, 0, 0);

            // AllEnable();
        }

        private void ButtonChuckXLeftS_Click(object sender, EventArgs e)
        {
            Moving();

            // iUpDownChuck = 3;

            int iRet = iWMemory[GV.Plc.iiDSChuckX];

            Int32.TryParse(comboBoxChuckXS.SelectedItem.ToString(), out int iMoving);

            iWMemory[GV.Plc.iiDSChuckX] -= iMoving;
            //iWMemory[GV.Plc.iiDSChuckXSpeed] = 300;
            //iWMemory[GV.Plc.iiDSChuckY1Speed] = 10;
            //iWMemory[GV.Plc.iiDSChuckY2Speed] = 10;

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void ButtonChuckXRightS_Click(object sender, EventArgs e)
        {
            Moving();

            // iUpDownChuck = 3;

            int iRet = iWMemory[GV.Plc.iiDSChuckX];

            Int32.TryParse(comboBoxChuckXS.SelectedItem.ToString(), out int iMoving);

            iWMemory[GV.Plc.iiDSChuckX] += iMoving;
            //iWMemory[GV.Plc.iiDSChuckXSpeed] = 300;
            //iWMemory[GV.Plc.iiDSChuckY1Speed] = 10;
            //iWMemory[GV.Plc.iiDSChuckY2Speed] = 10;

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();

            // GV.Plc.AlignXyyTableMovePad(iMoving, 0, 0);

            // AllEnable();

        }

        private void ButtonChuckXRightB_Click(object sender, EventArgs e)
        {
            Moving();


            // iUpDownChuck = 3;

            int iRet = iWMemory[GV.Plc.iiDSChuckX];

            Int32.TryParse(comboBoxChuckXB.SelectedItem.ToString(), out int iMoving);

            iWMemory[GV.Plc.iiDSChuckX] += iMoving;
            //iWMemory[GV.Plc.iiDSChuckXSpeed] = 300;
            //iWMemory[GV.Plc.iiDSChuckY1Speed] = 10;
            //iWMemory[GV.Plc.iiDSChuckY2Speed] = 10;

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();

            // GV.Plc.AlignXyyTableMovePad(iMoving, 0, 0);

            // AllEnable();
        }

        private void ReLoad_Click(object sender, EventArgs e)
        {
            int iBigY = GV.Plc.ReciveD(GV.Plc.iDSSMaskY);
            int iRightX = GV.Plc.ReciveD(GV.Plc.iDSSUpRightX);
            int iRightY = GV.Plc.ReciveD(GV.Plc.iDSSUpRightY);
            int iLeftX = GV.Plc.ReciveD(GV.Plc.iDSSUpLeftX);
            int iLeftY = GV.Plc.ReciveD(GV.Plc.iDSSUpLeftY);
            int iChuckX = GV.Plc.ReciveD(GV.Plc.iDSSChuckX);
            int iChuckY1 = GV.Plc.ReciveD(GV.Plc.iDSSChuckY1);
            int iChuckY2 = GV.Plc.ReciveD(GV.Plc.iDSSChuckY2);
            int iChuckZ = GV.Plc.ReciveD(GV.Plc.iDSSChuckZ);

            GV.Plc.McSend(GV.Plc.iDSBigY, iBigY);
            GV.Plc.McSend(GV.Plc.iDSUpLeftX, iLeftX);
            GV.Plc.McSend(GV.Plc.iDSUpLeftY, iLeftY);
            GV.Plc.McSend(GV.Plc.iDSUpRightX, iRightX);
            GV.Plc.McSend(GV.Plc.iDSUpRightY, iRightY);
            GV.Plc.McSend(GV.Plc.iDSChuckX, iChuckX);
            GV.Plc.McSend(GV.Plc.iDSChuckY1, iChuckY1);
            GV.Plc.McSend(GV.Plc.iDSChuckY2, iChuckY2);
            GV.Plc.McSend(GV.Plc.iDSChuckZ, iChuckZ);
        }

        private void CCDPad_Activated(object sender, EventArgs e)
        {
            int iOk = GV.Plc.ReciveM(GV.Plc.iBarcodeChageOK);
            if (iOk == 1)
            {
                GV.Plc.SendM(GV.Plc.iPadWork, 1);
            } else
            {
                GV.Plc.SendM(GV.Plc.iPadWork, 0);
            }
        }

        private void CCDPad_Deactivate(object sender, EventArgs e)
        {
            GV.Plc.SendM(GV.Plc.iPadWork, 0);
        }

        private void ButtonChuckYUpS_Click(object sender, EventArgs e)
        {
            Moving();

            // iUpDownChuck = 3;

            int iRet = iWMemory[GV.Plc.iiDSChuckY1];

            Int32.TryParse(comboBoxChuckYS.SelectedItem.ToString(), out int iMoving);

            iWMemory[GV.Plc.iiDSChuckY1] -= iMoving;
            iWMemory[GV.Plc.iiDSChuckY2] -= iMoving;
            //iWMemory[GV.Plc.iiDSChuckXSpeed] = 10;
            //iWMemory[GV.Plc.iiDSChuckY1Speed] = 300;
            //iWMemory[GV.Plc.iiDSChuckY2Speed] = 300;

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();

            // GV.Plc.AlignXyyTableMovePad(0, -iMoving, 0);

            // AllEnable();
        }

        private void ButtonChuckYDownB_Click(object sender, EventArgs e)
        {
            Moving();

            // iUpDownChuck = 3;

            int iRet = iWMemory[GV.Plc.iiDSChuckY1];

            Int32.TryParse(comboBoxChuckYB.SelectedItem.ToString(), out int iMoving);

            iWMemory[GV.Plc.iiDSChuckY1] += iMoving;
            iWMemory[GV.Plc.iiDSChuckY2] += iMoving;
            //iWMemory[GV.Plc.iiDSChuckXSpeed] = 10;
            //iWMemory[GV.Plc.iiDSChuckY1Speed] = 300;
            //iWMemory[GV.Plc.iiDSChuckY2Speed] = 300;

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void ButtonCWRotationS_Click(object sender, EventArgs e)
        {
            Moving();

            // iUpDownChuck = 3;

            int iRet = iWMemory[GV.Plc.iiDSChuckY1];

            Int32.TryParse(comboBoxRotationS.SelectedItem.ToString(), out int iMoving);

            iWMemory[GV.Plc.iiDSChuckX] -= iMoving;
            iWMemory[GV.Plc.iiDSChuckY1] += iMoving;
            iWMemory[GV.Plc.iiDSChuckY2] -= iMoving;
            //iWMemory[GV.Plc.iiDSChuckXSpeed] = 300;
            //iWMemory[GV.Plc.iiDSChuckY1Speed] = 300;
            //iWMemory[GV.Plc.iiDSChuckY2Speed] = 300;

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void ButtonWCRotationS_Click(object sender, EventArgs e)
        {
            Moving();

            // iUpDownChuck = 3;

            int iRet = iWMemory[GV.Plc.iiDSChuckY1];

            Int32.TryParse(comboBoxRotationS.SelectedItem.ToString(), out int iMoving);

            iWMemory[GV.Plc.iiDSChuckX] += iMoving;
            iWMemory[GV.Plc.iiDSChuckY1] -= iMoving;
            iWMemory[GV.Plc.iiDSChuckY2] += iMoving;
            //iWMemory[GV.Plc.iiDSChuckXSpeed] = 300;
            //iWMemory[GV.Plc.iiDSChuckY1Speed] = 300;
            //iWMemory[GV.Plc.iiDSChuckY2Speed] = 300;

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void ButtonCWRotationB_Click(object sender, EventArgs e)
        {
            Moving();

            // iUpDownChuck = 3;

            int iRet = iWMemory[GV.Plc.iiDSChuckY1];

            Int32.TryParse(comboBoxRotationB.SelectedItem.ToString(), out int iMoving);

            iWMemory[GV.Plc.iiDSChuckX] -= iMoving;
            iWMemory[GV.Plc.iiDSChuckY1] += iMoving;
            iWMemory[GV.Plc.iiDSChuckY2] -= iMoving;
            //iWMemory[GV.Plc.iiDSChuckXSpeed] = 300;
            //iWMemory[GV.Plc.iiDSChuckY1Speed] = 300;
            //iWMemory[GV.Plc.iiDSChuckY2Speed] = 300;

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void ButtonWCRotationB_Click(object sender, EventArgs e)
        {
            Moving();

            // iUpDownChuck = 3;

            int iRet = iWMemory[GV.Plc.iiDSChuckY1];

            Int32.TryParse(comboBoxRotationB.SelectedItem.ToString(), out int iMoving);

            iWMemory[GV.Plc.iiDSChuckX] += iMoving;
            iWMemory[GV.Plc.iiDSChuckY1] -= iMoving;
            iWMemory[GV.Plc.iiDSChuckY2] += iMoving;
            //iWMemory[GV.Plc.iiDSChuckXSpeed] = 300;
            //iWMemory[GV.Plc.iiDSChuckY1Speed] = 300;
            //iWMemory[GV.Plc.iiDSChuckY2Speed] = 300;

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void UpCCD_Click(object sender, EventArgs e)
        {
            iUpDown = 1;
            ChangeUpDown();
        }

        private void DownCCD_Click(object sender, EventArgs e)
        {
            iUpDown = 2;
            ChangeUpDown();
        }

        private void ChangeUpDown()
        {
            bool bUp = false;
            bool bDown = false;
            int iLb = 0;
            int iRb = 0;

            if (iUpDown == 1)
            {
                GV.skView = 0;
                GV.LeftBackCam.Freeze();
                GV.RightBackCam.Freeze();
                GV.LeftUpCam.SetWindow(GV.LeftUpWindowOnAlignPage);
                GV.RightUpCam.SetWindow(GV.RightUpWindowOnAlignPage);
                GV.LeftUpCam.Live();
                GV.RightUpCam.Live();
                iLb = GV.NowAlignCondition.LeftBrightness[GV.NowAlignCondition.AlignLowMagnification];
                iRb = GV.NowAlignCondition.RightBrightness[GV.NowAlignCondition.AlignLowMagnification];
                GV.Light.ChangeBrightness(0, iRb, iLb);
                GV.Light.ChangeBrightness(1, 0, 0);
                bUp = true;
            } else if (iUpDown == 2)
            {
                GV.skView = 1;
                GV.LeftUpCam.Freeze();
                GV.RightUpCam.Freeze();
                GV.LeftBackCam.SetWindow(GV.LeftDownWindowOnAlignPage);
                GV.RightBackCam.SetWindow(GV.RightDownWindowOnAlignPage);
                GV.LeftBackCam.Live();
                GV.RightBackCam.Live();
                if (iMaskWafer == 1)
                {
                    iLb = GV.NowAlignCondition.LBMaskBright;
                    iRb = GV.NowAlignCondition.RBMaskBright;
                } else if (iMaskWafer == 2)
                {
                    iLb = GV.NowAlignCondition.LBWaferBright;
                    iRb = GV.NowAlignCondition.RBWaferBright;
                }
                GV.Light.ChangeBrightness(1, iRb, iLb);
                GV.Light.ChangeBrightness(0, 0, 0);
                bDown = true;
            }

             UpCCD.Visible = bDown;
            lbLeftZ.Visible = bDown;
            lbRightZ.Visible = bDown;
            labelLeftZ.Visible = bDown;
            labelRightZ.Visible = bDown;
            cbLeftZ.Visible = bDown;
            cbRightZ.Visible = bDown;
            nUDLeftZ1.Visible = bDown;
            nUDLeftZ2.Visible = bDown;
            nUDRightZ1.Visible = bDown;
            nUDRightZ2.Visible = bDown;
            btLeftZSDown.Visible = bDown;
            btLeftZSUp.Visible = bDown;
            btLeftZBDown.Visible = bDown;
            btLeftZBUp.Visible = bDown;
            btRightZSDown.Visible = bDown;
            btRightZSUp.Visible = bDown;
            btRightZBDown.Visible = bDown;
            btRightZBUp.Visible = bDown;

            DownCCD.Visible = bUp;
            BigY.Visible = bUp;
            labelBigY.Visible = bUp;
            BigYSmallDown.Visible = bUp;
            BigYSmallUp.Visible = bUp;
            BigYLargeDown.Visible = bUp;
            BigYLargeUp.Visible = bUp;
            nUDBigY1.Visible = bUp;
            nUDBigY2.Visible = bUp;
            cbBigY.Visible = bUp;

        }

        private void MoveRecipe_Click(object sender, EventArgs e)
        {
            /*
            GV.NowRecipe = GM.GetRecipeInfo(1);

            GV.Plc.McSend(GV.Plc.iDSUpLeftX, GV.NowRecipe.iUpCCDLeftX);
            GV.Plc.McSend(GV.Plc.iDSUpLeftY, GV.NowRecipe.iUpCCDLeftY);
            GV.Plc.McSend(GV.Plc.iDSUpRightX, GV.NowRecipe.iUpCCDRightX);
            GV.Plc.McSend(GV.Plc.iDSUpRightY, GV.NowRecipe.iUpCCDRightY);
            GV.Plc.McSend(GV.Plc.iDSUpBY, GV.NowRecipe.iUpCCDBigY);

            GV.Plc.McSend(GV.Plc.iDSDownLeftX, GV.NowRecipe.iBackCCDLeftX);
            GV.Plc.McSend(GV.Plc.iDSDownLeftY, GV.NowRecipe.iBackCCDLeftY);
            GV.Plc.McSend(GV.Plc.iDSDownLeftZ, GV.NowRecipe.iBackCCDLeftZ);
            GV.Plc.McSend(GV.Plc.iDSDownRightX, GV.NowRecipe.iBackCCDRightX);
            GV.Plc.McSend(GV.Plc.iDSDownRightY, GV.NowRecipe.iBackCCDRightY);
            GV.Plc.McSend(GV.Plc.iDSDownRightZ, GV.NowRecipe.iBackCCDRightZ);

            GV.Plc.McSend(GV.Plc.iDSChuckX, GV.NowRecipe.iChuckX);
            GV.Plc.McSend(GV.Plc.iDSChuckY1, GV.NowRecipe.iChuckY1);
            GV.Plc.McSend(GV.Plc.iDSChuckY2, GV.NowRecipe.iChuckY2);
            GV.Plc.McSend(GV.Plc.iDSChuckZ, GV.NowRecipe.iChuckZ);

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
            */
        }

        private void MoveChuckRecipe_Click(object sender, EventArgs e)
        {
            /*
            GV.NowRecipe = GM.GetRecipeInfo(1);

            GV.Plc.McSend(GV.Plc.iDSUpLeftX, GV.NowRecipe.iUpCCDLeftX);
            GV.Plc.McSend(GV.Plc.iDSUpLeftY, GV.NowRecipe.iUpCCDLeftY);
            GV.Plc.McSend(GV.Plc.iDSUpRightX, GV.NowRecipe.iUpCCDRightX);
            GV.Plc.McSend(GV.Plc.iDSUpRightY, GV.NowRecipe.iUpCCDRightY);
            GV.Plc.McSend(GV.Plc.iDSUpBY, GV.NowRecipe.iUpCCDBigY);

            GV.Plc.McSend(GV.Plc.iDSDownLeftX, GV.NowRecipe.iBackCCDLeftX);
            GV.Plc.McSend(GV.Plc.iDSDownLeftY, GV.NowRecipe.iBackCCDLeftY);
            GV.Plc.McSend(GV.Plc.iDSDownLeftZ, GV.NowRecipe.iBackCCDLeftZ);
            GV.Plc.McSend(GV.Plc.iDSDownRightX, GV.NowRecipe.iBackCCDRightX);
            GV.Plc.McSend(GV.Plc.iDSDownRightY, GV.NowRecipe.iBackCCDRightY);
            GV.Plc.McSend(GV.Plc.iDSDownRightZ, GV.NowRecipe.iBackCCDRightZ);

            GV.Plc.McSend(GV.Plc.iDSChuckX, GV.NowRecipe.iChuckX);
            GV.Plc.McSend(GV.Plc.iDSChuckY1, GV.NowRecipe.iChuckY1);
            GV.Plc.McSend(GV.Plc.iDSChuckY2, GV.NowRecipe.iChuckY2);
            GV.Plc.McSend(GV.Plc.iDSChuckZ, GV.NowRecipe.iChuckZ);

            iUpDownChuck = 3;

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
            */
        }

        private void CCDPad_FormClosed(object sender, FormClosedEventArgs e)
        {
            // GV.HwndFormMain.StopCheckPLc = false;
            // Thread scanPlcThread = new Thread(GV.HwndFormMain.CheckPlcDram);
            // scanPlcThread.Start();
            GV.thCCDPad = null;
        }

        private void CCDPad_Shown(object sender, EventArgs e)
        {
            ChangeUpDown();
        }

        private void BtGoto1_Click(object sender, EventArgs e)
        {
            Moving();

            if (iUpDown == 1)
            {
                if (cbBigY.Checked)
                {
                    iWMemory[GV.Plc.iiDSBigY] = Decimal.ToInt32(nUDBigY1.Value);
                }

                if (cbLeftX.Checked)
                {
                    iWMemory[GV.Plc.iiDSUpLeftX] = Decimal.ToInt32(nUDLeftX1.Value);
                }

                if (cbLeftY.Checked)
                {
                    iWMemory[GV.Plc.iiDSUpLeftY] = Decimal.ToInt32(nUDLeftY1.Value);
                }

                if (cbRightX.Checked)
                {
                    iWMemory[GV.Plc.iiDSUpRightX] = Decimal.ToInt32(nUDRightX1.Value);

                }

                if (cbRightY.Checked)
                {
                    iWMemory[GV.Plc.iiDSUpRightY] = Decimal.ToInt32(nUDRightY1.Value);
                }
            }
            else if (iUpDown == 2)
            {
                if (cbLeftZ.Checked)
                {
                    iWMemory[GV.Plc.iiDSDownLeftZ] = Decimal.ToInt32(nUDLeftZ1.Value);
                }

                if (cbRightZ.Checked)
                {
                    iWMemory[GV.Plc.iiDSDownRightZ] = Decimal.ToInt32(nUDRightZ1.Value);
                }

                if (cbLeftX.Checked)
                {
                    iWMemory[GV.Plc.iiDSDownLeftX] = Decimal.ToInt32(nUDLeftX1.Value);
                }

                if (cbLeftY.Checked)
                {
                    iWMemory[GV.Plc.iiDSDownLeftY] = Decimal.ToInt32(nUDLeftY1.Value);
                }

                if (cbRightX.Checked)
                {
                    iWMemory[GV.Plc.iiDSDownRightX] = Decimal.ToInt32(nUDRightX1.Value);

                }

                if (cbRightY.Checked)
                {
                    iWMemory[GV.Plc.iiDSDownRightY] = Decimal.ToInt32(nUDRightY1.Value);
                }
            }

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void BtGoto2_Click(object sender, EventArgs e)
        {
            Moving();

            if (iUpDown == 1)
            {
                if (cbBigY.Checked)
                {
                    iWMemory[GV.Plc.iiDSBigY] = Decimal.ToInt32(nUDBigY2.Value);
                }

                if (cbLeftX.Checked)
                {
                    iWMemory[GV.Plc.iiDSUpLeftX] = Decimal.ToInt32(nUDLeftX2.Value);
                }

                if (cbLeftY.Checked)
                {
                    iWMemory[GV.Plc.iiDSUpLeftY] = Decimal.ToInt32(nUDLeftY2.Value);
                }

                if (cbRightX.Checked)
                {
                    iWMemory[GV.Plc.iiDSUpRightX] = Decimal.ToInt32(nUDRightX2.Value);

                }

                if (cbRightY.Checked)
                {
                    iWMemory[GV.Plc.iiDSUpRightY] = Decimal.ToInt32(nUDRightY2.Value);
                }
            }
            else if (iUpDown == 2)
            {
                if (cbLeftZ.Checked)
                {
                    iWMemory[GV.Plc.iiDSDownLeftZ] = Decimal.ToInt32(nUDLeftZ2.Value);
                }

                if (cbRightZ.Checked)
                {
                    iWMemory[GV.Plc.iiDSDownRightZ] = Decimal.ToInt32(nUDRightZ2.Value);
                }

                if (cbLeftX.Checked)
                {
                    iWMemory[GV.Plc.iiDSDownLeftX] = Decimal.ToInt32(nUDLeftX2.Value);
                }

                if (cbLeftY.Checked)
                {
                    iWMemory[GV.Plc.iiDSDownLeftY] = Decimal.ToInt32(nUDLeftY2.Value);
                }

                if (cbRightX.Checked)
                {
                    iWMemory[GV.Plc.iiDSDownRightX] = Decimal.ToInt32(nUDRightX2.Value);

                }

                if (cbRightY.Checked)
                {
                    iWMemory[GV.Plc.iiDSDownRightY] = Decimal.ToInt32(nUDRightY2.Value);
                }
            }

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void BtChuckGoto1_Click(object sender, EventArgs e)
        {
            Moving();

            if (cBChuckX.Checked)
            {
                iWMemory[GV.Plc.iiDSChuckX] = Decimal.ToInt32(nUDChuckX1.Value);
            }

            if (cBChuckY1.Checked)
            {
                iWMemory[GV.Plc.iiDSChuckY1] = Decimal.ToInt32(nUDChuckY11.Value);
            }

            if (cBChuckY2.Checked)
            {
                iWMemory[GV.Plc.iiDSChuckY2] = Decimal.ToInt32(nUDChuckY21.Value);
            }

            if (cBChuckZ.Checked)
            {
                iWMemory[GV.Plc.iiDSChuckZ] = Decimal.ToInt32(nUDChuckZ1.Value);
            }

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();

        }

        private void BtChuckGoto2_Click(object sender, EventArgs e)
        {
            Moving();

            if (cBChuckX.Checked)
            {
                iWMemory[GV.Plc.iiDSChuckX] = Decimal.ToInt32(nUDChuckX2.Value);
            }

            if (cBChuckY1.Checked)
            {
                iWMemory[GV.Plc.iiDSChuckY1] = Decimal.ToInt32(nUDChuckY12.Value);
            }

            if (cBChuckY2.Checked)
            {
                iWMemory[GV.Plc.iiDSChuckY2] = Decimal.ToInt32(nUDChuckY22.Value);
            }

            if (cBChuckZ.Checked)
            {
                iWMemory[GV.Plc.iiDSChuckZ] = Decimal.ToInt32(nUDChuckZ2.Value);
            }

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();

        }

        private void BtLeftZSDown_Click(object sender, EventArgs e)
        {
            Moving();

            int iRet = iWMemory[GV.Plc.iiDSDownLeftZ];

            Int32.TryParse(comboBoxBigYS.SelectedItem.ToString(), out int iMoving);

            iWMemory[GV.Plc.iiDSDownLeftZ] += iMoving;

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void BtRightZSDown_Click(object sender, EventArgs e)
        {
            Moving();

            int iRet = iWMemory[GV.Plc.iiDSDownRightZ];

            Int32.TryParse(comboBoxBigYS.SelectedItem.ToString(), out int iMoving);

            iWMemory[GV.Plc.iiDSDownRightZ] += iMoving;

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void BtLeftZBDown_Click(object sender, EventArgs e)
        {
            Moving();

            int iRet = iWMemory[GV.Plc.iiDSDownLeftZ];

            Int32.TryParse(comboBoxBigYB.SelectedItem.ToString(), out int iMoving);

            iWMemory[GV.Plc.iiDSDownLeftZ] += iMoving;

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();

        }

        private void BtRightZBDown_Click(object sender, EventArgs e)
        {
            Moving();

            int iRet = iWMemory[GV.Plc.iiDSDownRightZ];

            Int32.TryParse(comboBoxBigYB.SelectedItem.ToString(), out int iMoving);

            iWMemory[GV.Plc.iiDSDownRightZ] += iMoving;

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void BtRightZSUp_Click(object sender, EventArgs e)
        {
            Moving();

            int iRet = iWMemory[GV.Plc.iiDSDownRightZ];

            Int32.TryParse(comboBoxBigYS.SelectedItem.ToString(), out int iMoving);

            iWMemory[GV.Plc.iiDSDownRightZ] -= iMoving;

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void BtRightZBUp_Click(object sender, EventArgs e)
        {
            Moving();

            int iRet = iWMemory[GV.Plc.iiDSDownRightZ];

            Int32.TryParse(comboBoxBigYB.SelectedItem.ToString(), out int iMoving);

            iWMemory[GV.Plc.iiDSDownRightZ] -= iMoving;

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void BtLeftZSUp_Click(object sender, EventArgs e)
        {
            Moving();

            int iRet = iWMemory[GV.Plc.iiDSDownLeftZ];

            Int32.TryParse(comboBoxBigYS.SelectedItem.ToString(), out int iMoving);

            iWMemory[GV.Plc.iiDSDownLeftZ] -= iMoving;

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void BtLeftZBUp_Click(object sender, EventArgs e)
        {
            Moving();

            int iRet = iWMemory[GV.Plc.iiDSDownLeftZ];

            Int32.TryParse(comboBoxBigYB.SelectedItem.ToString(), out int iMoving);

            iWMemory[GV.Plc.iiDSDownLeftZ] -= iMoving;

            thWaitRun = new Thread(WaitRun);
            thWaitRun.Start();
        }

        private void WaferMove_Click(object sender, EventArgs e)
        {

        }

        private void BtDWafer_Click(object sender, EventArgs e)
        {
            if (iMaskWafer == 1)
            {
                iMaskWafer = 2;
 
                int iLb = GV.NowAlignCondition.LBWaferBright;
                int iRb = GV.NowAlignCondition.RBWaferBright;
                GV.Light.ChangeBrightness(1, iRb, iLb);
            }
        }

        private void BtDMask_Click(object sender, EventArgs e)
        {
            if (iMaskWafer == 2)
            {
                iMaskWafer = 1;
                int iLb = GV.NowAlignCondition.LBMaskBright;
                int iRb = GV.NowAlignCondition.RBMaskBright;
                GV.Light.ChangeBrightness(1, iRb, iLb);
            }
        }
    }
}

