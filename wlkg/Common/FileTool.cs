using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Web.Configuration;
using Aspose.Words;
using Microsoft.Office.Interop.Word;
using Document = Microsoft.Office.Interop.Word.Document;

namespace ServiceInterface.Common
{
    public static class FileTool
    {
        ///// <summary>
        ///// 根据新闻类型、附件Url将附件文件由Word转换为Pdf。
        ///// </summary>
        ///// <param name="newsClass">新闻类型</param>
        ///// <param name="url">附件url</param>
        ///// <param name="fileName">附件文件文件名</param>
        ///// <returns>Pdf文件名</returns>
        //public static string TransferAttechmentFromWordToPdf(NewsClass newsClass, string url, string fileName)
        //{
        //    if (string.IsNullOrEmpty(fileName) || string.IsNullOrWhiteSpace(fileName)) return null;
        //    if (!fileName.ToLower().Contains(".doc") && !fileName.ToLower().Contains(".docx")) return null;

        //    //通过URL下载附件文件到本地
        //    var tempPath = string.Format("{0}{1}", GetTempPath(newsClass), fileName);

        //    //var webClient = new WebClient();
        //    //webClient.DownloadFile(url, fileName);

        //    var webClient = new WebClient {Encoding = System.Text.Encoding.UTF8};
        //    var uri = new Uri(url);
        //    var byteArray = webClient.DownloadData(uri);

        //    var fi = new FileInfo(tempPath);
        //    if (fi.Exists) fi.Delete();

        //    var fs = new FileStream(tempPath, FileMode.Create, FileAccess.Write);
        //    fs.Write(byteArray, 0, byteArray.Length);
        //    //fs.Flush();
        //    fs.Close();


        //    //var req = WebRequest.Create(new Uri(url));
        //    //var resp = req.GetResponse();
        //    //var stream = resp.GetResponseStream();
        //    //if (null == stream) return null;



        //    //var sr = new StreamReader(stream);
        //    //var str = sr.ReadToEnd();
        //    //var byteArray = System.Text.Encoding.Default.GetBytes(str);

        //    //var tempPath = string.Format("{0}{1}", GetTempPath(newsClass),
        //    //    fileName);
        //    var pdfFilename = fileName.ToLower().Replace(".doc", ".pdf").Replace(".docx", ".pdf");
        //    var downPath = string.Format("{0}{1}", GetDownPath(newsClass), pdfFilename);

        //    //var fi = new FileInfo(tempPath);
        //    //if (fi.Exists) fi.Delete();

        //    //var fs = new FileStream(tempPath, FileMode.Create, FileAccess.Write);
        //    //fs.Write(byteArray, 0, byteArray.Length);
        //    ////fs.Flush();
        //    //fs.Close();


        //    //将临时文件夹中的Doc文件转为Pdf文件后保存到下载文件夹
        //    //WordToPdf(tempPath, downPath);

        //    //ToPdf(tempPath);

        //    var doc = new Aspose.Words.Document(tempPath);
            
        //    doc.Save(downPath, SaveFormat.Pdf);

        //    return pdfFilename;
        //}

        //public static bool ToPdf(string sourcePath)
        //{
        //    //读取doc文档
        //    var doc = new Aspose.Words.Document(@sourcePath);
        //    //保存为PDF文件，此处的SaveFormat支持很多种格式，如图片，epub,rtf 等等
        //    doc.Save("temp.pdf", SaveFormat.Pdf);
        //    return true;
        //}

        ///// <summary>
        ///// 根据文件路径将Word文件转换为Pdf文件。
        ///// </summary>
        ///// <param name="sourcePath">源Word文件路径</param>
        ///// <param name="targetPath">目标Pdf文件路径</param>
        ///// <returns>转换结果</returns>
        //public static bool WordToPdf(string sourcePath, string targetPath)
        //{
        //    const WdExportFormat exportFormat = WdExportFormat.wdExportFormatPDF;
        //    ApplicationClass application = null;
        //    Document document = null;

