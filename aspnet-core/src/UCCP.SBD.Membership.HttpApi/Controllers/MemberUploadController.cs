using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using UCCP.SBD.Membership.Members;
using Volo.Abp.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace UCCP.SBD.Membership.Controllers
{
    [Route("api/app/members/upload-excel")]
    public class MemberUploadController : AbpController
    {
        private readonly IMemberAppService _memberAppService;

        public MemberUploadController(IMemberAppService memberAppService)
        {
            _memberAppService = memberAppService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadExcelAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                using var workbook = new XLWorkbook(stream);
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RangeUsed().RowsUsed();

                int rowCount = 0;
                foreach (var row in rows)
                {
                    if (row.RowNumber() == 1) continue; // Skip header

                    var firstName = row.Cell(1).GetString()?.Trim();
                    var lastName = row.Cell(3).GetString()?.Trim();

                    if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
                    {
                        continue; // Skip invalid rows
                    }

                    var isActiveStr = row.Cell(14).GetString()?.Trim()?.ToLower();
                    bool isActive = isActiveStr == "true" || isActiveStr == "1" || isActiveStr == "yes" || isActiveStr == "active";

                    var dto = new CreateUpdateMemberDto
                    {
                        FirstName = firstName,
                        MiddleName = row.Cell(2).GetString()?.Trim(),
                        LastName = lastName,
                        Birthday = row.Cell(4).GetString()?.Trim() ?? string.Empty,
                        Occupation = row.Cell(5).GetString()?.Trim() ?? string.Empty,
                        BaptismDate = row.Cell(6).GetString()?.Trim(),
                        BaptizedBy = row.Cell(7).GetString()?.Trim(),
                        PlaceOfBirth = row.Cell(8).GetString()?.Trim(),
                        FatherName = row.Cell(9).GetString()?.Trim(),
                        MotherName = row.Cell(10).GetString()?.Trim(),
                        Sponsors = row.Cell(11).GetString()?.Trim(),
                        MemberTypeId = row.Cell(12).GetString()?.Trim() ?? string.Empty,
                        OrganizationId = row.Cell(13).GetString()?.Trim() ?? string.Empty,
                        IsActive = isActive
                    };

                    try
                    {
                        await _memberAppService.CreateAsync(dto);
                        rowCount++;
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, $"Failed to insert member at row {row.RowNumber()}");
                    }
                }

                return Ok(new { Count = rowCount });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to parse excel file");
                return StatusCode(500, "Failed to process the excel file: " + ex.Message);
            }
        }
    }
}
