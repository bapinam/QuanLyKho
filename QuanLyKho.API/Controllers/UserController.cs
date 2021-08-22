using QuanLyKho.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyKho.Service.UserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.Authencate(request);

            if (string.IsNullOrEmpty(result.ResultObj))
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetUserPagingRequest index)
        {
            var result = await _userService.GetUsersPaging(index);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultId = await _userService.Register(request);
            if (!resultId.IsSuccessed)
            {
                return BadRequest(resultId);
            }

            var result = await _userService.GetByUserName(resultId.ResultObj);
            return Ok(result);
        }

        [HttpGet("check-card")]
        public async Task<IActionResult> iCard(string card, Guid? id)
        {
            var resultId = await _userService.iCard(card, id);
            return Ok(resultId);
        }

        [HttpGet("check-email")]
        public async Task<IActionResult> iEmail(string email, Guid? id)
        {
            var resultId = await _userService.iEmail(email, id);
            return Ok(resultId);
        }

        [HttpGet("check-email-name")]
        public async Task<IActionResult> iEmailName(string email, string name)
        {
            var resultId = await _userService.iEmailName(email, name);
            return Ok(resultId);
        }

        [HttpGet("get-image/{name}")]
        public async Task<IActionResult> GetImage(string name)
        {
            var result = await _userService.GetImage(name);
            return Ok(result);
        }

        [HttpGet("reset-password/{id}")]
        public async Task<IActionResult> ResetPassWord(Guid id)
        {
            var result = await _userService.ResetPassWord(id);
            return Ok(result);
        }

        [HttpGet("paging-role")]
        public async Task<IActionResult> GetUserRole([FromQuery] GetUserPagingRequest index)
        {
            var result = await _userService.GetUserRole(index);
            return Ok(result);
        }

        [HttpGet("get-role/{id}")]
        public async Task<IActionResult> GetRole(Guid id)
        {
            var user = await _userService.GetRole(id);
            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetById(id);
            return Ok(user);
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var user = await _userService.GetByName(name);
            return Ok(user);
        }

        [HttpGet("user-name/{id}")]
        public async Task<IActionResult> GetByUserName(Guid id)
        {
            var user = await _userService.GetByUserName(id);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.Delete(id);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.Update(id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("infor")]
        public async Task<IActionResult> Update([FromBody] UpdateInfor request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.UpdateInfor(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> Update([FromBody] UserUpdatePassword request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.UpdatePassword(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("job-stautus")]
        public async Task<IActionResult> UpdateJobStauts([FromBody] UpdateJobStauts bundle)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.UpdateJobStauts(bundle);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("image")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateImage([FromForm] UpdateImageUser bundle)
        {
            var result = await _userService.UpdateImage(bundle);
            return Ok(result);
        }

        [HttpPut("giao-quyen")]
        public async Task<IActionResult> GiaoQuyenHan(GiaoQuyenHanNguoiDung bundle)
        {
            var result = await _userService.GiaoQuyenHan(bundle);
            return Ok(result);
        }
    }
}