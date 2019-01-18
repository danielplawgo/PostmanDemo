using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Bogus;
using PostmanDemo.Models;

namespace PostmanDemo.Controllers
{
    public class ProductsController : ApiController
    {
        private static List<Product> _products;

        static ProductsController()
        {
            int id = 1;
            _products = new Faker<Product>()
                .RuleFor(o => o.Id, f => id++)
                .RuleFor(p => p.Name, (f, p) => f.Commerce.ProductName())
                .RuleFor(p => p.Category, (f, p) => f.Commerce.Categories(1).FirstOrDefault())
                .RuleFor(p => p.Price, (f, p) => f.Random.Number(100, 100000) / 100M)
                .Generate(10);
        }

        public IEnumerable<Product> Get()
        {
            return _products;
        }

        public Product Get(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public Product Post([FromBody]Product product)
        {
            product.Id = _products.Select(p => p.Id).Max() + 1;
            _products.Add(product);

            return product;
        }

        public Product Put(int id, [FromBody]Product product)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == id);

            if (existingProduct != null)
            {
                existingProduct.Category = product.Category;
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
            }

            return existingProduct;
        }

        public void Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);

            if (product != null)
            {
                _products.Remove(product);
            }
        }
    }
}