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
        public AppUserAddressController(IAppUserAddressService appUserAddressService)
        {
            _appUserAddressService = appUserAddressService;
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
            var addressList = await _appUserAddressService.GetListAsync(d => d.Status == false && d.AppUserId == HttpWx.AppUserId);
            return Json(addressList);
        }
        [HttpGet("detail")]
        public async Task<IActionResult> Detail(int addressId)
        {
            var addressModel = await _appUserAddressService.GetModelAsync(d => d.Status == false && d.AppUserId == HttpWx.AppUserId && d.Id.Equals(addressId));
            return Json(addressModel);
        }
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromForm] AddressInput input)
        {
            var tupleValue = GetNamesBySplitRegions(input.Region);
            AppUserAddress model = new AppUserAddress()
            {

                CreateTime = DateTime.Now,
                AppUserId = HttpWx.AppUserId,
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
            }, d => d.Status == false && d.Id == input.AddressId && d.AppUserId == HttpWx.AppUserId);
            return Json(sign);
        }

        [HttpPost("delete")]
        public async Task<ApiResult> Delete([FromForm] int addressId)
        {
            var sign = await _appUserAddressService.UpdateAsync(d => new AppUserAddress() { Status = false }, d => d.Id == addressId);
            return new ApiResult(sign);
        }
        [HttpPost("setIsDefault")]
        public async Task<IActionResult> SetDefault([FromForm] string addressId)
        {
            //把有默认地址的取消
            var result = await _appUserAddressService.UpdateAsync(d => new AppUserAddress() { IsDefault = false }, d => d.Status == true && d.AppUserId == HttpWx.AppUserId);

            //设置当前新的默认地址
            result = await _appUserAddressService.UpdateAsync(d => new AppUserAddress() { IsDefault = true }, d => d.Status == true && d.Id == HttpWx.AppUserId && d.Id.Equals(addressId));
            return Json(result);
        }
    }
}