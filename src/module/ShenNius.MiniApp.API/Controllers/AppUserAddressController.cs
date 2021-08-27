using Microsoft.AspNetCore.Mvc;
using ShenNius.MiniApp.API.Controllers;
using ShenNius.Share.Domain.Services.Shop;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Dtos.Input.Shop;
using ShenNius.Share.Models.Entity.Shop;
using System;
using System.Threading.Tasks;

/*************************************
* 类名：AppUserController
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/23 11:01:15
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Shop.API.Controllers
{

    public class AppUserAddressController : MiniAppBaseController
    {
        private readonly IAppUserAddressService _appUserAddressService;
        private readonly IAppUserService _appUserService;

        public AppUserAddressController(IAppUserAddressService appUserAddressService, IAppUserService appUserService)
        {
            _appUserAddressService = appUserAddressService;
            _appUserService = appUserService;
        }
       
        private Tuple<string, string, string> GetNamesBySplitRegions(string regionName)
        {
            if (string.IsNullOrEmpty(regionName))
            {
                throw new ArgumentNullException("用户省市区为空!");
            }
            var regionsArry = regionName.Split(',', StringSplitOptions.RemoveEmptyEntries);
            return new Tuple<string, string, string>(regionsArry[0], regionsArry[1], regionsArry[2]);
        }
        [HttpGet("lists")]
        public async Task<IActionResult> Lists()
        {
            var member = await _appUserService.GetModelAsync(d => d.OpenId == appUser.Openid);
            var addressList = await _appUserAddressService.GetListAsync(d => d.Status == false && d.AppUserId == member.Id);
            return Json(addressList);
        }
        [HttpGet("detail")]
        public async Task<IActionResult> Detail(int addressId)
        {
            var member = await _appUserService.GetModelAsync(d => d.OpenId == appUser.Openid);
            var addressModel = await _appUserAddressService.GetModelAsync(d => d.Status == false && d.AppUserId == member.Id && d.Id.Equals(addressId));
            return Json(addressModel);
        }
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromForm] AddressInput input)
        {
            var member = await _appUserService.GetModelAsync(d => d.OpenId == appUser.Openid && d.Status);
            var tupleValue = GetNamesBySplitRegions(input.Region);
            AppUserAddress model = new AppUserAddress()
            {
               
                CreateTime = DateTime.Now,
                AppUserId = member.Id,
                Status = false,
                Name = input.Name,
                Detail = input.Detail,
                Phone = input.Phone,
                Province = tupleValue.Item1,
                City = tupleValue.Item2,
                Region = tupleValue.Item3,
            };
            var sign = await _appUserAddressService.AddAsync(model);
            return Json(sign);
        }
        [HttpPost("edit")]
        public async Task<IActionResult> Edit([FromForm] AddressInput input)
        {
            var member = await _appUserService.GetModelAsync(d => d.OpenId == appUser.Openid && d.Status);
            var tupleValue = GetNamesBySplitRegions(input.Region);
            var sign = await _appUserAddressService.UpdateAsync(d => new AppUserAddress()
            {
                Name = input.Name,
                Detail = input.Detail,
                Province = tupleValue.Item1,
                City = tupleValue.Item2,
                Region = tupleValue.Item3,
                Phone = input.Phone,
                ModifyTime = DateTime.Now
            }, d => d.Status == false && d.Id == input.AddressId && d.AppUserId == member.Id);           
            return Json(sign);

        }

        [HttpPost("delete")]
        public async Task<ApiResult> Delete([FromForm] int addressId)
        {           
            var sign = await _appUserAddressService.UpdateAsync(d=>new AppUserAddress() {Status =false},d=>d.Id==addressId);
            return new ApiResult(sign);
        }
        [HttpPost("setIsDefault")]
        public async Task<IActionResult> SetDefault([FromForm] string addressId)
        {
            var member = await _appUserService.GetModelAsync(d => d.OpenId == appUser.Openid && d.Status);

            //把有默认地址的取消
            var result = await _appUserAddressService.UpdateAsync(d => new AppUserAddress() { IsDefault = false }, d => d.Status == true && d.AppUserId == member.Id);

            //设置当前新的默认地址
            result = await _appUserAddressService.UpdateAsync(d => new AppUserAddress() { IsDefault = true }, d => d.Status == true && d.Id == member.Id && d.Id.Equals(addressId));
            return Json(result);
        }
    }
}