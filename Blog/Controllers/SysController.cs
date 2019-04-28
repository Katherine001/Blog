using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    public class SysController:ControllerBase
    {
        private readonly MySQLDbContext _mySqlDbContext;

        public SysController(MySQLDbContext mySqlDbContext)
        {
            _mySqlDbContext = mySqlDbContext;
        }

        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> getUsers()
        {
            return await _mySqlDbContext.User.ToListAsync();
        }

        /// <summary>
        /// 根据用户id获取用户信息
        /// </summary>
        /// <param name="u_id"></param>
        /// <returns></returns>
        [HttpGet("{u_id}")]
        public async Task<ActionResult<User>> getUsers(int u_id)
        {
            var user = await _mySqlDbContext.User.FindAsync(u_id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        /// <summary>
        /// 根据用户id删除用户
        /// </summary>
        /// <param name="u_id"></param>
        /// <returns></returns>
        [HttpDelete("{u_id}")]
        public async Task<IActionResult> deleteUser(int u_id)
        {
            var user = await _mySqlDbContext.User.FindAsync(u_id);
            if (user == null)
            {
                return NotFound();
            }
            _mySqlDbContext.User.Remove(user);
            await _mySqlDbContext.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<User>> postUser(User user)
        {
            _mySqlDbContext.User.Add(user);
            await _mySqlDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(getUsers), new {u_id = user.u_id});
        }
        
        /// <summary>
        /// 根据id修改指定id的用户信息
        /// </summary>
        /// <param name="u_id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut("{u_id}")]
        public async Task<IActionResult> putUser(int u_id, User user)
        {
            if (u_id != user.u_id)
            {
                return BadRequest();
            }
            _mySqlDbContext.Entry(user).State = EntityState.Modified;
            await _mySqlDbContext.SaveChangesAsync();
            return NoContent();
        }

    }
}