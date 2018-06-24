﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsController : ControllerBase
    {
        private readonly SmsDbContext _context;

        public SmsController(SmsDbContext context)
        {
            _context = context;
        }

        // GET: api/Sms
        [HttpGet]
        public IEnumerable<SmsModel> GetSmsModels()
        {
            if (_context.SmsModels.Count() == 0)
            {
                var smsModels = new List<SmsModel>() {
                new SmsModel
                    {
                        Sid = "a101",
                        MessageBody = "hi there 1",
                        DeliveryStatus = SmsDelieveryStatus.New,
                        SmsPhoneNumber = "4081112222",
                        SmsSentDateTime = DateTime.Now
                    },new SmsModel
                     {
                        Sid = "a102",
                        MessageBody = "hi there 2",
                        DeliveryStatus = SmsDelieveryStatus.New,
                        SmsPhoneNumber = "4081112223",
                        SmsSentDateTime = DateTime.Now
                    }
                };

                _context.SmsModels.AddRange(smsModels);
                _context.SaveChanges();
            }


            return _context.SmsModels;
        }

        // GET: api/Sms/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSmsModel([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var smsModel = await _context.SmsModels.FindAsync(id);

            if (smsModel == null)
            {
                return NotFound();
            }

            return Ok(smsModel);
        }

        // PUT: api/Sms/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSmsModel([FromRoute] string id, [FromBody] SmsModel smsModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != smsModel.Sid)
            {
                return BadRequest();
            }

            _context.Entry(smsModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SmsModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Sms
        [HttpPost]
        public async Task<IActionResult> PostSmsModel([FromBody] SmsModel smsModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.SmsModels.Add(smsModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSmsModel", new { id = smsModel.Sid }, smsModel);
        }

        // DELETE: api/Sms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSmsModel([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var smsModel = await _context.SmsModels.FindAsync(id);
            if (smsModel == null)
            {
                return NotFound();
            }

            _context.SmsModels.Remove(smsModel);
            await _context.SaveChangesAsync();

            return Ok(smsModel);
        }

        private bool SmsModelExists(string id)
        {
            return _context.SmsModels.Any(e => e.Sid == id);
        }
    }
}