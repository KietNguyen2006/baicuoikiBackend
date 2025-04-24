using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ToDoListAPI.Data;
using ToDoListAPI.Models;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ProductDetailController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProductDetailController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetProductDetail(int productId)
    {
        var detail = await _context.ProductDetails.FirstOrDefaultAsync(pd => pd.ProductId == productId);
        if (detail == null)
            return NotFound(new { message = "Product detail not found." });
        return Ok(detail);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateProductDetail([FromBody] ProductDetail detail)
    {
        if (await _context.ProductDetails.AnyAsync(pd => pd.ProductId == detail.ProductId))
            return BadRequest(new { message = "Product already has details." });

        _context.ProductDetails.Add(detail);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProductDetail), new { productId = detail.ProductId }, detail);
    }

    [HttpPut("{productId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateProductDetail(int productId, [FromBody] ProductDetail detail)
    {
        var existing = await _context.ProductDetails.FirstOrDefaultAsync(pd => pd.ProductId == productId);
        if (existing == null)
            return NotFound(new { message = "Product detail not found." });

        // Update fields as needed
        existing.Description = detail.Description;
        // ... update other fields

        await _context.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("{productId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProductDetail(int productId)
    {
        var detail = await _context.ProductDetails.FirstOrDefaultAsync(pd => pd.ProductId == productId);
        if (detail == null)
            return NotFound(new { message = "Product detail not found." });

        _context.ProductDetails.Remove(detail);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}