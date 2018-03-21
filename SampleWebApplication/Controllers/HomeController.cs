using AjaxMapper;
using SampleWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SampleWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private List<SampleModel> Model
        {
            get
            {
                if (Session["sampleModel"] == null)
                {
                    Session["sampleModel"] = new List<SampleModel> {
                        new SampleModel{Id=1,Name="one",Note="first" },
                        new SampleModel{Id=2,Name="two",Note="second" },
                        new SampleModel{Id=3,Name="three",Note="third" },
                        new SampleModel{Id=4,Name="four",Note="fourth" },
                        new SampleModel{Id=5,Name="five",Note="fifth" },
                    };
                }
                return Session["sampleModel"] as List<SampleModel>;
            }
            set
            {
                Session["sampleModel"] = value;
            }
        }


        public ActionResult Index()
        {
            var model = this.Model;

            return View(model);
        }

        [ReturnObjectType(typeof(List<SampleModel>))]
        public ActionResult Remove(int id)
        {
            var model = this.Model;
            if (model == null)
                throw new Exception();
            
            model.Remove(model.FirstOrDefault(m => m.Id == id));

            return Json(model);
        }

        [ReturnObjectType(typeof(List<SampleModel>))]
        public ActionResult Add(string name, string note)
        {
            var model = this.Model;
            if (model == null)
                throw new Exception();

            if (model.Any(m => m.Name == name))
                throw new Exception();

            var id = model.Max(m => m.Id) + 1;

            model.Add(new SampleModel { Id = id, Name = name, Note = note });
            return Json(model);
        }

        [ReturnObjectType(typeof(List<SampleModel>))]
        public ActionResult Edit(int id, string name, string note)
        {
            var model = this.Model;
            if (model == null)
                throw new Exception();

            if (model.Any(m => m.Name == name && m.Id != id))
                throw new Exception();

            var item = model.FirstOrDefault(m => m.Id == id);

            item.Name = name;
            item.Note = note;

            return Json(model);
        }
    }
}