using APSTUnitTesting.Controllers;
using APSTUnitTesting.Models;
using APSTUnitTesting.Services;
using Microsoft.AspNetCore.Mvc;

namespace XUnitTest
{
    public class ProductTesting
    {
        private readonly ProductController _controller;
        private readonly IProductService _service;
        public ProductTesting()
        {
           _service = new ProductService();
            _controller = new ProductController(_service);
        }

        // Prueba para verificar que la respuesta es Ok (200) al obtener todos los productos
        [Fact]
        public void GetOk()
        {
            var result = _controller.Get();
            Assert.IsType<OkObjectResult>(result);
        }

        // Prueba para verificar que la cantidad de productos obtenidos es mayor que cero
        [Fact]
        public void GetQuantity()
        {
            var result = (OkObjectResult)_controller.Get();
            var products = Assert.IsType<List<Product>>(result.Value);
            Assert.True(products.Count > 0);

        }

        // Prueba para verificar que la respuesta es Ok (200) al obtener un producto por ID
        [Fact]
        public void GetByIdOk()
        {
            int id = 1;
            var result = _controller.GetById(id);
            Assert.IsType<OkObjectResult>(result);
        }

        // Prueba para verificar que el producto existe cuando se busca por un ID específico
        [Fact]
        public void IdExistOk()
        {
            int id = 1;
            var result = (OkObjectResult)_controller.GetById(id);
            var product = Assert.IsType<Product>(result?.Value);
            Assert.True(product != null);
            Assert.Equal(product.Id, id);
        }

        // Prueba para verificar que se devuelve un resultado NotFound (404) cuando el ID del producto no existe
        [Fact]
        public void GetByIdNotFound()
        {
            int id = 13;
            var result = _controller.GetById(id);
            var product = Assert.IsType<NotFoundResult>(result);
        }

        // Prueba para verificar que se puede agregar un producto válido al inventario
        [Fact]
        public void AddProductValid()
        {
            // Arrange: Configurar un producto válido
            var newProduct = new Product { Id = 4, Name = "Bocina", Brand = "JBL", Quantity = 25 };
            // Act: Intentar agregar el producto
            var result = _controller.AddProduct(newProduct);
            // Assert: Verificar que la respuesta es Ok y que el producto fue agregado
            Assert.IsType<OkResult>(result);
        }

        // Prueba para verificar que no se pueden agregar productos duplicados al inventario
        [Fact]
        public void AddProductDuplicate()
        {
            var duplicateProduct = new Product { Id = 2, Name = "Monitor", Brand= "Sceptre", Quantity = 5 };
            var result = _controller.AddProduct(duplicateProduct);
            Assert.IsType<BadRequestResult>(result);
        }

        // Prueba para verificar que el inventario rechaza productos con datos inválidos
        [Fact]
        public void AddProductInvalidData()
        {
            var invalidProduct = new Product { Id = 5, Name = "Camara WEB", Brand = "Foscam", Quantity = -10 };
            var result = _controller.AddProduct(invalidProduct);
            Assert.IsType<BadRequestResult>(result);
        }

    }
}