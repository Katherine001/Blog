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
    public class VisterController : ControllerBase
    {
        private readonly MySQLDbContext _mySqlDbContext;

        public VisterController(MySQLDbContext mySqlDbContext)
        {
            _mySqlDbContext = mySqlDbContext;
        }
        /// <summary>
        /// 获取所有评论
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Breview>>> getRemarks()
        {
            return await _mySqlDbContext.Breview.ToListAsync();
        }

        /// <summary>
        /// 根据id修改访客个人信息
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

        /// <summary>
        /// 根据博客id添加评论
        /// </summary>
        /// <param name="p_aid"></param>
        /// <param name="bw"></param>
        /// <returns></returns>
        [HttpPost("blogid/{p_aid}")]
        public async Task<ActionResult<Models.Breview>> remarkByAid(int p_aid, Models.Breview bw)
        {
            bw.p_aid = p_aid;
            _mySqlDbContext.Breview.Add(bw);
            await _mySqlDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(getRemarks), new {p_id = bw.p_id});
        }
        /// <summary>
        /// 获得所有博主的信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("bozhu")]
        public async Task<ActionResult<IEnumerable<User>>> getUsers()
        {
            return await _mySqlDbContext.User.FromSql($"select * from user where u_rid=1002").ToListAsync();
        }
    }
}