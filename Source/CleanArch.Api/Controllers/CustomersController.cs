using AutoMapper;
using CleanArch.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArch.Core.Wrappers;
using CleanArch.Domain.Dtos;
using CleanArch.Domain.Entities;
using CleanArch.Domain.Parameters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArch.Api.Controllers
{
    [Authorize]
    [ApiVersion("1")]
    public class CustomersController : BaseApiController
    {
        private IMapper _mapper;
        private readonly ICustomerService _customerService;

        public CustomersController(IMapper mapper, ICustomerService customerService)
        {
            _mapper = mapper;
            _customerService = customerService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAsync([FromQuery] QueryStringParameters parameters)
        {
            var (list, total) = await _customerService.GetAsync(parameters);

            var response = new PagedApiResponse<List<CustomerDto>>(_mapper.Map<List<CustomerDto>>(list), total, parameters.PageIndex, parameters.PageSize);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var existedEntity = await _customerService.GetByIdAsync(id);

            var response = new ApiResponse<CustomerDto>(_mapper.Map<CustomerDto>(existedEntity));
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteByIdAsync(Guid id)
        {
            var isDeleted = await _customerService.DeleteByIdAsync(id);

            var response = new ApiResponse<bool>(isDeleted);
            return Ok(response);
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateAsync(CustomerDto model)
        {
            var entity = _mapper.Map<Customer>(model);
            var createdEntity = await _customerService.CreateAsync(entity);

            var response = new ApiResponse<CustomerDto>(_mapper.Map<CustomerDto>(createdEntity));
            return Ok(response);
        }

        [HttpPut("")]
        public async Task<IActionResult> UpdateAsync(CustomerDto model)
        {
            var entity = _mapper.Map<Customer>(model);
            var updatedEntity = await _customerService.UpdateAsync(entity);

            var response = new ApiResponse<CustomerDto>(_mapper.Map<CustomerDto>(updatedEntity));
            return Ok(response);
        }
    }
}