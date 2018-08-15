
//======================================================================
//        Copyright (C) 2008-2009   
//        All rights reserved
//
//        Filename :ImageOp
//        Description : 
//
//        Created by Conan at  2009-3-6 16:39:35
//        http://conan87810.cnblogs.com      
//======================================================================
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Web;

namespace MUSystem.Utils
{
    public static class ZImage
    {

        #region 图片格式转换
        /// <summary>
        /// 图片格式转换
        /// </summary>
        /// <param name="OriFilename">原始文件相对路径</param>
        /// <param name="DesiredFilename">生成目标文件相对路径</param>
        /// <returns></returns>
        ///  JPG采用的是有损压缩所以JPG图像有可能会降低图像清晰度，而像素是不会降低的   
        ///  GIF采用的是无损压缩所以GIF图像是不会降低原图图像清晰度和像素的，但是GIF格式只支持256色图像。
        public static bool ConvertImage(string OriFilename, string DesiredFilename)
        {
            string extname = DesiredFilename.Substring(DesiredFilename.LastIndexOf('.') + 1).ToLower();
            ImageFormat DesiredFormat;
            //根据扩张名，指定ImageFormat
            switch (extname)
            {
                case "bmp":
                    DesiredFormat = ImageFormat.Bmp;
                    break;
                case "gif":
                    DesiredFormat = ImageFormat.Gif;
                    break;
                case "jpeg":
                    DesiredFormat = ImageFormat.Jpeg;
                    break;
                case "ico":
                    DesiredFormat = ImageFormat.Icon;
                    break;
                case "png":
                    DesiredFormat = ImageFormat.Png;
                    break;
                default:
                    DesiredFormat = ImageFormat.Jpeg;
                    break;
            }
            try
            {
                System.Drawing.Image imgFile = System.Drawing.Image.FromFile(WebPathTran(OriFilename));
                imgFile.Save(WebPathTran(DesiredFilename), DesiredFormat);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion


        #region 图片缩放
        /// <summary>
        /// 图片固定大小缩放
        /// </summary>
        /// <param name="OriFileName">源文件相对地址</param>
        /// <param name="DesiredFilename">目标文件相对地址</param>
        /// <param name="IntWidth">目标文件宽</param>
        /// <param name="IntHeight">目标文件高</param>
        /// <param name="imageFormat">图片文件格式</param>
        public static bool ChangeImageSize(string OriFileName, string DesiredFilename, int IntWidth, int IntHeight, ImageFormat imageFormat)
        {
            string SourceFileNameStr = WebPathTran(OriFileName); //来源图片名称路径
            string TransferFileNameStr = WebPathTran(DesiredFilename); //目的图片名称路径
            FileStream myOutput = null;
            try
            {
                System.Drawing.Image.GetThumbnailImageAbort myAbort = new System.Drawing.Image.GetThumbnailImageAbort(imageAbort);
                Image SourceImage = System.Drawing.Image.FromFile(OriFileName);//来源图片定义
                Image TargetImage = SourceImage.GetThumbnailImage(IntWidth, IntHeight, myAbort, IntPtr.Zero);  //目的图片定义
                //将TargetFileNameStr的图片放宽为IntWidth，高为IntHeight 
                myOutput = new FileStream(TransferFileNameStr, FileMode.Create, FileAccess.Write, FileShare.Write);
                TargetImage.Save(myOutput, imageFormat);
                myOutput.Close();
                return true;
            }
            catch
            {
                myOutput.Close();
                return false;
            }


        }

        private static bool imageAbort()
        {
            return false;
        }
        #endregion


        #region 文字水印
        /// <summary>
        /// 文字水印
        /// </summary>
        /// <param name="wtext">水印文字</param>
        /// <param name="source">原图片物理文件名</param>
        /// <param name="target">生成图片物理文件名</param>
        public static bool ImageWaterText(string wtext, string source, string target)
        {
            bool resFlag = false;
            Image image = Image.FromFile(source);
            Graphics graphics = Graphics.FromImage(image);
            try
            {
                graphics.DrawImage(image, 0, 0, image.Width, image.Height);
                Font font = new System.Drawing.Font("Verdana", 60);
                Brush brush = new System.Drawing.SolidBrush(System.Drawing.Color.Green);
                graphics.DrawString(wtext, font, brush, 35, 35);
                image.Save(target);
                resFlag = true;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                graphics.Dispose();
                image.Dispose();

            }
            return resFlag;
        }


        #endregion


        #region 图片水印

        /// <summary>
        /// 在图片上生成图片水印
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_syp">生成的带图片水印的图片路径</param>
        /// <param name="Path_sypf">水印图片路径</param>
        public static bool ImageWaterPic(string source, string target, string waterPicSource)
        {
            bool resFlag = false;
            Image sourceimage = Image.FromFile(source);
            Graphics sourcegraphics = Graphics.FromImage(sourceimage);
            Image waterPicSourceImage = Image.FromFile(waterPicSource);
            try
            {
                sourcegraphics.DrawImage(waterPicSourceImage, new System.Drawing.Rectangle(sourceimage.Width - waterPicSourceImage.Width, sourceimage.Height - waterPicSourceImage.Height, waterPicSourceImage.Width, waterPicSourceImage.Height), 0, 0, waterPicSourceImage.Width, waterPicSourceImage.Height, GraphicsUnit.Pixel);
                sourceimage.Save(target);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                sourcegraphics.Dispose();
                sourceimage.Dispose();
                waterPicSourceImage.Dispose();
            }
            return resFlag;

        }

        #endregion


        /// <summary>
        /// 路径转换（转换成绝对路径）
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string WebPathTran(string path)
        {
            try
            {
                return HttpContext.Current.Server.MapPath(path);
            }
            catch
            {
                return path;
            }
        }
    }
}
