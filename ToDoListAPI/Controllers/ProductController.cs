using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Http;
using ToDoListAPI.DTOs;
using ToDoListAPI.Data;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProductController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] string? name, [FromQuery] int? categoryId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? sortBy = null, [FromQuery] bool desc = false)
    {
        var query = _context.Products.Include(p => p.Category).AsQueryable();

        if (!string.IsNullOrEmpty(name))
            query = query.Where(p => p.Name.Contains(name));
        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId);

        // Sorting
        if (!string.IsNullOrEmpty(sortBy))
        {
            if (sortBy == "price")
                query = desc ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price);
            else if (sortBy == "name")
                query = desc ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name);
            // ... add more sort options
        }

        // Paging
        var total = await query.CountAsync();
        var products = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return Ok(new { total, products });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.ProductDetail)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return NotFound(new { message = "Product not found." });

        return Ok(product);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateProduct([FromForm] ProductCreateDTO dto, IFormFile? image)
    {
        // Validate
        if (string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest(new { message = "Product name is required." });
        if (dto.Price < 0)
            return BadRequest(new { message = "Price must be non-negative." });
        if (!_context.Categories.Any(c => c.Id == dto.CategoryId))
            return BadRequest(new { message = "Category does not exist." });

        string? imageUrl = null;
        if (image != null && image.Length > 0)
        {
            var ext = Path.GetExtension(image.FileName).ToLower();
            var allowedExt = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            if (!allowedExt.Contains(ext))
                return BadRequest(new { message = "Invalid image format." });

            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            if (!Directory.Exists(uploadsDir))
                Directory.CreateDirectory(uploadsDir);

            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsDir, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
            imageUrl = $"/uploads/{fileName}";
        }

        var product = new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            CategoryId = dto.CategoryId,
            ImageUrl = imageUrl,
            // ... set other fields if needed
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductCreateDTO dto, IFormFile? image)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return NotFound(new { message = "Product not found." });

        if (string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest(new { message = "Product name is required." });
        if (dto.Price < 0)
            return BadRequest(new { message = "Price must be non-negative." });
        if (!_context.Categories.Any(c => c.Id == dto.CategoryId))
            return BadRequest(new { message = "Category does not exist." });

        product.Name = dto.Name;
        product.Price = dto.Price;
        product.CategoryId = dto.CategoryId;

        if (image != null && image.Length > 0)
        {
            var ext = Path.GetExtension(image.FileName).ToLower();
            var allowedExt = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            if (!allowedExt.Contains(ext))
                return BadRequest(new { message = "Invalid image format." });

            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            if (!Directory.Exists(uploadsDir))
                Directory.CreateDirectory(uploadsDir);

            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsDir, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
            product.ImageUrl = $"/uploads/{fileName}";
        }

        await _context.SaveChangesAsync();
        return Ok(product);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return NotFound(new { message = "Product not found." });

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("upload")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var path = Path.Combine("uploads", file.FileName);

        using (var stream = new FileStream(path, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Ok(new { FilePath = path });
    }
}