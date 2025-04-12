using BigPurpleBank.Application.Interfaces;
using BigPurpleBank.Model.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("banking/accounts")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAccounts([FromQuery] AccountFilter filter)
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
}