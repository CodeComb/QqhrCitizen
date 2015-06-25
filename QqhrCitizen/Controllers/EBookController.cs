﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QqhrCitizen.Models;
using QqhrCitizen.Models.ViewModel;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.Word;
using Aspose.Words;
using System.IO;

namespace QqhrCitizen.Controllers
{
    public class EBookController : BaseController
    {
        // GET: EBook
        public ActionResult Index()
        {
            /*string tid = HttpContext.Request.QueryString["tid"].ToString();
            ViewBag.Tid = tid;
            var lstBooks = db.EBooks.OrderByDescending(b => b.Browses).Take(8).ToList();
            var type = new TypeDictionary();
            if (tid != "0")
            {
                int id = Convert.ToInt32(tid);
                type = db.TypeDictionaries.Find(id);
            }
            ViewBag.Type = type.TypeValue;
            ViewBag.EBooks = lstBooks;*/

            List<EBook> HotBooks = new List<EBook>();
            List<EBook> NewBooks = new List<EBook>();

            HotBooks = db.EBooks.OrderByDescending(b => b.Browses).Take(10).ToList();
            NewBooks = db.EBooks.OrderByDescending(b => b.Time).Take(12).ToList();
            ViewBag.HotBooks = HotBooks;
            ViewBag.NewBooks = NewBooks;
            return View();
        }

        #region 分页获取图书
        /// <summary>
        ///   分页获取图书
        /// </summary>
        /// <param name="page"></param>
        /// <param name="tid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult getEBookes(int page, int tid)
        {
            List<EBook> lstEBook = new List<EBook>();
            List<vEBook> _lstEBook = new List<vEBook>();
            int index = page * 10;
            if (tid == 0)
            {
                lstEBook = db.EBooks.OrderByDescending(eb => eb.Time).Skip(index).Take(10).ToList();
            }
            else
            {
                var iebook = db.EBooks.Where(eb => eb.EBookTypeID == tid);
                var ifbook = db.EBooks.Where(eb => eb.TypeDictionary.FatherID == tid);
                lstEBook = iebook.Union(ifbook).OrderByDescending(eb => eb.Time).Skip(index).Take(10).ToList();
            }

            foreach (var item in lstEBook)
            {
                _lstEBook.Add(new vEBook(item));
            }

            return Json(_lstEBook);
        }
        #endregion



        #region 电子书下载
        /// <summary>
        /// 电子书下载
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Download(int id)
        {
            EBook book = new EBook();
            book = db.EBooks.Find(id);
            var path = Server.MapPath("~/Upload/" + book.File.Path);
            return File(path,book.File.ContentType, Url.Encode(book.File.FileName));
        }
        #endregion


        #region 显示图书信息
        /// <summary>
        /// 显示图书信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
 
        public ActionResult Show(int id)
        {
            List<EBook> LstNewEBook = new List<EBook>();
            List<vLession> lessions = new List<vLession>();
            var Ebook = db.EBooks.Find(id);
            Ebook.Browses += 1;
            db.SaveChanges();
            LstNewEBook = db.EBooks.OrderByDescending(c => c.Time).Take(12).ToList();
            ViewBag.LstNewEBook = LstNewEBook;
            ViewBag.Ebook = Ebook;
            return View();
        }
        #endregion
        #region 图书截图
        /// <summary>
        /// 图书截图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ShowPicture(int id)
        {
            EBook book = new EBook();
            book = db.EBooks.Find(id);
            return File(book.Picture, "image/jpg");
        }
        #endregion


        /// <summary>
        ///  发现电子书
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Discovery(int id)
        {
            List<TypeDictionary> types = new List<TypeDictionary>();
            types = db.TypeDictionaries.Where(t => t.Belonger == TypeBelonger.电子书 && t.FatherID == 0).ToList();
            ViewBag.Tid = id;
            var type = new TypeDictionary();
            if (id != 0)
            {
                type = db.TypeDictionaries.Find(id);
            }
            ViewBag.Type = id;

            ViewBag.Types = types;
            return View();
        }
        public ActionResult ReadBook()
        {
            //存放word文件的完整路径
            string wordPath = Server.MapPath("/Upload/1.doc");
            string fileName = "1";
            //存放html文件的完整路径
             ViewBag.html=WordToHtml(wordPath, fileName);
            return View();
        }
        private string WordToHtml(string wordFileName, string fileName)
        {
            Aspose.Words.Document d = new Aspose.Words.Document(wordFileName);
            string filePhysicalPath = "/Upload/" + fileName + "/";
            string filepath = Server.MapPath(filePhysicalPath);
            string setfileload = filePhysicalPath + fileName + ".html";
            Directory.CreateDirectory(filepath);
            d.Save(Server.MapPath(setfileload),SaveFormat.Html);
            d.SaveToPdf(Server.MapPath(filePhysicalPath+"1.pdf"));
            return setfileload;
        }
    }
} 