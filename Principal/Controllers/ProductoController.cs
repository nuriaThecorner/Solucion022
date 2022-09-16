using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Datos.Datos;

namespace Principal.Controllers
{
    public class ProductoController : Controller
    {
        private NorthWindTuneadoDbContext db = new NorthWindTuneadoDbContext();


        // GET: Producto
        public ActionResult Index(int? id, bool category)
        {
            var products = db.Producto.Include(p => p.Categoria).Include(p => p.Proveedor);
            if (id != null && id> 0)
            {
                if (category != null)
                {
                    if (category == true)
                    {
                        products = products.Where(x => x.CategoryID == id);
                        if (products != null && products.Count() >0)
                        {
                            ViewBag.Message = "Productos de la categoria: " + products.FirstOrDefault().Categoria.CategoryName + " | Descripcion: " + products.FirstOrDefault().Categoria.Description;
                        }
                    }

                    else
                    {
                        products = products.Where(x => x.supplierID == id);
                        if (products != null && products.Count() > 0)
                        {
                            ViewBag.Message = "Productos del proveedor: " + products.FirstOrDefault().Proveedor.ContactName;
                        }
                    }
                }

            }


            return View(products.ToList());
        }




        //// GET: Producto
        //public ActionResult Index()
        //{
        //    var producto = db.Producto.Include(p => p.Categoria).Include(p => p.Proveedor);
        //    return View(producto.ToList());
        //}

        // GET: Producto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // GET: Producto/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categoria, "CategoryID", "CategoryName");
            ViewBag.supplierID = new SelectList(db.Proveedor, "supplierID", "supplierName");
            return View();
        }

        // POST: Producto/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,supplierID,CategoryID,unit,Price")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Producto.Add(producto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categoria, "CategoryID", "CategoryName", producto.CategoryID);
            ViewBag.supplierID = new SelectList(db.Proveedor, "supplierID", "supplierName", producto.supplierID);
            return View(producto);
        }

        // GET: Producto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categoria, "CategoryID", "CategoryName", producto.CategoryID);
            ViewBag.supplierID = new SelectList(db.Proveedor, "supplierID", "supplierName", producto.supplierID);
            return View(producto);
        }

        // POST: Producto/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,supplierID,CategoryID,unit,Price")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categoria, "CategoryID", "CategoryName", producto.CategoryID);
            ViewBag.supplierID = new SelectList(db.Proveedor, "supplierID", "supplierName", producto.supplierID);
            return View(producto);
        }

        // GET: Producto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Producto producto = db.Producto.Find(id);
            db.Producto.Remove(producto);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
