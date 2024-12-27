using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Text;
using WebApp.Data;
using WebApp.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApp.Areas.Identity.Pages.Account.Manage
{
    public class UserContact: PageModel
    {
        private readonly AgriculturalProductsContext _context;

        public UserContact(AgriculturalProductsContext context)
        {
            _context = context;
        }
        [BindProperty]
        public List<ThongTinLienHe>? ThongTinLienHes { get; set; }
        [BindProperty]
        public ThongTinLienHe InputModel { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        private Task LoadAsync(List<ThongTinLienHe> listTTLH)
        {
            ThongTinLienHes = listTTLH;
            InputModel = new ThongTinLienHe(); 
            return Task.CompletedTask;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirst("UserId").Value ?? "";
            if (userId == null)
            {
                return NotFound($"Unable to load all contact info of unknown user.");
            }
            var listTTLH = await _context.ThongTinLienHes
                .Where(p => p.UserId == userId)
                .ToListAsync();
            if (listTTLH == null)
            {
                ThongTinLienHes = new List<ThongTinLienHe>();
                return Page();
            }
            
            await LoadAsync(listTTLH);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(ThongTinLienHe ttlh)
        {
            ModelState.Remove("ttlh.User");
            ModelState.Remove("User");
            ModelState.Remove("UserId");
            ModelState.Remove("Idttlh");
            ModelState.Remove("DcgiaoHang");
            ModelState.Remove("Phone");
            ModelState.Remove("DefaultPos");
            if (!ModelState.IsValid)
            {
                var errorMessages = new StringBuilder();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        errorMessages.AppendLine(error.ErrorMessage);
                    }
                }
                StatusMessage = "Cập nhật địa chỉ không thành công. Các lỗi nhập liệu đã đã xảy ra: " + errorMessages.ToString();
                return RedirectToPage();
            }
            try
            {
                var userId = User.FindFirst("UserId").Value ?? "";
                ttlh.UserId = userId;
                //Thiết lập địa chỉ mặc định duy nhất
                var listTTLH = await _context.ThongTinLienHes
                    .Where(p => p.UserId == userId)
                    .AsNoTracking()
                    .ToListAsync();
                //Trong DB đã có hơn 1 địa chỉ
                if (listTTLH.Count > 1)
                {
                    if (ttlh.DefaultPos)
                    {
                        foreach (var item in listTTLH)
                        {
                            if (item.Idttlh != ttlh.Idttlh)
                            {
                                item.DefaultPos = false;
                                _context.ThongTinLienHes.Update(item);
                            }
                        }
                    }
                }
                //Trong Db chỉ có 1 địa chỉ
                else
                {
                    ttlh.DefaultPos = true;
                }
                
                _context.ThongTinLienHes.Update(ttlh);
                await _context.SaveChangesAsync();
                
            }catch(Exception ex)
            {
                StatusMessage = "Unexpected error when trying to update list of contact info: "+ex.ToString();
                return RedirectToPage();
            }
            StatusMessage = "Your contact info has been updated";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            ModelState.Remove("InputModel.User");
            ModelState.Remove("InputModel.Idttlh");
            ModelState.Remove("InputModel.UserId");
            if (!ModelState.IsValid)
            {
                var errorMessages = new StringBuilder();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        errorMessages.AppendLine(error.ErrorMessage);
                    }
                }
                StatusMessage = "Thêm địa chỉ không thành công. Các lỗi nhập liệu đã đã xảy ra: " + errorMessages.ToString();
                return RedirectToPage();
            }
            try
            {
                var userId = User.FindFirst("UserId").Value ?? "";
                var listTTLH = await _context.ThongTinLienHes
                    .Where(p => p.UserId == userId)
                    .ToListAsync();
                if (listTTLH.Count > 2)
                {
                    StatusMessage = "Bạn chỉ có thể thêm tối đa 3 thông tin liên hệ!";
                    return RedirectToPage();
                }
                if (listTTLH.Count == 0)
                {
                    InputModel.DefaultPos = true;
                }
                InputModel.UserId = userId;
                InputModel.Idttlh = Guid.NewGuid().ToString();
                await _context.ThongTinLienHes.AddAsync(InputModel);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = "Unexpected error when trying to add a new record to list of contact info: " + ex.ToString();
                return RedirectToPage();
            }
            StatusMessage = "Your contact info has been added successfully";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string idTtlh)
        {
            try
            {
                var ttlh = await _context.ThongTinLienHes.FindAsync(idTtlh);
                if (ttlh == null)
                {
                    StatusMessage = "Địa chỉ không tồn tại!";
                    return RedirectToPage();
                }
                if (ttlh.DefaultPos)
                {
                    var otherTtlh = await _context.ThongTinLienHes
                    .Where(p => p.UserId == ttlh.UserId && p.Idttlh != idTtlh)
                    .FirstOrDefaultAsync();
                    if (otherTtlh != null)
                    {
                        otherTtlh.DefaultPos = true;
                        _context.ThongTinLienHes.Update(otherTtlh);
                    }
                }
                _context.ThongTinLienHes.Remove(ttlh);
                await _context.SaveChangesAsync();
                StatusMessage = "Xóa địa chỉ thành công.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                StatusMessage = "Gặp lỗi trong khi xóa địa chỉ. Xóa không thành công!";
                return RedirectToPage();
            }
        }
    }
}
