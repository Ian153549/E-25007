using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;

namespace NSAA_16Axis
{
    public partial class DialogCreatePatternMask : Form
    {

        TemplatesMask.MaskData _maskData;
        public Mat _patternMask;
        public Mat _patternAfterModifyCenter;
        public Mat orgMat;

        public DialogCreatePatternMask(Mat mat)
        {
            InitializeComponent();
            orgMat = mat;
            templateMask1.InputImage(mat);
            templateMask1.LiveNow();
        }

        private void DialogCreatePatternMask_FormClosing(object sender, FormClosingEventArgs e)
        {
            _maskData = (TemplatesMask.MaskData)templateMask1.GetMaskData();
            _patternMask = templateMask1.GetMaskImage();
            if (_patternMask == null)
            {
                GV.DialogMat = orgMat;
            } else
            {
                GV.DialogMat = _patternMask;
            }
             
            GV.DialogMatCenter = _patternAfterModifyCenter = templateMask1.GetOriginalImage();
        }
    }
}
