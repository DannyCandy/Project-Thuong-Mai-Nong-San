using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using WebApp.Data;
using WebApp.MobileApp.DTO;
using WebApp.Models;

namespace WebApp.MobileApp.Controllers
{
    [ApiController]
    [Route("api/app/[controller]/{action}")]
    public class DeliveryAddressController : ControllerBase
    {
        private readonly AgriculturalProductsContext _context;

        public DeliveryAddressController(AgriculturalProductsContext context)
        {
            _context = context;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IActionResult> GetAllAddresses(string userId)
        {
            var status = new Status();
            try
            {
                if (userId is null)
                {
                    status.StatusCode = 0;
                    status.Message = "Request không đúng định dạng.";
                    return NotFound(status);
                }
                var listTTLH = await _context.ThongTinLienHes
                    .Where(p => p.UserId == userId)
                    .AsNoTracking()
                    .ToListAsync();
                if (listTTLH.Count == 0)
                {
                    status.StatusCode = 2;
                    status.Message = "Người dùng chưa có địa chỉ giao hàng";
                    return BadRequest(status);
                }

                return Ok(listTTLH);
            }
            catch (Exception ex)
            {
                return BadRequest("Xảy ra lỗi khi truy vấn địa chỉ: "+ex.ToString());
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> CreateAddress([FromBody] ThongTinLienHe address)
        {
            var status = new Status();
            if (!ModelState.IsValid) 
            { 
                status.StatusCode = 0;
                status.Message = "Request không đúng định dạng.";
                return BadRequest(status);
            }
            try
            {
                string idGetDefault = "";
                var listTTLH = await _context.ThongTinLienHes
                    .Where(p => p.UserId == address.UserId)
                    .AsNoTracking()
                    .ToListAsync();
                if (listTTLH.Count > 2)
                {
                    status.StatusCode = 3;
                    status.Message = "Bạn chỉ có thể thêm tối đa 3 thông tin liên hệ!";
                    return BadRequest(status);
                }
                if (listTTLH.Count == 0)
                {
                    address.DefaultPos = true;
                    idGetDefault = address.Idttlh;
                }
                else
                {
                    if (address.DefaultPos)
                    {
                        foreach (var item in listTTLH)
                        {
                            if (item.Idttlh != address.Idttlh)
                            {
                                item.DefaultPos = false;
                                _context.ThongTinLienHes.Update(item);
                            }
                        }
                        idGetDefault = address.Idttlh;
                    }
                }
                
                await _context.ThongTinLienHes.AddAsync(address);
                await _context.SaveChangesAsync();

                status.StatusCode = 1;
                status.Message = "Thêm thông tin liên hệ thành công.";
                return Ok(new
                {
                    status.StatusCode,
                    status.Message,
                    idttlh = idGetDefault,
                });
            }
            catch (Exception ex)
            {
                return BadRequest("Xảy ra lỗi khi tạo mới địa chỉ: "+ex.ToString());
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        public async Task<IActionResult> UpdateAddress([FromBody] ThongTinLienHe address)
        {
            var status = new Status();
            if (!ModelState.IsValid)
            {
                status.StatusCode = 0;
                status.Message = "Request không đúng định dạng.";
                return BadRequest(status);
            }
            try
            {
                string idGetDefault = "";
                //Thiết lập địa chỉ mặc định duy nhất
                var listTTLH = await _context.ThongTinLienHes
                    .Where(p => p.UserId == address.UserId)
                    .AsNoTracking()
                    .ToListAsync();
                //Trong DB đã có hơn 1 địa chỉ
                if (listTTLH.Count > 1)
                {
                    if (address.DefaultPos)
                    {
                        foreach (var item in listTTLH)
                        {
                            if (item.Idttlh != address.Idttlh)
                            {
                                item.DefaultPos = false;
                                _context.ThongTinLienHes.Update(item);
                            }
                        }
                        idGetDefault = address.Idttlh;
                    }
                }
                //Trong Db chỉ có 1 địa chỉ
                else
                {
                    address.DefaultPos = true;
                    idGetDefault = address.Idttlh;
                }

                _context.ThongTinLienHes.Update(address);
                await _context.SaveChangesAsync();

                status.StatusCode = 1;
                status.Message = "Cập nhật thông tin liên hệ thành công.";
                return Ok(new
                {
                    status.StatusCode,
                    status.Message,
                    idttlh = idGetDefault,
                });
            }
            catch (Exception ex)
            {
                return BadRequest("Xảy ra lỗi khi cập nhật địa chỉ: " + ex.ToString());
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        public async Task<IActionResult> DeleteAddress(string ttlhId)
        {
            var status = new Status();
            try
            {
                string idGetDefault = "";
                var ttlh = await _context.ThongTinLienHes.FindAsync(ttlhId);
                if (ttlh is null)
                {
                    status.StatusCode = 0;
                    status.Message = "Địa chỉ không tồn tại";
                    return NotFound(status);
                }
                if (ttlh.DefaultPos)
                {
                    var otherTtlh = await _context.ThongTinLienHes
                    .Where(p => p.UserId == ttlh.UserId && p.Idttlh != ttlhId)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
                    if (otherTtlh != null)
                    {
                        otherTtlh.DefaultPos = true;
                        _context.ThongTinLienHes.Update(otherTtlh);
                        idGetDefault = otherTtlh.Idttlh;
                    }
                    
                }

                _context.ThongTinLienHes.Remove(ttlh);
                await _context.SaveChangesAsync();

                status.StatusCode = 1;
                status.Message = "Xóa thông tin liên hệ thành công.";
                return Ok(new
                {
                    status.StatusCode,
                    status.Message,
                    idttlh = idGetDefault,
                });
            }
            catch (Exception ex)
            {
                return BadRequest("Xảy ra lỗi khi xóa địa chỉ: " + ex.ToString());
            }
        }
    }
}
