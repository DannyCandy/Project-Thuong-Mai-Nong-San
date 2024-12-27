using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ChungNhanController : Controller
    {
        private readonly IChungNhanRepository _chungNhanRepository;

        public ChungNhanController(IChungNhanRepository chungNhanRepository)
        {
            _chungNhanRepository = chungNhanRepository;
        }

        // Hiển thị danh sách nhà phân phối
        public async Task<IActionResult> Index()
        {
            var nppList = await _chungNhanRepository.GetAllAsync();
            return View(nppList);
        }

        // Hiển thị form thêm nhà phân phối mới

        public IActionResult AddAsync()
        {
            return View();
        }

        // Xử lý thêm nhà phân phối mới

        [HttpPost]
        public async Task<IActionResult> Add(ChungNhan chungNhan, IFormFile HinhAnhChungNhan)
        {
            chungNhan.IdchungNhan = Guid.NewGuid().ToString();
            ModelState.Remove("IdchungNhan");
            if (ModelState.IsValid)
            {
                if (HinhAnhChungNhan is not null)
                {
                    // Lưu hình ảnh sản phẩm
                    chungNhan.HinhAnhChungNhan = await SaveImage(HinhAnhChungNhan);
                }
                await _chungNhanRepository.AddAsync(chungNhan);
                return RedirectToAction(nameof(Index));
            }
            // Nếu ModelState không hợp lệ, hiển thị form với dữ liệu đã nhập
            return View(chungNhan);
        }

        //Xử lý lưu đường dẫn hình ảnh
        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/images/ChungNhanImg", image.FileName); // Thay đổi đường dẫn theo cấu hình của bạn
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/ChungNhanImg/" + image.FileName; // Trả về đường dẫn tương đối
        }


        // Hiển thị form cập nhật nhà phân phối
        public async Task<IActionResult> Update(string id)
        {
            var chungNhan = await _chungNhanRepository.GetByIdAsync(id);
            if (chungNhan is null)
            {
                return NotFound();
            }

            return View(chungNhan);
        }

        // Xử lý cập nhật nhà phân phối

        [HttpPost]
        public async Task<IActionResult> Update(string id, ChungNhan chungNhan, IFormFile HinhAnhChungNhan)
        {
            if (id != chungNhan.IdchungNhan)
            {
                return BadRequest();
            }
            ModelState.Remove("HinhAnhChungNhan");
            if (ModelState.IsValid)
            {
                if (HinhAnhChungNhan is not null)
                {
                    // Lưu hình ảnh sản phẩm
                    chungNhan.HinhAnhChungNhan = await SaveImage(HinhAnhChungNhan);
                }
                await _chungNhanRepository.UpdateAsync(chungNhan);
                return RedirectToAction(nameof(Index));
            }
            return View(chungNhan);
        }

        // Hiển thị form xác nhận xóa nhà phân phối

        public async Task<IActionResult> Delete(string id)
        {
            var chungNhan = await _chungNhanRepository.GetByIdAsync(id);
            if (chungNhan == null)
            {
                return NotFound();
            }
            return View(chungNhan);
        }

        // Xử lý xóa sản phẩm

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var chungNhan = await _chungNhanRepository.GetByIdAsync(id);
                chungNhan.IsDeleted = true;
                await _chungNhanRepository.UpdateAsync(chungNhan);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi khi xóa dữ liệu: " + ex.Message);
            }
        }

    }
}
