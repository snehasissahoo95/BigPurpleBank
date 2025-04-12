using BigPurpleBank.Application.Interfaces;
using BigPurpleBank.Model.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("banking/accounts")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly ILogger<AccountsController> _logger;

    public AccountsController(IAccountService accountService, ILogger<AccountsController> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAccounts([FromQuery] AccountFilter filter)
    {
        try
        {
            var query = await _accountService.GetAccountsAsync(filter);

            var totalRecords = await query.CountAsync();
            var accounts = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var response = new
            {
                data = new { accounts },
                links = new
                {
                    self = $"{Request.Scheme}://{Request.Host}{Request.Path}",
                    first = $"{Request.Scheme}://{Request.Host}{Request.Path}?page=1&pageSize={filter.PageSize}",
                    last = $"{Request.Scheme}://{Request.Host}{Request.Path}?page={(totalRecords + filter.PageSize - 1) / filter.PageSize}&pageSize={filter.PageSize}"
                },
                meta = new
                {
                    totalRecords,
                    totalPages = (totalRecords + filter.PageSize - 1) / filter.PageSize
                }
            };

            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid arguments passed in filter.");
            return BadRequest(new { error = ex.Message });
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database update error occurred while fetching accounts.");
            return StatusCode(500, new { error = "A database error occurred while retrieving account data." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while fetching accounts.");
            return StatusCode(500, new { error = "An unexpected error occurred. Please try again later." });
        }
    }
}
