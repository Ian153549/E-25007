using OpenCvSharp;
using Euresys.Open_eVision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MRLibrary
{
    public class EVisionMatch : IDisposable
    {
        private readonly EMatcher eMatch;

        private MatchPosition MPosition = null;

        private EImageBW8 nMatchBW8;

        public SizeF TemplateSize = new SizeF(0, 0);

        private bool disposed = false;

        public object Locker;

        public string dMsg = null;

        private bool isInitialized = false;
       

        public EVisionMatch(string debugmsg)
        {
            try
            {
                eMatch = new EMatcher
                {
                    MaxPositions = 1,
                    MaxInitialPositions = 3,
                    DontCareThreshold = 1,
                    Interpolate = true,
                    MinReducedArea = 512
                };
                dMsg = debugmsg;
                nMatchBW8 = new EImageBW8(4000, 3000);
                Locker = new object();
            }
            catch (Exception exception1)
            {                
                Exception exception = exception1;
                MessageBoxM.Show(exception.Message, "Open eVision", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Application.Exit();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    eMatch.Dispose();
                }
                disposed = true;
            }
        }

        public void Learn(GrayImage pattern, Rectangle rect, GrayImage dontCare)
        {
            lock (eMatch)
            {
                GrayImage grayImage = new GrayImage(pattern);
                if (dontCare != null)
                {
                    grayImage.SetMask(dontCare);
                }
                EImageBW8 imageBW8 = ImageConvert.GrayImageToImageBW8(grayImage);
                EROIBW8 eROIBW8 = new EROIBW8();
                eROIBW8.Attach(imageBW8);
                eROIBW8.SetPlacement(rect.X, rect.Y, rect.Width, rect.Height);
                eMatch.LearnPattern(eROIBW8);
            }
        }

        public void Learn(GrayImage pattern, GrayImage dontCare)
        {
            lock (eMatch)
            {
                GrayImage grayImage = new GrayImage(pattern);
                if (dontCare != null)
                {
                    grayImage.SetMask(dontCare);
                }
                EImageBW8 imageBW8 = ImageConvert.GrayImageToImageBW8(grayImage);
                TemplateSize = new SizeF(imageBW8.Width, imageBW8.Height);
                eMatch.LearnPattern(imageBW8);
                imageBW8.Dispose();
                imageBW8 = null;
                grayImage = null;
            }
        }

        public void SetUnLearn()
        {
            eMatch?.ClearImage();
        }

        private bool BLearn(GrayImage pattern, GrayImage dontCare)
        {
            bool bRet = true;
            lock (eMatch)
            {
                GrayImage grayImage = new GrayImage(pattern);
                if (dontCare != null)
                {
                    grayImage.SetMask(dontCare);
                }
                EImageBW8 imageBW8 = ImageConvert.GrayImageToImageBW8(grayImage);
                eMatch.LearnPattern(imageBW8);
                if (!eMatch.PatternLearnt) bRet = false;
            }
            return bRet;
        }

        public MatchPosition Match(GrayImage bmp, GrayImage template, GrayImage mask)
        {
            MatchPosition matchPosition;
            bool flag;
            bool bRet;

            if (bmp != null)
            {
                EImageBW8 imageBW8 = ImageConvert.GrayImageToImageBW8(bmp);
                try
                {
                    try
                    {
                        if (mask == null)
                        {
                            flag = false;
                        }
                        else
                        {
                            flag = (template.Width != mask.Width || template.Height != mask.Height);
                        }
                        if (!flag)
                        {
                            bRet = BLearn(template, mask);
                            eMatch.CorrelationMode = ECorrelationMode.Normalized;
                            eMatch.Match(imageBW8);
                            if (eMatch.NumPositions != 0)
                            {
                                EMatchPosition position = eMatch.GetPosition(0);
                                MPosition = new MatchPosition(position.CenterX, position.CenterY, position.Score, new SizeF((float)template.Width, (float)template.Height));
                            }
                            else
                            {
                                matchPosition = new MatchPosition();
                                return matchPosition;
                            }
                        }
                        else
                        {
                            matchPosition = new MatchPosition();
                            return matchPosition;
                        }
                    }
                    catch (Exception exception1)
                    {
                        Exception exception = exception1;
                        MessageBoxM.Show(exception.Message, "Open eVision", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        matchPosition = new MatchPosition();
                        return matchPosition;
                    }
                }
                finally
                {
                   
                }
                matchPosition = MPosition;
            }
            else
            {
                matchPosition = new MatchPosition();
            }
            return matchPosition;
        }

        public bool BMatch(GrayImage bmp, GrayImage template, GrayImage mask, ref MatchPosition mp)
        {
            bool flag = false;
            if ((bmp != null) && (mp != null))
            {
                EImageBW8 imageBW8 = ImageConvert.GrayImageToImageBW8(bmp);
                try
                {
                    try
                    {
                        if (mask == null)
                        {
                            flag = false;
                        }
                        else
                        {
                            flag = (template.Width != mask.Width || template.Height != mask.Height);
                        }
                        if (!flag)
                        {
                            Learn(template, mask);
                            eMatch.CorrelationMode = ECorrelationMode.Normalized;
                            if (!eMatch.PatternLearnt) flag = false;
                            eMatch.Match(imageBW8);
                            if (eMatch.NumPositions != 0)
                            {
                                EMatchPosition position = eMatch.GetPosition(0);
                                MPosition = new MatchPosition(position.CenterX, position.CenterY, position.Score, new SizeF((float)template.Width,(float)template.Height));
                                mp.X = position.CenterX; 
                                mp.Y = position.CenterY;
                                mp.HotSpot.X = (int)mp.X;
                                mp.HotSpot.Y = (int)mp.Y;
                                mp.Score = position.Score;
                                mp.TemplateSize = new SizeF((int)template.Width, (int)template.Height);
                                flag = true;
                            }
                            else
                            {
                                flag = false;
                            }
                        }
                        else
                        {
                            flag = false;
                        }
                    }
                    catch (Exception)
                    {
                        // Exception exception = exception1;
                        // MessageBoxM.Show(exception.Message, "Open eVision", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        flag = false;
                    }
                }
                finally
                {
                    
                }
            }
            return flag;
        }

        //public void Learn(GrayImage pattern, GrayImage dontCare)
        //{
        //    lock (eMatch)
        //    {
        //        GrayImage grayImage = new GrayImage(pattern);
        //        if (dontCare != null)
        //        {
        //            grayImage.SetMask(dontCare);
        //        }
        //        EImageBW8 imageBW8 = ImageConvert.GrayImageToImageBW8(grayImage);
        //        TemplateSize = new SizeF(imageBW8.Width, imageBW8.Height);
        //        eMatch.LearnPattern(imageBW8);
        //        imageBW8.Dispose();
        //        imageBW8 = null;
        //        grayImage.Dispose();
        //        grayImage = null;
        //    }
        //}

        //public void MatMatch(Mat bmp, ref MatchPosition mPos)
        //{
        //    if (!eMatch.PatternLearnt)
        //    {
        //        mPos.X = 0;
        //        mPos.Y = 0;
        //        mPos.Score = 0.04;
        //        return;
        //    }

        //    if (bmp != null)
        //    {
        //        ImageConvert.MatToEImageBW8(bmp, nMatchBW8);
        //        try
        //        {
        //            eMatch.CorrelationMode = ECorrelationMode.Normalized;
        //            eMatch.Match(nMatchBW8);
        //            if (eMatch.NumPositions != 0)
        //            {
        //                EMatchPosition position = eMatch.GetPosition(0);
        //                mPos.X = position.CenterX;
        //                mPos.Y = position.CenterY;
        //                mPos.Score = position.Score;
        //                mPos.TemplateSize = new SizeF(TemplateSize.Width,TemplateSize.Width);
        //            }
        //            else
        //            {
        //                mPos.X = 0;
        //                mPos.Y = 0;
        //                mPos.Score = 0.01;
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            mPos.X = 0;
        //            mPos.Y = 0;
        //            mPos.Score = 0.02;
        //            // MessageBoxM.Show(ex.Message, "Open eVision", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        //        }
        //        finally
        //        {
        //        }
        //    }
        //    else
        //    {
        //        mPos.X = 0;
        //        mPos.Y = 0;
        //        mPos.Score = 0.03;
        //    }
        //}
        public void MatMatch(Mat bmp, ref MatchPosition mPos)
        {
            lock (eMatch)
            {
                if (!eMatch.PatternLearnt)
                {
                    mPos.X = 0;
                    mPos.Y = 0;
                    mPos.Score = 0.04;
                    return;
                }
            }

            if (bmp == null || bmp.Empty())
            {
                mPos.X = 0;
                mPos.Y = 0;
                mPos.Score = 0.03;
                return;
            }

            try
            {
                // 取得（或重建）有效的 EImageBW8，並儲回成員欄位
                nMatchBW8 = ImageConvert.MatToEImageBW8(bmp, nMatchBW8);

                lock (eMatch)
                {
                    eMatch.CorrelationMode = ECorrelationMode.Normalized;
                    eMatch.Match(nMatchBW8);
                    if (eMatch.NumPositions != 0)
                    {
                        EMatchPosition position = eMatch.GetPosition(0);
                        mPos.X = position.CenterX;
                        mPos.Y = position.CenterY;
                        mPos.Score = position.Score;
                        mPos.TemplateSize = new SizeF(TemplateSize.Width, TemplateSize.Height);
                    }
                    else
                    {
                        mPos.X = 0;
                        mPos.Y = 0;
                        mPos.Score = 0.01;
                    }
                }
            }
            catch (Exception)
            {
                mPos.X = 0;
                mPos.Y = 0;
                mPos.Score = 0.02;
            }
        }
    }
}