        //    try
        //    {
        //        application = new ApplicationClass {Visible = false};
        //        document = application.Documents.Open(sourcePath);
        //        document.SaveAs2();
        //        document.ExportAsFixedFormat(targetPath, exportFormat);
        //    }
        //    finally
        //    {
        //        if (document != null)
        //        {
        //            document.Close();
        //            //document = null;
        //        }
        //        if (application != null)
        //        {
        //            application.Quit();
        //            //application = null;
        //        }
        //        GC.Collect();
        //        GC.WaitForPendingFinalizers();
        //        GC.Collect();
        //        GC.WaitForPendingFinalizers();
        //    }
        //    return true;
        //}

        ///// <summary>
        ///// 根据新闻类型获取临时文件夹路径。
        ///// </summary>
        ///// <param name="newsClass">新闻类型</param>
        ///// <returns>文件夹路径</returns>
        //public static string GetTempPath(NewsClass newsClass)
        //{
        //    switch (newsClass)
        //    {
        //        case NewsClass.Notice:
        //            return GetWebConfigKey("NoticeTempPath");
        //        case NewsClass.News:
        //            return GetWebConfigKey("NewsTempPath");
        //            //TODO:扩充其他类型新闻的处理代码
        //        default:
        //            goto case NewsClass.Notice;
        //    }
        //}

        ///// <summary>
        ///// 根据新闻类型获取下载文件夹路径。
        ///// </summary>
        ///// <param name="newsClass">新闻类型</param>
        ///// <returns>文件夹路径</returns>
        //public static string GetDownPath(NewsClass newsClass)
        //{
        //    switch (newsClass)
        //    {
        //        case NewsClass.Notice:
        //            return GetWebConfigKey("NoticeDownPath");
        //        case NewsClass.News:
        //            return GetWebConfigKey("NewsDownPath");
        //        //TODO:扩充其他类型新闻的处理代码
        //        default:
        //            goto case NewsClass.Notice;
        //    }
        //}

        ///// <summary>
        ///// 根据新闻类型获取下载文件夹URL。
        ///// </summary>
        ///// <param name="newsClass">新闻类型</param>
        ///// <returns>文件夹URL</returns>
        //public static string GetDownUrl(NewsClass newsClass)
        //{
        //    switch (newsClass)
        //    {
        //        case NewsClass.Notice:
        //            return GetWebConfigKey("NoticeDownUrl");
        //        case NewsClass.News:
        //            return GetWebConfigKey("NewsDownUrl");
        //        //TODO:扩充其他类型新闻的处理代码
        //        default:
        //            goto case NewsClass.Notice;
        //    }
        //}

        /// <summary>
        /// 获取Web.config应用设置。
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>设置值</returns>
        public static string GetWebConfigKey(string key)
        {
            return WebConfigurationManager.AppSettings[key];
        }

        //public static Image GetImageFromString(string stringPath)
        //{
        //    var sr = new StreamReader(stringPath);
        //    var s = sr.ReadToEnd();
        //    sr.Close();
        //    var buf = Convert.FromBase64String(s);

        //    var ms = new MemoryStream(buf);
        //    var img = Image.FromStream(ms);
        //    img.Save("filename.gif",System.Drawing.Imaging.ImageFormat.Gif);
        //    ms.Close();
        //    ms.Dispose();

        //    return img;
        //}

        //public static void GetImageToString(string imagePath, string savePath)
        //{
        //    var s = File.Open(imagePath, FileMode.Open);
        //    var leng = 0;
        //    if (s.Length < Int32.MaxValue)
        //        leng = (int) s.Length;
        //    var by = new byte[leng];

        //    var ms = new MemoryStream(by);
        //    ms.Read(by, 0, (int) s.Length);
        //    ms.Close();

        //    var str = Convert.ToBase64String(by);
        //    var sw = File.CreateText(savePath);
        //    sw.Write(str);;
        //    sw.Close();
        //    sw.Dispose();
        //}


    }

    public enum NewsClass
    {
        Notice,
        News
    }

    public static class DisplayType
    {
        public const string Content = "1";
        public const string Attachment = "2";
    }
}