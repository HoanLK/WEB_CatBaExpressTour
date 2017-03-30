﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CMS.Models;

namespace CMS.Areas.Admin.Controllers
{
    public class ProductsAPIController : ApiController
    {
        private HoaBanTravelEntities db = new HoaBanTravelEntities();

        // GET: api/ProductsAPI
        public IQueryable<Product> GetProduct()
        {
            return db.Product.OrderByDescending(p => p.timeModified);
        }

        // GET: api/ProductsAPI/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> GetProduct(int id)
        {
            Product product = await db.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        //GET: API/ProductsAPI?att=...&&value=...
        public IQueryable<Product> GetProduct(string att, string value)
        {
            var product = db.Product;

            if (att == "idCategoryProduct" && att != null && value != null)
            {
                int idCategoryProduct = int.Parse(value);
                var model = db.Product.Where(p => p.idCategoryProduct == idCategoryProduct && p.published == 1).OrderByDescending(p => p.timeModified);

                return model;
            }

            if (att == "spMoi" && att != null && value != null)
            {
                int idCategoryProduct = int.Parse(value);
                var model = db.Product.Where(p => p.published == 1).OrderByDescending(p => p.timeModified).Take(6);

                return model;
            }

            return product;
        }

        // PUT: api/ProductsAPI/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.idProduct)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ProductsAPI
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Product.Add(product);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = product.idProduct }, product);
        }

        // DELETE: api/ProductsAPI/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> DeleteProduct(int id)
        {
            Product product = await db.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Product.Remove(product);
            await db.SaveChangesAsync();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Product.Count(e => e.idProduct == id) > 0;
        }
    }
}