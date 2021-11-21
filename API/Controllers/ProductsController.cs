using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Entities;
using Core.Interfaces;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IGenericRepository<Product> _productsRepo;
    private readonly IGenericRepository<ProductBrand> _productBrandRepo;
    private readonly IGenericRepository<ProductType> _productTypeRepo;
    private readonly ILogger<ProductsController> _logger;


    public ProductsController
    (IGenericRepository<Product> productsRepo,
     IGenericRepository<ProductBrand> productBrandRepo,
     IGenericRepository<ProductType> productTypeRepo,
     ILogger<ProductsController> logger)
    {
        _productsRepo = productsRepo;
        _productBrandRepo = productBrandRepo;
        _productTypeRepo = productTypeRepo;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetProducts()
    {
        var products = await _productsRepo.ListAllAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        return await _productsRepo.GetByIdAsync(id);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
    {
        var productBrands = await _productBrandRepo.ListAllAsync();
        return Ok(productBrands);
    } 

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
    {
        var productTypes = await _productTypeRepo.ListAllAsync();
        return Ok(productTypes);
    }

}
